using Microsoft.AspNet.Identity.EntityFramework;
using SportsStore.Domain.Models;

namespace SportsStore.Domain
{
    /// <summary>
    /// Reprezentuje kontekst poświadczenia dla użytkownika systemu.
    /// </summary>
    public class AuthenticationContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Inicjuje nową instancję klasy.
        /// </summary>
        public AuthenticationContext()
            : base()
        {
        }

        /// <summary>
        /// Tworzy i zwraca nowy kontekst poświadczenia dla użytkownika systemu.
        /// </summary>
        /// <returns></returns>
        public static AuthenticationContext Create()
        {
            return new AuthenticationContext();
        }
    }
}
