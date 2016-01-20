namespace DrillingRig.Commands.AinTelemetry {
	public interface IAinTelemetry {
		FaultState CommonFaultState { get; }
		EngineState CommonEngineState { get; }

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

		bool Ain1LinkFault { get; }
		bool Ain2LinkFault { get; }
		bool Ain3LinkFault { get; }
	}
}