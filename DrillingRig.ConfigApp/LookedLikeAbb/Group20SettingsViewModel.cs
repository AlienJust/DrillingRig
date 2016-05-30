using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group20SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		public ParameterDoubleEditableViewModel Parameter01Vm { get; }
		public ParameterDoubleEditableViewModel Parameter02Vm { get; }

		public Group20SettingsViewModel(IUserInterfaceRoot uiRoot) {
			_uiRoot = uiRoot;
			Parameter01Vm = new ParameterDoubleEditableViewModel("20.01. МИН СКОРОСТЬ, об/мин", "f0", -9000,1500, null);
			Parameter02Vm = new ParameterDoubleEditableViewModel("20.02. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter03Vm = new ParameterDoubleEditableViewModel("20.03. ПРЕДЕЛ НУЛЕВ СКОР, об/мин", "f0", 0, 15000, null);
			Parameter04Vm = new ParameterDoubleEditableViewModel("20.04. МАКС ТОК", "f0", -1500, 9000, null);
			Parameter05Vm = new ParameterDoubleEditableViewModel("20.05. МАКС МОМЕНТ, А", "f0", 0, 10000, null); // TODO: спросить Марата, в процентах или как задаётся момент.
			Parameter06Vm = new ParameterDoubleEditableViewModel("20.06. МИН МОМЕНТ, А", "f0", 0, 10000, null); 77777
			Parameter07Vm = new ParameterDoubleEditableViewModel("20.07. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter08Vm = new ParameterDoubleEditableViewModel("20.08. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter09Vm = new ParameterDoubleEditableViewModel("20.09. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter10Vm = new ParameterDoubleEditableViewModel("20.10. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter11Vm = new ParameterDoubleEditableViewModel("20.11. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter12Vm = new ParameterDoubleEditableViewModel("20.12. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter13Vm = new ParameterDoubleEditableViewModel("20.13. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter14Vm = new ParameterDoubleEditableViewModel("20.14. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter15Vm = new ParameterDoubleEditableViewModel("20.15. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter16Vm = new ParameterDoubleEditableViewModel("20.16. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter17Vm = new ParameterDoubleEditableViewModel("20.17. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter18Vm = new ParameterDoubleEditableViewModel("20.18. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter19Vm = new ParameterDoubleEditableViewModel("20.19. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter20Vm = new ParameterDoubleEditableViewModel("20.20. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
			Parameter21Vm = new ParameterDoubleEditableViewModel("20.21. МАКС СКОРОСТЬ, об/мин", "f0", -1500, 9000, null);
		}
	}

	internal interface ICheckableParameter {
		bool IsChecked { get; set; }
	}
}
