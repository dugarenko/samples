using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Pharmacy.Domain;
using Pharmacy.UnitTest.Extensions;
using System;
using System.IO;
using System.Linq;

namespace Pharmacy.UnitTest.Data
{
    public static class AppDbContextMocker
    {
        private static bool UseSqlite()
        {
            var builder = new ConfigurationBuilder();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testsettings.json");
            builder.AddJsonFile(filePath, optional: false);
            var configuration = builder.Build();
            bool useSqlite = configuration.GetValue("UseSqlite", true);
            return useSqlite;
        }

        public static ApplicationDbContext GetMemoryDbContext(string databaseName)
        {
            bool useSqlite = UseSqlite();

            if (useSqlite)
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connectionStringBuilder.ConnectionString)
                    .Options;

                var dbContext = new ApplicationDbContext(options);
                dbContext.Database.OpenConnection();
                ModelBuilderEx.SqliteAddIndexes(dbContext.Model.GetEntityTypes().Cast<IMutableEntityType>());
                dbContext.Database.EnsureCreated();

                return dbContext;
            }
            else
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName)
                    .Options;

                return new ApplicationDbContext(options);
            }
        }

        public static void ResetStateAllEntries(this DbContext context, EntityState from, EntityState to)
        {
            EntityEntry[] entries = context.ChangeTracker.Entries().Where(e => e.State == from).ToArray();
            foreach (var item in entries)
            {
                item.State = to;
            }
        }
    }
}