[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WANinject.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WANinject.App_Start.NinjectWebCommon), "Stop")]

namespace WANinject.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using System;
    using System.Web;
    using WANinject.Models;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper _bootstrapper = new Bootstrapper();
        /// <summary>
        /// Publiczny Kernel. Mo¿na ukryæ w³aœciwoœæ, wówczas nie bêdzie mo¿liwoœci rêcznego pobierania obiektów
        /// np. poprzez metodê 'Get'. Pozostanie wtedy tylko 'DependencyResolver.Current.GetService<IProdukt>()'.
        /// </summary>
        public static StandardKernel Kernel { get; private set; }

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            _bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            _bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            // Mo¿emy nie udostêpniaæ klasy publicznie.
            //kernel = new StandardKernel();
            Kernel = new StandardKernel();
            try
            {
                Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(Kernel);
                return Kernel;
            }
            catch
            {
                Kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Ninject zwracaæ bêdzie zawsze nowy egzemplarz klasy.
            kernel.Bind<IProdukt>().To<Produkt>();
            // Ninject zwracaæ bêdzie zawsze ten sam egzemplarz klasy (pojedyncza).
            //kernel.Bind<IProdukt>().To<Produkt>().InSingletonScope();

            // Zwróæ uwagê, ¿e wi¹zanie interfejsu 'IRabat' wystêpuje z dwoma odrêbnymi klasami. W takim przypadku trzeba u¿yæ wi¹zania warunkowego.
            kernel.Bind<IRabat>().To<RabatDomyslny>();
            kernel.Bind<IRabat>().To<RabatElastyczny>().WhenInjectedInto<Produkt>().WithPropertyValue("WysokoscRabatu", 30m);
        }
    }
}