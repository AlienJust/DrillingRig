using AlienJust.Support.Collections;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	/// <summary>
	/// Конвертер данных для между Ромой и UI (Q8)
	/// </summary>
	static class BytesPairToDoubleQ8Converter {
		public static BytesPair? ConvertNullableDoubleToBytesPairQ8(double? value) {
			if (!value.HasValue) return null;
			var iValue = (int)(value.Value * 16777216.0);
			var shortValue = (short)(iValue >> 16);
			return BytesPair.FromSignedShortLowFirst(shortValue);
		}

		public static double? ConvertNullableBytesPairToDoubleQ8(BytesPair? value) {
			if (!value.HasValue) return null;
			var shortValue = value.Value.LowFirstSignedValue;
			var iValue = shortValue << 16;
			return iValue / 16777216.0;
		}
	}
}
