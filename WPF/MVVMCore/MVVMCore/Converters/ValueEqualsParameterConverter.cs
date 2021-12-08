using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class ValueEqualsParameterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            if (value == null)
            {
                return (value == parameter);
            }

            Type valueType = value.GetType();

            if (valueType.IsEnum)
            {
                var valueEnum = (int)Enum.ToObject(valueType, value);
                Type parameterType = parameter.GetType();
                var parameterEnum = (int)Enum.ToObject(parameterType, parameter);

                return (valueEnum == parameterEnum);
            }
            else
            {
                var value2 = System.Convert.ToDouble(value);
                var parameter2 = System.Convert.ToDouble(parameter);

                return (value2 == parameter2);
            }
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            if ((bool)value)
			{
                return System.Convert.ChangeType(parameter, targetType, culture);
            }
            else
            {
                Type p = parameter.GetType();
                if (p.IsEnum)
                {
                    return Enum.ToObject(p, 0);
                }
                return System.Convert.ChangeType(value, p, culture);
            }
        }
	}
}
