using System;
using System.Linq;
using AlienJust.Support.Collections;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AinSettings {
	public class ReadAinSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IAinSettings>, IRrModbusCommandWithTestReply
	{
		private readonly byte _zeroBasedAinNumber;

		public ReadAinSettingsCommand(byte zeroBasedAinNumber)
		{
			_zeroBasedAinNumber = zeroBasedAinNumber;
		}

		public byte CommandCode => 0x8F;

		public string Name => "Чтение настроек АИН #" + (_zeroBasedAinNumber + 1);

		public byte[] Serialize()
		{
			return new[] { OneBasedAinNumber };
		}

		private byte OneBasedAinNumber => (byte)(_zeroBasedAinNumber + 1);

		public IAinSettings GetResult(byte[] reply)
		{
			if (reply[0] != OneBasedAinNumber) throw new Exception("неверный номер АИН в ответе, ожидался " + OneBasedAinNumber);

			// TODO: check if reply[0] is equal oneBasedAinNumber
			var replyWithoutAinNumber = reply.Skip(1).ToList();


			var bp52 = new BytesPair(replyWithoutAinNumber[104], replyWithoutAinNumber[105]);
			var np = bp52.First & 0x1F;
			var nimpFloorCode = bp52.First & 0xE0;
			var fanMode = AinTelemetryFanWorkmodeExtensions.FromIoBits(bp52.Second & 0x03);

			return new AinSettingsSimple(
				new BytesPair(replyWithoutAinNumber[0], replyWithoutAinNumber[1]),
				BytesPairToDoubleQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[2], replyWithoutAinNumber[3])),
				
				(int)(replyWithoutAinNumber[4] + (replyWithoutAinNumber[5] <<8) + (replyWithoutAinNumber[6] << 16) + (replyWithoutAinNumber[7] << 24)),
				(short)(replyWithoutAinNumber[8] + (replyWithoutAinNumber[9] <<8)),
				(short)(replyWithoutAinNumber[10] + (replyWithoutAinNumber[11] <<8)),
				(short)(replyWithoutAinNumber[12] + (replyWithoutAinNumber[13] <<8)),
				(short)(replyWithoutAinNumber[14] + (replyWithoutAinNumber[15] <<8)),
				(short)(replyWithoutAinNumber[16] + (replyWithoutAinNumber[17] <<8)),
				(short)(replyWithoutAinNumber[18] + (replyWithoutAinNumber[19] <<8)),

				(short)(replyWithoutAinNumber[20] + (replyWithoutAinNumber[21] << 8)),
				(short)(replyWithoutAinNumber[22] + (replyWithoutAinNumber[23] << 8)),

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

				// reserved 24:
				new BytesPair(replyWithoutAinNumber[48], replyWithoutAinNumber[49]),

				BytesPairToDoubleQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[50], replyWithoutAinNumber[51])),

				replyWithoutAinNumber[52] + (replyWithoutAinNumber[53] <<8) + (replyWithoutAinNumber[54] << 16) + (replyWithoutAinNumber[55] << 24),

				// reserved 28:
				new BytesPair(replyWithoutAinNumber[56], replyWithoutAinNumber[57]),

				// kpId:
				BytesPairToDoubleQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[58], replyWithoutAinNumber[59])),

				replyWithoutAinNumber[60] + (replyWithoutAinNumber[61] <<8) + (replyWithoutAinNumber[62] << 16) + (replyWithoutAinNumber[63] << 24),

				// reserverd 32:
				new BytesPair(replyWithoutAinNumber[64], replyWithoutAinNumber[65]),

				// kpIq:
				BytesPairToDoubleQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[66], replyWithoutAinNumber[67])),

				replyWithoutAinNumber[68] + (replyWithoutAinNumber[69] <<8) + (replyWithoutAinNumber[70] << 16) + (replyWithoutAinNumber[71] << 24),

				(short)(replyWithoutAinNumber[72] + (replyWithoutAinNumber[73] <<8)),
				(short)(replyWithoutAinNumber[74] + (replyWithoutAinNumber[75] <<8)),
				(short)(replyWithoutAinNumber[76] + (replyWithoutAinNumber[77] <<8)),

				(short)(replyWithoutAinNumber[78] + (replyWithoutAinNumber[79] << 8)),

				(short)(replyWithoutAinNumber[80] + (replyWithoutAinNumber[81] <<8)),
				(short)(replyWithoutAinNumber[82] + (replyWithoutAinNumber[83] <<8)),
				(short)(replyWithoutAinNumber[84] + (replyWithoutAinNumber[85] <<8)),
				(short)(replyWithoutAinNumber[86] + (replyWithoutAinNumber[87] <<8)),
				(short)(replyWithoutAinNumber[88] + (replyWithoutAinNumber[89] <<8)),
				(short)(replyWithoutAinNumber[90] + (replyWithoutAinNumber[91] <<8)),
				(short)(replyWithoutAinNumber[92] + (replyWithoutAinNumber[93] <<8)),
				(short)(replyWithoutAinNumber[94] + (replyWithoutAinNumber[95] <<8)),

				new BytesPair(replyWithoutAinNumber[96], replyWithoutAinNumber[97]),
				new BytesPair(replyWithoutAinNumber[98], replyWithoutAinNumber[99]),
				
				// reserverd 50:
				new BytesPair(replyWithoutAinNumber[100], replyWithoutAinNumber[101]),
				
				// reserverd 51:
				new BytesPair(replyWithoutAinNumber[102], replyWithoutAinNumber[103]),
				
				// Param52 (np and others):
				np,
				nimpFloorCode,
				fanMode,

				(short)(replyWithoutAinNumber[106] + (replyWithoutAinNumber[107] << 8)),

				(short)(replyWithoutAinNumber[108] + (replyWithoutAinNumber[109] <<8)),
				(short)(replyWithoutAinNumber[110] + (replyWithoutAinNumber[111] <<8)),
				(short)(replyWithoutAinNumber[112] + (replyWithoutAinNumber[113] <<8)),

				//status byte:
				(replyWithoutAinNumber[114] & 0x01) == 0x01,
				(replyWithoutAinNumber[114] & 0x02) == 0x02,
				(replyWithoutAinNumber[114] & 0x04) == 0x04
				);
				
		}

		public int ReplyLength => 1 + 114 + 1; // ain number + settings + ain link fault flags

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			result[0] = OneBasedAinNumber;
			return result;
		}
	}
}