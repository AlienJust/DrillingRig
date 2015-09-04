using System;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AikTelemetry {
	public class ReadAinTelemetryCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IAinTelemetry>, IRrModbusCommandWithTestReply
	{
		private readonly byte _zeroBasedAinNumber;

		public ReadAinTelemetryCommand(byte zeroBasedAinNumber) {
			_zeroBasedAinNumber = zeroBasedAinNumber;
		}

		public byte CommandCode
		{
			get { return (byte)(0x85 + _zeroBasedAinNumber); }
		}

		public string Name
		{
			get { return "Чтение настроек АИН #" + (_zeroBasedAinNumber + 1); }
		}

		public byte[] Serialize()
		{
			return new[] {_zeroBasedAinNumber};
		}

		public IAinTelemetry GetResult(byte[] reply) {
			return new AinTelemetrySimple(
				((short) (reply[0] + (reply[1] << 8)))*0.1,
				((short) (reply[2] + (reply[3] << 8)))*1.0,
				((short) (reply[4] + (reply[5] << 8)))*1.0,
				((short) (reply[6] + (reply[7] << 8)))*1.0,
				((short) (reply[8] + (reply[9] << 8)))*1.0,
				((short) (reply[10] + (reply[11] << 8)))*1.0,
				((short) (reply[12] + (reply[13] << 8)))*1.0/256.0,
				((short) (reply[14] + (reply[15] << 8)))*1.0/256.0,
				((short) (reply[16] + (reply[17] << 8)))*0.1,
				((short) (reply[18] + (reply[19] << 8)))*1.0,
				((short) (reply[20] + (reply[21] << 8)))*1.0,
				((short) (reply[22] + (reply[23] << 8)))*1.0,
				((short) (reply[24] + (reply[25] << 8)))*1.0,
				((short) (reply[26] + (reply[27] << 8)))*1.0,

				ModeSetRunModeBits12Extensions.FromInt((reply[28] & 0x03)),

				((reply[28] & 0x04) == 0x04),

				((reply[30] & 0x01) == 0x01),
				((reply[30] & 0x02) == 0x02),
				((reply[30] & 0x04) == 0x04),
				((reply[30] & 0x08) == 0x08),
				((reply[30] & 0x10) == 0x10),
				((reply[30] & 0x20) == 0x20),

				((reply[30] & 0x40) == 0x40),
				((reply[30] & 0x80) == 0x80),

				((reply[31] & 0x01) == 0x01),
				((reply[31] & 0x10) == 0x10),
				((reply[31] & 0x20) == 0x20),

				((short) (reply[32] + (reply[33] << 8)))*1.0,
				((short) (reply[34] + (reply[35] << 8)))*1.0,
				((short) (reply[36] + (reply[37] << 8)))*1.0,
				((short) (reply[38] + (reply[39] << 8)))*1.0,
				((short) (reply[40] + (reply[41] << 8)))*1.0,
				((short) (reply[42] + (reply[43] << 8)))*1.0,
				((short) (reply[44] + (reply[45] << 8)))*1.0,
				((short) (reply[46] + (reply[47] << 8)))*1.0,
				((short) (reply[48] + (reply[49] << 8)))*1.0,
				((short) (reply[50] + (reply[51] << 8)))*1.0,
				((short) (reply[52] + (reply[53] << 8)))*1.0,
				((short) (reply[54] + (reply[55] << 8)))*1.0,
				((short) (reply[56] + (reply[57] << 8)))*1.0);
		}

		public int ReplyLength
		{
			get {
				return 58; // three Aiks each 29 2 bytes params = 29 * 2 * 3 = 174 total
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