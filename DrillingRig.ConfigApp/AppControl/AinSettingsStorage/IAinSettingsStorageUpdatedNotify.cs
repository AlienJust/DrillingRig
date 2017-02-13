namespace DrillingRig.ConfigApp.AppControl.AinSettingsStorage {
	/// <summary>
	/// Сообщает о том, что настройки были обновлены
	/// </summary>
	interface IAinSettingsStorageUpdatedNotify {
		event StoredAinSettingsUpdatedDelegate AinSettingsUpdated;
	}
}