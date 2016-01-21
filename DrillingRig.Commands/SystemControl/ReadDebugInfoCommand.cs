using System;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.SystemControl {
	public class ReadDebugInfoCommand : IRrModbusCommandWithReply, IRrModbusCommandWithTestReply
	{
		public byte CommandCode {
			get { return 0x8A; }
		}

		public string Name {
			get { return "Чтение отладочной информации"; }
		}

		public byte[] Serialize() {
			return new byte[0];
		}

		public int ReplyLength
		{
			get { return 32; }
		}

		public byte[] GetTestReply()
		{
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}