using System;

namespace DrillingRig.Commands.AinTelemetry {
	internal class AinTelemetrySimple : IAinTelemetry {
		public AinTelemetrySimple(
			ushort commonEngineState,
			ushort commonFaultState,

			double rotationFriquencyCalculated,
			double pwmModulationCoefficient,
			double momentumCurrentSetting,
			double radiatorTemperature,
			double dcBusVoltage,
			double allPhasesCurrentAmplitudeEnvelopeCurve,
			double regulatorCurrentDoutput,
			double regulatorCurrentQoutput,
			double friquencyIntensitySetpointOutput,
			double flowSetting,
			double measuredMoment,
			double speedRegulatorOutputOrMomentSetting,
			double measuredFlow,
			double settingExcitationCurrent,

			ModeSetRunModeBits12 runModeBits12,

			bool resetZiToZero,
			bool resetFault,
			bool limitRegulatorId,
			bool limitRegulatorIq,
			bool limitRegulatorSpeed,
			bool limitRegulatorFlow,

			ModeSetMomentumSetterSelector momentumSetterSelector,

			ushort status,

			bool driver1HasErrors,
			bool driver2HasErrors,
			bool driver3HasErrors,
			bool driver4HasErrors,
			bool driver5HasErrors,
			bool driver6HasErrors,
			bool somePhaseMaximumAlowedCurrentExcess,
			bool radiatorKeysTemperatureRiseTo85DegreesExcess,
			bool allowedDcVoltageExcess, bool noLinkOnSyncLine, bool externalTemperatureLimitExcess, bool rotationFriquecnySensorFault,
			bool eepromI2CErrorDefaultParamsAreLoaded,
			bool eepromCrcErrorDefaultParamsAreLoaded, bool someSlaveFault, bool configChangeDuringParallelWorkConfirmationNeed,
			double rotationFriquencyMeasuredDcv,
			double afterFilterSpeedControllerFeedbackFriquency,
			double afterFilterFimag,
			double currentDpartMeasured,
			double currentQpartMeasured,
			double afterFilterFset,
			double afterFilterTorq,
			double externalTemperature,
			double dCurrentRegulatorProportionalPart,
			double qcurrentRegulatorProportionalPart,
			double speedRegulatorProportionalPart,
			double flowRegulatorProportionalPart,
			double calculatorDflowRegulatorOutput,
			double calculatorQflowRegulatorOutput,

			ushort aux1, ushort aux2, ushort pver, DateTime? pvDate,

			bool ain1LinkFault,
			bool ain2LinkFault,
			bool ain3LinkFault) {
			CommonEngineState = commonEngineState;
			CommonFaultState = commonFaultState;

			RotationFriquencyCalculated = rotationFriquencyCalculated;
			PwmModulationCoefficient = pwmModulationCoefficient;
			MomentumCurrentSetting = momentumCurrentSetting;
			RadiatorTemperature = radiatorTemperature;
			DcBusVoltage = dcBusVoltage;
			AllPhasesCurrentAmplitudeEnvelopeCurve = allPhasesCurrentAmplitudeEnvelopeCurve;
			RegulatorCurrentDoutput = regulatorCurrentDoutput;
			RegulatorCurrentQoutput = regulatorCurrentQoutput;
			FriquencyIntensitySetpointOutput = friquencyIntensitySetpointOutput;
			FlowSetting = flowSetting;
			MeasuredMoment = measuredMoment;
			SpeedRegulatorOutputOrMomentSetting = speedRegulatorOutputOrMomentSetting;
			MeasuredFlow = measuredFlow;
			SettingExcitationCurrent = settingExcitationCurrent;

			RunModeBits12 = runModeBits12;
			ResetZiToZero = resetZiToZero;
			ResetFault = resetFault;
			LimitRegulatorId = limitRegulatorId;
			LimitRegulatorIq = limitRegulatorIq;
			LimitRegulatorSpeed = limitRegulatorSpeed;
			LimitRegulatorFlow = limitRegulatorFlow;

			MomentumSetterSelector = momentumSetterSelector;


			// Status
			Status = status;

			Driver1HasErrors = driver1HasErrors;
			Driver2HasErrors = driver2HasErrors;
			Driver3HasErrors = driver3HasErrors;
			Driver4HasErrors = driver4HasErrors;
			Driver5HasErrors = driver5HasErrors;
			Driver6HasErrors = driver6HasErrors;

			SomePhaseMaximumAlowedCurrentExcess = somePhaseMaximumAlowedCurrentExcess;
			RadiatorKeysTemperatureRiseTo85DegreesExcess = radiatorKeysTemperatureRiseTo85DegreesExcess;

			AllowedDcVoltageExcess = allowedDcVoltageExcess;
			NoLinkOnSyncLine = noLinkOnSyncLine;
			ExternalTemperatureLimitExcess = externalTemperatureLimitExcess;
			RotationFriquecnySensorFault = rotationFriquecnySensorFault;
			EepromI2CErrorDefaultParamsAreLoaded = eepromI2CErrorDefaultParamsAreLoaded;
			EepromCrcErrorDefaultParamsAreLoaded = eepromCrcErrorDefaultParamsAreLoaded;
			SomeSlaveFault = someSlaveFault;
			ConfigChangeDuringParallelWorkConfirmationNeed = configChangeDuringParallelWorkConfirmationNeed;

			//=============================================================


			RotationFriquencyMeasuredDcv = rotationFriquencyMeasuredDcv;
			AfterFilterSpeedControllerFeedbackFriquency = afterFilterSpeedControllerFeedbackFriquency;
			AfterFilterFimag = afterFilterFimag;
			CurrentDpartMeasured = currentDpartMeasured;
			CurrentQpartMeasured = currentQpartMeasured;
			AfterFilterFset = afterFilterFset;
			AfterFilterTorq = afterFilterTorq;

			ExternalTemperature = externalTemperature;

			DCurrentRegulatorProportionalPart = dCurrentRegulatorProportionalPart;
			QcurrentRegulatorProportionalPart = qcurrentRegulatorProportionalPart;
			SpeedRegulatorProportionalPart = speedRegulatorProportionalPart;
			FlowRegulatorProportionalPart = flowRegulatorProportionalPart;
			CalculatorDflowRegulatorOutput = calculatorDflowRegulatorOutput;
			CalculatorQflowRegulatorOutput = calculatorQflowRegulatorOutput;

			Aux1 = aux1;
			Aux2 = aux2;
			Pver = pver;
			PvDate = pvDate;

			Ain1LinkFault = ain1LinkFault;
			Ain2LinkFault = ain2LinkFault;
			Ain3LinkFault = ain3LinkFault;

		}

