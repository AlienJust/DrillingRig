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
			Console.WriteLine("<<READ>> NPRM = " + bp52.First.ToString("X2") + bp52.Second.ToString("X2"));
			var np = bp52.First & 0x1F;
			var nimpFloorCode = (bp52.First & 0xE0) >> 5;
			var fanMode = AinTelemetryFanWorkmodeExtensions.FromIoBits(bp52.Second & 0x03);

			return new AinSettingsSimple(
				reserved00:new BytesPair(replyWithoutAinNumber[0], replyWithoutAinNumber[1]),
				kpW: BytesPairToDoubleQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[2], replyWithoutAinNumber[3])),
				// kiW
				kiW: (replyWithoutAinNumber[4] + (replyWithoutAinNumber[5] <<8) + (replyWithoutAinNumber[6] << 16) + (replyWithoutAinNumber[7] << 24)) / 16777216.0,

				fiNom: (short)(replyWithoutAinNumber[8] + (replyWithoutAinNumber[9] <<8)),
				imax: (short)(replyWithoutAinNumber[10] + (replyWithoutAinNumber[11] <<8)),
				udcMax: (short)(replyWithoutAinNumber[12] + (replyWithoutAinNumber[13] <<8)),
				udcMin: (short)(replyWithoutAinNumber[14] + (replyWithoutAinNumber[15] <<8)),
				
				// Fnom:
				fnom: (replyWithoutAinNumber[16] + (replyWithoutAinNumber[17] <<8)) / 10.0,
				// Fmax:
				fmax: (replyWithoutAinNumber[18] + (replyWithoutAinNumber[19] <<8)) / 10.0,
				// DflLim:
				dflLim: (replyWithoutAinNumber[20] + (replyWithoutAinNumber[21] << 8)) / 1000.0,

				flMinMin: (short)(replyWithoutAinNumber[22] + (replyWithoutAinNumber[23] << 8)),

				ioutMax: (short)(replyWithoutAinNumber[24] + (replyWithoutAinNumber[25] <<8)),
				fiMin: (short)(replyWithoutAinNumber[26] + (replyWithoutAinNumber[27] <<8)),
				dacCh: (short)(replyWithoutAinNumber[28] + (replyWithoutAinNumber[29] <<8)),
				imcw: (short)(replyWithoutAinNumber[30] + (replyWithoutAinNumber[31] <<8)),
				ia0: (short)(replyWithoutAinNumber[32] + (replyWithoutAinNumber[33] <<8)),
				ib0: (short)(replyWithoutAinNumber[34] + (replyWithoutAinNumber[35] <<8)),
				ic0: (short)(replyWithoutAinNumber[36] + (replyWithoutAinNumber[37] <<8)),
				udc0: (short)(replyWithoutAinNumber[38] + (replyWithoutAinNumber[39] <<8)),

				// TauR:
				tauR: (replyWithoutAinNumber[40] + (replyWithoutAinNumber[41] <<8)) / 10000.0,
				// Lm:
				lm: (replyWithoutAinNumber[42] + (replyWithoutAinNumber[43] <<8)) / 100000.0,
				// Lsl:
				lsl: (replyWithoutAinNumber[44] + (replyWithoutAinNumber[45] <<8)) / 1000000.0,
				// Lrl:
				lrl: (replyWithoutAinNumber[46] + (replyWithoutAinNumber[47] <<8)) / 1000000.0,

				// reserved 24:
				reserved24: new BytesPair(replyWithoutAinNumber[48], replyWithoutAinNumber[49]),

				kpFi: BytesPairToDoubleQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[50], replyWithoutAinNumber[51])),
				// kiFi:
				kiFi: (replyWithoutAinNumber[52] + (replyWithoutAinNumber[53] <<8) + (replyWithoutAinNumber[54] << 16) + (replyWithoutAinNumber[55] << 24)) / 16777216.0, 

				// reserved 28:
				reserved28: new BytesPair(replyWithoutAinNumber[56], replyWithoutAinNumber[57]),

				// kpId:
				kpId: BytesPairToDoubleQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[58], replyWithoutAinNumber[59])),
				// kiId:
				kiId: (replyWithoutAinNumber[60] + (replyWithoutAinNumber[61] <<8) + (replyWithoutAinNumber[62] << 16) + (replyWithoutAinNumber[63] << 24)) / 16777216.0,

				// reserverd 32:
				reserved32: new BytesPair(replyWithoutAinNumber[64], replyWithoutAinNumber[65]),

				// kpIq:
				kpIq: BytesPairToDoubleQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[66], replyWithoutAinNumber[67])),
				// kiIq:
				kiIq: (replyWithoutAinNumber[68] + (replyWithoutAinNumber[69] <<8) + (replyWithoutAinNumber[70] << 16) + (replyWithoutAinNumber[71] << 24)) / 16777216.0,

				accDfDt: (short)(replyWithoutAinNumber[72] + (replyWithoutAinNumber[73] <<8)),
				decDfDt: (short)(replyWithoutAinNumber[74] + (replyWithoutAinNumber[75] <<8)),
				// Unom:
				unom: (replyWithoutAinNumber[76] + (replyWithoutAinNumber[77] <<8)) / Math.Sqrt(2.0),
				// TauFlLim:
				tauFlLim: (replyWithoutAinNumber[78] + (replyWithoutAinNumber[79] << 8)) / 1000.0,
				// Rs:
				rs: (replyWithoutAinNumber[80] + (replyWithoutAinNumber[81] <<8)) / 10000.0,
				// fmin:
				fmin: (replyWithoutAinNumber[82] + (replyWithoutAinNumber[83] <<8)) / 10.0,
				tauM: (short)(replyWithoutAinNumber[84] + (replyWithoutAinNumber[85] <<8)),
				tauF: (short)(replyWithoutAinNumber[86] + (replyWithoutAinNumber[87] <<8)),
				tauFSet: (short)(replyWithoutAinNumber[88] + (replyWithoutAinNumber[89] <<8)),
				tauFi: (short)(replyWithoutAinNumber[90] + (replyWithoutAinNumber[91] <<8)),
				idSetMin: (short)(replyWithoutAinNumber[92] + (replyWithoutAinNumber[93] <<8)),
				idSetMax: (short)(replyWithoutAinNumber[94] + (replyWithoutAinNumber[95] <<8)),

				uchMin: new BytesPair(replyWithoutAinNumber[96], replyWithoutAinNumber[97]),
				uchMax: new BytesPair(replyWithoutAinNumber[98], replyWithoutAinNumber[99]),
				
				// reserverd 50:
				reserved50: new BytesPair(replyWithoutAinNumber[100], replyWithoutAinNumber[101]),
				
				// reserverd 51:
				reserved51: new BytesPair(replyWithoutAinNumber[102], replyWithoutAinNumber[103]),
				
				// Param52 (np and others):
				np: np,
				nimpFloorCode: nimpFloorCode,
				fanMode: fanMode,

				umodThr: (replyWithoutAinNumber[106] + (replyWithoutAinNumber[107] << 8)) / 1000.0,

				emdecDfdt: (short)(replyWithoutAinNumber[108] + (replyWithoutAinNumber[109] <<8)),
				textMax: (short)(replyWithoutAinNumber[110] + (replyWithoutAinNumber[111] <<8)),
				toHl: (short)(replyWithoutAinNumber[112] + (replyWithoutAinNumber[113] <<8)),

				// Status byte:
				ain1LinkFault: (replyWithoutAinNumber[114] & 0x01) == 0x01,
				ain2LinkFault: (replyWithoutAinNumber[114] & 0x02) == 0x02,
				ain3LinkFault: (replyWithoutAinNumber[114] & 0x04) == 0x04
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