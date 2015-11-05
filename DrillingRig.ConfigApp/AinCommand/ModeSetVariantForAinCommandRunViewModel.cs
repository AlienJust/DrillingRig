namespace DrillingRig.ConfigApp.AinCommand {
	class ModeSetVariantForAinCommandRunViewModel : IModeSetVariantForAinCommandViewModel
	{
		public ushort Value
		{
			get { return 0x40F; /*100 0000 1111*/ }
		}

		public string Name
		{
			get { return "Тяга (RUN)"; }
		}
	}
}