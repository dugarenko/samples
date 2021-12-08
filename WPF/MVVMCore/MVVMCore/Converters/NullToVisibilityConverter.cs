using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVMCore.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter != null)
            {
                if(parameter.ToString().ToLower().Equals("tohidden"))
				{
					if (value != null)
					{
						return Visibility.Visible;
					}
					return Visibility.Hidden;
				}
				else if (parameter.ToString().ToLower().Equals("tocollapsed"))
				{
					if (value != null)
					{
						return Visibility.Visible;
					}
					return Visibility.Collapsed;
				}
				else if (parameter.ToString().ToLower().Equals("inverttohidden"))
				{
					if (value != null)
					{
						return Visibility.Hidden;
					}
					return Visibility.Visible;
				}
				else if (parameter.ToString().ToLower().Equals("inverttocollapsed"))
				{
					if (value != null)
					{
						return Visibility.Collapsed;
					}
					return Visibility.Visible;
				}
			}

			if (value != null)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter != null)
			{
				if (parameter.ToString().ToLower().Equals("inverttohidden") || parameter.ToString().ToLower().Equals("inverttocollapsed"))
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
