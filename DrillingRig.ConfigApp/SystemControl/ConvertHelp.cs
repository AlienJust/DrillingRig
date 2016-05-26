using System;
using System.Collections.Generic;

namespace DrillingRig.ConfigApp.SystemControl {
	static class ConvertHelp {
		public static short ToInt16(byte lowByte, byte highByte) {
			if (BitConverter.IsLittleEndian) {
				return BitConverter.ToInt16(new[] { highByte, lowByte }, 0);
			}
			return BitConverter.ToInt16(new[] { lowByte, highByte }, 0);
		}

		public static ushort ToUInt16(byte lowByte, byte highByte) {
			if (BitConverter.IsLittleEndian) {
				return BitConverter.ToUInt16(new[] { highByte, lowByte }, 0);
			}
			return BitConverter.ToUInt16(new[] { lowByte, highByte }, 0);
		}

		// todo: move text methods to extentions or some static class
		public static string GetByteText(IList<byte> bytes, int zeroBasedRow, int oneBasedCol) {
			try {
				var b = bytes[zeroBasedRow * 4 + oneBasedCol - 1];
				return b.ToString("X2") + " (" + b + ")";
			}
			catch {
				return "----";
			}
		}

		public static string GetParamText(IList<byte> bytes, int zeroBasedRow, int oneBasedCol) {
			try {
				const int bytesCountPerValue = 2;
				const int valuesCountInRow = 2;
				const int bytesInRow = bytesCountPerValue * valuesCountInRow;
				int firstCurrentRowByteIndex = zeroBasedRow * bytesInRow;

				int lowByteIndex = firstCurrentRowByteIndex + (oneBasedCol - 1) * bytesCountPerValue;
				int highByteIndex = firstCurrentRowByteIndex + ((oneBasedCol - 1) * bytesCountPerValue + 1);

				var bu = ToUInt16(bytes[lowByteIndex], bytes[highByteIndex]);
				var bs = ToInt16(bytes[lowByteIndex], bytes[highByteIndex]);

				return bu.ToString("X4") + " (" + bu + ") + [" + bs + "]";
			}
			catch {
				return "--------";
			}
		}
	}
}