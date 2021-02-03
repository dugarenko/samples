using Ninject;
using System.Web.Mvc;
using WANinject.App_Start;
using WANinject.Models;

namespace WANinject.Controllers
{
    public class HomeController : Controller
    {
        private IProdukt _produkt;
        private IRabat _rabatDomyslny;

        public HomeController(IProdukt produkt, IRabat rabatDomyslny)
        {
            // Produkt w konstruktorze również implementuje interfejs 'IRabat'.
            // Patrz jak wykonane zostało powiązanie w 'NinjectWebCommon.RegisterServices()'
            // tego samego interfejsu 'IRabat' w dwóch odrębnych klasach. Zastosowano wiązanie warunkowe.

            _produkt = produkt;
            _rabatDomyslny = rabatDomyslny;
        }

        public ActionResult Index()
        {
            decimal wartosc = 100m;
            decimal wartoscZRabatem = _rabatDomyslny.Oblicz(wartosc);

            _produkt.Wartosc = wartosc;
            decimal wartoscZRabatem2 = _produkt.WartoscZRabatem;

            _produkt.Wartosc = wartosc * 2;
            decimal wartoscZRabatem3 = _produkt.WartoscZRabatem;

            #region Pobranie obiektu przy użyciu statycznej klasy 'DependencyResolver'.

            IProdukt produkt = DependencyResolver.Current.GetService<IProdukt>();
            produkt.Wartosc = wartosc * 3;
            decimal wartoscZRabatem4 = produkt.WartoscZRabatem;

            #endregion

            // Pobranie obiektu przy użyciu udostępnionej statycznej klasy 'StandardKernel'.
            IProdukt produkt2 = NinjectWebCommon.Kernel.Get<IProdukt>();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}