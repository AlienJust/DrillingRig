using AlienJust.Support.Collections;

namespace DrillingRig.Commands.AinSettings {
	/// <summary>
	/// Конвертер данных для между Ромой и UI (Q8)
	/// </summary>
	static class BytesPairToDoubleQ8Converter {
		public static BytesPair? ConvertNullableDoubleToBytesPairQ8(double? value) {
			if (!value.HasValue) return null;
			return ConvertDoubleToBytesPairQ8(value.Value);
		}

		public static double? ConvertNullableBytesPairToDoubleQ8(BytesPair? value) {
			if (!value.HasValue) return null;
			return ConvertBytesPairToDoubleQ8(value.Value);
		}

		public static BytesPair ConvertDoubleToBytesPairQ8(double value)
		{
			var iValue = (int)(value * 16777216.0);
			var shortValue = (short)(iValue >> 16);
			return BytesPair.FromSignedShortLowFirst(shortValue);
		}

		public static double ConvertBytesPairToDoubleQ8(BytesPair value)
		{
			var shortValue = value.LowFirstSignedValue;
			var iValue = shortValue << 16;
			return iValue / 16777216.0;
		}
	}
}
