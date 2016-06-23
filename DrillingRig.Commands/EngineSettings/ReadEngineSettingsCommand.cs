using System;
using System.Linq;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.EngineSettings {
	public class ReadEngineSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IEngineSettings>, IRrModbusCommandWithTestReply
	{
		public byte CommandCode => 0x8C;

		public string Name => "Чтение настроек двигателя";

		public byte[] Serialize() {
			return new byte[0];
		}

		public IEngineSettings GetResult(byte[] reply)
		{
			var replyWithoutAinNumber = reply.Skip(1).ToList();
			return new EngineSettingsSimple {
				Icontinious = (ushort) (reply[0] + (reply[1] << 8)),
				I2Tmax = (uint) (replyWithoutAinNumber[2] + (replyWithoutAinNumber[3] << 8) + (replyWithoutAinNumber[4] << 16) + (replyWithoutAinNumber[5] << 24)),
				Mnom = (ushort) (reply[6] + (reply[7] << 8)),
				Pnom = (uint) (replyWithoutAinNumber[8] + (replyWithoutAinNumber[9] << 8) + (replyWithoutAinNumber[10] << 16) + (replyWithoutAinNumber[11] << 24)),
				ZeroF = (ushort) (reply[12] + (reply[13] << 8)),
				Ks = reply[14]};
		}

		public int ReplyLength => 15;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}