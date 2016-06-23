using System.Collections.Generic;

namespace DrillingRig.Commands {
	static class IlistExtensions {
		public static void SerializeInt(this IList<byte> container, int position, int value)
		{
			container[position + 0] = (byte)(value & 0xFF);
			container[position + 1] = (byte)((value >> 8) & 0xFF);
			container[position + 2] = (byte)((value >> 16) & 0xFF);
			container[position + 3] = (byte)((value >> 24) & 0xFF);
		}

		public static void SerializeUint(this IList<byte> container, int position, uint value) {
			container[position + 0] = (byte)(value & 0xFF);
			container[position + 1] = (byte)((value >> 8) & 0xFF);
			container[position + 2] = (byte)((value >> 16) & 0xFF);
			container[position + 3] = (byte)((value >> 24) & 0xFF);
		}

		public static void SerializeShort(this IList<byte> container, int position, short value)
		{
			container[position + 0] = (byte)(value & 0xFF);
			container[position + 1] = (byte)((value >> 8) & 0xFF);
		}

		public static void SerializeUshort(this IList<byte> container, int position, ushort value) {
			container[position + 0] = (byte)(value & 0xFF);
			container[position + 1] = (byte)((value >> 8) & 0xFF);
		}
	}
}