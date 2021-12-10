using MVVMCore.Properties;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MVVMCore
{
    /// <summary>
    /// Definicja wyjątku dla pustego parametru.
    /// </summary>
    [Serializable]
	public class ArgumentEmptyException : ArgumentException
    {
        /// <summary>
        /// Konstruktor.
        /// </summary>
        public ArgumentEmptyException()
            : base(Resources.ArgumentEmptyException)
        { }

        /// <summary>
        /// Sparametryzowany konstruktor.
        /// </summary>
        /// <param name="paramName">Nazwa parametru.</param>
        public ArgumentEmptyException(string paramName)
            : base(Resources.ArgumentEmptyException, paramName)
        { }

        /// <summary>
        /// Initializes a new instance of the System.ArgumentEmptyException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        protected ArgumentEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Sparametryzowany konstruktor.
        /// </summary>
        /// <param name="message">Komunikat.</param>
        /// <param name="innerException">Wewnętrzny wyjątek.</param>
        public ArgumentEmptyException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Sparametryzowany konstruktor.
        /// </summary>
        /// <param name="paramName">Nazwa parametru.</param>
        /// <param name="message">Komunikat.</param>
        public ArgumentEmptyException(string paramName, string message)
            : base(message, paramName)
        { }
    }
}