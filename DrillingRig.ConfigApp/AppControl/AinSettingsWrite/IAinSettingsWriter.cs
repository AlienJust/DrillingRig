using System;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;

namespace DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw {
	/// <summary>
	/// Реализация должна учитывать число АИНов в системе и производить запись соответсвтенно
	/// </summary>
	internal interface IAinSettingsWriter {
		void WriteSettingsAsync(IAinSettingsPart settings, Action<Exception> callback);
	}
}