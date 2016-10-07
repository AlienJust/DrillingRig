using System;
using System.Globalization;
using System.Windows.Data;

namespace DrillingRig.ConfigApp.AinCommand
{
	class NullableBoolToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nb =  value as bool?;
			if (!nb.HasValue) return " ? ";
			return nb.Value ? " 1 " : " 0 ";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}