using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.SystemControl {
	public class SetBootloaderCommand : IRrModbusCommandWithReply, IRrModbusCommandWithTestReply {
		public byte CommandCode => 0x88;

		public string Name => "Режим загрузчика (bootloader)";

		public byte[] Serialize() {
			return new byte[0];
		}

		public int ReplyLength => 0;
		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}