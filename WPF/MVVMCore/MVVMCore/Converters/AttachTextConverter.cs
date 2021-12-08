using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class AttachTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (string)parameter;
            }
            else if (parameter == null)
            {
                return value;
            }
            return value.ToString() + parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
