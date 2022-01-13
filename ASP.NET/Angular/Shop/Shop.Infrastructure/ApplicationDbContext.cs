using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shop.Infrastructure.Models;
using Shop.Infrastructure.Models.Identity;

#nullable disable

namespace Shop.Infrastructure
{
    public partial class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public virtual DbSet<Kraj> Kraje { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Kraj>().HasData(new Kraj() { Id = 1, NazwaPolska = "Polska", NazwaAngielska = "Poland", KodKrajuISO2 = "PL", KodKrajuISO3 = "POL", KodWalutyISO = "PLN", UE = true });
        }
    }
}
