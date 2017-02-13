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
			var settingsSerialized = new byte[28]; // including KS
			settingsSerialized.SerializeUshortLowFirst(0, _settings.Inom);
			settingsSerialized.SerializeUshortLowFirst(2, _settings.Nnom);
			settingsSerialized.SerializeUshortLowFirst(4, _settings.Nmax);
			settingsSerialized.SerializeUshortLowFirst(6, _settings.Pnom);
			settingsSerialized.SerializeUshortLowFirst(8, _settings.CosFi);
			settingsSerialized.SerializeUshortLowFirst(10, _settings.Eff);
			settingsSerialized.SerializeUshortLowFirst(12, _settings.Mass);
			settingsSerialized.SerializeUshortLowFirst(14, _settings.MmM);
			settingsSerialized.SerializeUshortLowFirst(16, _settings.Height);

			settingsSerialized.SerializeUintLowFirst(18, _settings.I2Tmax);
			settingsSerialized.SerializeUshortLowFirst(22, _settings.Icontinious);
			settingsSerialized.SerializeUshortLowFirst(24, _settings.ZeroF);
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