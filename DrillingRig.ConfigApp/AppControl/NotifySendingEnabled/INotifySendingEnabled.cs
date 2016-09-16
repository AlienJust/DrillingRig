namespace DrillingRig.ConfigApp.AppControl.NotifySendingEnabled {
	/// <summary>
	/// Позволяет узнать (в т.ч. и по событию), разрешена отправка команд, или нет.
	/// </summary>
	internal interface INotifySendingEnabled {
		event SendingEnabledChangedDelegate SendingEnabledChanged;
		bool IsSendingEnabled { get; }
	}
}