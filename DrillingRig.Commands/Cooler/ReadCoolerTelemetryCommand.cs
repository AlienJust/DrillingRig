using System;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.Cooler {
	/// <summary>
	/// Команда чтения телеметрии выпрямителей
	/// </summary>
	public class ReadCoolerTelemetryCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ICoolerTelemetry>,
			IRrModbusCommandWithTestReply {
		public byte CommandCode => 0x91;

		public string Name => "Чтение телеметрии охладителя";

		public byte[] Serialize() {
			return new byte[0];
		}

		public ICoolerTelemetry GetResult(byte[] reply) {
			if (reply.Length != ReplyLength) throw new Exception("неверная длина ответа");
			var result = new CoolerTelemetrySimple(
					(ushort)(reply[0] + (reply[1] << 8)),
					(short)(reply[2] + (reply[3] << 8)),
					(short)(reply[4] + (reply[5] << 8)),
					(short)(reply[6] + (reply[7] << 8)),
					(ushort)(reply[8] + (reply[9] << 8)),
					(ushort)(reply[10] + (reply[11] << 8)));
			return result;
		}

		public int ReplyLength => 12;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}