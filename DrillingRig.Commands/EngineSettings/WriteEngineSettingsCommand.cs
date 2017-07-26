using AlienJust.Support.Collections;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.EngineSettings {
	public class WriteEngineSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<bool>, IRrModbusCommandWithTestReply
	{
		private readonly IEngineSettings _settings;

		public WriteEngineSettingsCommand(IEngineSettings settings)
		{
			_settings = settings;
		}

		public byte CommandCode => 0x8B;

		public string Name => "Запись настроек двигателя";

		public byte[] Serialize() {
			var settingsSerialized = new byte[28];

			settingsSerialized.SerializeUintLowFirst(0, (uint)(_settings.Pnom * 1000.0m));
			settingsSerialized.SerializeUintLowFirst(4, _settings.I2Tmax);
			settingsSerialized.SerializeUshortLowFirst(8, _settings.Icontinious);

			settingsSerialized.SerializeUshortLowFirst(10, _settings.Inom);
			settingsSerialized.SerializeUshortLowFirst(12, _settings.Nnom);
			settingsSerialized.SerializeUshortLowFirst(14, _settings.Nmax);
			
			settingsSerialized.SerializeUshortLowFirst(16, (ushort)(_settings.CosFi * 100.0m));
			settingsSerialized.SerializeUshortLowFirst(18, (ushort)(_settings.Eff * 10.0m));
			settingsSerialized.SerializeUshortLowFirst(20, _settings.Mass);
			settingsSerialized.SerializeUshortLowFirst(22, _settings.MmM);
			settingsSerialized.SerializeUshortLowFirst(24, _settings.Height);
			settingsSerialized.SerializeUshortLowFirst(26, _settings.ZeroF);

			return settingsSerialized;
		}

		public bool GetResult(byte[] reply)
		{
			return true;
		}

		public int ReplyLength => 0;

		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}