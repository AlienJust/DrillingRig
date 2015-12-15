using System;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AinTelemetry {
	public class ReadAinTelemetryCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IAinTelemetry>, IRrModbusCommandWithTestReply
	{
		private readonly byte _zeroBasedAinNumber;

		public ReadAinTelemetryCommand(byte zeroBasedAinNumber) {
			_zeroBasedAinNumber = zeroBasedAinNumber;
		}

		public byte CommandCode
		{
			get { return 0x85; }
		}

		public string Name
		{
			get { return "Чтение телеметрии АИН #" + (_zeroBasedAinNumber + 1); }
		}

		public byte[] Serialize()
		{
			return new[] {(byte)(_zeroBasedAinNumber + 1)};
		}

		public IAinTelemetry GetResult(byte[] reply) {
			// TODO: check if reply[0] is equal _zbAinNumber
			return new AinTelemetrySimple(
				((short) (reply[1] + (reply[2] << 8)))*0.1,
				((short) (reply[3] + (reply[4] << 8)))*1.0,
				((short) (reply[5] + (reply[6] << 8)))*1.0,
				((short) (reply[7] + (reply[8] << 8)))*1.0,
				((short) (reply[9] + (reply[10] << 8)))*1.0,
				((short) (reply[11] + (reply[12] << 8)))*1.0,
				((short) (reply[13] + (reply[14] << 8)))*1.0/256.0,
				((short) (reply[15] + (reply[16] << 8)))*1.0/256.0,
				((short) (reply[17] + (reply[18] << 8)))*0.1,
				((short) (reply[19] + (reply[20] << 8)))*1.0,
				((short) (reply[21] + (reply[22] << 8)))*1.0,
				((short) (reply[23] + (reply[24] << 8)))*1.0,
				((short) (reply[25] + (reply[26] << 8)))*1.0,
				((short) (reply[27] + (reply[28] << 8)))*1.0,

				ModeSetRunModeBits12Extensions.FromInt((reply[29] & 0x03)),

				((reply[29] & 0x04) == 0x04),

				((reply[31] & 0x01) == 0x01),
				((reply[31] & 0x02) == 0x02),
				((reply[31] & 0x04) == 0x04),
				((reply[31] & 0x08) == 0x08),
				((reply[31] & 0x10) == 0x10),
				((reply[31] & 0x20) == 0x20),

				((reply[31] & 0x40) == 0x40),
				((reply[31] & 0x80) == 0x80),

				((reply[32] & 0x01) == 0x01),
				((reply[32] & 0x10) == 0x10),
				((reply[32] & 0x20) == 0x20),

				((short) (reply[33] + (reply[34] << 8)))*1.0,
				((short) (reply[35] + (reply[36] << 8)))*1.0,
				((short) (reply[37] + (reply[38] << 8)))*1.0,
				((short) (reply[39] + (reply[40] << 8)))*1.0,
				((short) (reply[41] + (reply[42] << 8)))*1.0,
				((short) (reply[43] + (reply[44] << 8)))*1.0,
				((short) (reply[45] + (reply[46] << 8)))*1.0,
				((short) (reply[47] + (reply[48] << 8)))*1.0,
				((short) (reply[49] + (reply[50] << 8)))*1.0,
				((short) (reply[51] + (reply[52] << 8)))*1.0,
				((short) (reply[53] + (reply[54] << 8)))*1.0,
				((short) (reply[55] + (reply[56] << 8)))*1.0,
				((short) (reply[57] + (reply[58] << 8)))*1.0);
		}

		public int ReplyLength
		{
			get {
				return 70;
			}
		}

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}