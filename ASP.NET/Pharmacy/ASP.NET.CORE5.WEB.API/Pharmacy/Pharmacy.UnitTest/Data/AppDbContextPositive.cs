using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain;
using Pharmacy.Domain.Models;
using Pharmacy.Domain.Queries;
using System.Collections.Generic;

namespace Pharmacy.UnitTest.Data
{
    /// <summary>
    /// Reprezentuje rozszerzenie klasy AppDbContext - dane dla testów pozytywnych.
    /// </summary>
    public static class AppDbContextPositive
    {
        #region Medicaments - GetAll.

        /// <summary>
        /// Zwraca dane tabeli Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Producers_Positive_GetAll(this ApplicationDbContext dbContext)
        {
            dbContext.Producers.AddRange(
                new Producer() { IdProducer = 1, Name = "Producer1", Description = "Opis Producer1" },
                new Producer() { IdProducer = 2, Name = "Producer2", Description = "Opis Producer2" },
                new Producer() { IdProducer = 3, Name = "Producer3", Description = "Opis Producer3" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca dane tabeli Medicament i Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Medicaments_Positive_GetAll(this ApplicationDbContext dbContext)
        {
            Producers_Positive_GetAll(dbContext);

            dbContext.Medicaments.AddRange(
                new Medicament() { IdProducer = 1, IdMedicament = 1, Name = "Lek1", Price = 15.99m, Description = "Opis Lek1" },
                new Medicament() { IdProducer = 1, IdMedicament = 2, Name = "Lek2", Price = 25m, Description = "Opis Lek2" },
                new Medicament() { IdProducer = 2, IdMedicament = 3, Name = "Lek3", Price = 30.99m, Description = "Opis Lek3" },
                new Medicament() { IdProducer = 2, IdMedicament = 4, Name = "Lek4", Price = 25m, Description = "Opis Lek4" },
                new Medicament() { IdProducer = 3, IdMedicament = 5, Name = "Lek5", Price = 35m, Description = "Opis Lek5" },
                new Medicament() { IdProducer = 3, IdMedicament = 6, Name = "Lek6", Price = 35m, Description = "Opis Lek6" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca parametry dla metody GetAll.
        /// </summary>
        public static QueryMedicaments[] Medicaments_Positive_GetAll_Parameters()
        {
            return new QueryMedicaments[]
                {
                    new QueryMedicaments(),
                    new QueryMedicaments() { IdProducer = 1, Price = 25 },
                    new QueryMedicaments() { IdProducer = 2, },
                    new QueryMedicaments() { Price = 35 },
                };
        }

        #endregion

        #region Medicaments - Get.

        /// <summary>
        /// Zwraca dane tabeli Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Producers_Positive_Get(this ApplicationDbContext dbContext)
        {
            dbContext.Producers.AddRange(
                new Producer() { IdProducer = 1, Name = "Producer1", Description = "Opis Producer1" },
                new Producer() { IdProducer = 2, Name = "Producer2", Description = "Opis Producer2" },
                new Producer() { IdProducer = 3, Name = "Producer3", Description = "Opis Producer3" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca dane tabeli Medicament i Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Medicaments_Positive_Get(this ApplicationDbContext dbContext)
        {
            Producers_Positive_Get(dbContext);

            dbContext.Medicaments.AddRange(
                new Medicament() { IdProducer = 1, IdMedicament = 1, Name = "Lek1", Price = 15.99m, Description = "Opis Lek1" },
                new Medicament() { IdProducer = 1, IdMedicament = 2, Name = "Lek2", Price = 25m, Description = "Opis Lek2" },
                new Medicament() { IdProducer = 2, IdMedicament = 3, Name = "Lek3", Price = 30.99m, Description = "Opis Lek3" },
                new Medicament() { IdProducer = 2, IdMedicament = 4, Name = "Lek4", Price = 25m, Description = "Opis Lek4" },
                new Medicament() { IdProducer = 3, IdMedicament = 5, Name = "Lek5", Price = 35m, Description = "Opis Lek5" },
                new Medicament() { IdProducer = 3, IdMedicament = 6, Name = "Lek6", Price = 35m, Description = "Opis Lek6" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca parametry dla metody Get.
        /// </summary>
        public static int[] Medicaments_Positive_Get_Parameters()
        {
            return new int[] { 1, 2, 3, };
        }

        #endregion

        #region Medicaments - Put.

        /// <summary>
        /// Zwraca dane tabeli Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Producers_Positive_Put(this ApplicationDbContext dbContext)
        {
            dbContext.Producers.AddRange(
                new Producer() { IdProducer = 1, Name = "Producer1", Description = "Opis Producer1" },
                new Producer() { IdProducer = 2, Name = "Producer2", Description = "Opis Producer2" },
                new Producer() { IdProducer = 3, Name = "Producer3", Description = "Opis Producer3" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca dane tabeli Medicament i Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Medicaments_Positive_Put(this ApplicationDbContext dbContext)
        {
            Producers_Positive_Put(dbContext);

            dbContext.Medicaments.AddRange(
                new Medicament() { IdProducer = 1, IdMedicament = 1, Name = "Lek1", Price = 15.99m, Description = "Opis Lek1" },
                new Medicament() { IdProducer = 1, IdMedicament = 2, Name = "Lek2", Price = 25m, Description = "Opis Lek2" },
                new Medicament() { IdProducer = 2, IdMedicament = 3, Name = "Lek3", Price = 30.99m, Description = "Opis Lek3" },
                new Medicament() { IdProducer = 2, IdMedicament = 4, Name = "Lek4", Price = 25m, Description = "Opis Lek4" },
                new Medicament() { IdProducer = 3, IdMedicament = 5, Name = "Lek5", Price = 35m, Description = "Opis Lek5" },
                new Medicament() { IdProducer = 3, IdMedicament = 6, Name = "Lek6", Price = 35m, Description = "Opis Lek6" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca parametry dla metody Put.
        /// </summary>
        public static KeyValuePair<int, Medicament>[] Medicaments_Positive_Put_Parameters()
        {
            return new KeyValuePair<int, Medicament>[]
                {
                    new KeyValuePair<int, Medicament>(1, new Medicament() { IdProducer = 1, IdMedicament = 1, Name = "Lek1", Price = 15.99m, Description = "Opis Lek1 - zaktualizowane" }),
                    new KeyValuePair<int, Medicament>(2, new Medicament() { IdProducer = 1, IdMedicament = 2, Name = "Lek2", Price = 25m, Description = "Opis Lek2 - zaktualizowane" }),
                    new KeyValuePair<int, Medicament>(3, new Medicament() { IdProducer = 2, IdMedicament = 3, Name = "Lek3", Price = 30.99m, Description = "Opis Lek3 - zaktualizowane" }),
                    new KeyValuePair<int, Medicament>(4, new Medicament() { IdProducer = 2, IdMedicament = 4, Name = "Lek4", Price = 25m, Description = "Opis Lek4 - zaktualizowane" }),
                    new KeyValuePair<int, Medicament>(5, new Medicament() { IdProducer = 3, IdMedicament = 5, Name = "Lek5", Price = 35m, Description = "Opis Lek5 - zaktualizowane" }),
                    new KeyValuePair<int, Medicament>(6, new Medicament() { IdProducer = 3, IdMedicament = 6, Name = "Lek6", Price = 35m, Description = "Opis Lek6 - zaktualizowane" })
                };
        }

        #endregion

        #region Medicaments - Post.

        /// <summary>
        /// Zwraca dane tabeli Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Producers_Positive_Post(this ApplicationDbContext dbContext)
        {
            dbContext.Producers.AddRange(
                new Producer() { IdProducer = 1, Name = "Producer1", Description = "Opis Producer1" },
                new Producer() { IdProducer = 2, Name = "Producer2", Description = "Opis Producer2" },
                new Producer() { IdProducer = 3, Name = "Producer3", Description = "Opis Producer3" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca dane tabeli Medicament i Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Medicaments_Positive_Post(this ApplicationDbContext dbContext)
        {
            Producers_Positive_Post(dbContext);
        }

        /// <summary>
        /// Zwraca parametry dla metody Post.
        /// </summary>
        public static Medicament[] Medicaments_Positive_Post_Parameters()
        {
            return new Medicament[]
                {
                    new Medicament() { IdProducer = 1, Name = "Lek1", Price = 15.99m, Description = "Opis Lek1" },
                    new Medicament() { IdProducer = 1, Name = "Lek2", Price = 25m, Description = "Opis Lek2" },
                    new Medicament() { IdProducer = 2, Name = "Lek3", Price = 30.99m, Description = "Opis Lek3" },
                    new Medicament() { IdProducer = 2, Name = "Lek4", Price = 25m, Description = "Opis Lek4" },
                    new Medicament() { IdProducer = 3, Name = "Lek5", Price = 35m, Description = "Opis Lek5" },
                    new Medicament() { IdProducer = 3, Name = "Lek6", Price = 35m, Description = "Opis Lek6" },

                    new Medicament() { IdProducer = 1, Name = "Lek7", Price = 15.99m },
                    new Medicament() { IdProducer = 1, Name = "Lek8", Price = 25m },
                    new Medicament() { IdProducer = 2, Name = "Lek9", Price = 30.99m },
                    new Medicament() { IdProducer = 2, Name = "Lek10", Price = 25m },
                    new Medicament() { IdProducer = 3, Name = "Lek11", Price = 35m },
                    new Medicament() { IdProducer = 3, Name = "Lek12", Price = 35m },

                    new Medicament() { IdProducer = 1, Name = "Lek13" },
                    new Medicament() { IdProducer = 1, Name = "Lek14" },
                    new Medicament() { IdProducer = 2, Name = "Lek15" },
                    new Medicament() { IdProducer = 2, Name = "Lek16" },
                    new Medicament() { IdProducer = 3, Name = "Lek17" },
                    new Medicament() { IdProducer = 3, Name = "Lek18" }
                };
        }

        #endregion

        #region Medicaments - Delete.

        /// <summary>
        /// Zwraca dane tabeli Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Producers_Positive_Delete(this ApplicationDbContext dbContext)
        {
            dbContext.Producers.AddRange(
                new Producer() { IdProducer = 1, Name = "Producer1", Description = "Opis Producer1" },
                new Producer() { IdProducer = 2, Name = "Producer2", Description = "Opis Producer2" },
                new Producer() { IdProducer = 3, Name = "Producer3", Description = "Opis Producer3" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca dane tabeli Medicament i Producer.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        public static void Medicaments_Positive_Delete(this ApplicationDbContext dbContext)
        {
            Producers_Positive_Delete(dbContext);

            dbContext.Medicaments.AddRange(
                new Medicament() { IdProducer = 1, IdMedicament = 1, Name = "Lek1", Price = 15.99m, Description = "Opis Lek1" },
                new Medicament() { IdProducer = 1, IdMedicament = 2, Name = "Lek2", Price = 25m, Description = "Opis Lek2" },
                new Medicament() { IdProducer = 2, IdMedicament = 3, Name = "Lek3", Price = 30.99m, Description = "Opis Lek3" },
                new Medicament() { IdProducer = 2, IdMedicament = 4, Name = "Lek4", Price = 25m, Description = "Opis Lek4" },
                new Medicament() { IdProducer = 3, IdMedicament = 5, Name = "Lek5", Price = 35m, Description = "Opis Lek5" },
                new Medicament() { IdProducer = 3, IdMedicament = 6, Name = "Lek6", Price = 35m, Description = "Opis Lek6" }
                );
            dbContext.SaveChanges();
            dbContext.ResetStateAllEntries(EntityState.Unchanged, EntityState.Detached);
        }

        /// <summary>
        /// Zwraca parametry dla metody Delete.
        /// </summary>
        public static int[] Medicaments_Positive_Delete_Parameters()
        {
            return new int[] { 1, 2, 3, };
        }

        #endregion
    }
}
