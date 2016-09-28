using System;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	/// <summary>
	/// Реализация должна учитывать число АИНов в системе и производить запись соответсвтенно
	/// </summary>
	internal interface IAinSettingsWriter {
		void WriteSettingsAsync(IAinSettingsPart settings, Action<Exception> callback);
	}
}