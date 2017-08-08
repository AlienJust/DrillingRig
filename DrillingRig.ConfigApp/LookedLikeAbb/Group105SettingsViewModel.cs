using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleEditCheck;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group105SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinSettingsReaderWriter _readerWriter;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsStorage _storage;
		private readonly IAinSettingsStorageUpdatedNotify _storageUpdatedNotify;
		private readonly IAinsCounter _ainsCounter;

		public ParameterDecimalEditCheckViewModel Parameter01Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter02Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter03Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter04Vm { get; }

		public ParameterDecimalEditCheckViewModel Parameter101Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter102Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter103Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter104Vm { get; }

		public ParameterDecimalEditCheckViewModel Parameter201Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter202Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter203Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter204Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group105SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter, /*IAinSettingsReadNotify ainSettingsReadNotify,*/ IAinSettingsStorage storage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;
			//_ainSettingsReadNotify = ainSettingsReadNotify;
			_storage = storage;
			_storageUpdatedNotify = storageUpdatedNotify;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDecimalEditCheckViewModel("105.01. Калибровка нуля тока фазы A", "f0", -10000, 10000);
			Parameter02Vm = new ParameterDecimalEditCheckViewModel("105.02. Калибровка нуля тока фазы B", "f0", -10000, 10000);
			Parameter03Vm = new ParameterDecimalEditCheckViewModel("105.03. Калибровка нуля тока фазы C", "f0", -10000, 10000);
			Parameter04Vm = new ParameterDecimalEditCheckViewModel("105.04. Калибровка нуля напряжения шины DC", "f0", -10000, 10000);

			Parameter101Vm = new ParameterDecimalEditCheckViewModel("105.01. Калибровка нуля тока фазы A", "f0", -10000, 10000);
			Parameter102Vm = new ParameterDecimalEditCheckViewModel("105.02. Калибровка нуля тока фазы B", "f0", -10000, 10000);
			Parameter103Vm = new ParameterDecimalEditCheckViewModel("105.03. Калибровка нуля тока фазы C", "f0", -10000, 10000);
			Parameter104Vm = new ParameterDecimalEditCheckViewModel("105.04. Калибровка нуля напряжения шины DC", "f0", -10000, 10000);

			Parameter201Vm = new ParameterDecimalEditCheckViewModel("105.01. Калибровка нуля тока фазы A", "f0", -10000, 10000);
			Parameter202Vm = new ParameterDecimalEditCheckViewModel("105.02. Калибровка нуля тока фазы B", "f0", -10000, 10000);
			Parameter203Vm = new ParameterDecimalEditCheckViewModel("105.03. Калибровка нуля тока фазы C", "f0", -10000, 10000);
			Parameter204Vm = new ParameterDecimalEditCheckViewModel("105.04. Калибровка нуля напряжения шины DC", "f0", -10000, 10000);

			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
			WriteSettingsCmd = new RelayCommand(WriteSettings, () => IsWriteEnabled); // TODO: read only when connected to COM

			//_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete;
			_storageUpdatedNotify.AinSettingsUpdated += (zbAinNuber, settings) => {
				_uiRoot.Notifier.Notify(() => WriteSettingsCmd.RaiseCanExecuteChanged());
				AinSettingsReadNotifyOnAinSettingsReadComplete(zbAinNuber, settings == null ? new Exception("Настройки недоступны") : null, settings);
			};
		}

		private bool IsWriteEnabled {
			get {
				for (byte i = 0; i < _ainsCounter.SelectedAinsCount; ++i) {
					var settings = _storage.GetSettings(i);
					if (settings == null) return false; // TODO: по идее еще можно проверять AinLinkFault внутри настроек
				}
				return true;
			}
		}

		private void AinSettingsReadNotifyOnAinSettingsReadComplete(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings) {
			if (zeroBasedAinNumber == 0) {
				UpdateSettingsInUiThread(readInnerException, settings);
			}
			else if (zeroBasedAinNumber == 1) {
				UpdateSettingsInUiThread1(readInnerException, settings);
			}
			else if (zeroBasedAinNumber == 2) {
				UpdateSettingsInUiThread2(readInnerException, settings);
			}
		}

		private void WriteSettings() {
			try {
				_uiRoot.Notifier.Notify(() => { _logger.Log("Запись группы настроек..."); });
				var settingsPart = new AinSettingsPartWritable {
					Ia0 = ConvertDecimalToShort(Parameter01Vm.CurrentValue),
					Ib0 = ConvertDecimalToShort(Parameter02Vm.CurrentValue),
					Ic0 = ConvertDecimalToShort(Parameter03Vm.CurrentValue),
					Udc0 = ConvertDecimalToShort(Parameter04Vm.CurrentValue)
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
				for (byte i = 0; i < _ainsCounter.SelectedAinsCount; ++i)
					_readerWriter.ReadSettingsAsync(i, true, (exception, settings) => { });
		}
			catch (Exception ex) {
				_logger.Log("Не удалось прочитать группу настроек. " + ex.Message);
			}
		}

		private short? ConvertDecimalToShort(decimal? value) {
			if (!value.HasValue) return null;
			return (short) value.Value;
		}

		private void UpdateSettingsInUiThread(Exception exception, IAinSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (exception != null) {
					//_logger.Log("Не удалось прочитать настройки АИН");
					Parameter01Vm.CurrentValue = null;
					Parameter02Vm.CurrentValue = null;
					Parameter03Vm.CurrentValue = null;
					Parameter04Vm.CurrentValue = null;
					return;
				}

				Parameter01Vm.CurrentValue = settings.Ia0;
				Parameter02Vm.CurrentValue = settings.Ib0;
				Parameter03Vm.CurrentValue = settings.Ic0;
				Parameter04Vm.CurrentValue = settings.Udc0;
			});
		}

		private void UpdateSettingsInUiThread1(Exception exception, IAinSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (exception != null) {
					//_logger.Log("Не удалось прочитать настройки АИН");
					Parameter101Vm.CurrentValue = null;
					Parameter102Vm.CurrentValue = null;
					Parameter103Vm.CurrentValue = null;
					Parameter104Vm.CurrentValue = null;
					return;
				}

				Parameter101Vm.CurrentValue = settings.Ia0;
				Parameter102Vm.CurrentValue = settings.Ib0;
				Parameter103Vm.CurrentValue = settings.Ic0;
				Parameter104Vm.CurrentValue = settings.Udc0;
			});
		}

		private void UpdateSettingsInUiThread2(Exception exception, IAinSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (exception != null) {
					//_logger.Log("Не удалось прочитать настройки АИН");
					Parameter201Vm.CurrentValue = null;
					Parameter202Vm.CurrentValue = null;
					Parameter203Vm.CurrentValue = null;
					Parameter204Vm.CurrentValue = null;
					return;
				}

				Parameter201Vm.CurrentValue = settings.Ia0;
				Parameter202Vm.CurrentValue = settings.Ib0;
				Parameter203Vm.CurrentValue = settings.Ic0;
				Parameter204Vm.CurrentValue = settings.Udc0;
			});
		}
	}
}
