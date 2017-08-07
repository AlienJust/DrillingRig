using AlienJust.Support.Collections;

namespace DrillingRig.Commands.AinSettings
{
	static class BytesPairToDecimalQ8Converter {
		public static BytesPair? ConvertNullableDecimalToBytesPairQ8(decimal? value) {
			if (!value.HasValue) return null;
			return ConvertToBytesPairQ8(value.Value);
		}

		public static decimal? ConvertNullableBytesPairToDecimalQ8(BytesPair? value) {
			if (!value.HasValue) return null;
			return ConvertBytesPairToDecimalQ8(value.Value);
		}

		public static BytesPair ConvertToBytesPairQ8(decimal value) {
			var iValue = (int)(value * 16777216.0m);
			var ushortValue = (ushort)(iValue >> 16);
			return BytesPair.FromUnsignedShortLowFirst(ushortValue);
		}

		public static decimal ConvertBytesPairToDecimalQ8(BytesPair value) {
			var ushortValue = value.LowFirstUnsignedValue;
			var iValue = ushortValue << 16;
			return iValue / 16777216.0m;
		}
	}
}