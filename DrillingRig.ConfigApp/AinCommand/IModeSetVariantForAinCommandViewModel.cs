namespace DrillingRig.ConfigApp.AinCommand {
	internal interface IModeSetVariantForAinCommandViewModel{
		ushort Value { get; }
		string Name { get; }
	}

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

	class ModeSetVariantForAinCommandOff2ViewModel : IModeSetVariantForAinCommandViewModel {
		public ushort Value {
			get { return 0x405; /*100 0000 0101*/ }
		}

		public string Name {
			get { return "Останов 2 (выбег - OFF 2)"; }
		}
	}

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

	class ModeSetVariantForAinCommandInching1ViewModel : IModeSetVariantForAinCommandViewModel
	{
		public ushort Value
		{
			get { return 0x50F; /*101 0000 1111*/ }
		}

		public string Name
		{
			get { return "Толчек 1 (INCHING 1)"; }
		}
	}

	class ModeSetVariantForAinCommandInching2ViewModel : IModeSetVariantForAinCommandViewModel
	{
		public ushort Value
		{
			get { return 0x60F; /*110 0000 1111*/ }
		}

		public string Name
		{
			get { return "Толчек 2 (INCHING 2)"; }
		}
	}

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