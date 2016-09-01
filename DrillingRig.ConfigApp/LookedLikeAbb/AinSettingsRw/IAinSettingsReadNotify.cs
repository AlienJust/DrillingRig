namespace DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw {
	/// <summary>
	/// Сообщает о том, что настройки были прочитаны
	/// </summary>
	interface IAinSettingsReadNotify {
		event AinSettingsReadStartedDelegate AinSettingsReadStarted;
		event AinSettingsReadCompleteDelegate AinSettingsReadComplete;
	}
}