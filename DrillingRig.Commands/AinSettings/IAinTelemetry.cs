
namespace DrillingRig.Commands.AinSettings {
	public interface IAinSettings {
		double SpeedRegulatorProportionalCoefficient { get; }
		double SpeedRegulatorIntegralCoefficient { get; }
		double FlowWithoutFieldWeakening { get; }
		double MaximumCurrentAmplitudeForDefence { get; }
		double MaximumDcBusVoltageForDefence { get; }
		double MimimalDcBusVoltage { get; }
		double NominalElectricFriquency { get; }
		double MaximumElectricFriquency { get; }
		double CurrentAmplitudeLimitation { get; }
		double FlowSetting { get; }
		double MeasuredMoment { get; }
		double SpeedRegulatorOutputOrMomentSetting { get; }
		double MeasuredFlow { get; }
		double SettingExcitationCurrent { get; }

		bool RunModeRotationDirection { get; }

		bool Driver1HasErrors { get; }
		bool Driver2HasErrors { get; }
		bool Driver3HasErrors { get; }
		bool Driver4HasErrors { get; }
		bool Driver5HasErrors { get; }
		bool Driver6HasErrors { get; }

		bool SomePhaseMaximumAlowedCurrentExcess { get; }
		bool RadiatorKeysTemperatureRiseTo85DegreesExcess { get; }

		bool AllowedDcVoltageExcess { get; }
		bool EepromI2CErrorDefaultParamsAreLoaded { get; }
		bool EepromCrcErrorDefaultParamsAreLoaded { get; }

		double RotationFriquencyMeasuredDcv { get; }
		double AfterFilterSpeedControllerFeedbackFriquency { get; }
		double AfterFilterFimag { get; }
		double CurrentDpartMeasured { get; }
		double CurrentQpartMeasured { get; }
		double AfterFilterFset { get; }
		double AfterFilterTorq { get; }

		double DCurrentRegulatorProportionalPart { get; }
		double QcurrentRegulatorProportionalPart { get; }
		double SpeedRegulatorProportionalPart { get; }
		double FlowRegulatorProportionalPart { get; }

		double CalculatorDflowRegulatorOutput { get; }
		double CalculatorQflowRegulatorOutput { get; }
		
	}
}