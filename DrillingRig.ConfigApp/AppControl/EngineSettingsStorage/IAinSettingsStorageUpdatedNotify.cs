namespace DrillingRig.ConfigApp.AppControl.EngineSettingsStorage
{
	/// <summary>
	/// Сообщает о том, что настройки были обновлены
	/// </summary>
	interface IEngineSettingsStorageUpdatedNotify
	{
		/// <summary>
		/// Возникает при обновлении настроек в хранилище
		/// </summary>
		event StoredEngineSettingsUpdatedDelegate EngineSettingsUpdated;
	}
}