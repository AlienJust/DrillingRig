using System;
using System.Linq;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboEditable;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboIntEditable;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleEditCheck;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group99SettingsViewModel : ViewModelBase {
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
		public ParameterComboEditableViewModel<int> Parameter07Vm { get; }

		public ParameterDoubleEditCheckViewModel Parameter08Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter09Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter10Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter11Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter12Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group99SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage storage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_storage = storage;
			_storageUpdatedNotify = storageUpdatedNotify;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleEditCheckViewModel("99.01. Номинальное напряжение двигателя, В", "f0", 0, 10000, null);
			Parameter02Vm = new ParameterDoubleEditCheckViewModel("99.02. Номинальный ток двигателя, А", "f1", 0, 10000, null);
			Parameter03Vm = new ParameterDoubleEditCheckViewModel("99.03. Номинальная частота двигателя, Гц", "f2", 8, 300, null);
			Parameter04Vm = new ParameterDoubleEditCheckViewModel("99.04. Номинальная скорость двигателя, об/мин", "f0", 0, 18000, null);
			Parameter05Vm = new ParameterDoubleEditCheckViewModel("99.05. Максимальная скорость двигателя, об/мин", "f0", 0, 18000, null); // TODO: продублировать в группе скроростей (21 вроде бы)
			Parameter06Vm = new ParameterDoubleEditCheckViewModel("99.06. Номинальная мощность двигателя, кВт", "f2", 0, 9000, null);
			Parameter07Vm = new ParameterComboEditableViewModel<int>("99.07. Режим управления двигателем",
				new[]
				{
					new ComboItemViewModel<int> {ComboText = "Скалярный", ComboValue = 0}
					, new ComboItemViewModel<int> {ComboText = "Векторный", ComboValue = 1}
				});
			Parameter08Vm = new ParameterDoubleEditCheckViewModel("99.08. cos(φ)", "f2", 0, 1.0, null);
			Parameter09Vm = new ParameterDoubleEditCheckViewModel("99.09. Кпд двигателя, %", "f2", 0, 100.0, null);
			Parameter10Vm = new ParameterDoubleEditCheckViewModel("99.10. Масса двигателя, кг", "f2", 0, 10000, null);
			Parameter11Vm = new ParameterDoubleEditCheckViewModel("99.11. Кратность момента (Mm/Mnom)", "f2", 0, 10000, null);
			Parameter12Vm = new ParameterDoubleEditCheckViewModel("99.12. Конструктивная высота, мм", "f2", 0, 10000, null);

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
					Unom = Parameter01Vm.CurrentValue,
					Fnom = Parameter03Vm.CurrentValue
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
					Parameter03Vm.CurrentValue = null;
					Parameter07Vm.SelectedComboItem = null;
					return;
				}
				Parameter01Vm.CurrentValue = settings.Unom;
				Parameter03Vm.CurrentValue = settings.Fnom;
				int comboValue = (settings.Imcw & 0x0080) == 0x0080 ? 1 : 0;
				Parameter07Vm.SelectedComboItem = Parameter07Vm.ComboItems.First(ci => ci.ComboValue == comboValue);
			});
		}
	}
}
