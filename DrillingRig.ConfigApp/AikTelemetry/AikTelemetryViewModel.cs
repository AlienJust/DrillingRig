using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AikTelemetry;

namespace DrillingRig.ConfigApp.AikTelemetry
{
	class AikTelemetryViewModel :ViewModelBase
	{
		private readonly string _aikName;
		private IAikTelemetry _telemetry;

		public AikTelemetryViewModel(string aikName) {
			_aikName = aikName;
			_telemetry = null;
		}

		public double? RotationFriquencyCalculated
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.RotationFriquencyCalculated;
			}
		}

		public double? PwmModulationCoefficient
		{
			get { if (_telemetry == null) return null;
				return _telemetry.PwmModulationCoefficient;
				}
			}

		public double? MomentumCurrentSetting
		{
			get { if (_telemetry == null) return null;
				return _telemetry.MomentumCurrentSetting;
			}
		}

		public double? RadiatorTemperature
		{
			get { if (_telemetry == null) return null;
				return _telemetry.RadiatorTemperature;
			}
		}

		public double? DcBusVoltage
		{
			get { if (_telemetry == null) return null;
				return _telemetry.DcBusVoltage;
			}
		}

		public double? AllPhasesCurrentAmplitudeEnvelopeCurve
		{
			get { if (_telemetry == null) return null;
				return _telemetry.AllPhasesCurrentAmplitudeEnvelopeCurve;
			}
		}

		public double? RegulatorCurrentDoutput
		{
			get { if (_telemetry == null) return null;
				return _telemetry.RegulatorCurrentDoutput;
			}
		}

		public double? RegulatorCurrentQoutput
		{
			get { if (_telemetry == null) return null;
				return _telemetry.RegulatorCurrentQoutput;
			}
		}

		public double? FriquencyIntensitySetpointOutput
		{
			get { if (_telemetry == null) return null;
				return _telemetry.FriquencyIntensitySetpointOutput;
			}
		}

		public double? FlowSetting
		{
			get { if (_telemetry == null) return null;
				return _telemetry.FlowSetting;
			}
		}

		public double? MeasuredMoment
		{
			get { if (_telemetry == null) return null;
				return _telemetry.MeasuredMoment;
			}
		}

		public double? SpeedRegulatorOutputOrMomentSetting
		{
			get { if (_telemetry == null) return null;
				return _telemetry.SpeedRegulatorOutputOrMomentSetting;
			}
		}

		public double? MeasuredFlow
		{
			get { if (_telemetry == null) return null;
				return _telemetry.MeasuredFlow;
			}
		}

		public double? SettingExcitationCurrent
		{
			get { if (_telemetry == null) return null;
				return _telemetry.SettingExcitationCurrent;
			}
		}



		public string AikName {
			get { return _aikName; }
		}



		// TODO: other props

		public void UpdateTelemetry(IAikTelemetry telemetry) {
			_telemetry = telemetry;
			// TODO: a tons of RaisePropertyChanged();
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
		}
	}
}
