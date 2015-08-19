using System;
using System.Collections.Generic;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AikTelemetry {
	public class ReadAiksTelemetryCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IAiksTelemetry>, IRrModbusCommandWithTestReply
	{

		public byte CommandCode
		{
			get { return 0x85; } // TODO: 0x83 and 0x84 are for nominal setting values
		}

		public string Name
		{
			get { return "Чтение настроек БС-Ethernet"; }
		}

		public byte[] Serialize()
		{
			return new byte[0];
		}

		public IAiksTelemetry GetResult(byte[] reply) {
			var aikTelemetries = new List<IAikTelemetry>();
			for (int i = 0; i < 3; ++i) {
				int offset = i*58;
				aikTelemetries.Add(
					new AikTelemetrySimple(
						((short) (reply[offset + 0] + (reply[offset + 1] << 8)))*0.1,
						((short) (reply[offset + 2] + (reply[offset + 3] << 8)))*1.0,
						((short) (reply[offset + 4] + (reply[offset + 5] << 8)))*1.0,
						((short) (reply[offset + 6] + (reply[offset + 7] << 8)))*1.0,
						((short) (reply[offset + 8] + (reply[offset + 9] << 8)))*1.0,
						((short) (reply[offset + 10] + (reply[offset + 11] << 8)))*1.0,
						((short) (reply[offset + 12] + (reply[offset + 13] << 8)))*1.0/256.0,
						((short) (reply[offset + 14] + (reply[offset + 15] << 8)))*1.0/256.0,
						((short) (reply[offset + 16] + (reply[offset + 17] << 8)))*0.1,
						((short) (reply[offset + 18] + (reply[offset + 19] << 8)))*1.0,
						((short) (reply[offset + 20] + (reply[offset + 21] << 8)))*1.0,
						((short) (reply[offset + 22] + (reply[offset + 23] << 8)))*1.0,
						((short) (reply[offset + 24] + (reply[offset + 25] << 8)))*1.0,
						((short) (reply[offset + 26] + (reply[offset + 27] << 8)))*1.0,

						ModeSetRunModeBits12Extensions.FromInt((reply[offset + 28] & 0x03)),

						((reply[offset + 28] & 0x04) == 0x04),

						((reply[offset + 30] & 0x01) == 0x01),
						((reply[offset + 30] & 0x02) == 0x02),
						((reply[offset + 30] & 0x04) == 0x04),
						((reply[offset + 30] & 0x08) == 0x08),
						((reply[offset + 30] & 0x10) == 0x10),
						((reply[offset + 30] & 0x20) == 0x20),

						((reply[offset + 30] & 0x40) == 0x40),
						((reply[offset + 30] & 0x80) == 0x80),

						((reply[offset + 31] & 0x01) == 0x01),
						((reply[offset + 31] & 0x10) == 0x10),
						((reply[offset + 31] & 0x20) == 0x20),

						((short) (reply[offset + 32] + (reply[offset + 33] << 8)))*1.0,
						((short) (reply[offset + 34] + (reply[offset + 35] << 8)))*1.0,
						((short) (reply[offset + 36] + (reply[offset + 37] << 8)))*1.0,
						((short) (reply[offset + 38] + (reply[offset + 39] << 8)))*1.0,
						((short) (reply[offset + 40] + (reply[offset + 41] << 8)))*1.0,
						((short) (reply[offset + 42] + (reply[offset + 43] << 8)))*1.0,
						((short) (reply[offset + 44] + (reply[offset + 45] << 8)))*1.0,
						((short) (reply[offset + 46] + (reply[offset + 47] << 8)))*1.0,
						((short) (reply[offset + 48] + (reply[offset + 49] << 8)))*1.0,
						((short) (reply[offset + 50] + (reply[offset + 51] << 8)))*1.0,
						((short) (reply[offset + 52] + (reply[offset + 53] << 8)))*1.0,
						((short) (reply[offset + 54] + (reply[offset + 55] << 8)))*1.0,
						((short) (reply[offset + 56] + (reply[offset + 57] << 8)))*1.0));
			}
			return new AiksTelemetrySimple(
				aikTelemetries[0],
				aikTelemetries[1],
				aikTelemetries[2]);
		}

		public int ReplyLength
		{
			get {
				return 174; // three Aiks each 29 2 bytes params = 29 * 2 * 3 = 174 total
			}
		}

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[174];
			rnd.NextBytes(result);
			return result;
		}
	}
}