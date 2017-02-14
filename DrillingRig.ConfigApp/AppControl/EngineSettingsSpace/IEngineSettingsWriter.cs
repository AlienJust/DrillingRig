using System;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace {
	
	/// <summary>
	/// Интерфейс для записи настроек двигателя
	/// </summary>
	internal interface IEngineSettingsWriter {
		/// <summary>
		/// Записывает настройки асинхронно
		/// </summary>
		void WriteSettingsAsync(IEngineSettingsPart settings, Action<Exception> callback);
	}
}