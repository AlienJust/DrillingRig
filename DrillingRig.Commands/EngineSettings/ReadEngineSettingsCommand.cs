using System;
using AlienJust.Support.Collections;
using DrillingRid.Commands.Contracts;

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
				Inom = new BytesPair(reply[0], reply[1]).LowFirstUnsignedValue,
				Nnom = new BytesPair(reply[2], reply[3]).LowFirstUnsignedValue,
				Nmax = new BytesPair(reply[4], reply[5]).LowFirstUnsignedValue,
				Pnom = new BytesPair(reply[6], reply[7]).LowFirstUnsignedValue,
				CosFi = new BytesPair(reply[8], reply[9]).LowFirstUnsignedValue,
				Eff = new BytesPair(reply[10], reply[11]).LowFirstUnsignedValue,
				Mass = new BytesPair(reply[12], reply[13]).LowFirstUnsignedValue,
				MmM = new BytesPair(reply[14], reply[15]).LowFirstUnsignedValue,
				Height = new BytesPair(reply[16], reply[17]).LowFirstUnsignedValue,

				I2Tmax = new BytesQuad(reply[18], reply[19], reply[20], reply[21]).LowFirstUnsignedValue,
				Icontinious = new BytesPair(reply[22], reply[23]).LowFirstUnsignedValue,
				ZeroF = new BytesPair(reply[24], reply[25]).LowFirstUnsignedValue
				// 26, 27 is KS
			};
		}

		public int ReplyLength => 28;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}