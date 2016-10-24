﻿using System;
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

		public ParameterDoubleEditCheckViewModel Parameter01Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter02Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter03Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter04Vm { get; }

		public ParameterDoubleEditCheckViewModel Parameter101Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter102Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter103Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter104Vm { get; }

		public ParameterDoubleEditCheckViewModel Parameter201Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter202Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter203Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter204Vm { get; }

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

			Parameter01Vm = new ParameterDoubleEditCheckViewModel("105.01. Калибровка нуля тока фазы A", "f0", -10000, 10000, null);
			Parameter02Vm = new ParameterDoubleEditCheckViewModel("105.02. Калибровка нуля тока фазы B", "f0", -10000, 10000, null);
			Parameter03Vm = new ParameterDoubleEditCheckViewModel("105.03. Калибровка нуля тока фазы C", "f0", -10000, 10000, null);
			Parameter04Vm = new ParameterDoubleEditCheckViewModel("105.04. Калибровка нуля напряжения шины DC", "f0", -10000, 10000, null);

			Parameter101Vm = new ParameterDoubleEditCheckViewModel("105.01. Калибровка нуля тока фазы A", "f0", -10000, 10000, null);
			Parameter102Vm = new ParameterDoubleEditCheckViewModel("105.02. Калибровка нуля тока фазы B", "f0", -10000, 10000, null);
			Parameter103Vm = new ParameterDoubleEditCheckViewModel("105.03. Калибровка нуля тока фазы C", "f0", -10000, 10000, null);
			Parameter104Vm = new ParameterDoubleEditCheckViewModel("105.04. Калибровка нуля напряжения шины DC", "f0", -10000, 10000, null);

			Parameter201Vm = new ParameterDoubleEditCheckViewModel("105.01. Калибровка нуля тока фазы A", "f0", -10000, 10000, null);
			Parameter202Vm = new ParameterDoubleEditCheckViewModel("105.02. Калибровка нуля тока фазы B", "f0", -10000, 10000, null);
			Parameter203Vm = new ParameterDoubleEditCheckViewModel("105.03. Калибровка нуля тока фазы C", "f0", -10000, 10000, null);
			Parameter204Vm = new ParameterDoubleEditCheckViewModel("105.04. Калибровка нуля напряжения шины DC", "f0", -10000, 10000, null);

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
				var settingsPart = new AinSettingsPartWritable {
					Ia0 = ConvertDoubleToShort(Parameter01Vm.CurrentValue),
					Ib0 = ConvertDoubleToShort(Parameter02Vm.CurrentValue),
					Ic0 = ConvertDoubleToShort(Parameter03Vm.CurrentValue),
					Udc0 = ConvertDoubleToShort(Parameter04Vm.CurrentValue)
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

		private short? ConvertDoubleToShort(double? value) {
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
