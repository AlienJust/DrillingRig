using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group100SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinSettingsReaderWriter _readerWriter;

		public ParameterDoubleEditableViewModel Parameter01Vm { get; }
		public ParameterDoubleEditableViewModel Parameter02Vm { get; }
		public ParameterDoubleEditableViewModel Parameter03Vm { get; }
		public ParameterDoubleEditableViewModel Parameter04Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group100SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;

			Parameter01Vm = new ParameterDoubleEditableViewModel("100.01. Пропорциональный коэф. регулятора тока D", "f0", -10000, 10000, null);
			Parameter02Vm = new ParameterDoubleEditableViewModel("100.02. Интегральный коэф. регулятора тока D", "f0", -10000, 10000, null);

			Parameter03Vm = new ParameterDoubleEditableViewModel("100.03. Пропорциональный коэф. регулятора тока Q", "f0", -10000, 10000, null);
			Parameter04Vm = new ParameterDoubleEditableViewModel("100.04. Интегральный коэф. регулятора тока Q", "f0", -10000, 10000, null);


			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
			WriteSettingsCmd = new RelayCommand(WriteSettings, () => true); // TODO: read only when connected to COM
		}

		private void WriteSettings() {
			try {
				var settingsPart = new AinSettingsPartWritable {
					KpId = ConvertDoubleToShort(Parameter01Vm.CurrentValue),
					KiId = ConvertDoubleToShort(Parameter02Vm.CurrentValue),
					KpIq = ConvertDoubleToShort(Parameter03Vm.CurrentValue),
					KiIq = ConvertDoubleToShort(Parameter04Vm.CurrentValue),
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
			_readerWriter.ReadSettingsAsync(0, (exception, settings) => {
				_uiRoot.Notifier.Notify(() => {
					if (exception != null) {
						_logger.Log("Не удалось прочитать настройки АИН");
						Parameter01Vm.CurrentValue = null;
						Parameter02Vm.CurrentValue = null;
						Parameter03Vm.CurrentValue = null;
						Parameter04Vm.CurrentValue = null;
						return;
					}

					Parameter01Vm.CurrentValue = settings.KpId;
					Parameter02Vm.CurrentValue = settings.KiId;
					Parameter03Vm.CurrentValue = settings.KpIq;
					Parameter04Vm.CurrentValue = settings.KiIq;

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
