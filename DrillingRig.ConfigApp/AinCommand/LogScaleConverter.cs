using System;
using System.Globalization;
using System.Windows.Data;

namespace DrillingRig.ConfigApp.AinCommand {
	public class LogScaleConverter : IValueConverter {
		// convert - to display
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			try {
				double vmValue = (short)value; // 0 - 15000
				var viewValue = Math.Log(0.01 + vmValue / 15151.515151, 100) + 1.0;
				return viewValue;
			}
			catch {
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			try {
				double viewValue = (double)value; // 0 - 1
				return Math.Pow(100, viewValue - 1) * 15151.515151 - 151.51515151;
			}
			catch {
				return null;
			}
		}
	}
}