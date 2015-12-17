using System;
using System.Linq;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AinSettings {
	public class ReadAinSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IAinSettings>, IRrModbusCommandWithTestReply
	{
		private readonly byte _zeroBasedAinNumber;

		public ReadAinSettingsCommand(byte zeroBasedAinNumber)
		{
			_zeroBasedAinNumber = zeroBasedAinNumber;
		}

		public byte CommandCode
		{
			get { return 0x8F; } // TODO: 0x8E - write settings
		}

		public string Name
		{
			get { return "Чтение настроек АИН #" + (_zeroBasedAinNumber + 1); }
		}

		public byte[] Serialize()
		{
			return new[] {(byte)(_zeroBasedAinNumber + 1)};
		}

		private byte OneBasedAinNumber {
			get { return (byte)(_zeroBasedAinNumber + 1); }
		}

		public IAinSettings GetResult(byte[] reply)
		{
			if (reply[0] != OneBasedAinNumber) throw new Exception("неверный номер АИН в ответе, ожидался " + OneBasedAinNumber);

			// TODO: check if reply[0] is equal oneBasedAinNumber
			var replyWithoutAinNumber = reply.Skip(1).ToList();
			return new AinSettingsSimple(
				(int)(replyWithoutAinNumber[0] + (replyWithoutAinNumber[1] <<8) + (replyWithoutAinNumber[2] << 16) + (replyWithoutAinNumber[3] << 24)),
				(int)(replyWithoutAinNumber[4] + (replyWithoutAinNumber[5] <<8) + (replyWithoutAinNumber[6] << 16) + (replyWithoutAinNumber[7] << 24)),
				(short)(replyWithoutAinNumber[8] + (replyWithoutAinNumber[9] <<8)),
				(short)(replyWithoutAinNumber[10] + (replyWithoutAinNumber[11] <<8)),
				(short)(replyWithoutAinNumber[12] + (replyWithoutAinNumber[13] <<8)),
				(short)(replyWithoutAinNumber[14] + (replyWithoutAinNumber[15] <<8)),
				(short)(replyWithoutAinNumber[16] + (replyWithoutAinNumber[17] <<8)),
				(short)(replyWithoutAinNumber[18] + (replyWithoutAinNumber[19] <<8)),

				(short)(replyWithoutAinNumber[24] + (replyWithoutAinNumber[25] <<8)),
				(short)(replyWithoutAinNumber[26] + (replyWithoutAinNumber[27] <<8)),
				(short)(replyWithoutAinNumber[28] + (replyWithoutAinNumber[29] <<8)),
				(short)(replyWithoutAinNumber[30] + (replyWithoutAinNumber[31] <<8)),
				(short)(replyWithoutAinNumber[32] + (replyWithoutAinNumber[33] <<8)),
				(short)(replyWithoutAinNumber[34] + (replyWithoutAinNumber[35] <<8)),
				(short)(replyWithoutAinNumber[36] + (replyWithoutAinNumber[37] <<8)),
				(short)(replyWithoutAinNumber[38] + (replyWithoutAinNumber[39] <<8)),
				(short)(replyWithoutAinNumber[40] + (replyWithoutAinNumber[41] <<8)),
				(short)(replyWithoutAinNumber[42] + (replyWithoutAinNumber[43] <<8)),
				(short)(replyWithoutAinNumber[44] + (replyWithoutAinNumber[45] <<8)),
				(short)(replyWithoutAinNumber[46] + (replyWithoutAinNumber[47] <<8)),

				(int)(replyWithoutAinNumber[48] + (replyWithoutAinNumber[49] <<8) + (replyWithoutAinNumber[50] << 16) + (replyWithoutAinNumber[51] << 24)),
				(int)(replyWithoutAinNumber[52] + (replyWithoutAinNumber[53] <<8) + (replyWithoutAinNumber[54] << 16) + (replyWithoutAinNumber[55] << 24)),
				(int)(replyWithoutAinNumber[56] + (replyWithoutAinNumber[57] <<8) + (replyWithoutAinNumber[58] << 16) + (replyWithoutAinNumber[59] << 24)),
				(int)(replyWithoutAinNumber[60] + (replyWithoutAinNumber[61] <<8) + (replyWithoutAinNumber[62] << 16) + (replyWithoutAinNumber[63] << 24)),
				(int)(replyWithoutAinNumber[64] + (replyWithoutAinNumber[65] <<8) + (replyWithoutAinNumber[66] << 16) + (replyWithoutAinNumber[67] << 24)),
				(int)(replyWithoutAinNumber[68] + (replyWithoutAinNumber[69] <<8) + (replyWithoutAinNumber[70] << 16) + (replyWithoutAinNumber[71] << 24)),

				(short)(replyWithoutAinNumber[72] + (replyWithoutAinNumber[73] <<8)),
				(short)(replyWithoutAinNumber[74] + (replyWithoutAinNumber[75] <<8)),
				(short)(replyWithoutAinNumber[76] + (replyWithoutAinNumber[77] <<8)),

				(short)(replyWithoutAinNumber[80] + (replyWithoutAinNumber[81] <<8)),
				(short)(replyWithoutAinNumber[82] + (replyWithoutAinNumber[83] <<8)),
				(short)(replyWithoutAinNumber[84] + (replyWithoutAinNumber[85] <<8)),
				(short)(replyWithoutAinNumber[86] + (replyWithoutAinNumber[87] <<8)),
				(short)(replyWithoutAinNumber[88] + (replyWithoutAinNumber[89] <<8)),
				(short)(replyWithoutAinNumber[90] + (replyWithoutAinNumber[91] <<8)),
				(short)(replyWithoutAinNumber[92] + (replyWithoutAinNumber[93] <<8)),
				(short)(replyWithoutAinNumber[94] + (replyWithoutAinNumber[95] <<8)),

				(int)(replyWithoutAinNumber[96] + (replyWithoutAinNumber[97] <<8) + (replyWithoutAinNumber[98] << 16) + (replyWithoutAinNumber[99] << 24)),
				(int)(replyWithoutAinNumber[100] + (replyWithoutAinNumber[101] <<8) + (replyWithoutAinNumber[102] << 16) + (replyWithoutAinNumber[103] << 24)),

				(short)(replyWithoutAinNumber[104] + (replyWithoutAinNumber[105] <<8)),
				(short)(replyWithoutAinNumber[108] + (replyWithoutAinNumber[109] <<8)),
				(short)(replyWithoutAinNumber[110] + (replyWithoutAinNumber[111] <<8)),
				(short)(replyWithoutAinNumber[112] + (replyWithoutAinNumber[113] <<8))
				);
				
		}

		public int ReplyLength
		{
			get {
				return 1 + 114; // ain number + settings
			}
		}

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			result[0] = OneBasedAinNumber;
			return result;
		}
	}
}