using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace
{
	/// <summary>
	/// Хранилище настроек двигателя с возможностью записи настроек в хранилище
	/// </summary>
	interface IEngineSettingsStorageSettable : IEngineSettingsStorage
	{
		void SetSettings(IEngineSettings settings);
	}
}