using System;

namespace DrillingRig.Commands.AinTelemetry {
	public interface IAinTelemetry {
		ushort CommonFaultState { get; }
		ushort CommonEngineState { get; }

		double RotationFriquencyCalculated { get; }
		double PwmModulationCoefficient { get; }
		double MomentumCurrentSetting { get; }
		double RadiatorTemperature { get; }
		double DcBusVoltage { get; }
		double AllPhasesCurrentAmplitudeEnvelopeCurve { get; }
		double RegulatorCurrentDoutput { get; }
		double RegulatorCurrentQoutput { get; }
		double FriquencyIntensitySetpointOutput { get; }
		double FlowSetting { get; }
		double MeasuredMoment { get; }
		double SpeedRegulatorOutputOrMomentSetting { get; }
		double MeasuredFlow { get; }
		double SettingExcitationCurrent { get; }

		ModeSetRunModeBits12 RunModeBits12 { get; }
		bool RunModeRotationDirection { get; }

		ushort Status { get; }

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

		double ExternalTemperature { get; }

		double DCurrentRegulatorProportionalPart { get; }
		double QcurrentRegulatorProportionalPart { get; }
		double SpeedRegulatorProportionalPart { get; }
		double FlowRegulatorProportionalPart { get; }

		double CalculatorDflowRegulatorOutput { get; }
		double CalculatorQflowRegulatorOutput { get; }

		ushort Aux1 { get; }
		ushort Aux2 { get; }
		ushort Pver { get; }
		DateTime? PvDate { get; } // TODO: m.b. change to DateTime?

		bool Ain1LinkFault { get; }
		bool Ain2LinkFault { get; }
		bool Ain3LinkFault { get; }
	}
}