		public ushort CommonEngineState { get; }

		public ushort CommonFaultState { get; }


		public double RotationFriquencyCalculated { get; }

		public double PwmModulationCoefficient { get; }

		public double MomentumCurrentSetting { get; }

		public double RadiatorTemperature { get; }

		public double DcBusVoltage { get; }

		public double AllPhasesCurrentAmplitudeEnvelopeCurve { get; }

		public double RegulatorCurrentDoutput { get; }

		public double RegulatorCurrentQoutput { get; }

		public double FriquencyIntensitySetpointOutput { get; }

		public double FlowSetting { get; }

		public double MeasuredMoment { get; }

		public double SpeedRegulatorOutputOrMomentSetting { get; }

		public double MeasuredFlow { get; }

		public double SettingExcitationCurrent { get; }


		public ModeSetRunModeBits12 RunModeBits12 { get; }

		public bool ResetZiToZero { get; }

		public bool ResetFault { get; }

		public bool LimitRegulatorId { get; }

		public bool LimitRegulatorIq { get; }

		public bool LimitRegulatorSpeed { get; }

		public bool LimitRegulatorFlow { get; }

		public ModeSetMomentumSetterSelector MomentumSetterSelector { get; }


		public ushort Status { get; }

		public bool Driver1HasErrors { get; }

		public bool Driver2HasErrors { get; }

		public bool Driver3HasErrors { get; }

		public bool Driver4HasErrors { get; }

		public bool Driver5HasErrors { get; }

		public bool Driver6HasErrors { get; }

		public bool SomePhaseMaximumAlowedCurrentExcess { get; } // 6
		public bool RadiatorKeysTemperatureRiseTo85DegreesExcess { get; } // 7

		public bool AllowedDcVoltageExcess { get; } // 8

		public bool NoLinkOnSyncLine { get; } // 9
		public bool ExternalTemperatureLimitExcess { get; } // 10
		public bool RotationFriquecnySensorFault { get; } // 11

		public bool EepromI2CErrorDefaultParamsAreLoaded { get; } // 12
		public bool EepromCrcErrorDefaultParamsAreLoaded { get; } // 13

		public bool SomeSlaveFault { get; } // 14
		public bool ConfigChangeDuringParallelWorkConfirmationNeed { get; } // 15

		//--------------------------------------------------------------------------------------------

		public double RotationFriquencyMeasuredDcv { get; }

		public double AfterFilterSpeedControllerFeedbackFriquency { get; }

		public double AfterFilterFimag { get; }

		public double CurrentDpartMeasured { get; }

		public double CurrentQpartMeasured { get; }

		public double AfterFilterFset { get; }

		public double AfterFilterTorq { get; }

		public double ExternalTemperature { get; }

		public double DCurrentRegulatorProportionalPart { get; }

		public double QcurrentRegulatorProportionalPart { get; }

		public double SpeedRegulatorProportionalPart { get; }

		public double FlowRegulatorProportionalPart { get; }

		public double CalculatorDflowRegulatorOutput { get; }

		public double CalculatorQflowRegulatorOutput { get; }


		public ushort Aux1 { get; }

		public ushort Aux2 { get; }

		public ushort Pver { get; }

		public DateTime? PvDate { get; }


		public bool Ain1LinkFault { get; }

		public bool Ain2LinkFault { get; }

		public bool Ain3LinkFault { get; }
	}
}