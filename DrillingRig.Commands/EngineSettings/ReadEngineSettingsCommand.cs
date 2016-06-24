using System;
using System.Linq;
using DrillingRid.Commands.Contracts;
using DrillingRig.Commands.RtuModbus;

namespace DrillingRig.Commands.EngineSettings {
	public class ReadEngineSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IEngineSettings>, IRrModbusCommandWithTestReply
	{
		public byte CommandCode => 0x8C;

		public string Name => "Чтение настроек двигателя";

		public byte[] Serialize() {
			return new byte[0];
		}

		public IEngineSettings GetResult(byte[] reply) {
			return new EngineSettingsSimple {
				I2Tmax = new BytesQuad(reply[0], reply[1], reply[2], reply[3]).LowFirstUnsignedValue,
				Pnom = new BytesQuad(reply[4], reply[5], reply[6], reply[7]).LowFirstUnsignedValue,
				Icontinious = new BytesPair(reply[8], reply[9]).LowFirstUnsignedValue,
				Mnom = new BytesPair(reply[10], reply[11]).LowFirstUnsignedValue,
				ZeroF = new BytesPair(reply[12], reply[13]).LowFirstUnsignedValue
			};
		}

		public int ReplyLength => 14;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}