using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group20SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinSettingsReaderWriter _readerWriter;

		public ParameterDoubleEditableViewModel Parameter01Vm { get; }
		public ParameterDoubleEditableViewModel Parameter02Vm { get; }
		public ParameterDoubleEditableViewModel Parameter03Vm { get; }
		public ParameterDoubleEditableViewModel Parameter04Vm { get; }
		public ParameterDoubleEditableViewModel Parameter05Vm { get; }
		public ParameterDoubleEditableViewModel Parameter06Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }

		public Group20SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;

			Parameter01Vm = new ParameterDoubleEditableViewModel("20.01. Номинальная частота", "f0", -10000, 10000, null);
			Parameter02Vm = new ParameterDoubleEditableViewModel("20.02. Максимальная частота", "f0", -10000, 10000, null);

			Parameter03Vm = new ParameterDoubleEditableViewModel("20.03. Ограничение тока (амплитутда)", "f0", -10000, 10000, null);
			Parameter04Vm = new ParameterDoubleEditableViewModel("20.04. Минимальная частота (электрическая)", "f0", -10000, 10000, null);

			Parameter05Vm = new ParameterDoubleEditableViewModel("20.05. Минимальный момент", "f0", -10000, 10000, null); // TODO: спросить Марата, в процентах или как задаётся момент.
			Parameter06Vm = new ParameterDoubleEditableViewModel("20.06. Максимальный момент", "f0", -10000, 10000, null); 

			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
		}

		private void ReadSettings() {
			_readerWriter.ReadSettingsAsync((exception, settings) => {
				_uiRoot.Notifier.Notify(() => {
					if (exception != null) {
						_logger.Log("Не удалось прочитать настройки АИН");
						Parameter01Vm.CurrentValue = null;
						return;
					}

					Parameter01Vm.CurrentValue = settings.FiNom;
				});

			});
		}
	}
}
