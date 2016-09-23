using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsStorage {
	interface IAinSettingsStorage {
		IAinSettings GetSettings(byte zeroBasedAinNumber);
	}
}