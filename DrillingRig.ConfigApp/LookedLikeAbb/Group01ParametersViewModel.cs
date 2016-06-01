using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group01ParametersViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ICycleReader _cycleReader;
		private readonly IAinsCounter _ainsCounter;
		private readonly ILogger _logger;
		public ParameterDoubleReadonlyViewModel Parameter01Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter02Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter03Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter04Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter05Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter06Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter07Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter08Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter09Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter10Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter11Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter12Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter13Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter14Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter15Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter16Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter17Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter18Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter19Vm { get; }

		private bool _isReadingCycle;
		private int _currentAinsCountToRead;
		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		public Group01ParametersViewModel(IUserInterfaceRoot uiRoot, ILogger logger, ICycleReader cycleReader, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_cycleReader = cycleReader;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleReadonlyViewModel("01.01 Вычисленная частота вращения [об/мин]", "f1", null);
			Parameter02Vm = new ParameterDoubleReadonlyViewModel("01.02 Частота вращения, измеренная ДЧВ [об/мин]", "f1", null);
			Parameter03Vm = new ParameterDoubleReadonlyViewModel("01.03 Частота на ОС регулятора скорости после фильтра [об/мин]", "f0", null);

			Parameter04Vm = new ParameterDoubleReadonlyViewModel("01.04 Измеренный ток двигателя [А]", "f0", null);

			Parameter05Vm = new ParameterDoubleReadonlyViewModel("01.05 Вычисленное выходное напряжение на двигателе [В]", "f0", null); // TODO: спросить Марата, в процентах или как задаётся момент.
			Parameter06Vm = new ParameterDoubleReadonlyViewModel("01.06 Напряжение шины DC [В]", "f0", null);

			Parameter07Vm = new ParameterDoubleReadonlyViewModel("01.07 Температура радиатора АИН1 [град С]", "f0", null);
			Parameter08Vm = new ParameterDoubleReadonlyViewModel("01.08 Температура радиатора АИН2 [град С]", "f0", null);
			Parameter09Vm = new ParameterDoubleReadonlyViewModel("01.09 Температура радиатора АИН3 [град С]", "f0", null);

			Parameter10Vm = new ParameterDoubleReadonlyViewModel("01.10 Температура внешняя АИН1 [град С]", "f0", null);
			Parameter11Vm = new ParameterDoubleReadonlyViewModel("01.11 Температура внешняя АИН2 [град С]", "f0", null);
			Parameter12Vm = new ParameterDoubleReadonlyViewModel("01.12 Температура внешняя АИН3 [град С]", "f0", null);

			Parameter13Vm = new ParameterDoubleReadonlyViewModel("01.13 Измеренный момент [Нм]", "f0", null);
			Parameter14Vm = new ParameterDoubleReadonlyViewModel("01.14 Измеренный момент после фильтра [Нм]", "f0", null);

			Parameter15Vm = new ParameterDoubleReadonlyViewModel("01.15 Уставка моментного тока (Выход регулятора скорости) [%]", "f0", null);
			Parameter16Vm = new ParameterDoubleReadonlyViewModel("01.16 Мощность, подаваемая на двигатель", "f0", null);

			Parameter17Vm = new ParameterDoubleReadonlyViewModel("01.17 Состояние цифровых входов", "f0", null);
			Parameter18Vm = new ParameterDoubleReadonlyViewModel("01.18 Состояние релейных выходов", "f0", null);

			Parameter19Vm = new ParameterDoubleReadonlyViewModel("01.19 Активный режим регулирования (Управление по скорости/Управление крутящим моментом)", "f0", null); // (0 – регулятор скорости, 1 – внешний момент, 2 – их сумма, 3 - 0 )

			_isReadingCycle = false;
			ReadCycleCmd = new RelayCommand(ReadCycle, ()=>!_isReadingCycle);
			StopReadCycleCmd = new RelayCommand(StopReadCycle, () => _isReadingCycle);

			//_cycleReader.Ain1TelemetryReaded += CycleReaderOnAin1TelemetryReaded;
			//_cycleReader.Ain2TelemetryReaded += CycleReaderOnAin2TelemetryReaded;
			//_cycleReader.Ain3TelemetryReaded += CycleReaderOnAin3TelemetryReaded;


			_currentAinsCountToRead = _ainsCounter.SelectedAinsCount;
			_ainsCounter.AinsCountInSystemHasBeenChanged += AinsCounterOnAinsCountInSystemHasBeenChanged;
		}



		private void AinsCounterOnAinsCountInSystemHasBeenChanged() {
			var newAinsCount = _ainsCounter.SelectedAinsCount;
			if (newAinsCount != _currentAinsCountToRead) {
				if (_isReadingCycle) {
					if (_currentAinsCountToRead == 1) {
						if (newAinsCount > 1) {
							StartReadCycleAin2Params();
							if (newAinsCount > 2) {
								StartReadCycleAin3Params();
							}
						}
					}
					else if (_currentAinsCountToRead == 2) {
						if (newAinsCount > 2) {
							StartReadCycleAin3Params();
						}
						else if (newAinsCount < 2) {
							StopReadCycleAin2TelemetryAndResetParams();
						}
					}

					else if (_currentAinsCountToRead == 3) {
						if (newAinsCount < 3) {
							StopReadCycleAin3TelemetryAndResetParams();
							if (newAinsCount < 2) {
								StopReadCycleAin2TelemetryAndResetParams();
							}
						}
					}
				}
				_currentAinsCountToRead = newAinsCount;
			}
		}



		private void CycleReaderOnAin1TelemetryReaded(IAinTelemetry ainTelemetry) {
			if (_isReadingCycle) {
				//_logger.Log("Readed in cycle");
				Parameter01Vm.CurrentValue = ainTelemetry.RotationFriquencyCalculated;
				Parameter02Vm.CurrentValue = ainTelemetry.RotationFriquencyMeasuredDcv;
				Parameter03Vm.CurrentValue = ainTelemetry.AfterFilterSpeedControllerFeedbackFriquency;
				Parameter04Vm.CurrentValue = ainTelemetry.AllPhasesCurrentAmplitudeEnvelopeCurve;
				Parameter05Vm.CurrentValue = ainTelemetry.PwmModulationCoefficient;
				Parameter06Vm.CurrentValue = ainTelemetry.DcBusVoltage;

				Parameter07Vm.CurrentValue = ainTelemetry.RadiatorTemperature;
				// TODO: params 8 and 9
				Parameter10Vm.CurrentValue = ainTelemetry.ExternalTemperature;
				// TODO: params 11 and 12

				Parameter13Vm.CurrentValue = ainTelemetry.MeasuredMoment;
				Parameter14Vm.CurrentValue = ainTelemetry.AfterFilterTorq;

				Parameter15Vm.CurrentValue = ainTelemetry.SpeedRegulatorOutputOrMomentSetting;
			}
		}

		private void CycleReaderOnAin2TelemetryReaded(IAinTelemetry ainTelemetry) {
			Parameter08Vm.CurrentValue = ainTelemetry.RadiatorTemperature;
			Parameter11Vm.CurrentValue = ainTelemetry.ExternalTemperature;
		}

		private void CycleReaderOnAin3TelemetryReaded(IAinTelemetry ainTelemetry) {
			Parameter09Vm.CurrentValue = ainTelemetry.RadiatorTemperature;
			Parameter12Vm.CurrentValue = ainTelemetry.ExternalTemperature;
		}


		private void StartReadCycleAin1Params()
		{
			_cycleReader.Ain1TelemetryReaded += CycleReaderOnAin1TelemetryReaded;
			_cycleReader.AskToStartReadAin1TelemetryCycle();
		}

		private void StartReadCycleAin2Params() {
			_cycleReader.Ain2TelemetryReaded += CycleReaderOnAin2TelemetryReaded;
			_cycleReader.AskToStartReadAin2TelemetryCycle();
		}

		private void StartReadCycleAin3Params() {
			_cycleReader.Ain3TelemetryReaded += CycleReaderOnAin3TelemetryReaded;
			_cycleReader.AskToStartReadAin3TelemetryCycle();
		}



		private void StopReadCycleAin1TelemetryAndResetParams() {
			_cycleReader.Ain1TelemetryReaded -= CycleReaderOnAin1TelemetryReaded;
			_cycleReader.AskToStopReadAin1TelemetryCycle();

			Parameter01Vm.CurrentValue = null;
			Parameter02Vm.CurrentValue = null;
			Parameter03Vm.CurrentValue = null;
			Parameter04Vm.CurrentValue = null;
			Parameter05Vm.CurrentValue = null;
			Parameter06Vm.CurrentValue = null;

			Parameter07Vm.CurrentValue = null;
			// TODO: params 8 and 9
			Parameter10Vm.CurrentValue = null;
			// TODO: params 11 and 12

			Parameter13Vm.CurrentValue = null;
			Parameter14Vm.CurrentValue = null;

			Parameter15Vm.CurrentValue = null;
		}

		private void StopReadCycleAin2TelemetryAndResetParams() {
			_cycleReader.Ain2TelemetryReaded -= CycleReaderOnAin2TelemetryReaded;
			_cycleReader.AskToStopReadAin2TelemetryCycle();

			Parameter08Vm.CurrentValue = null;
			Parameter11Vm.CurrentValue = null;
		}

		private void StopReadCycleAin3TelemetryAndResetParams() {
			_cycleReader.Ain3TelemetryReaded -= CycleReaderOnAin3TelemetryReaded;
			_cycleReader.AskToStopReadAin3TelemetryCycle();

			Parameter09Vm.CurrentValue = null;
			Parameter12Vm.CurrentValue = null;
		}





		private void StopReadCycle() {
			_logger.Log("Завершение циклического опроса");
			_isReadingCycle = false;

			StopReadCycleCmd.RaiseCanExecuteChanged();

			StopReadCycleAin1TelemetryAndResetParams();
			if (_currentAinsCountToRead > 1) {
				_logger.Log("Завершение циклического опроса 2");
				StopReadCycleAin2TelemetryAndResetParams();
				if (_currentAinsCountToRead > 2) {
					_logger.Log("Завершение циклического опроса 3");
					StopReadCycleAin3TelemetryAndResetParams();
				}
			}

			ReadCycleCmd.RaiseCanExecuteChanged();
		}

		private void ReadCycle() {
			_logger.Log("Начало циклического опроса");
			_isReadingCycle = true;

			ReadCycleCmd.RaiseCanExecuteChanged();

			StartReadCycleAin1Params();
			if (_currentAinsCountToRead > 1) {
				_logger.Log("Начало циклического опроса 2");
				StartReadCycleAin2Params();
				if (_currentAinsCountToRead > 2) {
					_logger.Log("Начало циклического опроса 3");
					StartReadCycleAin3Params();
				}
			}

			StopReadCycleCmd.RaiseCanExecuteChanged();
		}
	}
}
