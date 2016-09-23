using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group100SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinSettingsReaderWriter _readerWriter;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsStorage _storage;
		private readonly IAinSettingsStorageUpdatedNotify _storageUpdatedNotify;
		private readonly IAinsCounter _ainsCounter;

		public ParameterDoubleEditableViewModel Parameter01Vm { get; }
		public ParameterDoubleEditableViewModel Parameter02Vm { get; }
		public ParameterDoubleEditableViewModel Parameter03Vm { get; }
		public ParameterDoubleEditableViewModel Parameter04Vm { get; }

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

			Parameter01Vm = new ParameterDoubleEditableViewModel("100.01. Пропорциональный коэф. регулятора тока D", "f6", -10000, 10000, null);
			Parameter02Vm = new ParameterDoubleEditableViewModel("100.02. Интегральный коэф. регулятора тока D", "f6", -10000, 10000, null);

			Parameter03Vm = new ParameterDoubleEditableViewModel("100.03. Пропорциональный коэф. регулятора тока Q", "f6", -10000, 10000, null);
			Parameter04Vm = new ParameterDoubleEditableViewModel("100.04. Интегральный коэф. регулятора тока Q", "f6", -10000, 10000, null);


			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
			WriteSettingsCmd = new RelayCommand(WriteSettings, () => IsWriteEnabled); // TODO: read only when connected to COM

			_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete;
			_storageUpdatedNotify.AinSettingsUpdated += (zbAinNuber, settings) => {
				_uiRoot.Notifier.Notify(()=>WriteSettingsCmd.RaiseCanExecuteChanged());
			};
		}

		private bool IsWriteEnabled
		{
			get
			{
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
					KpId = ConvertDoubleToShort(Parameter01Vm.CurrentValue * 16777216.0),
					KiId = ConvertDoubleToShort(Parameter02Vm.CurrentValue * 16777216.0),
					KpIq = ConvertDoubleToShort(Parameter03Vm.CurrentValue * 16777216.0),
					KiIq = ConvertDoubleToShort(Parameter04Vm.CurrentValue * 16777216.0),
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
				Parameter01Vm.CurrentValue = settings.KpId / 16777216.0;
				Parameter02Vm.CurrentValue = settings.KiId / 16777216.0;
				Parameter03Vm.CurrentValue = settings.KpIq / 16777216.0;
				Parameter04Vm.CurrentValue = settings.KiIq / 16777216.0;
			});
		}
	}
}
