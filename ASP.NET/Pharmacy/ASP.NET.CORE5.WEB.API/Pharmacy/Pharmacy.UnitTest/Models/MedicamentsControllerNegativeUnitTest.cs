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
            // Pobranie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(GetAll_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Negative_GetAll();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Pobranie wyniku.
                var response = await controller.GetAll(filter);
                var result = response.Value;

                // Assert.
                Assert.IsFalse(result.GetEnumerator().MoveNext(), "Lista nie powinna zawieraæ elementów.");
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
            // Pobranie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Get_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Negative_Get();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Pobranie wyniku.
                var response = await controller.Get((int)id);
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
            // Pobranie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Put_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Negative_Put();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                try
                {
                    // Pobranie wyniku.
                    var response = await controller.Put((int)id, medicament);

                    if (response is NoContentResult)
                    {
                        // Assert.
                        Assert.Fail("Uda³o siê zaktualizowaæ dane elementu: '{0}'.", id);
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
            // Pobranie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Post_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Negative_Post();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Pobranie wyniku.
                var response = await controller.Post(medicament);
                var result = (response.Result as CreatedAtActionResult).Value as Medicament;

                // Assert.
                Assert.IsFalse((result != null && result.IdMedicament > 0), "Uda³o siê dodaæ element: '{0}'.", result.Name);
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception ex)
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
            // Pobranie kontekstu bazy danych.
            var dbContext = AppDbContextMocker.GetMemoryDbContext(nameof(Delete_NegativeTest) + Guid.NewGuid());

            try
            {
                // Za³adowanie danych.
                dbContext.Medicaments_Negative_Delete();

                // Utworzenie instancji kontrolera.
                var controller = new MedicamentsController(dbContext, new Logger<MedicamentsController>(new LoggerFactory()));

                // Pobranie wyniku.
                var response = await controller.Delete(id);
                var result = response.Value;

                // Assert.
                Assert.IsFalse(result != null, "Uda³o siê usun¹æ element: '{0}'.", id);
            }
            finally
            {
                // Zniszczenie kontekstu bazy danych.
                dbContext.Dispose();
            }
        }
    }
}
