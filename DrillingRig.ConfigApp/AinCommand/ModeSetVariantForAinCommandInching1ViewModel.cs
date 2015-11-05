namespace DrillingRig.ConfigApp.AinCommand {
	class ModeSetVariantForAinCommandInching1ViewModel : IModeSetVariantForAinCommandViewModel
	{
		public ushort Value
		{
			get { return 0x50F; /*101 0000 1111*/ }
		}

		public string Name
		{
			get { return "Толчок 1 (INCHING 1)"; }
		}
	}
}