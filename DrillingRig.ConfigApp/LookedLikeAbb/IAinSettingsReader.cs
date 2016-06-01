using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	/// <summary>
	/// Интерфейс чтения и записи настроек АИН
	/// Реализация должна учитывать число АИНов в системе и производить запись соответсвтенно
	/// </summary>
	internal interface IAinSettingsReader {
		void ReadSettingsAsync(Action<Exception, IAinSettings> callback);
		void WriteSettingsAsync(IAinSettingsPart settings, Action<Exception> callback);
	}
}