using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsStorage
{
	/// <summary>
	/// Хранилище настроек двигателя
	/// </summary>
	interface IEngineSettingsStorage
	{
		IEngineSettings EngineSettings { get; }
	}
}