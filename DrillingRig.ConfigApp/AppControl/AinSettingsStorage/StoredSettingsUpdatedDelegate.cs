using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsStorage {
	delegate void StoredSettingsUpdatedDelegate(byte zeroBasedAinNumber, IAinSettings settings);
}