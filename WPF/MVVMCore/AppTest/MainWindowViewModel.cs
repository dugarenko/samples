using MVVMCore;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AppTest
{
    internal class MainWindowViewModel : AppViewModel
    {
        private IEnumerable _countryItemsSource = null;
        private List<Country> _countrySelectedItems = new List<Country>();

        public MainWindowViewModel()
        {
            LoadBigData();
        }

        private void LoadBigData()
        {
            CountryItemsSource = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                                                           .Where(c => !c.IsNeutralCulture)
                                                                           .Select(c => new Country { DisplayName = c.DisplayName, NativeName = c.NativeName, ThreeLetterISOLanguageName = c.ThreeLetterISOLanguageName });
        }

        public IEnumerable CountryItemsSource
        {
            get
            {
                return _countryItemsSource;
            }
            set
            {
                _countryItemsSource = value;
                OnPropertyChanged();
            }
        }

        public IList CountrySelectedItems
        {
            get => _countrySelectedItems;
            set
            {
                if (value == null)
                {
                    _countrySelectedItems.Clear();
                }
                else
                {
                    _countrySelectedItems = value.Cast<Country>().ToList();
                }
            }
        }
    }
}
