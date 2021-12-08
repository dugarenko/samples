using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class ValueLessThanParameterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            Type valueType = null;

            if (value != null)
            {
                valueType = value.GetType();
            }

            if (valueType != null && valueType.IsEnum)
            {
                var valueEnum = (int)Enum.ToObject(valueType, value);
                Type parameterType = parameter.GetType();
                var parameterEnum = (int)Enum.ToObject(parameterType, parameter);

                return (valueEnum < parameterEnum);
            }
            else
            {
                var value2 = System.Convert.ToDouble(value);
                var parameter2 = System.Convert.ToDouble(parameter);

                return (value2 < parameter2);
            }
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            throw new NotSupportedException();
        }
	}
}
