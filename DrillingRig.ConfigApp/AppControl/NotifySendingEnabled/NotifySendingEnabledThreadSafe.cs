namespace DrillingRig.ConfigApp.AppControl.NotifySendingEnabled {
	class NotifySendingEnabledThreadSafe : INotifySendingEnabledRaisable {
		private bool _isSendingEnabled;
		private readonly object _isSendingEnabledSync;
		public event SendingEnabledChangedDelegate SendingEnabledChanged;

		public NotifySendingEnabledThreadSafe(bool isSendingEnabled) {
			_isSendingEnabledSync = new object();
			_isSendingEnabled = isSendingEnabled;
		}

		public bool IsSendingEnabled
		{
			get { lock(_isSendingEnabledSync) return _isSendingEnabled; }
		}

		public void SetIsSendingEnabledAndRaiseChange(bool isSendingEnabled) {
			lock (_isSendingEnabledSync) {
				_isSendingEnabled = isSendingEnabled;
				var eve = SendingEnabledChanged; // TODO: thing if I need lock
				eve?.Invoke(isSendingEnabled);
			}
		}
	}
}