using System;

namespace DrillingRig.Commands.AinTelemetry {
	public static class ModeSetRunModeBits12Extensions {
		public static ModeSetRunModeBits12 FromInt(int value) {
			switch (value) {
				case 0:
					return ModeSetRunModeBits12.Freewell;
				case 1:
					return ModeSetRunModeBits12.Traction;
				case 2:
					return ModeSetRunModeBits12.Unknown2;
				case 3:
					return ModeSetRunModeBits12.Unknown3;
				default:
					throw new Exception("Cannot cast integer value " + value + " to ModeSetRunModeBits12 enum");
			}
		}
	}
}