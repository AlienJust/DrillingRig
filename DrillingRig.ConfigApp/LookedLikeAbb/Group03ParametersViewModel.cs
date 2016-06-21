using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group03ParametersViewModel : ViewModelBase {
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

		private bool _isReadingCycle;
		private int _currentAinsCountToRead;
		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		public Group03ParametersViewModel(IUserInterfaceRoot uiRoot, ILogger logger, ICycleReader cycleReader, IAinsCounter ainsCounter, IParameterLogger parameterLogger) {
			_uiRoot = uiRoot;
			_logger = logger;
			_cycleReader = cycleReader;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleReadonlyViewModel("02.01 Коэффициент модуляции ШИМ [%]", "f2", null, parameterLogger);

			Parameter02Vm = new ParameterDoubleReadonlyViewModel("02.02 Выход регулятора тока D [%]", "f0", null, parameterLogger);
			Parameter03Vm = new ParameterDoubleReadonlyViewModel("02.03 Выход регулятора тока Q [%]", "f2", null, parameterLogger);

			Parameter04Vm = new ParameterDoubleReadonlyViewModel("02.04 Измеренная составляющая тока D [%]", "f2", null, parameterLogger);
			Parameter05Vm = new ParameterDoubleReadonlyViewModel("02.05 Измеренная составляющая тока Q [%]", "f2", null, parameterLogger);

			Parameter06Vm = new ParameterDoubleReadonlyViewModel("02.06 Выход регулятора компенсатора вычислителя потока D [В]", "f0", null, parameterLogger);
			Parameter07Vm = new ParameterDoubleReadonlyViewModel("02.07 Выход регулятора компенсатора вычислителя потока Q [В]", "f0", null, parameterLogger);

			Parameter08Vm = new ParameterDoubleReadonlyViewModel("02.08 Вспомогательная ячейка №1 АИН1", "f0", null, parameterLogger);
			Parameter09Vm = new ParameterDoubleReadonlyViewModel("02.09 Вспомогательная ячейка №2 АИН1", "f0", null, parameterLogger);

			Parameter10Vm = new ParameterDoubleReadonlyViewModel("02.10 Вычисленное текущее значение теплового показателя двигателя [А^2*c]", "f0", null, parameterLogger); // TODO: ask Marat
			Parameter11Vm = new ParameterDoubleReadonlyViewModel("02.11 (Ведомый привод) Уставка моментного тока (Выход регулятора скорости) [%]", "f2", null, parameterLogger); // TODO: ask Marat

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
				Parameter01Vm.CurrentValue = ainTelemetry.PwmModulationCoefficient;

				Parameter02Vm.CurrentValue = ainTelemetry.RegulatorCurrentDoutput;
				Parameter03Vm.CurrentValue = ainTelemetry.RegulatorCurrentQoutput;

				Parameter04Vm.CurrentValue = ainTelemetry.CurrentDpartMeasured;
				Parameter05Vm.CurrentValue = ainTelemetry.CurrentQpartMeasured;

				Parameter06Vm.CurrentValue = ainTelemetry.CalculatorDflowRegulatorOutput;
				Parameter07Vm.CurrentValue = ainTelemetry.CalculatorQflowRegulatorOutput;

				Parameter08Vm.CurrentValue = ainTelemetry.Aux1;
				Parameter09Vm.CurrentValue = ainTelemetry.Aux2;
				//Parameter10Vm.CurrentValue = ainTelemetry.SpeedRegulatorProportionalPart; // TODO: ask Marat
				//Parameter11Vm.CurrentValue = ainTelemetry.FlowRegulatorProportionalPart; // TODO: ask Marat
			}
		}

		private void CycleReaderOnAin2TelemetryReaded(IAinTelemetry ainTelemetry) {
			// TODO: if ain 3 telemetry needed
		}

		private void CycleReaderOnAin3TelemetryReaded(IAinTelemetry ainTelemetry) {
			// TODO: if ain 2 telemetry needed
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
			Parameter08Vm.CurrentValue = null;
			Parameter09Vm.CurrentValue = null;
			// Parameter10Vm.CurrentValue = null; // TODO: ASK Marat
			// Parameter11Vm.CurrentValue = null; // TODO: ASK Marat

		}

		private void StopReadCycleAin2TelemetryAndResetParams() {
			_cycleReader.Ain2TelemetryReaded -= CycleReaderOnAin2TelemetryReaded;
			_cycleReader.AskToStopReadAin2TelemetryCycle();
			
			//TODO: null some params if needed
		}

		private void StopReadCycleAin3TelemetryAndResetParams() {
			_cycleReader.Ain3TelemetryReaded -= CycleReaderOnAin3TelemetryReaded;
			_cycleReader.AskToStopReadAin3TelemetryCycle();

			//TODO: null some params if needed
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
