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
	class Group27SettingsViewModel : ViewModelBase {
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
		public ParameterDoubleEditCheckViewModel Parameter05Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter06Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group27SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage storage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_storage = storage;
			_storageUpdatedNotify = storageUpdatedNotify;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleEditCheckViewModel("27.01. Поток без ослабления поля, мВб", "f3", -32.768, 32.767, null) { Increment = 0.001 };
			Parameter02Vm = new ParameterDoubleEditCheckViewModel("27.02. Минимальный поток с ослаблением поля, мВб", "f3", -32.768, 32.767, null) { Increment = 0.001 };
			Parameter03Vm = new ParameterDoubleEditCheckViewModel("27.03. Максимально возможная компенсация потока, мВб", "f3", -32.768, 32.767, null) { Increment = 0.001 };
			Parameter04Vm = new ParameterDoubleEditCheckViewModel("27.04. Минимальный возможный поток (коэф. от номинала), мВб", "f3", -32.768, 32.767, null) {Increment = 0.001};
			Parameter05Vm = new ParameterDoubleEditCheckViewModel("27.05. Постоянная времени регулятора компенсации напр-я, мс", "f3", -3.2768, 3.2767, null) { Increment = 0.0001 };
			Parameter06Vm = new ParameterDoubleEditCheckViewModel("27.06. Порог компенсации напряжения DC за счет потока, В", "f3", -32.768, 32.767, null) { Increment = 0.001 };

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
				UpdateSettingsInUiThread(readInnerException, settings);
			}
		}

		private void WriteSettings() {
			try {
				var settingsPart = new AinSettingsPartWritable {
					FiNom = Parameter01Vm.CurrentValue,
					FiMin = Parameter02Vm.CurrentValue,
					DflLim = (short)Parameter03Vm.CurrentValue,
					FlMinMin = Parameter04Vm.CurrentValue
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
					Parameter01Vm.CurrentValue = null;
					Parameter02Vm.CurrentValue = null;
					Parameter03Vm.CurrentValue = null;
					Parameter04Vm.CurrentValue = null;
					Parameter05Vm.CurrentValue = null;
					Parameter06Vm.CurrentValue = null;
					return;
				}

				Parameter01Vm.CurrentValue = settings.FiNom;
				Parameter02Vm.CurrentValue = settings.FiMin;
				Parameter03Vm.CurrentValue = settings.DflLim;
				Parameter04Vm.CurrentValue = settings.FlMinMin;
				Parameter05Vm.CurrentValue = settings.TauFlLim;
				Parameter06Vm.CurrentValue = settings.UmodThr;
			});
		}
	}
}
