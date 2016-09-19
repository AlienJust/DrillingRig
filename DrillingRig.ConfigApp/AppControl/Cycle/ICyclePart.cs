namespace DrillingRig.ConfigApp {
	internal interface ICyclePart {
		void InCycleAction();
		bool Cancel { get; }
	}
}