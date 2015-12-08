using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.SystemControl {
	public class RestartCommand : IRrModbusCommandWithReply, IRrModbusCommandWithTestReply {
		public byte CommandCode {
			get { return 0x89; }
		}

		public string Name {
			get { return "Рестарт контроллера"; }
		}

		public byte[] Serialize() {
			return new byte[0];
		}


		public int ReplyLength {
			get { return 0; // three Aiks each: 1 byte - ainNumber + 32 * 2 bytes + 1 byte of Marat's status (flags)
			}
		}

		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}