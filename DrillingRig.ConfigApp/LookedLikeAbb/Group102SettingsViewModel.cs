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
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanEditCheck;
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

		public ParameterDecimalEditCheckViewModel Parameter01Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter02Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter03Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter04Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter05Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter06Vm { get; }
		public ParameterComboEditableViewModel<int> Parameter07Vm { get; }
		public ParameterComboEditableViewModel<AinTelemetryFanWorkmode> Parameter08Vm { get; }
		public ParameterBooleanEditCheckViewModel Parameter09Vm { get; }

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

			Parameter01Vm = new ParameterDecimalEditCheckViewModel("102.01. Постоянная времени ротора, мс", "f4", -3.2768m, 3.2767m) { Increment = 0.0001m };
			Parameter02Vm = new ParameterDecimalEditCheckViewModel("102.02. Индуктивность намагничивания, мкГн", "f5", -0.32768m, 0.32767m) { Increment = 0.00001m };
			Parameter03Vm = new ParameterDecimalEditCheckViewModel("102.03. Индуктивность рассеяния статора, мкГн", "f6", -0.032768m, 0.032768m) { Increment = 0.000001m };
			Parameter04Vm = new ParameterDecimalEditCheckViewModel("102.04. Индуктивность рассеяния ротора, мкГн", "f6", -0.032768m, 0.032768m) { Increment = 0.000001m };
			Parameter05Vm = new ParameterDecimalEditCheckViewModel("102.05. Активное сопротивление статора", "f4", -3.2768m, 3.2767m) { Increment = 0.0001m };
			Parameter06Vm = new ParameterDecimalEditCheckViewModel("102.06. Число пар полюсов (не путать с числом полюсов) АД", "f0", 0, 31);

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
			Parameter09Vm = new ParameterBooleanEditCheckViewModel("102.09. Намагничивание постоянным током", "Нет", "Да");


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
					TauR = Parameter01Vm.CurrentValue,
					Lm = Parameter02Vm.CurrentValue,
					Lsl = Parameter03Vm.CurrentValue,
					Lrl = Parameter04Vm.CurrentValue,
					Rs = Parameter05Vm.CurrentValue,
					Np = ConvertDecimalToShort(Parameter06Vm.CurrentValue),
					NimpFloorCode = Parameter07Vm.SelectedComboItem.ComboValue,
					FanMode = Parameter08Vm.SelectedComboItem.ComboValue,
					DirectCurrentMagnetization = Parameter09Vm.Value
				};
				_readerWriter.WriteSettingsAsync(settingsPart, exception => {
					_uiRoot.Notifier.Notify(() => {
						if (exception != null) {
							_logger.Log("Ошибка при записи настроек АИН. " + exception.Message);
						}
						else _logger.Log("Группа настроек 102 была успешно записана");
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

		private short? ConvertDecimalToShort(decimal? value) {
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
					Parameter09Vm.Value = null;
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
				Parameter09Vm.Value = settings.DirectCurrentMagnetization;
			});
		}
	}
}
