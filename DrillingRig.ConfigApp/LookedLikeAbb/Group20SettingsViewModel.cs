using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group20SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		public ParameterDoubleEditableViewModel Parameter01Vm { get; }
		public ParameterDoubleEditableViewModel Parameter02Vm { get; }
		public ParameterDoubleEditableViewModel Parameter03Vm { get; }
		public ParameterDoubleEditableViewModel Parameter04Vm { get; }
		public ParameterDoubleEditableViewModel Parameter05Vm { get; }
		public ParameterDoubleEditableViewModel Parameter06Vm { get; }

		public Group20SettingsViewModel(IUserInterfaceRoot uiRoot) {
			_uiRoot = uiRoot;
			Parameter01Vm = new ParameterDoubleEditableViewModel("20.01. Номинальная частота", "f0", -10000, 10000, null);
			Parameter02Vm = new ParameterDoubleEditableViewModel("20.02. Максимальная частота", "f0", -10000, 10000, null);

			Parameter03Vm = new ParameterDoubleEditableViewModel("20.03. Ограничение тока (амплитутда)", "f0", -10000, 10000, null);
			Parameter04Vm = new ParameterDoubleEditableViewModel("20.04. Минимальная частота (электрическая)", "f0", -10000, 10000, null);

			Parameter05Vm = new ParameterDoubleEditableViewModel("20.05. Минимальный момент", "f0", -10000, 10000, null); // TODO: спросить Марата, в процентах или как задаётся момент.
			Parameter06Vm = new ParameterDoubleEditableViewModel("20.06. Максимальный момент", "f0", -10000, 10000, null); 
		}
	}
}
