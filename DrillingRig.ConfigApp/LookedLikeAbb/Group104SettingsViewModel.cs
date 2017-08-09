using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group104SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinSettingsReaderWriter _readerWriter;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsStorage _storage;
		private readonly IAinSettingsStorageUpdatedNotify _storageUpdatedNotify;
		private readonly IAinsCounter _ainsCounter;

		//public ParameterDoubleEditCheckViewModel Parameter01Vm { get; }
		//public ParameterDoubleEditCheckViewModel Parameter02Vm { get; }
		//public ParameterDoubleEditCheckViewModel Parameter03Vm { get; }
		//public ParameterDoubleEditCheckViewModel Parameter04Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group104SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage storage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_storage = storage;
			_storageUpdatedNotify = storageUpdatedNotify;
			_ainsCounter = ainsCounter;

			//Parameter01Vm = new ParameterDoubleEditCheckViewModel("104.01. Максимально возможная компенсация потока", "f3", -10000, 10000, null) {Increment = 0.1};
			//Parameter02Vm = new ParameterDoubleEditCheckViewModel("104.02. Минимальный возможный поток (в % от номинала)", "f0", -10000, 10000, null);
			//Parameter03Vm = new ParameterDoubleEditCheckViewModel("104.03. Постоянная времени регулятора компенсации напр-я, мс", "f4", -3.2768, 3.2767, null) { Increment = 0.0001 };
			//Parameter04Vm = new ParameterDoubleEditCheckViewModel("104.04. Порог компенсации напряжения DC за счет потока", "f3", -10000, 10000, null) { Increment = 0.1 };

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
				_uiRoot.Notifier.Notify(() => { _logger.Log("Группа настроек успешно прочитана"); });
				UpdateSettingsInUiThread(readInnerException, settings);
			}
		}

		private void WriteSettings() {
			try {
				_uiRoot.Notifier.Notify(() => { _logger.Log("Запись группы настроек..."); });
				var settingsPart = new AinSettingsPartWritable {
					//DflLim = Parameter01Vm.CurrentValue,
					//FlMinMin = ConvertDoubleToShort(Parameter02Vm.CurrentValue),
					//TauFlLim = Parameter03Vm.CurrentValue,
					//UmodThr = Parameter04Vm.CurrentValue
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
					//_logger.Log("Не удалось прочитать настройки АИН");
					//Parameter01Vm.CurrentValue = null;
					//Parameter02Vm.CurrentValue = null;
					//Parameter03Vm.CurrentValue = null;
					//Parameter04Vm.CurrentValue = null;
					return;
				}

				//Parameter01Vm.CurrentValue = settings.DflLim;
				//Parameter02Vm.CurrentValue = settings.FlMinMin;
				//Parameter03Vm.CurrentValue = settings.TauFlLim;
				//Parameter04Vm.CurrentValue = settings.UmodThr;
			});
		}
	}
}
