namespace DrillingRig.ConfigApp.AinCommand {
	class ModeSetVariantForAinCommandOff3ViewModel : IModeSetVariantForAinCommandViewModel
	{
		public ushort Value
		{
			get { return 0x403; /*100 0000 0011*/ }
		}

		public string Name
		{
			get { return "Останов 3 (OFF 3)"; }
		}
	}
}