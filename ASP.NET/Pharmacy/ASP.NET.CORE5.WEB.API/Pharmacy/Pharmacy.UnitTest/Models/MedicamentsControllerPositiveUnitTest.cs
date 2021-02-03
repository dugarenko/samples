using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pharmacy.Controllers;
using Pharmacy.Domain.Models;
using Pharmacy.Domain.Queries;
using Pharmacy.UnitTest.Data;
using System;
using System.Threading.Tasks;

namespace Pharmacy.UnitTest.Models
{
    [TestClass]
    public class MedicamentsControllerPositiveUnitTest
    {
        [DataTestMethod]
        [MedicamentsParameters]
        public async Task GetAll_PositiveTest(QueryMedicaments filter)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(GetAll_PositiveTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Positive_GetAll();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Pobranie elementów wed³ug filtra.
                var response = await controller.GetAll(filter);

                // Pobranie wyniku.
                var result = response.Value;

                // Assert.
                Assert.IsTrue(result.GetEnumerator().MoveNext(), "Lista powinna zawieraæ elementy.");
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }

        [DataTestMethod]
        [MedicamentsParameters]
        public async Task Get_PositiveTest(int id)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Get_PositiveTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Positive_Get();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Pobranie elementu o wskazanym ID.
                var response = await controller.Get(id);

                // Pobranie wyniku.
                var result = response.Value;

                // Assert.
                Assert.IsTrue(result != null, "Nie znaleziono elementu: '{0}'.", id);
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }

        [DataTestMethod]
        [MedicamentsParameters]
        public async Task Put_PositiveTest(int id, Medicament medicament)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Put_PositiveTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Positive_Put();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                try
                {
                    // Aktualizacja elementu.
                    await controller.Put(id, medicament);
                }
                catch
                {
                    // Assert.
                    Assert.Fail("Nie uda³o siê zaktualizowaæ danych elementu: '{0}'.", id);
                }
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }

        [DataTestMethod]
        [MedicamentsParameters]
        public async Task Post_PositiveTest(Medicament medicament)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Post_PositiveTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Positive_Post();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Dodanie elementu.
                var response = await controller.Post(medicament);

                // Pobranie wyniku.
                var result = response.Result as CreatedAtActionResult;
                
                // Assert.
                Assert.IsTrue(result.Value != null, "Nie uda³o siê dodaæ elementu: '{0}'.", medicament.Name);
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }

        [DataTestMethod]
        [MedicamentsParameters]
        public async Task Delete_PositiveTest(int id)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Delete_PositiveTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Positive_Delete();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Usuniêcie elementu.
                await controller.Delete(id);

                // Pobranie elementu.
                var response = await controller.Get(id);
                var result = response.Value;

                // Assert.
                Assert.IsTrue(result == null, "Nie uda³o siê usun¹æ elementu: '{0}'.", id);
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }
    }
}
