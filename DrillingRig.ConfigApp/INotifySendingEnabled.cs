namespace DrillingRig.ConfigApp {
	internal interface INotifySendingEnabled {
		event SendingEnabledChangedDelegate SendingEnabledChanged;
		bool IsSendingEnabled { get; set; }
	}
}