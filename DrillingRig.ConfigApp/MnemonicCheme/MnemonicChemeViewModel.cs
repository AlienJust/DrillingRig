using DrillingRig.ConfigApp.AvaDock;

namespace DrillingRig.ConfigApp.MnemonicCheme {
	internal class MnemonicChemeViewModel : DockWindowViewModel {
		public MnemonicChemeViewModel(string pathToImage) {
			PathToImage = pathToImage;
		}

		public string PathToImage { get; }
	}
}