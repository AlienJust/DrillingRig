using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.Commands.EngineSettings;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.AppControl.EngineSettingsSpace;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleEditCheck;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group20SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;

		private readonly IAinSettingsReaderWriter _ainSettingsReaderWriter;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsStorage _ainSettingsStorage;
		private readonly IAinSettingsStorageUpdatedNotify _ainSettingsStorageUpdatedNotify;
		private readonly IAinsCounter _ainsCounter;

		private readonly IEngineSettingsReader _engineSettingsReader;
		private readonly IEngineSettingsWriter _engineSettingsWriter;
		private readonly IEngineSettingsReadNotify _engineSettingsReadNotify;
		private readonly IEngineSettingsStorage _engineSettingsStorage;
		private readonly IEngineSettingsStorageUpdatedNotify _engineSettingsStorageUpdatedNotify;

		public ParameterDecimalEditCheckViewModel Parameter01Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter02Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter03Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter04Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter05Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter06Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter07Vm { get; }

		public ParameterDecimalEditCheckViewModel Parameter08Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter09Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter10Vm { get; }

		public ParameterDecimalEditCheckViewModel Parameter11Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter12Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group20SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger,
			IAinSettingsReaderWriter ainSettingsReaderWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage ainSettingsStorage, IAinSettingsStorageUpdatedNotify ainSettingsStorageUpdatedNotify, IAinsCounter ainsCounter,
			IEngineSettingsReader engineSettingsReader, IEngineSettingsWriter engineSettingsWriter, IEngineSettingsReadNotify engineSettingsReadNotify, IEngineSettingsStorage engineSettingsStorage, IEngineSettingsStorageUpdatedNotify engineSettingsStorageUpdatedNotify) {

			_uiRoot = uiRoot;
			_logger = logger;

			_ainSettingsReaderWriter = ainSettingsReaderWriter;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_ainSettingsStorage = ainSettingsStorage;
			_ainSettingsStorageUpdatedNotify = ainSettingsStorageUpdatedNotify;
			_ainsCounter = ainsCounter;

			_engineSettingsReader = engineSettingsReader;
			_engineSettingsWriter = engineSettingsWriter;
			_engineSettingsReadNotify = engineSettingsReadNotify;
			_engineSettingsStorage = engineSettingsStorage;
			_engineSettingsStorageUpdatedNotify = engineSettingsStorageUpdatedNotify;

			Parameter01Vm = new ParameterDecimalEditCheckViewModel("20.01. Максимальная частота, Гц", "f1", 0, 6553.5m);

			Parameter02Vm = new ParameterDecimalEditCheckViewModel("20.02. Ограничение тока (амплитутда), А", "f0", -10000, 10000);
			Parameter03Vm = new ParameterDecimalEditCheckViewModel("20.03. Минимальная частота (электрическая), Гц", "f1", -3276.8m, 3276.7m);

			Parameter04Vm = new ParameterDecimalEditCheckViewModel("20.04. Максимальный ток (амплитуда) для защиты, А", "f0", -32768, 32767);
			Parameter05Vm = new ParameterDecimalEditCheckViewModel("20.05. Максимальное напряжение шины DC для защиты, В", "f0", -1000, 1000);
			Parameter06Vm = new ParameterDecimalEditCheckViewModel("20.06. Минимальное напряжение шины DC, В", "f0", -1000, 1000);
			Parameter07Vm = new ParameterDecimalEditCheckViewModel("20.07. Порог защиты по внешней температуре", "f0", -32768, 32767);


			Parameter08Vm = new ParameterDecimalEditCheckViewModel("20.08. Тепловая защита, граница перегрева, А² × 0.1сек", "f0", -10000, 10000);
			Parameter09Vm = new ParameterDecimalEditCheckViewModel("20.09. Тепловая защита, номинальный ток, при котором остывание равно нагреву (RMS), А", "f0", -10000, 10000);
			Parameter10Vm = new ParameterDecimalEditCheckViewModel("20.10. Скорость вращения двигателя (электрическая) ниже нулевого предела (ZERO_SPEED), Гц", "f0", -10000, 10000);

			Parameter11Vm = new ParameterDecimalEditCheckViewModel("20.11. Минимальный момент", "f0", -10000, 10000); // TODO: WTF?
			Parameter12Vm = new ParameterDecimalEditCheckViewModel("20.12. Максимальный момент", "f0", -10000, 10000); // TODO: WTF?

			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
			WriteSettingsCmd = new RelayCommand(WriteSettings, () => IsWriteEnabled); // TODO: read only when connected to COM

			_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete;
			_ainSettingsStorageUpdatedNotify.AinSettingsUpdated += (zbAinNuber, settings) => {
				_uiRoot.Notifier.Notify(() => WriteSettingsCmd.RaiseCanExecuteChanged());
			};

			_engineSettingsReadNotify.EngineSettingsReadComplete += EngineSettingsReadNotifyOnEngineSettingsReadComplete;
			_engineSettingsStorageUpdatedNotify.EngineSettingsUpdated += settings => {
				_uiRoot.Notifier.Notify(() => WriteSettingsCmd.RaiseCanExecuteChanged());
			};
		}

		private void EngineSettingsReadNotifyOnEngineSettingsReadComplete(Exception readInnerException, IEngineSettings settings) {
			UpdateEngineSettingsInUiThread(readInnerException, settings);
		}

		private bool AnyAinParameterSetted => Parameter01Vm.CurrentValue.HasValue
			|| Parameter02Vm.CurrentValue.HasValue
			|| Parameter03Vm.CurrentValue.HasValue
			|| Parameter04Vm.CurrentValue.HasValue
			|| Parameter05Vm.CurrentValue.HasValue
			|| Parameter06Vm.CurrentValue.HasValue
			|| Parameter07Vm.CurrentValue.HasValue;

		private bool AnyEngineParameterSetted => Parameter08Vm.CurrentValue.HasValue || Parameter09Vm.CurrentValue.HasValue || Parameter10Vm.CurrentValue.HasValue;

		private bool IsWriteEnabled {
			get {
				bool resultForAin = false;
				if (AnyAinParameterSetted) {
					resultForAin = true;
					for (byte i = 0; i < _ainsCounter.SelectedAinsCount; ++i) {
						var curAinSettings = _ainSettingsStorage.GetSettings(i);
						if (curAinSettings == null) {
							resultForAin = false; // TODO: по идее еще можно проверять AinLinkFault внутри настроек
							break;
						}
					}
				}
				bool resultForEngine = AnyEngineParameterSetted && _engineSettingsStorage.EngineSettings != null;
				return resultForAin || resultForEngine;
			}
		}

		private void AinSettingsReadNotifyOnAinSettingsReadComplete(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings) {
			if (zeroBasedAinNumber == 0) {
				_uiRoot.Notifier.Notify(() => { _logger.Log("Группа настроек успешно прочитана"); });
				UpdateAinSettingsInUiThread(readInnerException, settings);
			}
		}

		private void WriteSettings() {
			try {
				// А зачем отправлять команду. если ничего нет? :)
				if (AnyAinParameterSetted) {
					_uiRoot.Notifier.Notify(() => { _logger.Log("Запись группы настроек..."); });
					var settingsPart = new AinSettingsPartWritable {
						Fmax = Parameter01Vm.CurrentValue,
						IoutMax = ConvertDecimalToShort(Parameter02Vm.CurrentValue),
						Fmin = Parameter03Vm.CurrentValue,
						Imax = (short)Parameter04Vm.CurrentValue,
						UdcMin = (short)Parameter05Vm.CurrentValue,
						UdcMax = (short)Parameter06Vm.CurrentValue,
						TextMax = (short)Parameter07Vm.CurrentValue
					};
					_ainSettingsReaderWriter.WriteSettingsAsync(settingsPart, exception => {
						_uiRoot.Notifier.Notify(() => {
							if (exception != null) {
								_logger.Log("Ошибка при записи настроек. " + exception.Message);
							}
							else _logger.Log("Группа настроек была успешно записана");
						});
					});
				}

				// А зачем отправлять команду. если ничего нет? :)
				if (AnyEngineParameterSetted) {
					_uiRoot.Notifier.Notify(() => { _logger.Log("Запись настроек двигателя..."); });
					var settingsPart = new EngineSettingsPartWritable {
						I2Tmax = ConvertDecimalToUint(Parameter08Vm.CurrentValue),
						Icontinious = ConvertDecimalToUshort(Parameter09Vm.CurrentValue),
						ZeroF = ConvertDecimalToUshort(Parameter10Vm.CurrentValue)
					};
					_engineSettingsWriter.WriteSettingsAsync(settingsPart, exception => {
						if (exception != null) {
							_logger.Log("Ошибка при записи настроек двигателя. " + exception.Message);
						}
						else _logger.Log("Настройки двигателя были успешно записаны");
					});
				}
			}
			catch (Exception ex) {
				_logger.Log("Не удалось записать группу настроек. " + ex.Message);
			}
		}

		private void ReadSettings() {
			// TODO: remove method from each group
			try {
				_uiRoot.Notifier.Notify(() => { _logger.Log("Чтение группы настроек..."); });
				_ainSettingsReaderWriter.ReadSettingsAsync(0, true, (ex, settings) => { }); // empty action, because settings will be updated OnAinSettingsReadComplete
				_uiRoot.Notifier.Notify(() => { _logger.Log("Чтение настроек двигателя..."); });
				_engineSettingsReader.ReadSettingsAsync(true, (ex, settings) => { });
			}
			catch (Exception ex) {
				_logger.Log("Не удалось прочитать группу настроек. " + ex.Message);
			}
		}

		private void UpdateEngineSettingsInUiThread(Exception readInnerException, IEngineSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (readInnerException != null) {
					//_logger.Log("Не удалось прочитать настройки АИН");
					Parameter08Vm.CurrentValue = null;
					Parameter09Vm.CurrentValue = null;
					Parameter10Vm.CurrentValue = null;
					return;
				}

				Parameter08Vm.CurrentValue = settings.I2Tmax;
				Parameter09Vm.CurrentValue = settings.Icontinious;
				Parameter10Vm.CurrentValue = settings.ZeroF;
			});
		}

		private void UpdateAinSettingsInUiThread(Exception readInnerException, IAinSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (readInnerException != null) {
					//_logger.Log("Не удалось прочитать настройки АИН");
					Parameter01Vm.CurrentValue = null;
					Parameter02Vm.CurrentValue = null;
					Parameter03Vm.CurrentValue = null;
					Parameter04Vm.CurrentValue = null;
					Parameter05Vm.CurrentValue = null;
					Parameter06Vm.CurrentValue = null;
					Parameter07Vm.CurrentValue = null;
					return;
				}

				Parameter01Vm.CurrentValue = settings.Fmax;
				Parameter02Vm.CurrentValue = settings.IoutMax;
				Parameter03Vm.CurrentValue = settings.Fmin;
				Parameter04Vm.CurrentValue = settings.Imax;
				Parameter05Vm.CurrentValue = settings.UdcMin;
				Parameter06Vm.CurrentValue = settings.UdcMax;
				Parameter07Vm.CurrentValue = settings.TextMax;
			});
		}

		private short? ConvertDecimalToShort(decimal? value) {
			if (!value.HasValue) return null;
			return (short)value.Value;
		}

		private ushort? ConvertDecimalToUshort(decimal? value) {
			if (!value.HasValue) return null;
			return (ushort)value.Value;
		}

		private uint? ConvertDecimalToUint(decimal? value) {
			if (!value.HasValue) return null;
			return (uint)value.Value;
		}
	}
}
