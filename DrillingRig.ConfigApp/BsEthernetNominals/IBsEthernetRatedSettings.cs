namespace DrillingRig.ConfigApp.BsEthernetNominals
{
	interface IBsEthernetRatedSettings
	{
		ushort RatedRotationFriquencyCalculated { get; set; }

		ushort RatedPwmModulationCoefficient { get; set; }

		ushort RatedMomentumCurrentSetting { get; set; }

		ushort RatedRadiatorTemperature { get; set; }

		ushort RatedDcBusVoltage { get; set; }

		ushort RatedAllPhasesCurrentAmplitudeEnvelopeCurve { get; set; }

		ushort RatedRegulatorCurrentDoutput { get; set; }

		ushort RatedRegulatorCurrentQoutput { get; set; }

		ushort RatedFriquencyIntensitySetpointOutput { get; set; }

		ushort RatedFlowSetting { get; set; }

		ushort RatedMeasuredMoment { get; set; }

		ushort RatedSpeedRegulatorOutputOrMomentSetting { get; set; }

		ushort RatedMeasuredFlow { get; set; }

		ushort RatedSettingExcitationCurrent { get; set; }
	}
}
