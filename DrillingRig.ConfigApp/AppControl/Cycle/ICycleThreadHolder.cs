namespace DrillingRig.ConfigApp.AppControl.Cycle {
	internal interface ICycleThreadHolder {
		void RegisterAsCyclePart(ICyclePart part);
	}
}