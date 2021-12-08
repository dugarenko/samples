using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class AllEqualsMultiConverter : IMultiValueConverter
    {
        private static Type _typeBoolean = typeof(bool);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return null;
            }

            object p = System.Convert.ChangeType(parameter, _typeBoolean);
            return values.All(x => x.Equals(p));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
