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
	class Group20SettingsViewModel : ViewModelBase {
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
		public ParameterDoubleEditCheckViewModel Parameter07Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter08Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group20SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, IAinSettingsReaderWriter readerWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage storage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_storage = storage;
			_storageUpdatedNotify = storageUpdatedNotify;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleEditCheckViewModel("20.01. Максимальная частота", "f1", -10000, 10000, null);

			Parameter02Vm = new ParameterDoubleEditCheckViewModel("20.02. Ограничение тока (амплитутда)", "f0", -10000, 10000, null);
			Parameter03Vm = new ParameterDoubleEditCheckViewModel("20.03. Минимальная частота (электрическая)", "f1", -10000, 10000, null);

			Parameter04Vm = new ParameterDoubleEditCheckViewModel("20.04. Минимальный момент", "f0", -10000, 10000, null); // TODO: спросить Марата, в процентах или как задаётся момент.
			Parameter05Vm = new ParameterDoubleEditCheckViewModel("20.05. Максимальный момент", "f0", -10000, 10000, null);
			Parameter06Vm = new ParameterDoubleEditCheckViewModel("20.06. Тепловая защита, граница перегрева, А² × 0.1сек", "f0", -10000, 10000, null);
			Parameter07Vm = new ParameterDoubleEditCheckViewModel("20.07. Тепловая защита, номинальный ток, при котором остывание равно нагреву (RMS), А", "f0", -10000, 10000, null);
			Parameter08Vm = new ParameterDoubleEditCheckViewModel("20.08. Скорость вращения двигателя (электрическая) ниже нулевого предела (ZERO_SPEED), Гц", "f0", -10000, 10000, null); // TODO: * 0.1 при приёме

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
					Fmax = Parameter01Vm.CurrentValue,
					IoutMax = ConvertDoubleToShort(Parameter02Vm.CurrentValue),
					Fmin = Parameter03Vm.CurrentValue,
				};
				_readerWriter.WriteSettingsAsync(settingsPart, exception => {
					_uiRoot.Notifier.Notify(() => {
						if (exception != null) {
							_logger.Log("Ошибка при записи настроек. " + exception.Message);
						}
						else _logger.Log("Группа настроек была успешно записана");
					});
				});
			}
			catch (Exception ex) {
				_logger.Log("Не удалось записать группу настроек. " + ex.Message);
			}
		}

		private void ReadSettings() {
			// TODO: remove method from each group
			try {
				_readerWriter.ReadSettingsAsync(0, true, (ex, settings) => { }); // empty action, because settings will be updated OnAinSettingsReadComplete
			}
			catch (Exception ex) {
				_logger.Log("Не удалось прочитать группу настроек. " + ex.Message);
			}
		}

		private void UpdateSettingsInUiThread(Exception readInnerException, IAinSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (readInnerException != null) {
					//_logger.Log("Не удалось прочитать настройки АИН");
					Parameter01Vm.CurrentValue = null;
					Parameter02Vm.CurrentValue = null;
					Parameter03Vm.CurrentValue = null;
					Parameter04Vm.CurrentValue = null;
					Parameter05Vm.CurrentValue = null;
					return;
				}

				Parameter01Vm.CurrentValue = settings.Fmax;
				Parameter02Vm.CurrentValue = settings.IoutMax;
				Parameter03Vm.CurrentValue = settings.Fmin;

				//Parameter05Vm.CurrentValue = settings.Fmax;
				//Parameter06Vm.CurrentValue = settings.Fmin;
			});
		}
		private short? ConvertDoubleToShort(double? value) {
			if (!value.HasValue) return null;
			return (short)value.Value;
		}
	}
}
