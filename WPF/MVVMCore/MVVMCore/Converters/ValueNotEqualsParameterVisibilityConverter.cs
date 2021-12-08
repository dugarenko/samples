using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class ValueNotEqualsParameterVisibilityConverter : IValueConverter
	{
        private static BooleanToVisibilityConverter _booleanToVisibilityConverter = new BooleanToVisibilityConverter();
        private static ValueNotEqualsParameterConverter _valueNotEqualsParameterConverter = new ValueNotEqualsParameterConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            object result = _valueNotEqualsParameterConverter.Convert(value, targetType, parameter, culture);
            return _booleanToVisibilityConverter.Convert(result, targetType, null, culture);
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            throw new NotImplementedException();
        }
	}
}
