using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pharmacy.Domain.Models;
using System;
using System.IO;

#nullable disable

namespace Pharmacy.Domain
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                builder.AddJsonFile(filePath, optional: false);
                var configuration = builder.Build();
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Medicament> Medicaments { get; set; }
        public virtual DbSet<City> Cities { get; set; }
    }
}
