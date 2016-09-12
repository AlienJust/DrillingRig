namespace DrillingRig.ConfigApp {
	internal interface IAinsLinkControl {
		bool? Ain1LinkError { get; }
		bool? Ain2LinkError { get; }
		bool? Ain3LinkError { get; }

		event AinsLinkInformationHasBeenUpdatedDelegate AinsLinkInformationHasBeenUpdated;
	}
}