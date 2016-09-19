namespace DrillingRig.ConfigApp.AppControl.AinSettingsRead {
	/// <summary>
	/// Сообщает о том, что настройки были прочитаны
	/// </summary>
	interface IAinSettingsReadNotify {
		event AinSettingsReadStartedDelegate AinSettingsReadStarted;
		event AinSettingsReadCompleteDelegate AinSettingsReadComplete;
	}
}