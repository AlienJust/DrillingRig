using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group102SettingsViewModel : ViewModelBase {
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
		public RelayCommand WriteSettingsCmd { get; }

		public Group102SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;

			Parameter01Vm = new ParameterDoubleEditableViewModel("102.01. Постоянная времени ротора", "f0", -10000, 10000, null);
			Parameter02Vm = new ParameterDoubleEditableViewModel("102.02. Индуктивность намагничивания", "f0", -10000, 10000, null);
			Parameter03Vm = new ParameterDoubleEditableViewModel("102.03. Индуктивность рассеяния статора", "f0", -10000, 10000, null);
			Parameter04Vm = new ParameterDoubleEditableViewModel("102.04. Индуктивность рассеяния ротора", "f0", -10000, 10000, null);
			Parameter05Vm = new ParameterDoubleEditableViewModel("102.05. Активное сопротивление статора", "f0", -10000, 10000, null);
			Parameter06Vm = new ParameterDoubleEditableViewModel("102.06. Число пар полюсов (не путать с числом полюсов) АД", "f0", -10000, 10000, null);

			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
			WriteSettingsCmd = new RelayCommand(WriteSettings, () => true); // TODO: read only when connected to COM
		}

		private void WriteSettings() {
			try {
				var settingsPart = new AinSettingsPartWritable {
					TauR = ConvertDoubleToShort(Parameter01Vm.CurrentValue),
					Lm = ConvertDoubleToShort(Parameter02Vm.CurrentValue),
					Lsl = ConvertDoubleToShort(Parameter03Vm.CurrentValue),
					Lrl = ConvertDoubleToShort(Parameter04Vm.CurrentValue),
					Rs = ConvertDoubleToShort(Parameter05Vm.CurrentValue),
					Np = ConvertDoubleToShort(Parameter06Vm.CurrentValue),
				};
				_readerWriter.WriteSettingsAsync(settingsPart, exception => {
					_uiRoot.Notifier.Notify(() => {
						if (exception != null) {
							_logger.Log("Ошибка при записи настроек. " + exception.Message);
						}
						else _logger.Log("Группа настроек была успешно записана");
					});
				}
					);
			}
			catch (Exception ex) {
				_logger.Log("Не удалось записать группу настроек. " + ex.Message);
			}
		}

		private void ReadSettings() {
			try {
			_readerWriter.ReadSettingsAsync((exception, settings) => {
				_uiRoot.Notifier.Notify(() => {
					if (exception != null) {
						_logger.Log("Не удалось прочитать настройки АИН");
						Parameter01Vm.CurrentValue = null;
						Parameter02Vm.CurrentValue = null;
						Parameter03Vm.CurrentValue = null;
						Parameter04Vm.CurrentValue = null;
						Parameter05Vm.CurrentValue = null;
						Parameter06Vm.CurrentValue = null;
						return;
					}

					Parameter01Vm.CurrentValue = settings.TauR;
					Parameter02Vm.CurrentValue = settings.Lm;
					Parameter03Vm.CurrentValue = settings.Lsl;
					Parameter04Vm.CurrentValue = settings.Lrl;
					Parameter05Vm.CurrentValue = settings.Rs;
					Parameter06Vm.CurrentValue = settings.Np;
				});
			});
			}
			catch (Exception ex) {
				_logger.Log("Не удалось прочитать группу настроек. " + ex.Message);
			}
		}

		private short? ConvertDoubleToShort(double? value) {
			if (!value.HasValue) return null;
			return (short) value.Value;
		}
	}
}
