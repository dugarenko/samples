using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if (parameter != null && parameter.ToString().ToLower().Equals("invert"))
                {
                    return !(bool)value;
                }
                return (bool)value;
            }
            else if (value is string)
            {
                string val = value.ToString().ToLower();
                if (val == "1" || val == "true")
                {
                    if (parameter != null && parameter.ToString().ToLower().Equals("invert"))
                    {
                        return false;
                    }
                    return true;
                }
                else if (val == "0" || val == "false")
                {
                    if (parameter != null && parameter.ToString().ToLower().Equals("invert"))
                    {
                        return true;
                    }
                    return false;
                }
            }
            return System.Convert.ToBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                if (parameter.ToString().ToLower().Equals("invert"))
                {
                    if (value is bool)
                    {
                        return !(bool)value;
                    }
                }
                else if (parameter.ToString().ToLower().Equals("tostring"))
                {
                    if (value is bool)
                    {
                        return ((bool)value).ToString();
                    }
                    else if (value is string)
                    {
                        string val = value.ToString().ToLower();
                        if (val == "1" || val == "true")
                        {
                            return true.ToString();
                        }
                        else if (val == "0" || val == "false")
                        {
                            return false.ToString();
                        }
                    }
                    return System.Convert.ToString(value);
                }
            }
            return value;
        }
    }
}
