﻿using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group106SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinSettingsReaderWriter _readerWriter;

		public ParameterDoubleEditableViewModel Parameter01Vm { get; }
		public ParameterDoubleEditableViewModel Parameter02Vm { get; }
		public ParameterDoubleEditableViewModel Parameter03Vm { get; }
		public ParameterDoubleEditableViewModel Parameter04Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group106SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;

			Parameter01Vm = new ParameterDoubleEditableViewModel("106.01. Каналы ЦАП", "f0", -10000, 10000, null);
			Parameter02Vm = new ParameterDoubleEditableViewModel("106.02. Внутреннее слово режимов", "f0", -10000, 10000, null);
			Parameter03Vm = new ParameterDoubleEditableViewModel("106.03. Таймаут по системной линии связи", "f0", -10000, 10000, null);
			Parameter04Vm = new ParameterDoubleEditableViewModel("106.04. Применить параметры и сохранить в EEPROM", "f0", -10000, 10000, null);

			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
			WriteSettingsCmd = new RelayCommand(WriteSettings, () => true); // TODO: read only when connected to COM
		}

		private void WriteSettings() {
			try {
				var settingsPart = new AinSettingsPartWritable {
					DacCh = ConvertDoubleToShort(Parameter01Vm.CurrentValue),
					Imcw = ConvertDoubleToShort(Parameter02Vm.CurrentValue),
					ToHl = ConvertDoubleToShort(Parameter03Vm.CurrentValue)
					//, Apply = ConvertDoubleToShort(Parameter04Vm.CurrentValue) // TODO
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
						// Parameter04Vm.CurrentValue = null; // TODO
						return;
					}

					Parameter01Vm.CurrentValue = settings.DacCh;
					Parameter02Vm.CurrentValue = settings.Imcw;
					Parameter03Vm.CurrentValue = settings.ToHl;
					// Parameter04Vm.CurrentValue = settings.Apply; // TODO
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