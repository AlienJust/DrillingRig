using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsStorage {
	delegate void StoredAinSettingsUpdatedDelegate(byte zeroBasedAinNumber, IAinSettings settings);
}