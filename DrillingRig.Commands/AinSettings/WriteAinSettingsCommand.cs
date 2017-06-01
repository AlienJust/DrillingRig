using System;
using AlienJust.Support.Collections;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AinSettings {
	public class WriteAinSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<bool>, IRrModbusCommandWithTestReply {
		private readonly byte _zeroBasedAinNumber;
		private readonly IAinSettings _settings;

		public WriteAinSettingsCommand(byte zeroBasedAinNumber, IAinSettings settings) {
			_zeroBasedAinNumber = zeroBasedAinNumber;
			_settings = settings;
		}

		public byte CommandCode => 0x8E;

		public string Name => "Запись настроек АИН #" + (_zeroBasedAinNumber + 1);

		public byte[] Serialize() {
			var settingsSerialized = new byte[114];
			settingsSerialized[0] = _settings.Reserved00.First;
			settingsSerialized[1] = _settings.Reserved00.Second;

			var bpKpW = BytesPairToDoubleQ8Converter.ConvertDoubleToBytesPairQ8(_settings.KpW);
			settingsSerialized[2] = bpKpW.First;
			settingsSerialized[3] = bpKpW.Second;

			settingsSerialized.SerializeIntLowFirst(4, (int)(_settings.KiW * 16777216.0));
			settingsSerialized.SerializeShortLowFirst(8, _settings.FiNom);
			settingsSerialized.SerializeShortLowFirst(10, _settings.Imax);
			settingsSerialized.SerializeShortLowFirst(12, _settings.UdcMax);
			settingsSerialized.SerializeShortLowFirst(14, _settings.UdcMin);

			settingsSerialized.SerializeUshortLowFirst(16, (ushort)(_settings.Fnom * 10.0));
			settingsSerialized.SerializeUshortLowFirst(18, (ushort)(_settings.Fmax * 10.0));

			settingsSerialized.SerializeShortLowFirst(20, (short)(_settings.DflLim * 1000.0));
			settingsSerialized.SerializeShortLowFirst(22, _settings.FlMinMin);

			settingsSerialized.SerializeShortLowFirst(24, _settings.IoutMax);
			settingsSerialized.SerializeShortLowFirst(26, _settings.FiMin);
			settingsSerialized.SerializeShortLowFirst(28, _settings.DacCh);
			settingsSerialized.SerializeShortLowFirst(30, _settings.Imcw);
			settingsSerialized.SerializeShortLowFirst(32, _settings.Ia0);
			settingsSerialized.SerializeShortLowFirst(34, _settings.Ib0);
			settingsSerialized.SerializeShortLowFirst(36, _settings.Ic0);
			settingsSerialized.SerializeShortLowFirst(38, _settings.Udc0);
			settingsSerialized.SerializeShortLowFirst(40, (short)(_settings.TauR * 10000.0));
			settingsSerialized.SerializeShortLowFirst(42, (short)(_settings.Lm * 100000.0));
			settingsSerialized.SerializeShortLowFirst(44, (short)(_settings.Lsl * 1000000.0));
			settingsSerialized.SerializeShortLowFirst(46, (short)(_settings.Lrl * 1000000.0));

			settingsSerialized[48] = _settings.Reserved24.First;
			settingsSerialized[49] = _settings.Reserved24.Second;

			var bpKpFi = BytesPairToDoubleQ8Converter.ConvertDoubleToBytesPairQ8(_settings.KpFi);
			settingsSerialized[50] = bpKpFi.First;
			settingsSerialized[51] = bpKpFi.Second;

			settingsSerialized.SerializeIntLowFirst(52, (int)(_settings.KiFi * 16777216.0));

			settingsSerialized[56] = _settings.Reserved28.First;
			settingsSerialized[57] = _settings.Reserved28.Second;

			var bpKpId = BytesPairToDoubleQ8Converter.ConvertDoubleToBytesPairQ8(_settings.KpId);
			settingsSerialized[58] = bpKpId.First;
			settingsSerialized[59] = bpKpId.Second;

			settingsSerialized.SerializeIntLowFirst(60, (int)(_settings.KiId * 16777216.0));


			settingsSerialized[64] = _settings.Reserved32.First;
			settingsSerialized[65] = _settings.Reserved32.Second;


			var bpKpIq = BytesPairToDoubleQ8Converter.ConvertDoubleToBytesPairQ8(_settings.KpIq);
			settingsSerialized[66] = bpKpIq.First;
			settingsSerialized[67] = bpKpIq.Second;

			settingsSerialized.SerializeIntLowFirst(68, (int)(_settings.KiIq * 16777216.0));

			settingsSerialized.SerializeShortLowFirst(72, _settings.AccDfDt);
			settingsSerialized.SerializeShortLowFirst(74, _settings.DecDfDt);
			settingsSerialized.SerializeUshortLowFirst(76, (ushort) Math.Round(_settings.Unom * Math.Sqrt(2.0)));

			settingsSerialized.SerializeShortLowFirst(78, (short)(_settings.TauFlLim * 1000.0));

			settingsSerialized.SerializeUshortLowFirst(80, (ushort)(_settings.Rs * 10000.0));

			settingsSerialized.SerializeUshortLowFirst(82, (ushort)(_settings.Fmin * 10.0));
			settingsSerialized.SerializeShortLowFirst(84, _settings.TauM);
			settingsSerialized.SerializeShortLowFirst(86, _settings.TauF);
			settingsSerialized.SerializeShortLowFirst(88, _settings.TauFSet);
			settingsSerialized.SerializeShortLowFirst(90, _settings.TauFi);
			settingsSerialized.SerializeShortLowFirst(92, _settings.IdSetMin);
			settingsSerialized.SerializeShortLowFirst(94, _settings.IdSetMax);


			settingsSerialized[96] = _settings.UchMin.First;
			settingsSerialized[97] = _settings.UchMin.Second;

			settingsSerialized[98] = _settings.UchMax.First;
			settingsSerialized[99] = _settings.UchMax.Second;


			settingsSerialized[100] = _settings.Reserved50.First;
			settingsSerialized[101] = _settings.Reserved50.Second;

			settingsSerialized[102] = _settings.Reserved51.First;
			settingsSerialized[103] = _settings.Reserved51.Second;

			//var bp52 = BytesPair.FromUnsignedShortLowFirst((ushort)(_settings.Np | (_settings.NimpFloorCode << 5) | (_settings.FanMode.ToIoBits() << 8)));
			var bp52 = BytesPair.FromUnsignedShortLowFirst((ushort)(_settings.Np | (2 << 5) | (_settings.FanMode.ToIoBits() << 8)));
			Console.WriteLine("NPRM = " + bp52.First.ToString("X2") + bp52.Second.ToString("X2"));

			settingsSerialized[104] = bp52.First;
			settingsSerialized[105] = bp52.Second;

			settingsSerialized.SerializeShortLowFirst(106, (short)(_settings.UmodThr * 1000.0));
			settingsSerialized.SerializeShortLowFirst(108, _settings.EmdecDfdt);
			settingsSerialized.SerializeShortLowFirst(110, _settings.TextMax);
			settingsSerialized.SerializeShortLowFirst(112, _settings.ToHl);

			var result = new byte[115];
			result[0] = OneBasedAinNumber;
			settingsSerialized.CopyTo(result, 1);
			return result;
		}

		private byte OneBasedAinNumber => (byte)(_zeroBasedAinNumber + 1);

		public bool GetResult(byte[] reply) {
			if (reply[0] != OneBasedAinNumber) throw new Exception("неверный номер АИН в ответе, ожидался " + OneBasedAinNumber);
			return true;
		}

		public int ReplyLength => 1;

		public byte[] GetTestReply() {
			var result = new[] { OneBasedAinNumber };
			return result;
		}
	}
}