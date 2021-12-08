using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter != null)
            {
                string p = parameter.ToString().ToLower();
                if (p == "tohidden")
				{
					if ((bool)value)
					{
						return Visibility.Visible;
					}
					return Visibility.Hidden;
				}
				else if (p == "tocollapsed")
				{
					if ((bool)value)
					{
						return Visibility.Visible;
					}
					return Visibility.Collapsed;
				}
				else if (p == "inverttohidden")
				{
					if ((bool)value)
					{
						return Visibility.Hidden;
					}
					return Visibility.Visible;
				}
				else if (p == "inverttocollapsed")
				{
					if ((bool)value)
					{
						return Visibility.Collapsed;
					}
					return Visibility.Visible;
				}
			}

			if ((bool)value)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter != null)
			{
                string p = parameter.ToString().ToLower();
                if (p == "inverttohidden" || p == "inverttocollapsed")
				{
					if ((Visibility)value == Visibility.Visible)
					{
						return false;
					}
					return true;
				}
			}

			if ((Visibility)value == Visibility.Visible)
			{
				return true;
			}
			return false;
		}
	}
}
