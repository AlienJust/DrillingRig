using DrillingRig.Commands.BsEthernetNominals;

namespace DrillingRig.ConfigApp.BsEthernetNominals {
	public class BsEthernetNominalsSimple : IBsEthernetNominals {
		private readonly short _ratedRotationFriquencyCalculated;
		private readonly short _ratedPwmModulationCoefficient;
		private readonly short _ratedMomentumCurrentSetting;
		private readonly short _ratedRadiatorTemperature;
		private readonly short _ratedDcBusVoltage;
		private readonly short _ratedAllPhasesCurrentAmplitudeEnvelopeCurve;
		private readonly short _ratedRegulatorCurrentDoutput;
		private readonly short _ratedRegulatorCurrentQoutput;
		private readonly short _ratedFriquencyIntensitySetpointOutput;
		private readonly short _ratedFlowSetting;
		private readonly short _ratedMeasuredMoment;
		private readonly short _ratedSpeedRegulatorOutputOrMomentSetting;
		private readonly short _ratedMeasuredFlow;
		private readonly short _ratedSettingExcitationCurrent;

		public BsEthernetNominalsSimple(short ratedRotationFriquencyCalculated, short ratedPwmModulationCoefficient, short ratedMomentumCurrentSetting, short ratedRadiatorTemperature, short ratedDcBusVoltage, short ratedAllPhasesCurrentAmplitudeEnvelopeCurve, short ratedRegulatorCurrentDoutput, short ratedRegulatorCurrentQoutput, short ratedFriquencyIntensitySetpointOutput, short ratedFlowSetting, short ratedMeasuredMoment, short ratedSpeedRegulatorOutputOrMomentSetting, short ratedMeasuredFlow, short ratedSettingExcitationCurrent) {
			_ratedRotationFriquencyCalculated = ratedRotationFriquencyCalculated;
			_ratedPwmModulationCoefficient = ratedPwmModulationCoefficient;
			_ratedMomentumCurrentSetting = ratedMomentumCurrentSetting;
			_ratedRadiatorTemperature = ratedRadiatorTemperature;
			_ratedDcBusVoltage = ratedDcBusVoltage;
			_ratedAllPhasesCurrentAmplitudeEnvelopeCurve = ratedAllPhasesCurrentAmplitudeEnvelopeCurve;
			_ratedRegulatorCurrentDoutput = ratedRegulatorCurrentDoutput;
			_ratedRegulatorCurrentQoutput = ratedRegulatorCurrentQoutput;
			_ratedFriquencyIntensitySetpointOutput = ratedFriquencyIntensitySetpointOutput;
			_ratedFlowSetting = ratedFlowSetting;
			_ratedMeasuredMoment = ratedMeasuredMoment;
			_ratedSpeedRegulatorOutputOrMomentSetting = ratedSpeedRegulatorOutputOrMomentSetting;
			_ratedMeasuredFlow = ratedMeasuredFlow;
			_ratedSettingExcitationCurrent = ratedSettingExcitationCurrent;
		}

		public short RatedRotationFriquencyCalculated {
			get { return _ratedRotationFriquencyCalculated; }
		}

		public short RatedPwmModulationCoefficient {
			get { return _ratedPwmModulationCoefficient; }
		}

		public short RatedMomentumCurrentSetting {
			get { return _ratedMomentumCurrentSetting; }
		}

		public short RatedRadiatorTemperature {
			get { return _ratedRadiatorTemperature; }
		}

		public short RatedDcBusVoltage {
			get { return _ratedDcBusVoltage; }
		}

		public short RatedAllPhasesCurrentAmplitudeEnvelopeCurve {
			get { return _ratedAllPhasesCurrentAmplitudeEnvelopeCurve; }
		}

		public short RatedRegulatorCurrentDoutput {
			get { return _ratedRegulatorCurrentDoutput; }
		}

		public short RatedRegulatorCurrentQoutput {
			get { return _ratedRegulatorCurrentQoutput; }
		}

		public short RatedFriquencyIntensitySetpointOutput {
			get { return _ratedFriquencyIntensitySetpointOutput; }
		}

		public short RatedFlowSetting {
			get { return _ratedFlowSetting; }
		}

		public short RatedMeasuredMoment {
			get { return _ratedMeasuredMoment; }
		}

		public short RatedSpeedRegulatorOutputOrMomentSetting {
			get { return _ratedSpeedRegulatorOutputOrMomentSetting; }
		}

		public short RatedMeasuredFlow {
			get { return _ratedMeasuredFlow; }
		}

		public short RatedSettingExcitationCurrent {
			get { return _ratedSettingExcitationCurrent; }
		}
	}
}