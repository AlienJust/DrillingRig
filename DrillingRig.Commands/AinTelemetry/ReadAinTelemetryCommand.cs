using System;
using System.Linq;
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

			var oldReply = reply.Skip(4).ToList();

			return new AinTelemetrySimple(
				EngineStateExtensions.GetStateFromUshort((ushort) (reply[1] + (reply[2] << 8))),
				FaultStateExtensions.GetStateFromUshort((ushort) (reply[3] + (reply[4] << 8))),

				((short) (oldReply[1] + (oldReply[2] << 8)))*0.1,
				((short) (oldReply[3] + (oldReply[4] << 8)))*1.0,
				((short) (oldReply[5] + (oldReply[6] << 8)))*1.0,
				((short) (oldReply[7] + (oldReply[8] << 8)))*1.0,
				((short) (oldReply[9] + (oldReply[10] << 8)))*1.0,
				((short) (oldReply[11] + (oldReply[12] << 8)))*1.0,
				((short) (oldReply[13] + (oldReply[14] << 8)))*1.0/256.0,
				((short) (oldReply[15] + (oldReply[16] << 8)))*1.0/256.0,
				((short) (oldReply[17] + (oldReply[18] << 8)))*0.1,
				((short) (oldReply[19] + (oldReply[20] << 8)))*1.0,
				((short) (oldReply[21] + (oldReply[22] << 8)))*1.0,
				((short) (oldReply[23] + (oldReply[24] << 8)))*1.0,
				((short) (oldReply[25] + (oldReply[26] << 8)))*1.0,
				((short) (oldReply[27] + (oldReply[28] << 8)))*1.0,

				ModeSetRunModeBits12Extensions.FromInt((oldReply[29] & 0x03)),

				((oldReply[29] & 0x04) == 0x04),

				((oldReply[31] & 0x01) == 0x01),
				((oldReply[31] & 0x02) == 0x02),
				((oldReply[31] & 0x04) == 0x04),
				((oldReply[31] & 0x08) == 0x08),
				((oldReply[31] & 0x10) == 0x10),
				((oldReply[31] & 0x20) == 0x20),

				((oldReply[31] & 0x40) == 0x40),
				((oldReply[31] & 0x80) == 0x80),

				((oldReply[32] & 0x01) == 0x01),
				((oldReply[32] & 0x10) == 0x10),
				((oldReply[32] & 0x20) == 0x20),

				((short) (oldReply[33] + (oldReply[34] << 8)))*1.0,
				((short) (oldReply[35] + (oldReply[36] << 8)))*1.0,
				((short) (oldReply[37] + (oldReply[38] << 8)))*1.0,
				((short) (oldReply[39] + (oldReply[40] << 8)))*1.0,
				((short) (oldReply[41] + (oldReply[42] << 8)))*1.0,
				((short) (oldReply[43] + (oldReply[44] << 8)))*1.0,
				((short) (oldReply[45] + (oldReply[46] << 8)))*1.0,

				// Text (External temperature)
				((short) (oldReply[47] + (oldReply[48] << 8)))*1.0,

				((short) (oldReply[49] + (oldReply[50] << 8)))*1.0,
				((short) (oldReply[51] + (oldReply[52] << 8)))*1.0,
				((short) (oldReply[53] + (oldReply[54] << 8)))*1.0,
				((short) (oldReply[55] + (oldReply[56] << 8)))*1.0,
				((short) (oldReply[57] + (oldReply[58] << 8)))*1.0,
				((short) (oldReply[59] + (oldReply[60] << 8)))*1.0,

				//61-62 - AUX1
				//63-64 - AUX2
				//65-66 - PVER
				//67-68 - PVDATE

				//status byte:
				(oldReply[69] & 0x01) == 0x01,
				(oldReply[69] & 0x02) == 0x02,
				(oldReply[69] & 0x04) == 0x04);
		}

		public int ReplyLength
		{
			get {
				return 74;
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