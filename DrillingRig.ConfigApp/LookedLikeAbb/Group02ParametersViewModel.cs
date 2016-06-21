using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group02ParametersViewModel : ViewModelBase {
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

		public Group02ParametersViewModel(IUserInterfaceRoot uiRoot, ILogger logger, ICycleReader cycleReader, IAinsCounter ainsCounter) {
			_uiRoot = uiRoot;
			_logger = logger;
			_cycleReader = cycleReader;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleReadonlyViewModel("02.01 Выход задатчика интенсивности частоты [об/мин]", "f0", null);
			Parameter02Vm = new ParameterDoubleReadonlyViewModel("02.02 Выход задатчика интенсивности после фильтра [об/мин]", "f0", null);
			Parameter03Vm = new ParameterDoubleReadonlyViewModel("02.03 Уставка потока [%]", "f2", null);

			Parameter04Vm = new ParameterDoubleReadonlyViewModel("02.04 Измеренный поток [%]", "f2", null);

			Parameter05Vm = new ParameterDoubleReadonlyViewModel("02.05 Измеренный поток после фильтра [%]", "f2", null);
			Parameter06Vm = new ParameterDoubleReadonlyViewModel("02.06 Задание моментного тока [А]", "f0", null);

			Parameter07Vm = new ParameterDoubleReadonlyViewModel("02.07 Задание тока возбуждения [А]", "f0", null);
			Parameter08Vm = new ParameterDoubleReadonlyViewModel("02.08 Пропорциональная часть регулятора тока D [А]", "f0", null);
			Parameter09Vm = new ParameterDoubleReadonlyViewModel("02.09 Пропорциональная часть регулятора тока Q [А]", "f0", null);

			Parameter10Vm = new ParameterDoubleReadonlyViewModel("02.10 Пропорциональная часть регулятора скорости [об/мин]", "f0", null);
			Parameter11Vm = new ParameterDoubleReadonlyViewModel("02.11 Пропорциональная часть регулятора потока [%]", "f2", null);

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
				Parameter01Vm.CurrentValue = ainTelemetry.FriquencyIntensitySetpointOutput;
				Parameter02Vm.CurrentValue = ainTelemetry.AfterFilterFset;

				Parameter03Vm.CurrentValue = ainTelemetry.FlowSetting;
				Parameter04Vm.CurrentValue = ainTelemetry.MeasuredFlow;
				Parameter05Vm.CurrentValue = ainTelemetry.AfterFilterFimag;

				Parameter06Vm.CurrentValue = ainTelemetry.MomentumCurrentSetting;
				Parameter07Vm.CurrentValue = ainTelemetry.SettingExcitationCurrent;

				Parameter08Vm.CurrentValue = ainTelemetry.DCurrentRegulatorProportionalPart;
				Parameter09Vm.CurrentValue = ainTelemetry.QcurrentRegulatorProportionalPart;
				Parameter10Vm.CurrentValue = ainTelemetry.SpeedRegulatorProportionalPart;
				Parameter11Vm.CurrentValue = ainTelemetry.FlowRegulatorProportionalPart;
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
			Parameter10Vm.CurrentValue = null;
			Parameter11Vm.CurrentValue = null;

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
