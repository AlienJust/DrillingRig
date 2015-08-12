using System;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.BsEthernetNominals {
	public class ReadBsEthernetNominalsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IBsEthernetNominals>, IRrModbusCommandWithTestReply
	{

		public byte CommandCode
		{
			get { return 0x84; }
		}

		public string Name
		{
			get { return "Чтение номинальных значений БС-Ethernet"; }
		}

		public byte[] Serialize() {
			return new byte[0];
		}

		public IBsEthernetNominals GetResult(byte[] reply)
		{
			//reply length = 14 * 2 = 28 bytes
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be " + ReplyLength);

			return new BsEthernetNominalsSimple(
				(short) (reply[0] + (reply[1] << 8)), 
				(short) (reply[2] + (reply[3] << 8)), 
				(short) (reply[4] + (reply[5] << 8)), 
				(short) (reply[6] + (reply[7] << 8)), 
				(short) (reply[8] + (reply[9] << 8)), 
				(short) (reply[10] + (reply[11] << 8)), 
				(short) (reply[12] + (reply[13] << 8)), 
				(short) (reply[14] + (reply[15] << 8)), 
				(short) (reply[16] + (reply[17] << 8)), 
				(short) (reply[18] + (reply[19] << 8)), 
				(short) (reply[20] + (reply[21] << 8)), 
				(short) (reply[22] + (reply[23] << 8)), 
				(short) (reply[24] + (reply[25] << 8)), 
				(short) (reply[26] + (reply[27] << 8)));
		}

		public int ReplyLength
		{
			get { return 28; }
		}

		public byte[] GetTestReply() {
			var rnd = new Random();
			var reply = new byte[ReplyLength];
			rnd.NextBytes(reply);
			return reply;
		}
	}
}