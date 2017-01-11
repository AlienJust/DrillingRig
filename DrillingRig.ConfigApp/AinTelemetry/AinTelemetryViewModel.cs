using System;
using System.Threading;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinTelemetry;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.CommandSenderHost;

namespace DrillingRig.ConfigApp.AinTelemetry {
	internal class AinTelemetryViewModel : ViewModelBase, ICyclePart {
		private readonly ICommonAinTelemetryVm _commonAinTelemetryVm;
		private readonly byte _zeroBasedAinNumber;
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ILogger _logger;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private IAinTelemetry _telemetry;

		private readonly object _syncCancel;
		private bool _cancel;

		public AinTelemetryViewModel(ICommonAinTelemetryVm commonAinTelemetryVm, byte zeroBasedAinNumber, ICommandSenderHost commandSenderHost, ILogger logger, IUserInterfaceRoot userInterfaceRoot) {
			_commonAinTelemetryVm = commonAinTelemetryVm;
			_zeroBasedAinNumber = zeroBasedAinNumber;
			_commandSenderHost = commandSenderHost;
			_logger = logger;
			_userInterfaceRoot = userInterfaceRoot;
			_telemetry = null;
			_syncCancel = new object();
			_cancel = true;
		}

		public double? RotationFriquencyCalculated => _telemetry?.RotationFriquencyCalculated;

		public double? PwmModulationCoefficient => _telemetry?.PwmModulationCoefficient;

		public double? MomentumCurrentSetting => _telemetry?.MomentumCurrentSetting;

		public double? RadiatorTemperature => _telemetry?.RadiatorTemperature;

		public double? DcBusVoltage => _telemetry?.DcBusVoltage;

		public double? AllPhasesCurrentAmplitudeEnvelopeCurve => _telemetry?.AllPhasesCurrentAmplitudeEnvelopeCurve;

		public double? RegulatorCurrentDoutput => _telemetry?.RegulatorCurrentDoutput;

		public double? RegulatorCurrentQoutput => _telemetry?.RegulatorCurrentQoutput;

		public double? FriquencyIntensitySetpointOutput => _telemetry?.FriquencyIntensitySetpointOutput;

		public double? FlowSetting => _telemetry?.FlowSetting;

		public double? MeasuredMoment => _telemetry?.MeasuredMoment;

		public double? SpeedRegulatorOutputOrMomentSetting => _telemetry?.SpeedRegulatorOutputOrMomentSetting;

		public double? MeasuredFlow => _telemetry?.MeasuredFlow;

		public double? SettingExcitationCurrent => _telemetry?.SettingExcitationCurrent;

		public string RunModeBits12 {
			get { // TODO: move to static class
				if (_telemetry == null) return string.Empty;
				switch (_telemetry.RunModeBits12) {
					case ModeSetRunModeBits12.Freewell:
						return "Выбег";
					case ModeSetRunModeBits12.Traction:
						return "Тяга";
					case ModeSetRunModeBits12.Unknown2:
						return "Штатный останов";
					case ModeSetRunModeBits12.Unknown3:
						return "Аварийный останов";
					default:
						return "WTF?";
				}
			}
		}

		public bool? ResetZiToZero => _telemetry?.ResetZiToZero;

		public bool? ResetFault => _telemetry?.ResetFault;

		public bool? LimitRegulatorId => _telemetry?.LimitRegulatorId;

		public bool? LimitRegulatorIq => _telemetry?.LimitRegulatorIq;

		public bool? LimitRegulatorSpeed => _telemetry?.LimitRegulatorSpeed;

		public bool? LimitRegulatorFlow => _telemetry?.LimitRegulatorFlow;

		public string MomentumSetterSelector {
			get { // TODO: move to static class
				if (_telemetry == null) return string.Empty;
				switch (_telemetry.MomentumSetterSelector) {
					case ModeSetMomentumSetterSelector.SpeedRegulator:
						return "Регулятор скорости";
					case ModeSetMomentumSetterSelector.ExternalMoment:
						return "Внешний момент";
					case ModeSetMomentumSetterSelector.Summary:
						return "Сумма";
					case ModeSetMomentumSetterSelector.Zero:
						return "0";
					default:
						return "WTF?";
				}
			}
		}


		// STATUS Bits:
		public bool? Driver1HasErrors => _telemetry?.Driver1HasErrors;
		public bool? Driver2HasErrors => _telemetry?.Driver2HasErrors;
		public bool? Driver3HasErrors => _telemetry?.Driver3HasErrors;
		public bool? Driver4HasErrors => _telemetry?.Driver4HasErrors;
		public bool? Driver5HasErrors => _telemetry?.Driver5HasErrors;
		public bool? Driver6HasErrors => _telemetry?.Driver6HasErrors;

