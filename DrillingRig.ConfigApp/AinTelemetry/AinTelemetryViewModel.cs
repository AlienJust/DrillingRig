using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.AinTelemetry {
	internal class AinTelemetryViewModel : ViewModelBase {
		private readonly string _ainName;
		private IAinTelemetry _telemetry;

		public AinTelemetryViewModel(string ainName) {
			_ainName = ainName;
			_telemetry = null;
		}

		public double? RotationFriquencyCalculated {
			get {
				if (_telemetry == null) return null;
				return _telemetry.RotationFriquencyCalculated;
			}
		}

		public double? PwmModulationCoefficient {
			get {
				if (_telemetry == null) return null;
				return _telemetry.PwmModulationCoefficient;
			}
		}

		public double? MomentumCurrentSetting {
			get {
				if (_telemetry == null) return null;
				return _telemetry.MomentumCurrentSetting;
			}
		}

		public double? RadiatorTemperature {
			get {
				if (_telemetry == null) return null;
				return _telemetry.RadiatorTemperature;
			}
		}

		public double? DcBusVoltage {
			get {
				if (_telemetry == null) return null;
				return _telemetry.DcBusVoltage;
			}
		}

		public double? AllPhasesCurrentAmplitudeEnvelopeCurve {
			get {
				if (_telemetry == null) return null;
				return _telemetry.AllPhasesCurrentAmplitudeEnvelopeCurve;
			}
		}

		public double? RegulatorCurrentDoutput {
			get {
				if (_telemetry == null) return null;
				return _telemetry.RegulatorCurrentDoutput;
			}
		}

		public double? RegulatorCurrentQoutput {
			get {
				if (_telemetry == null) return null;
				return _telemetry.RegulatorCurrentQoutput;
			}
		}

		public double? FriquencyIntensitySetpointOutput {
			get {
				if (_telemetry == null) return null;
				return _telemetry.FriquencyIntensitySetpointOutput;
			}
		}

		public double? FlowSetting {
			get {
				if (_telemetry == null) return null;
				return _telemetry.FlowSetting;
			}
		}

		public double? MeasuredMoment {
			get {
				if (_telemetry == null) return null;
				return _telemetry.MeasuredMoment;
			}
		}

		public double? SpeedRegulatorOutputOrMomentSetting {
			get {
				if (_telemetry == null) return null;
				return _telemetry.SpeedRegulatorOutputOrMomentSetting;
			}
		}

		public double? MeasuredFlow {
			get {
				if (_telemetry == null) return null;
				return _telemetry.MeasuredFlow;
			}
		}

		public double? SettingExcitationCurrent {
			get {
				if (_telemetry == null) return null;
				return _telemetry.SettingExcitationCurrent;
			}
		}

		public string RunModeBits12 {
			get {
				if (_telemetry == null) return string.Empty;
				switch (_telemetry.RunModeBits12)
				{
					case ModeSetRunModeBits12.Freewell: 
						return "Выбег";
					case ModeSetRunModeBits12.Traction: 
						return "Тяга";
					case ModeSetRunModeBits12.Unknown2:
						return "Х.З. 02";
					case ModeSetRunModeBits12.Unknown3:
						return "Х.З. 03";
					default:
						return "WTF?";
				}
			}
		}

		public bool? RunModeRotationDirection {
			get {
				if (_telemetry == null) return null;
				return _telemetry.RunModeRotationDirection;
			}
		}

		public bool? Driver1HasErrors {
			get {
				if (_telemetry == null) return null;
				return _telemetry.Driver1HasErrors;
			}
		}

		public bool? Driver2HasErrors {
			get {
				if (_telemetry == null) return null;
				return _telemetry.Driver2HasErrors;
			}
		}

		public bool? Driver3HasErrors {
			get {
				if (_telemetry == null) return null;
				return _telemetry.Driver3HasErrors;
			}
		}

		public bool? Driver4HasErrors {
			get {
				if (_telemetry == null) return null;
				return _telemetry.Driver4HasErrors;
			}
		}

		public bool? Driver5HasErrors {
			get {
				if (_telemetry == null) return null;
				return _telemetry.Driver5HasErrors;
			}
		}

		public bool? Driver6HasErrors {
			get {
				if (_telemetry == null) return null;
				return _telemetry.Driver6HasErrors;
			}
		}

		public bool? SomePhaseMaximumAlowedCurrentExcess {
			get {
				if (_telemetry == null) return null;
				return _telemetry.SomePhaseMaximumAlowedCurrentExcess;
			}
		}

		public bool? RadiatorKeysTemperatureRiseTo85DegreesExcess {
			get {
				if (_telemetry == null) return null;
				return _telemetry.RadiatorKeysTemperatureRiseTo85DegreesExcess;
			}
		}

		public bool? AllowedDcVoltageExcess {
			get {
				if (_telemetry == null) return null;
				return _telemetry.AllowedDcVoltageExcess;
			}
		}

		public bool? EepromI2CErrorDefaultParamsAreLoaded {
			get {
				if (_telemetry == null) return null;
				return _telemetry.EepromI2CErrorDefaultParamsAreLoaded;
			}
		}

		public bool? EepromCrcErrorDefaultParamsAreLoaded {
			get {
				if (_telemetry == null) return null;
				return _telemetry.EepromCrcErrorDefaultParamsAreLoaded;
			}
		}

		public double? RotationFriquencyMeasuredDcv {
			get {
				if (_telemetry == null) return null;
				return _telemetry.RotationFriquencyMeasuredDcv;
			}
		}

		public double? AfterFilterSpeedControllerFeedbackFriquency {
			get {
				if (_telemetry == null) return null;
				return _telemetry.AfterFilterSpeedControllerFeedbackFriquency;
			}
		}

		public double? AfterFilterFimag {
			get {
				if (_telemetry == null) return null;
				return _telemetry.AfterFilterFimag;
			}
		}

		public double? CurrentDpartMeasured {
			get {
				if (_telemetry == null) return null;
				return _telemetry.CurrentDpartMeasured;
			}
		}

		public double? CurrentQpartMeasured {
			get {
				if (_telemetry == null) return null;
				return _telemetry.CurrentQpartMeasured;
			}
		}

		public double? AfterFilterFset {
			get {
				if (_telemetry == null) return null;
				return _telemetry.AfterFilterFset;
			}
		}

		public double? AfterFilterTorq {
			get {
				if (_telemetry == null) return null;
				return _telemetry.AfterFilterTorq;
			}
		}

		public double? DCurrentRegulatorProportionalPart {
			get {
				if (_telemetry == null) return null;
				return _telemetry.DCurrentRegulatorProportionalPart;
			}
		}

		public double? QcurrentRegulatorProportionalPart {
			get {
				if (_telemetry == null) return null;
				return _telemetry.QcurrentRegulatorProportionalPart;
			}
		}

		public double? SpeedRegulatorProportionalPart {
			get {
				if (_telemetry == null) return null;
				return _telemetry.SpeedRegulatorProportionalPart;
			}
		}

		public double? FlowRegulatorProportionalPart {
			get {
				if (_telemetry == null) return null;
				return _telemetry.FlowRegulatorProportionalPart;
			}
		}

		public double? CalculatorDflowRegulatorOutput {
			get {
				if (_telemetry == null) return null;
				return _telemetry.CalculatorDflowRegulatorOutput;
			}
		}

		public double? CalculatorQflowRegulatorOutput {
			get {
				if (_telemetry == null) return null;
				return _telemetry.CalculatorQflowRegulatorOutput;
			}
		}

		public string AinName {
			get { return _ainName; }
		}



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
			RaisePropertyChanged(() => RunModeRotationDirection);

			RaisePropertyChanged(() => Driver1HasErrors);
			RaisePropertyChanged(() => Driver2HasErrors);
			RaisePropertyChanged(() => Driver3HasErrors);
			RaisePropertyChanged(() => Driver4HasErrors);
			RaisePropertyChanged(() => Driver5HasErrors);
			RaisePropertyChanged(() => Driver6HasErrors);

			RaisePropertyChanged(() => SomePhaseMaximumAlowedCurrentExcess);
			RaisePropertyChanged(() => RadiatorKeysTemperatureRiseTo85DegreesExcess);

			RaisePropertyChanged(() => AllowedDcVoltageExcess);
			RaisePropertyChanged(() => EepromI2CErrorDefaultParamsAreLoaded);
			RaisePropertyChanged(() => EepromCrcErrorDefaultParamsAreLoaded);

			RaisePropertyChanged(() => RotationFriquencyMeasuredDcv);
			RaisePropertyChanged(() => AfterFilterSpeedControllerFeedbackFriquency);
			RaisePropertyChanged(() => AfterFilterFimag);
			RaisePropertyChanged(() => CurrentDpartMeasured);
			RaisePropertyChanged(() => CurrentQpartMeasured);
			RaisePropertyChanged(() => AfterFilterFset);
			RaisePropertyChanged(() => AfterFilterTorq);

			RaisePropertyChanged(() => DCurrentRegulatorProportionalPart);
			RaisePropertyChanged(() => QcurrentRegulatorProportionalPart);
			RaisePropertyChanged(() => SpeedRegulatorProportionalPart);
			RaisePropertyChanged(() => FlowRegulatorProportionalPart);

			RaisePropertyChanged(() => CalculatorDflowRegulatorOutput);
			RaisePropertyChanged(() => CalculatorQflowRegulatorOutput);
		}
	}
}
