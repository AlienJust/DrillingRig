namespace DrillingRig.ConfigApp {
	internal interface IAinsCounter {
		int SelectedAinsCount { get; }
		event AinsCountInSystemHasBeenChangedDelegate AinsCountInSystemHasBeenChanged;
	}

	delegate void AinsCountInSystemHasBeenChangedDelegate();
}