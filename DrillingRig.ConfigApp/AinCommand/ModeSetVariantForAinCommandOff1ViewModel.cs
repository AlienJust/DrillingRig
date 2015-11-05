namespace DrillingRig.ConfigApp.AinCommand {
	class ModeSetVariantForAinCommandOff1ViewModel : IModeSetVariantForAinCommandViewModel
	{
		public ushort Value
		{
			get { return 0x406; /*100 0000 0110*/ }
		}

		public string Name
		{
			get { return "Останов 1 (OFF 1)"; }
		}
	}
}