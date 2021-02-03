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
    public class MedicamentsControllerNegativeUnitTest
    {
        [DataTestMethod]
        [MedicamentsParameters]
        public async Task GetAll_NegativeTest(QueryMedicaments filter)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(GetAll_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za�adowanie danych.
                dbContext.Medicaments_Negative_GetAll();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Pobranie element�w wed�ug filtra.
                var response = await controller.GetAll(filter);

                // Pobranie wyniku.
                var result = response.Value;

                // Assert.
                Assert.IsFalse(result.GetEnumerator().MoveNext(), "Lista nie powinna zawiera� element�w.");
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }

        [DataTestMethod]
        [MedicamentsParameters]
        public async Task Get_NegativeTest(object id)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Get_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za�adowanie danych.
                dbContext.Medicaments_Negative_Get();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Pobranie elementu o wskazanym ID.
                var response = await controller.Get((int)id);

                // Pobranie wyniku.
                var result = response.Value;

                // Assert.
                Assert.IsFalse(result != null, "Znaleziono element: '{0}'.", id);
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }

        [DataTestMethod]
        [MedicamentsParameters]
        public async Task Put_NegativeTest(int id, Medicament medicament)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Put_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za�adowanie danych.
                dbContext.Medicaments_Negative_Put();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                try
                {
                    // Aktualizacja elementu.
                    var response = await controller.Put((int)id, medicament);

                    if (response is NoContentResult)
                    {
                        // Assert.
                        Assert.Fail("Uda�o si� zaktualizowa� dane elementu: '{0}'.", id);
                    }
                }
                catch (AssertFailedException)
                {
                    throw;
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
        public async Task Post_NegativeTest(Medicament medicament)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Post_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za�adowanie danych.
                dbContext.Medicaments_Negative_Post();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Dodanie elementu.
                var response = await controller.Post(medicament);

                // Pobranie wyniku.
                var result = (response.Result as CreatedAtActionResult).Value as Medicament;

                // Assert.
                Assert.IsFalse(result != null && result.IdMedicament > 0, "Uda�o si� doda� element: '{0}'.", result.Name);
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch
            { }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }

        [DataTestMethod]
        [MedicamentsParameters]
        public async Task Delete_NegativeTest(int id)
        {
            // Utworzenie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Delete_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za�adowanie danych.
                dbContext.Medicaments_Negative_Delete();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Usuni�cie elementu.
                await controller.Delete(id);

                // Pobranie elementu.
                var response = await controller.Get(id);
                var result = response.Value;

                // Assert.
                Assert.IsFalse(result != null, "Uda�o si� usun�� element: '{0}'.", id);
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }
    }
}
