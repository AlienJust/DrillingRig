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
				Pnom = new BytesQuad(reply[0], reply[1], reply[2], reply[3]).LowFirstUnsignedValue / 1000.0,
				I2Tmax = new BytesQuad(reply[4], reply[5], reply[6], reply[7]).LowFirstUnsignedValue,
				Icontinious = new BytesPair(reply[8], reply[9]).LowFirstUnsignedValue,

				Inom = new BytesPair(reply[10], reply[11]).LowFirstUnsignedValue,
				Nnom = new BytesPair(reply[12], reply[13]).LowFirstUnsignedValue,
				Nmax = new BytesPair(reply[14], reply[15]).LowFirstUnsignedValue,
			

				CosFi = new BytesPair(reply[16], reply[17]).LowFirstUnsignedValue / 100.0,
				Eff = new BytesPair(reply[18], reply[19]).LowFirstUnsignedValue / 10.0,
				Mass = new BytesPair(reply[20], reply[21]).LowFirstUnsignedValue,
				MmM = new BytesPair(reply[22], reply[23]).LowFirstUnsignedValue,
				Height = new BytesPair(reply[24], reply[25]).LowFirstUnsignedValue,
				ZeroF = new BytesPair(reply[26], reply[27]).LowFirstUnsignedValue
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