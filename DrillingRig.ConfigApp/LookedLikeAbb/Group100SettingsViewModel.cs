﻿using System;
using AlienJust.Support.Collections;
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
	class Group100SettingsViewModel : ViewModelBase {
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

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group100SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage storage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_storage = storage;
			_storageUpdatedNotify = storageUpdatedNotify;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDecimalEditCheckViewModel("100.01. Пропорциональный коэф. регулятора тока D , ID Kp", "f8", -128.0m, 127.99609375m) { Increment = 0.00390625m };
			Parameter02Vm = new ParameterDecimalEditCheckViewModel("100.02. Интегральный коэф. регулятора тока D , ID Ki", "f6", -128.0m, 128.0m) { Increment = 0.000001m }; // min step = 1 / 16777216.0 = 0,000000059604644775390625

			Parameter03Vm = new ParameterDecimalEditCheckViewModel("100.03. Пропорциональный коэф. регулятора тока Q , IQ Kp", "f8", -128.0m, 127.99609375m) { Increment = 0.00390625m };
			Parameter04Vm = new ParameterDecimalEditCheckViewModel("100.04. Интегральный коэф. регулятора тока Q , IQ Ki", "f6", -128.0m, 128.0m) { Increment = 0.000001m }; // min step = 1 / 16777216.0 = 0,000000059604644775390625


			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
			WriteSettingsCmd = new RelayCommand(WriteSettings, () => IsWriteEnabled); // TODO: read only when connected to COM

			_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete;
			_storageUpdatedNotify.AinSettingsUpdated += (zbAinNuber, settings) => {
				_uiRoot.Notifier.Notify(() => WriteSettingsCmd.RaiseCanExecuteChanged());
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
				//_uiRoot.Notifier.Notify(() => { _logger.Log("Группа настроек успешно прочитана"); });
				UpdateSettingsInUiThread(readInnerException, settings);
			}
		}

		private void WriteSettings() {
			try {
				_uiRoot.Notifier.Notify(() => { _logger.Log("Запись группы настроек..."); });
				var settingsPart = new AinSettingsPartWritable {
					KpId = Parameter01Vm.CurrentValue,
					KiId = Parameter02Vm.CurrentValue,
					KpIq = Parameter03Vm.CurrentValue,
					KiIq = Parameter04Vm.CurrentValue
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
				_uiRoot.Notifier.Notify(() => { _logger.Log("Чтение группы настроек..."); });
				_readerWriter.ReadSettingsAsync(0, true, (exception, settings) => { });
			}
			catch (Exception ex) {
				_logger.Log("Не удалось прочитать группу настроек. " + ex.Message);
			}
		}

		private void UpdateSettingsInUiThread(Exception exception, IAinSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (exception != null) {
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
		}
	}
}
