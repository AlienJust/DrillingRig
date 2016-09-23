namespace DrillingRig.ConfigApp.AppControl.NotifySendingEnabled {
	internal interface INotifySendingEnabledRaisable : INotifySendingEnabled {
		void SetIsSendingEnabledAndRaiseChange(bool isSendingEnabled);
	}
}