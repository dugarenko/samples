using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class ValueLessThanParameterVisibilityConverter : IValueConverter
	{
		private static BooleanToVisibilityConverter _booleanToVisibilityConverter = new BooleanToVisibilityConverter();
		private static ValueLessThanParameterConverter _valueLessThanParameterConverter = new ValueLessThanParameterConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result = _valueLessThanParameterConverter.Convert(value, targetType, parameter, culture);
			return _booleanToVisibilityConverter.Convert(result, targetType, null, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            throw new NotSupportedException();
        }
	}
}