		public bool? SomePhaseMaximumAlowedCurrentExcess => _telemetry?.SomePhaseMaximumAlowedCurrentExcess;
		public bool? RadiatorKeysTemperatureRiseTo85DegreesExcess => _telemetry?.RadiatorKeysTemperatureRiseTo85DegreesExcess;
		public bool? AllowedDcVoltageExcess => _telemetry?.AllowedDcVoltageExcess;

		public bool? NoLinkOnSyncLine => _telemetry?.NoLinkOnSyncLine;
		public bool? ExternalTemperatureLimitExcess => _telemetry?.ExternalTemperatureLimitExcess;
		public bool? RotationFriquecnySensorFault => _telemetry?.RotationFriquecnySensorFault;

		public bool? EepromI2CErrorDefaultParamsAreLoaded => _telemetry?.EepromI2CErrorDefaultParamsAreLoaded;
		public bool? EepromCrcErrorDefaultParamsAreLoaded => _telemetry?.EepromCrcErrorDefaultParamsAreLoaded;

		public bool? SomeSlaveFault => _telemetry?.SomeSlaveFault;
		public bool? ConfigChangeDuringParallelWorkConfirmationNeed => _telemetry.ConfigChangeDuringParallelWorkConfirmationNeed;



		public double? RotationFriquencyMeasuredDcv => _telemetry?.RotationFriquencyMeasuredDcv;

		public double? AfterFilterSpeedControllerFeedbackFriquency => _telemetry?.AfterFilterSpeedControllerFeedbackFriquency;

		public double? AfterFilterFimag => _telemetry?.AfterFilterFimag;

		public double? CurrentDpartMeasured => _telemetry?.CurrentDpartMeasured;

		public double? CurrentQpartMeasured => _telemetry?.CurrentQpartMeasured;

		public double? AfterFilterFset => _telemetry?.AfterFilterFset;

		public double? AfterFilterTorq => _telemetry?.AfterFilterTorq;

		public double? ExternalTemperature => _telemetry?.ExternalTemperature;

		public double? DCurrentRegulatorProportionalPart => _telemetry?.DCurrentRegulatorProportionalPart;

		public double? QcurrentRegulatorProportionalPart => _telemetry?.QcurrentRegulatorProportionalPart;

		public double? SpeedRegulatorProportionalPart => _telemetry?.SpeedRegulatorProportionalPart;

		public double? FlowRegulatorProportionalPart => _telemetry?.FlowRegulatorProportionalPart;

		public double? CalculatorDflowRegulatorOutput => _telemetry?.CalculatorDflowRegulatorOutput;

		public double? CalculatorQflowRegulatorOutput => _telemetry?.CalculatorQflowRegulatorOutput;

		public ushort? Aux1 => _telemetry?.Aux1;

		public ushort? Aux2 => _telemetry?.Aux2;

		public ushort? Pver => _telemetry?.Pver;

		public DateTime? PvDate => _telemetry?.PvDate;

		// TODO: other props

