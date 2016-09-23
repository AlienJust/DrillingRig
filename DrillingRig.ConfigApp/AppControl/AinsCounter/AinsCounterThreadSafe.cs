namespace DrillingRig.ConfigApp.AppControl.AinsCounter {
	class AinsCounterThreadSafe : IAinsCounterRaisable {
		private int _ainsCount;
		private readonly object _ainsCountSync;
		public event AinsCountInSystemHasBeenChangedDelegate AinsCountInSystemHasBeenChanged;

		public AinsCounterThreadSafe(int ainsCount) {
			_ainsCountSync = new object();
			_ainsCount = ainsCount;
		}

		public int SelectedAinsCount {
			get { lock(_ainsCountSync) return _ainsCount; }
		}

		public void SetAinsCountAndRaiseChange(int ainsCount) {
			lock (_ainsCountSync) {
				_ainsCount = ainsCount;
				var eve = AinsCountInSystemHasBeenChanged; // TODO: thing if I need lock
				eve?.Invoke(ainsCount);
			}
		}
	}
}