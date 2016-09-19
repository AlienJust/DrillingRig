using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsRead {
	/// <summary>
	/// Интерфейс чтения и настроек АИН
	/// </summary>
	internal interface IAinSettingsReader {
		//void ReadSettingsAsync(Action<Exception, IAinSettings> callback);
		void ReadSettingsAsync(byte zeroBasedAinNumber, Action<Exception, IAinSettings> callback);
	}
}