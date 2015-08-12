namespace DrillingRig.Commands.BsEthernetNominals
{
	public interface IBsEthernetNominals
	{
		short RatedRotationFriquencyCalculated { get; }

		short RatedPwmModulationCoefficient { get; }

		short RatedMomentumCurrentSetting { get; }

		short RatedRadiatorTemperature { get; }

		short RatedDcBusVoltage { get; }

		short RatedAllPhasesCurrentAmplitudeEnvelopeCurve { get; }

		short RatedRegulatorCurrentDoutput { get; }

		short RatedRegulatorCurrentQoutput { get; }

		short RatedFriquencyIntensitySetpointOutput { get; }

		short RatedFlowSetting { get; }

		short RatedMeasuredMoment { get; }

		short RatedSpeedRegulatorOutputOrMomentSetting { get; }

		short RatedMeasuredFlow { get; }

		short RatedSettingExcitationCurrent { get; }
	}
}
