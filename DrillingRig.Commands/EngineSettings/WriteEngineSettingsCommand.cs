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
			var settingsSerialized = new byte[14];

			settingsSerialized.SerializeUintLowFirst(0, _settings.I2Tmax);
			settingsSerialized.SerializeUintLowFirst(4, _settings.Pnom);
			settingsSerialized.SerializeUshortLowFirst(8, _settings.Icontinious);
			settingsSerialized.SerializeUshortLowFirst(10, _settings.Mnom);
			settingsSerialized.SerializeUshortLowFirst(12, _settings.ZeroF);
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