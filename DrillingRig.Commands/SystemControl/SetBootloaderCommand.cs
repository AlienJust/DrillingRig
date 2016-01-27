using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.SystemControl {
	public class SetBootloaderCommand : IRrModbusCommandWithReply {
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
	}
}