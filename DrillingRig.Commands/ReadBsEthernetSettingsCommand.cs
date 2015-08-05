using System;
using System.Net;
using System.Net.NetworkInformation;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands {
	public class ReadBsEthernetSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IReadBsEthernetSettingsResult>, IRrModbusCommandWithTestReply
	{

		public byte CommandCode
		{
			get { return 0x82; }
		}

		public string Name
		{
			get { return "Чтение настроек БС-Ethernet"; }
		}

		public byte[] Serialize() {
			return new byte[0];
		}

		public IReadBsEthernetSettingsResult GetResult(byte[] reply)
		{
			//reply length = 4 + 4 + 4 + 4 + 6 + 1 + 2 = 25 bytes
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be " + ReplyLength);
			return new ReadBsEthernetSettingsResultSimple(
				new IPAddress(new[] {reply[0], reply[1], reply[2], reply[3]}),
				new IPAddress(new[] {reply[4], reply[5], reply[6], reply[7]}),
				new IPAddress(new[] {reply[8], reply[9], reply[10], reply[11]}),
				new IPAddress(new[] {reply[12], reply[13], reply[14], reply[15]}),
				new PhysicalAddress(new[] {reply[16], reply[17], reply[18], reply[19], reply[20], reply[21]}),
				reply[22],
				(ushort) (reply[23]*256 + reply[24]));
		}

		public int ReplyLength
		{
			get { return 25; }
		}

		public byte[] GetTestReply() {
			return new byte[] {
				192, 168, 0, 123,
				255, 255, 255, 0,
				192, 168, 0, 1,
				8, 8, 4, 4,
				0xAB, 0xCD, 0xDF, 1, 2, 3,
				7,
				2, 1
			};
		}
	}
}