		public void UpdateTelemetry(IAinTelemetry telemetry) {
			_telemetry = telemetry;

			RaisePropertyChanged(() => RotationFriquencyCalculated);
			RaisePropertyChanged(() => PwmModulationCoefficient);
			RaisePropertyChanged(() => MomentumCurrentSetting);
			RaisePropertyChanged(() => RadiatorTemperature);
			RaisePropertyChanged(() => DcBusVoltage);
			RaisePropertyChanged(() => AllPhasesCurrentAmplitudeEnvelopeCurve);
			RaisePropertyChanged(() => RegulatorCurrentDoutput);
			RaisePropertyChanged(() => RegulatorCurrentQoutput);
			RaisePropertyChanged(() => FriquencyIntensitySetpointOutput);
			RaisePropertyChanged(() => FlowSetting);
			RaisePropertyChanged(() => MeasuredMoment);
			RaisePropertyChanged(() => SpeedRegulatorOutputOrMomentSetting);
			RaisePropertyChanged(() => MeasuredFlow);
			RaisePropertyChanged(() => SettingExcitationCurrent);

			RaisePropertyChanged(() => RunModeBits12);
			RaisePropertyChanged(() => ResetZiToZero);
			RaisePropertyChanged(() => ResetFault);
			RaisePropertyChanged(() => LimitRegulatorId);
			RaisePropertyChanged(() => LimitRegulatorIq);
			RaisePropertyChanged(() => LimitRegulatorSpeed);
			RaisePropertyChanged(() => LimitRegulatorFlow);
			RaisePropertyChanged(() => MomentumSetterSelector);

			RaisePropertyChanged(() => Driver1HasErrors);
			RaisePropertyChanged(() => Driver2HasErrors);
			RaisePropertyChanged(() => Driver3HasErrors);
			RaisePropertyChanged(() => Driver4HasErrors);
			RaisePropertyChanged(() => Driver5HasErrors);
			RaisePropertyChanged(() => Driver6HasErrors);

			RaisePropertyChanged(() => SomePhaseMaximumAlowedCurrentExcess);
			RaisePropertyChanged(() => RadiatorKeysTemperatureRiseTo85DegreesExcess);
			RaisePropertyChanged(() => AllowedDcVoltageExcess);

			RaisePropertyChanged(() => NoLinkOnSyncLine);
			RaisePropertyChanged(() => ExternalTemperatureLimitExcess);
			RaisePropertyChanged(() => RotationFriquecnySensorFault);

			RaisePropertyChanged(() => EepromI2CErrorDefaultParamsAreLoaded);
			RaisePropertyChanged(() => EepromCrcErrorDefaultParamsAreLoaded);

			RaisePropertyChanged(() => SomeSlaveFault);
			RaisePropertyChanged(() => ConfigChangeDuringParallelWorkConfirmationNeed);

			RaisePropertyChanged(() => RotationFriquencyMeasuredDcv);
			RaisePropertyChanged(() => AfterFilterSpeedControllerFeedbackFriquency);
			RaisePropertyChanged(() => AfterFilterFimag);
			RaisePropertyChanged(() => CurrentDpartMeasured);
			RaisePropertyChanged(() => CurrentQpartMeasured);
			RaisePropertyChanged(() => AfterFilterFset);
			RaisePropertyChanged(() => AfterFilterTorq);

			RaisePropertyChanged(() => ExternalTemperature);

			RaisePropertyChanged(() => DCurrentRegulatorProportionalPart);
			RaisePropertyChanged(() => QcurrentRegulatorProportionalPart);
			RaisePropertyChanged(() => SpeedRegulatorProportionalPart);
			RaisePropertyChanged(() => FlowRegulatorProportionalPart);

			RaisePropertyChanged(() => CalculatorDflowRegulatorOutput);
			RaisePropertyChanged(() => CalculatorQflowRegulatorOutput);

			RaisePropertyChanged(() => Aux1);
			RaisePropertyChanged(() => Aux2);
			RaisePropertyChanged(() => Pver);
			RaisePropertyChanged(() => PvDate);

			//EngineState? commonEngineState = 
			_commonAinTelemetryVm.UpdateCommonEngineState(_telemetry?.CommonEngineState);
			_commonAinTelemetryVm.UpdateCommonFaultState(_telemetry?.CommonFaultState);
			_commonAinTelemetryVm.UpdateAinsLinkState(
				_telemetry?.Ain1LinkFault,
				_telemetry?.Ain2LinkFault,
				_telemetry?.Ain3LinkFault);
		}

		public void InCycleAction() {
			var waiter = new ManualResetEvent(false);


			var cmd = new ReadAinTelemetryCommand(_zeroBasedAinNumber);
			_commandSenderHost.SilentSender.SendCommandAsync(0x01,
				cmd, TimeSpan.FromSeconds(0.1),
				(exception, bytes) => {
					IAinTelemetry ainTelemetry = null;
					try {
						if (exception != null) {
							throw new Exception("Произошла ошибка во время обмена", exception);
						}
						var result = cmd.GetResult(bytes);
						ainTelemetry = result;
					}
					catch (Exception ex) {
						// TODO: log exception, null values
						//_logger.Log("Ошибка: " + ex.Message);
						//Console.WriteLine(ex);
					}
					finally {
						_userInterfaceRoot.Notifier.Notify(() => {
							//Console.WriteLine("UserInterface thread begin action =============================");
							//Console.WriteLine("AIN viewModel zbNumber: " + _zeroBasedAinNumber);
							UpdateTelemetry(ainTelemetry);
							if (_zeroBasedAinNumber == 0) _commonAinTelemetryVm.UpdateAinStatuses(ainTelemetry?.Status, null, null);
							//Console.WriteLine("UserInterface thread end action ===============================");
						});
						waiter.Set();
					}
				});
			waiter.WaitOne();
			waiter.Reset();
		}

		public bool Cancel {
			get {
				lock (_syncCancel) {
					return _cancel;
				}
			}
			set {
				lock (_syncCancel) {
					_cancel = value;
				}
			}
		}
	}
}
