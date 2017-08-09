using System;
using System.ComponentModel;
using System.Linq;
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
using DrillingRig.ConfigApp.LookedLikeAbb.Group106Settings.ImvcParameter;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboEditable;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboIntEditable;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleEditCheck;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group99SettingsViewModel : ViewModelBase {
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
		private readonly ImcwParameterViewModel _imcwParameterVm;

		public ParameterDecimalEditCheckViewModel Parameter01Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter02Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter03Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter04Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter05Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter06Vm { get; }
		public ParameterComboEditableViewModel<int> Parameter07Vm { get; }

		public ParameterDecimalEditCheckViewModel Parameter08Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter09Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter10Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter11Vm { get; }
		public ParameterDecimalEditCheckViewModel Parameter12Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group99SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger,
			IAinSettingsReaderWriter ainSettingsReaderWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage ainSettingsStorage, IAinSettingsStorageUpdatedNotify ainSettingsStorageUpdatedNotify, IAinsCounter ainsCounter,
			IEngineSettingsReader engineSettingsReader, IEngineSettingsWriter engineSettingsWriter, IEngineSettingsReadNotify engineSettingsReadNotify, IEngineSettingsStorage engineSettingsStorage, IEngineSettingsStorageUpdatedNotify engineSettingsStorageUpdatedNotify,
			ImcwParameterViewModel imcwParameterVm) {
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

			_imcwParameterVm = imcwParameterVm;


			Parameter01Vm = new ParameterDecimalEditCheckViewModel("99.01. Номинальное напряжение двигателя (действующее), В", "f0", 0, 10000);
			Parameter02Vm = new ParameterDecimalEditCheckViewModel("99.02. Номинальный ток двигателя, А", "f0", 0, 10000);
			Parameter03Vm = new ParameterDecimalEditCheckViewModel("99.03. Номинальная частота двигателя, Гц", "f1", 8, 300);
			Parameter04Vm = new ParameterDecimalEditCheckViewModel("99.04. Номинальная скорость двигателя, об/мин", "f0", 0, 18000);
			Parameter05Vm = new ParameterDecimalEditCheckViewModel("99.05. Максимальная скорость двигателя, об/мин", "f0", 0, 18000);
			Parameter06Vm = new ParameterDecimalEditCheckViewModel("99.06. Номинальная мощность двигателя, кВт", "f3", 0, 9000);
			Parameter07Vm = new ParameterComboEditableViewModel<int>("99.07. Режим управления двигателем",
				new[]
				{
					new ComboItemViewModel<int> {ComboText = "Скалярный", ComboValue = 0}
					, new ComboItemViewModel<int> {ComboText = "Векторный", ComboValue = 1}
				});
			Parameter07Vm.PropertyChanged += Parameter07VmOnPropertyChanged;

			_imcwParameterVm.PropertyChanged += ImcwParameterVmOnPropertyChanged;

			Parameter08Vm = new ParameterDecimalEditCheckViewModel("99.08. cos(φ)", "f2", 0, 1.0m);
			Parameter09Vm = new ParameterDecimalEditCheckViewModel("99.09. Кпд двигателя, %", "f1", 0, 100.0m);
			Parameter10Vm = new ParameterDecimalEditCheckViewModel("99.10. Масса двигателя, кг", "f0", 0, 10000);
			Parameter11Vm = new ParameterDecimalEditCheckViewModel("99.11. Кратность момента (Mm/Mnom)", "f0", 0, 10000);
			Parameter12Vm = new ParameterDecimalEditCheckViewModel("99.12. Конструктивная высота, мм", "f0", 0, 10000);

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

		private void ImcwParameterVmOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
			if (propertyChangedEventArgs.PropertyName == "Bit13") {
				if (_imcwParameterVm.Bit13.HasValue) {
					Parameter07Vm.SelectedComboItem = _imcwParameterVm.Bit13.Value ? Parameter07Vm.ComboItems.First() : Parameter07Vm.ComboItems.Last();
				}
				else Parameter07Vm.SelectedComboItem = null;
			}
		}

		private void Parameter07VmOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
			if (propertyChangedEventArgs.PropertyName == "SelectedComboItem") {
				if (Parameter07Vm.SelectedComboItem == null) _imcwParameterVm.Bit13 = null;
				else _imcwParameterVm.Bit13 = Parameter07Vm.SelectedComboItem.ComboValue == 0;
			}
		}

		private bool AnyAinParameterSetted => Parameter01Vm.CurrentValue.HasValue
					|| Parameter03Vm.CurrentValue.HasValue
					|| Parameter07Vm.SelectedComboItem != null;

		private bool AnyEngineParameterSetted => Parameter02Vm.CurrentValue.HasValue
			|| Parameter04Vm.CurrentValue.HasValue
			|| Parameter05Vm.CurrentValue.HasValue
			|| Parameter06Vm.CurrentValue.HasValue
			|| Parameter08Vm.CurrentValue.HasValue
			|| Parameter09Vm.CurrentValue.HasValue
			|| Parameter10Vm.CurrentValue.HasValue
			|| Parameter11Vm.CurrentValue.HasValue
			|| Parameter12Vm.CurrentValue.HasValue;

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
				UpdateSettingsInUiThread(readInnerException, settings);
			}
		}

		private void EngineSettingsReadNotifyOnEngineSettingsReadComplete(Exception readInnerException, IEngineSettings settings) {
			UpdateEngineSettingsInUiThread(readInnerException, settings);
		}

		private void WriteSettings() {
			try {
				if (AnyAinParameterSetted) {
					_uiRoot.Notifier.Notify(() => { _logger.Log("Запись группы настроек..."); });
					var settingsPart = new AinSettingsPartWritable {
						Unom = Parameter01Vm.CurrentValue,
						Fnom = Parameter03Vm.CurrentValue,
						//Imcw = _imcwParameterVm.FullValue.HasValue? (ushort)_imcwParameterVm.FullValue.Value : (ushort?)null
						Imcw = _imcwParameterVm.FullValue
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
				if (AnyEngineParameterSetted) {
					_uiRoot.Notifier.Notify(() => { _logger.Log("Запись настроек двигателя..."); });
					var settingsPart = new EngineSettingsPartWritable {
						Inom = ConvertDecimalToUshort(Parameter02Vm.CurrentValue),
						Nnom = ConvertDecimalToUshort(Parameter04Vm.CurrentValue),
						Nmax = ConvertDecimalToUshort(Parameter05Vm.CurrentValue),
						Pnom = ConvertDecimalToUshort(Parameter06Vm.CurrentValue),
						CosFi = ConvertDecimalToUshort(Parameter08Vm.CurrentValue),
						Eff = ConvertDecimalToUshort(Parameter09Vm.CurrentValue),
						Mass = ConvertDecimalToUshort(Parameter10Vm.CurrentValue),
						MmM = ConvertDecimalToUshort(Parameter11Vm.CurrentValue),
						Height = ConvertDecimalToUshort(Parameter12Vm.CurrentValue)
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
			try {
				_uiRoot.Notifier.Notify(() => { _logger.Log("Чтение группы настроек..."); });
				_ainSettingsReaderWriter.ReadSettingsAsync(0, true, (exception, settings) => { });
				_engineSettingsReader.ReadSettingsAsync(true, (ex, settings) => { });
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
					//Parameter09Vm.SelectedComboItem = null;
					return;
				}
				Parameter01Vm.CurrentValue = settings.Unom;
				Parameter03Vm.CurrentValue = settings.Fnom;
				//int comboValue = (settings.Imcw & 0x0080) == 0x0080 ? 1 : 0;
				//Parameter09Vm.SelectedComboItem = Parameter09Vm.ComboItems.First(ci => ci.ComboValue == comboValue);
			});
		}

		private void UpdateEngineSettingsInUiThread(Exception readInnerException, IEngineSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (readInnerException != null) {
					//_logger.Log("Не удалось прочитать настройки АИН");
					Parameter02Vm.CurrentValue = null;
					Parameter04Vm.CurrentValue = null;
					Parameter05Vm.CurrentValue = null;
					Parameter06Vm.CurrentValue = null;
					Parameter08Vm.CurrentValue = null;
					Parameter09Vm.CurrentValue = null;
					Parameter10Vm.CurrentValue = null;
					Parameter11Vm.CurrentValue = null;
					Parameter12Vm.CurrentValue = null;

					return;
				}

				Parameter02Vm.CurrentValue = settings.Inom;
				Parameter04Vm.CurrentValue = settings.Nnom;
				Parameter05Vm.CurrentValue = settings.Nmax;
				Parameter06Vm.CurrentValue = settings.Pnom;
				Parameter08Vm.CurrentValue = settings.CosFi;
				Parameter09Vm.CurrentValue = settings.Eff;
				Parameter10Vm.CurrentValue = settings.Mass;
				Parameter11Vm.CurrentValue = settings.MmM;
				Parameter12Vm.CurrentValue = settings.Height;
			});
		}

		private ushort? ConvertDecimalToUshort(decimal? value) {
			if (!value.HasValue) return null;
			return (ushort)value.Value;
		}
	}
}
