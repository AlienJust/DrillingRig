using System;
using System.Net;
using System.Net.NetworkInformation;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.BsEthernetSettings {
	public class ReadBsEthernetSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IBsEthernetSettings>, IRrModbusCommandWithTestReply
	{

		public byte CommandCode => 0x82;

		public string Name => "Чтение настроек БС-Ethernet";

		public byte[] Serialize() {
			return new byte[0];
		}

		public IBsEthernetSettings GetResult(byte[] reply)
		{
			//reply length = 4 + 4 + 4 + 4 + 6 + 1 + 2 = 25 bytes
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be " + ReplyLength);
			return new BsEthernetSettingsSimple(
				new PhysicalAddress(new[] { reply[0], reply[1], reply[2], reply[3], reply[4], reply[5] }),
				new IPAddress(new[] {reply[6], reply[7], reply[8], reply[9]}),
				new IPAddress(new[] {reply[10], reply[11], reply[12], reply[13]}),
				new IPAddress(new[] {reply[14], reply[15], reply[16], reply[17]}),
				new IPAddress(new[] {reply[18], reply[19], reply[20], reply[21]}),
				reply[22],
				reply[23],
				reply[24],
				FriquencyTransformerRoleExtension.FromByte(reply[25])
				);
		}

		public int ReplyLength => 26;

		public byte[] GetTestReply() {
			return new byte[] {
				0xAB, 0xCD, 0xDF, 1, 2, 3,
				192, 168, 0, 123,
				255, 255, 255, 0,
				192, 168, 0, 1,
				8, 8, 4, 4,
				7,
				2,
				5,
				1
			};
		}
	}
}