using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group04ParametersViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ICycleReader _cycleReader;
		private readonly IAinsCounter _ainsCounter;
		private readonly ILogger _logger;
		public ParameterDoubleReadonlyViewModel Parameter01Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter02Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter03Vm { get; }

		private bool _isReadingCycle;
		private int _currentAinsCountToRead;
		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		public Group04ParametersViewModel(IUserInterfaceRoot uiRoot, ILogger logger, ICycleReader cycleReader, IAinsCounter ainsCounter, IParameterLogger parameterLogger) {
			_uiRoot = uiRoot;
			_logger = logger;
			_cycleReader = cycleReader;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleReadonlyViewModel("04.01 Версия ПО (АИН)", "f0", null, parameterLogger);
			Parameter02Vm = new ParameterDoubleReadonlyViewModel("04.02 Дата билда ПО (АИН)", "f0", null, parameterLogger); // TODO: view as DateTime
			Parameter03Vm = new ParameterDoubleReadonlyViewModel("04.03 Версия ПО (БС-Ethernet)", "f0", null, parameterLogger);

			_isReadingCycle = false;
			ReadCycleCmd = new RelayCommand(ReadCycle, ()=>!_isReadingCycle);
			StopReadCycleCmd = new RelayCommand(StopReadCycle, () => _isReadingCycle);

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
					// TODO: remove if AIN2 and AIN3 are not used:
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
				Parameter01Vm.CurrentValue = ainTelemetry.Pver;

				Parameter02Vm.CurrentValue = ainTelemetry.PvDate?.Ticks;
				
				Parameter03Vm.CurrentValue = null; // TODO: ask Marat, откуда брать версию ПО БС-Ehternet
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
