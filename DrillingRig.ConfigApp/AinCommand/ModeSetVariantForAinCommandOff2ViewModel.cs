namespace DrillingRig.ConfigApp.AinCommand {
	class ModeSetVariantForAinCommandOff2ViewModel : IModeSetVariantForAinCommandViewModel {
		public ushort Value {
			get { return 0x405; /*100 0000 0101*/ }
		}

		public string Name {
			get { return "Останов 2 (выбег - OFF 2)"; }
		}
	}
}