using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsStorage {
	interface IAinSettingsStorageSettable : IAinSettingsStorage {
		void SetSettings(byte zeroBasedAinNumber, IAinSettings settings);
	}
}