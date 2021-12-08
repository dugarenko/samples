using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                if (parameter != null && parameter.ToString().ToLower().Equals("invert"))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (parameter != null && parameter.ToString().ToLower().Equals("invert"))
                {
                    return false;
                }
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
