using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visible = (Visibility)value;

            if (parameter != null)
            {
                if (parameter.ToString().ToLower().Equals("inverttohidden"))
                {
                    if (visible == Visibility.Visible)
                    {
                        return Visibility.Hidden;
                    }
                    return Visibility.Visible;
                }
                else if (parameter.ToString().ToLower().Equals("inverttocollapsed"))
                {
                    if (visible == Visibility.Visible)
                    {
                        return Visibility.Collapsed;
                    }
                    return Visibility.Visible;
                }
            }

            return visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
