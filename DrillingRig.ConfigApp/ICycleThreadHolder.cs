namespace DrillingRig.ConfigApp {
	internal interface ICycleThreadHolder {
		void RegisterAsCyclePart(ICyclePart part);
	}
}