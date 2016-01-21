using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.SystemControl {
	public class SetBootloaderCommand : IRrModbusCommandWithReply, IRrModbusCommandWithTestReply {
		public byte CommandCode {
			get { return 0x88; }
		}

		public string Name {
			get { return "Режим загрузчика (bootloader)"; }
		}

		public byte[] Serialize() {
			return new byte[0];
		}

		public int ReplyLength {
			get { return 0; }
		}

		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}