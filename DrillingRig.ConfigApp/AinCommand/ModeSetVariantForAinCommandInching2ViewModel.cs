namespace DrillingRig.ConfigApp.AinCommand {
	class ModeSetVariantForAinCommandInching2ViewModel : IModeSetVariantForAinCommandViewModel
	{
		public ushort Value
		{
			get { return 0x60F; /*110 0000 1111*/ }
		}

		public string Name
		{
			get { return "Толчок 2 (INCHING 2)"; }
		}
	}
}