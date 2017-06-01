using System;
using System.Linq;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboEditable;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboIntEditable;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleEditCheck;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group102SettingsViewModel : ViewModelBase {
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
		public ParameterComboEditableViewModel<AinTelemetryFanWorkmode> Parameter08Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group102SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage storage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_storage = storage;
			_storageUpdatedNotify = storageUpdatedNotify;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleEditCheckViewModel("102.01. Постоянная времени ротора", "f4", -10000, 10000, null) { Increment = 0.0001 };
			Parameter02Vm = new ParameterDoubleEditCheckViewModel("102.02. Индуктивность намагничивания", "f5", -10000, 10000, null) { Increment = 0.00001 };
			Parameter03Vm = new ParameterDoubleEditCheckViewModel("102.03. Индуктивность рассеяния статора", "f6", -10000, 10000, null) { Increment = 0.000001 };
			Parameter04Vm = new ParameterDoubleEditCheckViewModel("102.04. Индуктивность рассеяния ротора", "f6", -10000, 10000, null) { Increment = 0.000001 };
			Parameter05Vm = new ParameterDoubleEditCheckViewModel("102.05. Активное сопротивление статора", "f4", -10000, 10000, null) { Increment = 0.0001 };
			Parameter06Vm = new ParameterDoubleEditCheckViewModel("102.06. Число пар полюсов (не путать с числом полюсов) АД", "f0", 0, 31, null);

			Parameter07Vm = new ParameterComboEditableViewModel<int>("102.07. Число импульсов ДЧВ",
				new[]
				{
					new ComboItemViewModel<int> {ComboText = "256", ComboValue = 0}
					, new ComboItemViewModel<int> {ComboText = "512", ComboValue = 1}
					, new ComboItemViewModel<int> {ComboText = "1024", ComboValue = 2}
					, new ComboItemViewModel<int> {ComboText = "2048", ComboValue = 3}
					, new ComboItemViewModel<int> {ComboText = "4096", ComboValue = 4}
					, new ComboItemViewModel<int> {ComboText = "8192", ComboValue = 5}
					, new ComboItemViewModel<int> {ComboText = "16384", ComboValue = 6}
					, new ComboItemViewModel<int> {ComboText = "32768", ComboValue = 7}
				});

			Parameter08Vm = new ParameterComboEditableViewModel<AinTelemetryFanWorkmode>("102.08. Режим работы вентилятора",
				new[]
				{
					new ComboItemViewModel<AinTelemetryFanWorkmode> {ComboText = AinTelemetryFanWorkmode.AllwaysOff.ToHumanString(), ComboValue = AinTelemetryFanWorkmode.AllwaysOff}
					, new ComboItemViewModel<AinTelemetryFanWorkmode> {ComboText = AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffTwoMinutesLaterAfterPwmOff.ToHumanString(), ComboValue = AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffTwoMinutesLaterAfterPwmOff}
					, new ComboItemViewModel<AinTelemetryFanWorkmode> {ComboText = AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffAfterPwmOffAndTempGoesDownBelow45C.ToHumanString(), ComboValue = AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffAfterPwmOffAndTempGoesDownBelow45C}
					, new ComboItemViewModel<AinTelemetryFanWorkmode> {ComboText = AinTelemetryFanWorkmode.AllwaysOn.ToHumanString(), ComboValue = AinTelemetryFanWorkmode.AllwaysOn}
				});


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
					TauR = Parameter01Vm.CurrentValue * 10000.0,
					Lm = Parameter02Vm.CurrentValue * 100000.0,
					Lsl = Parameter03Vm.CurrentValue * 1000000.0,
					Lrl = Parameter04Vm.CurrentValue,
					Rs = Parameter05Vm.CurrentValue,
					Np = ConvertDoubleToShort(Parameter06Vm.CurrentValue),
					NimpFloorCode = Parameter07Vm.SelectedComboItem.ComboValue,
					FanMode = Parameter08Vm.SelectedComboItem.ComboValue
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
			return (short)value.Value;
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
					Parameter07Vm.SelectedComboItem = null;
					Parameter08Vm.SelectedComboItem = null;
					return;
				}

				Parameter01Vm.CurrentValue = settings.TauR;
				Parameter02Vm.CurrentValue = settings.Lm;
				Parameter03Vm.CurrentValue = settings.Lsl;
				Parameter04Vm.CurrentValue = settings.Lrl;
				Parameter05Vm.CurrentValue = settings.Rs;
				Parameter06Vm.CurrentValue = settings.Np;
				Parameter07Vm.SelectedComboItem = Parameter07Vm.ComboItems.FirstOrDefault(ci => ci.ComboValue == settings.NimpFloorCode);
				Parameter08Vm.SelectedComboItem = Parameter08Vm.ComboItems.FirstOrDefault(ci => ci.ComboValue == settings.FanMode);
			});
		}
	}
}
