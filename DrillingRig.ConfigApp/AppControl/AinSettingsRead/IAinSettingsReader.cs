using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsRead {
	/// <summary>
	/// Интерфейс чтения и настроек АИН
	/// </summary>
	internal interface IAinSettingsReader {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="zeroBasedAinNumber">Номер АИН</param>
		/// <param name="callback">Метод обратного вызова, в который передаётся внутреннее исключение (если возникло) и результирующие настройки</param>
		/// <param name="forceRead">Флаг, указывающий не использовать настройки считанные ранее</param>
		void ReadSettingsAsync(byte zeroBasedAinNumber, bool forceRead, Action<Exception, IAinSettings> callback);
	}
}