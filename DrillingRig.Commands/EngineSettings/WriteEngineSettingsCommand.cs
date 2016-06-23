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
			var settingsSerialized = new byte[15];
			settingsSerialized.SerializeUshort(0, _settings.Icontinious);
			settingsSerialized.SerializeUint(2, _settings.I2Tmax);
			settingsSerialized.SerializeUshort(6, _settings.Mnom);
			settingsSerialized.SerializeUint(8, _settings.Pnom);
			settingsSerialized.SerializeUshort(12, _settings.ZeroF);
			settingsSerialized[14] = _settings.Ks;

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