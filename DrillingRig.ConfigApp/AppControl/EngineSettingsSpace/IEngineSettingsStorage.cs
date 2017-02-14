using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace
{
	/// <summary>
	/// Хранилище настроек двигателя
	/// </summary>
	interface IEngineSettingsStorage
	{
		/// <summary>
		/// Сохранённые ранее настройки двигателя
		/// </summary>
		IEngineSettings EngineSettings { get; }
	}
}