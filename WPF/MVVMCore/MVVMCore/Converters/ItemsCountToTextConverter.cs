using MVVMCore.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class ItemsCountToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = "";

            if (parameter != null && culture != null)
            {
                if (value == null)
                {
                    v = ((int)0).ToString(parameter.ToString(), culture);
                }
                else
                {
                    v = ((int)value).ToString(parameter.ToString(), culture);
                }
            }
            else if (parameter != null)
            {
                if (value == null)
                {
                    v = ((int)0).ToString(parameter.ToString());
                }
                else
                {
                    v = ((int)value).ToString(parameter.ToString());
                }
            }
            else
            {
                if (value == null)
                {
                    v = ((int)0).ToString();
                }
                else
                {
                    v = ((int)value).ToString();
                }
            }

            return Resources.IloscElementow + " " + v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
