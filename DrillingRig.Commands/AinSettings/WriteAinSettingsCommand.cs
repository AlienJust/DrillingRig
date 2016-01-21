using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		public byte CommandCode
		{
			get { return 0x8E; }
		}

		public string Name
		{
			get { return "Запись настроек АИН #" + (_zeroBasedAinNumber + 1); }
		}

		public byte[] Serialize() {
			var settingsSerialized = new byte[114];
			settingsSerialized.SerializeInt(0, _settings.KpW);
			settingsSerialized.SerializeInt(4, _settings.KiW);
			settingsSerialized.SerializeShort(8, _settings.FiNom);
			settingsSerialized.SerializeShort(10, _settings.Imax);
			settingsSerialized.SerializeShort(12, _settings.UdcMax);
			settingsSerialized.SerializeShort(14, _settings.UdcMin);
			settingsSerialized.SerializeShort(16, _settings.Fnom);
			settingsSerialized.SerializeShort(18, _settings.Fmax);

			settingsSerialized.SerializeShort(20, _settings.Empty10);
			settingsSerialized.SerializeShort(22, _settings.Empty11);
			
			settingsSerialized.SerializeShort(24, _settings.IoutMax);
			settingsSerialized.SerializeShort(26, _settings.FiMin);
			settingsSerialized.SerializeShort(28, _settings.DacCh);
			settingsSerialized.SerializeShort(30, _settings.Imcw);
			settingsSerialized.SerializeShort(32, _settings.Ia0);
			settingsSerialized.SerializeShort(34, _settings.Ib0);
			settingsSerialized.SerializeShort(36, _settings.Ic0);
			settingsSerialized.SerializeShort(38, _settings.Udc0);
			settingsSerialized.SerializeShort(40, _settings.TauR);
			settingsSerialized.SerializeShort(42, _settings.Lm);
			settingsSerialized.SerializeShort(44, _settings.Lsl);
			settingsSerialized.SerializeShort(46, _settings.Lrl);

			settingsSerialized.SerializeInt(48, _settings.KpFi);
			settingsSerialized.SerializeInt(52, _settings.KiFi);

			settingsSerialized.SerializeInt(56, _settings.KpId);
			settingsSerialized.SerializeInt(60, _settings.KiId);

			settingsSerialized.SerializeInt(64, _settings.KpIq);
			settingsSerialized.SerializeInt(68, _settings.KiIq);

			settingsSerialized.SerializeShort(72, _settings.AccDfDt);
			settingsSerialized.SerializeShort(74, _settings.DecDfDt);
			settingsSerialized.SerializeShort(76, _settings.Unom);

			settingsSerialized.SerializeShort(78, _settings.Empty39);

			settingsSerialized.SerializeShort(80, _settings.Rs);
			settingsSerialized.SerializeShort(82, _settings.Fmin);
			settingsSerialized.SerializeShort(84, _settings.TauM);
			settingsSerialized.SerializeShort(86, _settings.TauF);
			settingsSerialized.SerializeShort(88, _settings.TauFSet);
			settingsSerialized.SerializeShort(90, _settings.TauFi);
			settingsSerialized.SerializeShort(92, _settings.IdSetMin);
			settingsSerialized.SerializeShort(94, _settings.IdSetMax);

			settingsSerialized.SerializeInt(96, _settings.KpFe);
			settingsSerialized.SerializeInt(100, _settings.KiFe);

			settingsSerialized.SerializeShort(104, _settings.Np);
			settingsSerialized.SerializeShort(106, _settings.Empty53);
			settingsSerialized.SerializeShort(108, _settings.EmdecDfdt);
			settingsSerialized.SerializeShort(110, _settings.TextMax);
			settingsSerialized.SerializeShort(112, _settings.ToHl);

			var result = new byte[115];
			result[0] = OneBasedAinNumber;
			settingsSerialized.CopyTo(result, 1);
			return result;
		}

		private byte OneBasedAinNumber {
			get { return (byte)(_zeroBasedAinNumber + 1); }
		}

		public bool GetResult(byte[] reply)
		{
			if (reply[0] != OneBasedAinNumber) throw new Exception("неверный номер АИН в ответе, ожидался " + OneBasedAinNumber);
			return true;
		}

		public int ReplyLength
		{
			get {
				return 1 ; // ain number
			}
		}

		public byte[] GetTestReply() {
			var result = new[]{OneBasedAinNumber};
			return result;
		}

		
	}

	static class IlistExtensions {
		public static void SerializeInt(this IList<byte> container, int position, int value)
		{
			container[position + 0] = (byte)(value & 0xFF);
			container[position + 1] = (byte)((value >> 8) & 0xFF);
			container[position + 2] = (byte)((value >> 16) & 0xFF);
			container[position + 3] = (byte)((value >> 24) & 0xFF);
		}
		public static void SerializeShort(this IList<byte> container, int position, short value)
		{
			container[position + 0] = (byte)(value & 0xFF);
			container[position + 1] = (byte)((value >> 8) & 0xFF);
		}
	}
}