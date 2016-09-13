namespace DrillingRig.ConfigApp.MnemonicCheme {
	internal class MnemonicChemeViewModel {
		public MnemonicChemeViewModel(string pathToImage) {
			PathToImage = pathToImage;
		}

		public string PathToImage { get; }
	}
}