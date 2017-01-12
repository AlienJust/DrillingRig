using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsStorage {
	/// <summary>
	/// Хранилище настроек используется прежде всего для записи настроек АИН. 
	/// Пока нет настроек в хранилище, запись настроек осуществаляется не может.
	/// </summary>
	interface IAinSettingsStorage {
		IAinSettings GetSettings(byte zeroBasedAinNumber);
	}
}