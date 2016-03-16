using System;
using System.Windows.Data;

namespace DrillingRig.ConfigApp.AinsSettings {
	[ValueConversion(typeof(double), typeof(int))]
	class NullableShortToNullableDoubleConverter01 : IValueConverter {
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			var ns = (short?)value; // TODO: might throw exception?
			double? result;

			if (ns.HasValue)
				result = ns.Value*0.1;
			else 
				result = null;
			
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			var nd = (double?) value;

			short? result;

			if (nd.HasValue)
				result = (short)(nd.Value * 10.0);
			else
				result = null;

			return result;
		}

		#endregion
	}
}