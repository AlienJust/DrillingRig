using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.SystemControl {
	public class RestartCommand : IRrModbusCommandWithReply, IRrModbusCommandWithTestReply {
		public byte CommandCode => 0x89;

		public string Name => "Рестарт контроллера";

		public byte[] Serialize() {
			return new byte[0];
		}

		public int ReplyLength => 0;

		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}