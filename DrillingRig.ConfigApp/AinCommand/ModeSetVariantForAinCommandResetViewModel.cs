namespace DrillingRig.ConfigApp.AinCommand {
	class ModeSetVariantForAinCommandResetViewModel : IModeSetVariantForAinCommandViewModel
	{
		public ushort Value
		{
			get { return 0x480; /*100 1000 0000*/ }
		}

		public string Name
		{
			get { return "Сброс аварий (RESET)"; }
		}
	}
}