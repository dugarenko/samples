using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAuthorizationKey.Areas.Api.Models;

#nullable disable

namespace WebAuthorizationKey.Areas.Api.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Kraj> Krajs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Kraj>(entity =>
            {
                entity.ToTable("Kraj");

                entity.Property(e => e.ISO2)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.ISO3).HasMaxLength(3);

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(70);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
