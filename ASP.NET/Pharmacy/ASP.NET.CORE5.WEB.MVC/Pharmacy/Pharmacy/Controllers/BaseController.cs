using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Linq;

namespace Pharmacy.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Wczytuje wartość do wskazanej właściwości obiektu.
        /// </summary>
        /// <param name="obj">Obiekt, którego właściwości przeznaczona jest do zaktualizowania.</param>
        /// <param name="propertyName">Nazwa właściwości do zaktualizowania.</param>
        /// <param name="value">Wartość do wczytania.</param>
        /// <returns>true, jeśli wartość udało się wczytać do właściwości przekazanego obiektu, w przeciwnym razie false.</returns>
        private static bool SetDecimalValue(object obj, string propertyName, object value)
        {
            try
            {
                string currencyDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                decimal v = Convert.ToDecimal(value.ToString()
                    .Replace(",", currencyDecimalSeparator)
                    .Replace(".", currencyDecimalSeparator));

                obj.GetType().GetProperties().Single(x => x.Name == propertyName).SetValue(obj, v);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Sprawdza stan modelu wskazanej właściwości i ewentualnie tłumaczy przekazaną wartość dziesiętną w formacie ciągu znaków na wartość Decimal.
        /// </summary>
        /// <param name="obj">Obiekt, którego właściwości przeznaczona jest do zaktualizowania.</param>
        /// <param name="propertyName">Nazwa właściwości do zaktualizowania.</param>
        /// <returns>true, jeśli tłumaczenie się powiodło, w przeciwnym razie false.</returns>
        public bool TranslateDecimal(object obj, string propertyName)
        {
            var validation = ModelState.SingleOrDefault(x => x.Key == propertyName && x.Value.ValidationState == ModelValidationState.Invalid);
            if (string.IsNullOrEmpty(validation.Key))
            {
                return true;
            }

            if (!SetDecimalValue(obj, propertyName, validation.Value.RawValue))
            {
                return false;
            }

            validation.Value.ValidationState = ModelValidationState.Valid;
            return true;
        }
    }
}
