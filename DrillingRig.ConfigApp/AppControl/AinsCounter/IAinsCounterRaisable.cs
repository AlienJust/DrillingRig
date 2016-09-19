namespace DrillingRig.ConfigApp.AppControl.AinsCounter {
	internal interface IAinsCounterRaisable : IAinsCounter {
		void SetAinsCountAndRaiseChange(int ainsCount);
	}
}