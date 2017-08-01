using System;
using System.Linq;
using AlienJust.Support.Collections;
using AlienJust.Support.Numeric.Bits;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AinSettings {
	public class ReadAinSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IAinSettings>, IRrModbusCommandWithTestReply {
		private readonly byte _zeroBasedAinNumber;

		public ReadAinSettingsCommand(byte zeroBasedAinNumber) {
			_zeroBasedAinNumber = zeroBasedAinNumber;
		}

		public byte CommandCode => 0x8F;

		public string Name => "Чтение настроек АИН #" + (_zeroBasedAinNumber + 1);

		public byte[] Serialize() {
			return new[] { OneBasedAinNumber };
		}

		private byte OneBasedAinNumber => (byte)(_zeroBasedAinNumber + 1);

		public IAinSettings GetResult(byte[] reply) {
			if (reply[0] != OneBasedAinNumber) throw new Exception("неверный номер АИН в ответе, ожидался " + OneBasedAinNumber);

			// TODO: check if reply[0] is equal oneBasedAinNumber
			var replyWithoutAinNumber = reply.Skip(1).ToList();


			var bp52 = new BytesPair(replyWithoutAinNumber[104], replyWithoutAinNumber[105]);
			//Console.WriteLine("<<READ>> NPRM = " + bp52.First.ToString("X2") + bp52.Second.ToString("X2"));
			var np = bp52.First & 0x1F;
			var nimpFloorCode = (bp52.First & 0xE0) >> 5;
			var fanMode = AinTelemetryFanWorkmodeExtensions.FromIoBits(bp52.Second & 0x03);
			var directCurrentMagnetization = bp52.Second.GetBit(3);

			return new AinSettingsSimple(
				reserved00: new BytesPair(replyWithoutAinNumber[0], replyWithoutAinNumber[1]),
				kpW: BytesPairToDecimalQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[2], replyWithoutAinNumber[3])),
				// kiW
				kiW: (replyWithoutAinNumber[4] + (replyWithoutAinNumber[5] << 8) + (replyWithoutAinNumber[6] << 16) + (replyWithoutAinNumber[7] << 24)) / 16777216.0m,

				fiNom: new BytesPair(replyWithoutAinNumber[8], replyWithoutAinNumber[9]).LowFirstSignedValue / 1000.0m,
				imax: new BytesPair(replyWithoutAinNumber[10], replyWithoutAinNumber[11]).LowFirstSignedValue,
				udcMax: new BytesPair(replyWithoutAinNumber[12], replyWithoutAinNumber[13]).LowFirstSignedValue,
				udcMin: new BytesPair(replyWithoutAinNumber[14], replyWithoutAinNumber[15]).LowFirstSignedValue,

				// Fnom:
				fnom: new BytesPair(replyWithoutAinNumber[16], replyWithoutAinNumber[17]).LowFirstSignedValue / 10.0m,
				// Fmax:
				fmax: new BytesPair(replyWithoutAinNumber[18], replyWithoutAinNumber[19]).LowFirstSignedValue / 10.0m,
				// DflLim:
				dflLim: new BytesPair(replyWithoutAinNumber[20], replyWithoutAinNumber[21]).LowFirstSignedValue / 1000.0m,
				flMinMin: new BytesPair(replyWithoutAinNumber[22], replyWithoutAinNumber[23]).LowFirstSignedValue / 1000.0m,

				ioutMax: new BytesPair(replyWithoutAinNumber[24], replyWithoutAinNumber[25]).LowFirstSignedValue,
				fiMin: new BytesPair(replyWithoutAinNumber[26], replyWithoutAinNumber[27]).LowFirstSignedValue / 1000.0m,

				dacCh: new BytesPair(replyWithoutAinNumber[28], replyWithoutAinNumber[29]).LowFirstUnsignedValue,
				imcw: new BytesPair(replyWithoutAinNumber[30], replyWithoutAinNumber[31]).LowFirstUnsignedValue,

				ia0: new BytesPair(replyWithoutAinNumber[32], replyWithoutAinNumber[33]).LowFirstSignedValue,
				ib0: new BytesPair(replyWithoutAinNumber[34], replyWithoutAinNumber[35]).LowFirstSignedValue,
				ic0: new BytesPair(replyWithoutAinNumber[36], replyWithoutAinNumber[37]).LowFirstSignedValue,
				udc0: new BytesPair(replyWithoutAinNumber[38], replyWithoutAinNumber[39]).LowFirstSignedValue,

				// TauR:
				tauR: new BytesPair(replyWithoutAinNumber[40],replyWithoutAinNumber[41]).LowFirstSignedValue / 10000.0m,
				// Lm:
				lm: new BytesPair(replyWithoutAinNumber[42],replyWithoutAinNumber[43]).LowFirstSignedValue / 100000.0m,
				// Lsl:
				lsl: new BytesPair(replyWithoutAinNumber[44], replyWithoutAinNumber[45]).LowFirstSignedValue / 1000000.0m,
				// Lrl:
				lrl: new BytesPair(replyWithoutAinNumber[46], replyWithoutAinNumber[47]).LowFirstSignedValue / 1000000.0m,

				// reserved 24:
				reserved24: new BytesPair(replyWithoutAinNumber[48], replyWithoutAinNumber[49]),

				kpFi: BytesPairToDecimalQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[50], replyWithoutAinNumber[51])),
				// kiFi:
				kiFi: (replyWithoutAinNumber[52] + (replyWithoutAinNumber[53] << 8) + (replyWithoutAinNumber[54] << 16) + (replyWithoutAinNumber[55] << 24)) / 16777216.0m,

				// reserved 28:
				reserved28: new BytesPair(replyWithoutAinNumber[56], replyWithoutAinNumber[57]),

				// kpId:
				kpId: BytesPairToDecimalQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[58], replyWithoutAinNumber[59])),
				// kiId:
				kiId: (replyWithoutAinNumber[60] + (replyWithoutAinNumber[61] << 8) + (replyWithoutAinNumber[62] << 16) + (replyWithoutAinNumber[63] << 24)) / 16777216.0m,

				// reserverd 32:
				reserved32: new BytesPair(replyWithoutAinNumber[64], replyWithoutAinNumber[65]),

				// kpIq:
				kpIq: BytesPairToDecimalQ8Converter.ConvertBytesPairToDoubleQ8(new BytesPair(replyWithoutAinNumber[66], replyWithoutAinNumber[67])),
				// kiIq:
				kiIq: (replyWithoutAinNumber[68] + (replyWithoutAinNumber[69] << 8) + (replyWithoutAinNumber[70] << 16) + (replyWithoutAinNumber[71] << 24)) / 16777216.0m,

				accDfDt: new BytesPair(replyWithoutAinNumber[72], replyWithoutAinNumber[73]).LowFirstSignedValue * 0.1m,
				decDfDt: new BytesPair(replyWithoutAinNumber[74], replyWithoutAinNumber[75]).LowFirstSignedValue * 0.1m,
				// Unom:
				unom: new BytesPair(replyWithoutAinNumber[76],replyWithoutAinNumber[77]).LowFirstSignedValue / (decimal)Math.Sqrt(2.0),
				// TauFlLim:
				tauFlLim: new BytesPair(replyWithoutAinNumber[78],replyWithoutAinNumber[79]).LowFirstSignedValue / 10000.0m,
				// Rs:
				rs: new BytesPair(replyWithoutAinNumber[80], replyWithoutAinNumber[81]).LowFirstSignedValue / 10000.0m,
				// fmin:
				fmin: new BytesPair(replyWithoutAinNumber[82], replyWithoutAinNumber[83]).LowFirstSignedValue / 10.0m,
				tauM: new BytesPair(replyWithoutAinNumber[84], replyWithoutAinNumber[85]).LowFirstSignedValue / 10000.0m,
				tauF: new BytesPair(replyWithoutAinNumber[86], replyWithoutAinNumber[87]).LowFirstSignedValue / 10000.0m,
				tauFSet: new BytesPair(replyWithoutAinNumber[88], replyWithoutAinNumber[89]).LowFirstSignedValue / 10000.0m,
				tauFi: new BytesPair(replyWithoutAinNumber[90], replyWithoutAinNumber[91]).LowFirstSignedValue / 10000.0m,
				idSetMin: new BytesPair(replyWithoutAinNumber[92], replyWithoutAinNumber[93]).LowFirstSignedValue,
				idSetMax: new BytesPair(replyWithoutAinNumber[94], replyWithoutAinNumber[95]).LowFirstSignedValue,

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
				directCurrentMagnetization: directCurrentMagnetization,

				umodThr: new BytesPair(replyWithoutAinNumber[106], replyWithoutAinNumber[107]).LowFirstSignedValue / 1000.0m,

				emdecDfdt: new BytesPair(replyWithoutAinNumber[108], replyWithoutAinNumber[109]).LowFirstSignedValue / 10.0m,
				textMax: new BytesPair(replyWithoutAinNumber[110], replyWithoutAinNumber[111]).LowFirstSignedValue,
				toHl: new BytesPair(replyWithoutAinNumber[112], replyWithoutAinNumber[113]).LowFirstSignedValue,

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