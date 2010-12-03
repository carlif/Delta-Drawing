using System;
using System.Globalization;
using System.Windows.Data;

namespace DeltaDrawing.UI
{
    delegate void NoArgDelegate();

	public class DummyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}