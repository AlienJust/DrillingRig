using System;
using AlienJust.Support.Collections;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AinSettings {
	public class WriteAinSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<bool>, IRrModbusCommandWithTestReply
	{
		private readonly byte _zeroBasedAinNumber;
		private readonly IAinSettings _settings;

		public WriteAinSettingsCommand(byte zeroBasedAinNumber, IAinSettings settings)
		{
			_zeroBasedAinNumber = zeroBasedAinNumber;
			_settings = settings;
		}

		public byte CommandCode => 0x8E;

		public string Name => "Запись настроек АИН #" + (_zeroBasedAinNumber + 1);

		public byte[] Serialize() {
			var settingsSerialized = new byte[114];
			settingsSerialized[0] = _settings.Reserved00.First;
			settingsSerialized[1] = _settings.Reserved00.Second;

			settingsSerialized[2] = _settings.KpW.First;
			settingsSerialized[3] = _settings.KpW.Second;

			settingsSerialized.SerializeIntLowFirst(4, _settings.KiW);
			settingsSerialized.SerializeShortLowFirst(8, _settings.FiNom);
			settingsSerialized.SerializeShortLowFirst(10, _settings.Imax);
			settingsSerialized.SerializeShortLowFirst(12, _settings.UdcMax);
			settingsSerialized.SerializeShortLowFirst(14, _settings.UdcMin);
			settingsSerialized.SerializeShortLowFirst(16, _settings.Fnom);
			settingsSerialized.SerializeShortLowFirst(18, _settings.Fmax);

			settingsSerialized.SerializeShortLowFirst(20, _settings.DflLim);
			settingsSerialized.SerializeShortLowFirst(22, _settings.FlMinMin);
			
			settingsSerialized.SerializeShortLowFirst(24, _settings.IoutMax);
			settingsSerialized.SerializeShortLowFirst(26, _settings.FiMin);
			settingsSerialized.SerializeShortLowFirst(28, _settings.DacCh);
			settingsSerialized.SerializeShortLowFirst(30, _settings.Imcw);
			settingsSerialized.SerializeShortLowFirst(32, _settings.Ia0);
			settingsSerialized.SerializeShortLowFirst(34, _settings.Ib0);
			settingsSerialized.SerializeShortLowFirst(36, _settings.Ic0);
			settingsSerialized.SerializeShortLowFirst(38, _settings.Udc0);
			settingsSerialized.SerializeShortLowFirst(40, _settings.TauR);
			settingsSerialized.SerializeShortLowFirst(42, _settings.Lm);
			settingsSerialized.SerializeShortLowFirst(44, _settings.Lsl);
			settingsSerialized.SerializeShortLowFirst(46, _settings.Lrl);

			settingsSerialized[48] = _settings.Reserved24.First;
			settingsSerialized[49] = _settings.Reserved24.Second;

			settingsSerialized[50] = _settings.KpFi.First;
			settingsSerialized[51] = _settings.KpFi.Second;
			
			settingsSerialized.SerializeIntLowFirst(52, _settings.KiFi);

			settingsSerialized[56] = _settings.Reserved28.First;
			settingsSerialized[57] = _settings.Reserved28.Second;

			settingsSerialized[58] = _settings.KpId.First;
			settingsSerialized[59] = _settings.KpId.Second;
			
			settingsSerialized.SerializeIntLowFirst(60, _settings.KiId);


			settingsSerialized[64] = _settings.Reserved32.First;
			settingsSerialized[65] = _settings.Reserved32.Second;

			settingsSerialized[66] = _settings.KpIq.First;
			settingsSerialized[67] = _settings.KpIq.Second;
			
			settingsSerialized.SerializeIntLowFirst(68, _settings.KiIq);

			settingsSerialized.SerializeShortLowFirst(72, _settings.AccDfDt);
			settingsSerialized.SerializeShortLowFirst(74, _settings.DecDfDt);
			settingsSerialized.SerializeShortLowFirst(76, _settings.Unom);

			settingsSerialized.SerializeShortLowFirst(78, _settings.TauFlLim);

			settingsSerialized.SerializeShortLowFirst(80, _settings.Rs);
			settingsSerialized.SerializeShortLowFirst(82, _settings.Fmin);
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

			var bp52 = BytesPair.FromUnsignedShortLowFirst((ushort)(_settings.Np | (_settings.NimpFloorCode << 5) | (_settings.FanMode.ToIoBits() << 8)));
			settingsSerialized[104] = bp52.First;
			settingsSerialized[105] = bp52.Second;

			settingsSerialized.SerializeShortLowFirst(106, _settings.UmodThr);
			settingsSerialized.SerializeShortLowFirst(108, _settings.EmdecDfdt);
			settingsSerialized.SerializeShortLowFirst(110, _settings.TextMax);
			settingsSerialized.SerializeShortLowFirst(112, _settings.ToHl);

			var result = new byte[115];
			result[0] = OneBasedAinNumber;
			settingsSerialized.CopyTo(result, 1);
			return result;
		}

		private byte OneBasedAinNumber => (byte)(_zeroBasedAinNumber + 1);

		public bool GetResult(byte[] reply)
		{
			if (reply[0] != OneBasedAinNumber) throw new Exception("неверный номер АИН в ответе, ожидался " + OneBasedAinNumber);
			return true;
		}

		public int ReplyLength => 1;

		public byte[] GetTestReply() {
			var result = new[]{OneBasedAinNumber};
			return result;
		}
	}
}