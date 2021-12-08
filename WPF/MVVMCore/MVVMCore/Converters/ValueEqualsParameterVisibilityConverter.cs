using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class ValueEqualsParameterVisibilityConverter : IValueConverter
	{
		private static BooleanToVisibilityConverter _booleanToVisibilityConverter = new BooleanToVisibilityConverter();
		private static ValueEqualsParameterConverter _valueEqualsParameterConverter = new ValueEqualsParameterConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result = _valueEqualsParameterConverter.Convert(value, targetType, parameter, culture);
			return _booleanToVisibilityConverter.Convert(result, targetType, null, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            throw new NotImplementedException();
        }
	}
}
