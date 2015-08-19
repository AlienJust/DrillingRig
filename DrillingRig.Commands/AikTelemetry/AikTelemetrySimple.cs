namespace DrillingRig.Commands.AikTelemetry {
	class AikTelemetrySimple : IAikTelemetry {
		private readonly double _rotationFriquencyCalculated;
		private readonly double _pwmModulationCoefficient;
		private readonly double _momentumCurrentSetting;
		private readonly double _radiatorTemperature;
		private readonly double _dcBusVoltage;
		private readonly double _allPhasesCurrentAmplitudeEnvelopeCurve;
		private readonly double _regulatorCurrentDoutput;
		private readonly double _regulatorCurrentQoutput;
		private readonly double _friquencyIntensitySetpointOutput;
		private readonly double _flowSetting;
		private readonly double _measuredMoment;
		private readonly double _speedRegulatorOutputOrMomentSetting;
		private readonly double _measuredFlow;
		private readonly double _settingExcitationCurrent;

		private readonly ModeSetRunModeBits12 _runModeBits12;
		private readonly bool _runModeRotationDirection;

		private readonly bool _driver1HasErrors;
		private readonly bool _driver2HasErrors;
		private readonly bool _driver3HasErrors;
		private readonly bool _driver4HasErrors;
		private readonly bool _driver5HasErrors;
		private readonly bool _driver6HasErrors;

		private readonly bool _somePhaseMaximumAlowedCurrentExcess;
		private readonly bool _radiatorKeysTemperatureRiseTo85DegreesExcess;

		private readonly bool _allowedDcVoltageExcess;
		private readonly bool _eepromI2CErrorDefaultParamsAreLoaded;
		private readonly bool _eepromCrcErrorDefaultParamsAreLoaded;
		
		private readonly double _rotationFriquencyMeasuredDcv;
		private readonly double _afterFilterSpeedControllerFeedbackFriquency;
		private readonly double _afterFilterFimag;
		private readonly double _currentDpartMeasured;
		private readonly double _currentQpartMeasured;
		private readonly double _afterFilterFset;
		private readonly double _afterFilterTorq;
		private readonly double _dCurrentRegulatorProportionalPart;
		private readonly double _qcurrentRegulatorProportionalPart;
		private readonly double _speedRegulatorProportionalPart;
		private readonly double _flowRegulatorProportionalPart;
		private readonly double _calculatorDflowRegulatorOutput;
		private readonly double _calculatorQflowRegulatorOutput;

		public AikTelemetrySimple(
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

			bool runModeRotationDirection, 
			bool driver1HasErrors, 
			bool driver2HasErrors, 
			bool driver3HasErrors, 
			bool driver4HasErrors, 
			bool driver5HasErrors, 
			bool driver6HasErrors, 
			bool somePhaseMaximumAlowedCurrentExcess, 
			bool radiatorKeysTemperatureRiseTo85DegreesExcess,
			bool allowedDcVoltageExcess,
			bool eepromI2CErrorDefaultParamsAreLoaded, 
			bool eepromCrcErrorDefaultParamsAreLoaded, 
			double rotationFriquencyMeasuredDcv, 
			double afterFilterSpeedControllerFeedbackFriquency, 
			double afterFilterFimag, 
			double currentDpartMeasured, 
			double currentQpartMeasured, 
			double afterFilterFset, 
			double afterFilterTorq, 
			double dCurrentRegulatorProportionalPart, 
			double qcurrentRegulatorProportionalPart, 
			double speedRegulatorProportionalPart, 
			double flowRegulatorProportionalPart, 
			double calculatorDflowRegulatorOutput, 
			double calculatorQflowRegulatorOutput) {

			_rotationFriquencyCalculated = rotationFriquencyCalculated;
			_pwmModulationCoefficient = pwmModulationCoefficient;
			_momentumCurrentSetting = momentumCurrentSetting;
			_radiatorTemperature = radiatorTemperature;
			_dcBusVoltage = dcBusVoltage;
			_allPhasesCurrentAmplitudeEnvelopeCurve = allPhasesCurrentAmplitudeEnvelopeCurve;
			_regulatorCurrentDoutput = regulatorCurrentDoutput;
			_regulatorCurrentQoutput = regulatorCurrentQoutput;
			_friquencyIntensitySetpointOutput = friquencyIntensitySetpointOutput;
			_flowSetting = flowSetting;
			_measuredMoment = measuredMoment;
			_speedRegulatorOutputOrMomentSetting = speedRegulatorOutputOrMomentSetting;
			_measuredFlow = measuredFlow;
			_settingExcitationCurrent = settingExcitationCurrent;

			_runModeBits12 = runModeBits12;
			_runModeRotationDirection = runModeRotationDirection;

			_driver1HasErrors = driver1HasErrors;
			_driver2HasErrors = driver2HasErrors;
			_driver3HasErrors = driver3HasErrors;
			_driver4HasErrors = driver4HasErrors;
			_driver5HasErrors = driver5HasErrors;
			_driver6HasErrors = driver6HasErrors;

			_somePhaseMaximumAlowedCurrentExcess = somePhaseMaximumAlowedCurrentExcess;
			_radiatorKeysTemperatureRiseTo85DegreesExcess = radiatorKeysTemperatureRiseTo85DegreesExcess;

			_allowedDcVoltageExcess = allowedDcVoltageExcess;
			_eepromI2CErrorDefaultParamsAreLoaded = eepromI2CErrorDefaultParamsAreLoaded;
			_eepromCrcErrorDefaultParamsAreLoaded = eepromCrcErrorDefaultParamsAreLoaded;

			_rotationFriquencyMeasuredDcv = rotationFriquencyMeasuredDcv;
			_afterFilterSpeedControllerFeedbackFriquency = afterFilterSpeedControllerFeedbackFriquency;
			_afterFilterFimag = afterFilterFimag;
			_currentDpartMeasured = currentDpartMeasured;
			_currentQpartMeasured = currentQpartMeasured;
			_afterFilterFset = afterFilterFset;
			_afterFilterTorq = afterFilterTorq;
			_dCurrentRegulatorProportionalPart = dCurrentRegulatorProportionalPart;
			_qcurrentRegulatorProportionalPart = qcurrentRegulatorProportionalPart;
			_speedRegulatorProportionalPart = speedRegulatorProportionalPart;
			_flowRegulatorProportionalPart = flowRegulatorProportionalPart;
			_calculatorDflowRegulatorOutput = calculatorDflowRegulatorOutput;
			_calculatorQflowRegulatorOutput = calculatorQflowRegulatorOutput;
		}

		public double RotationFriquencyCalculated {
			get { return _rotationFriquencyCalculated; }
		}

		public double PwmModulationCoefficient {
			get { return _pwmModulationCoefficient; }
		}

		public double MomentumCurrentSetting {
			get { return _momentumCurrentSetting; }
		}

		public double RadiatorTemperature {
			get { return _radiatorTemperature; }
		}

		public double DcBusVoltage {
			get { return _dcBusVoltage; }
		}

		public double AllPhasesCurrentAmplitudeEnvelopeCurve {
			get { return _allPhasesCurrentAmplitudeEnvelopeCurve; }
		}

		public double RegulatorCurrentDoutput {
			get { return _regulatorCurrentDoutput; }
		}

		public double RegulatorCurrentQoutput {
			get { return _regulatorCurrentQoutput; }
		}

		public double FriquencyIntensitySetpointOutput {
			get { return _friquencyIntensitySetpointOutput; }
		}

		public double FlowSetting {
			get { return _flowSetting; }
		}

		public double MeasuredMoment {
			get { return _measuredMoment; }
		}

		public double SpeedRegulatorOutputOrMomentSetting {
			get { return _speedRegulatorOutputOrMomentSetting; }
		}

		public double MeasuredFlow {
			get { return _measuredFlow; }
		}

		public double SettingExcitationCurrent {
			get { return _settingExcitationCurrent; }
		}

		public ModeSetRunModeBits12 RunModeBits12 {
			get { return _runModeBits12; }
		}

		public bool RunModeRotationDirection {
			get { return _runModeRotationDirection; }
		}

		public bool Driver1HasErrors {
			get { return _driver1HasErrors; }
		}

		public bool Driver2HasErrors {
			get { return _driver2HasErrors; }
		}

		public bool Driver3HasErrors {
			get { return _driver3HasErrors; }
		}

		public bool Driver4HasErrors {
			get { return _driver4HasErrors; }
		}

		public bool Driver5HasErrors {
			get { return _driver5HasErrors; }
		}

		public bool Driver6HasErrors {
			get { return _driver6HasErrors; }
		}

		public bool SomePhaseMaximumAlowedCurrentExcess {
			get { return _somePhaseMaximumAlowedCurrentExcess; }
		}

		public bool RadiatorKeysTemperatureRiseTo85DegreesExcess {
			get { return _radiatorKeysTemperatureRiseTo85DegreesExcess; }
		}

		public bool AllowedDcVoltageExcess
		{
			get { return _allowedDcVoltageExcess; }
		}

		public bool EepromI2CErrorDefaultParamsAreLoaded {
			get { return _eepromI2CErrorDefaultParamsAreLoaded; }
		}

		public bool EepromCrcErrorDefaultParamsAreLoaded {
			get { return _eepromCrcErrorDefaultParamsAreLoaded; }
		}

		public double RotationFriquencyMeasuredDcv {
			get { return _rotationFriquencyMeasuredDcv; }
		}

		public double AfterFilterSpeedControllerFeedbackFriquency {
			get { return _afterFilterSpeedControllerFeedbackFriquency; }
		}

		public double AfterFilterFimag {
			get { return _afterFilterFimag; }
		}

		public double CurrentDpartMeasured {
			get { return _currentDpartMeasured; }
		}

		public double CurrentQpartMeasured {
			get { return _currentQpartMeasured; }
		}

		public double AfterFilterFset {
			get { return _afterFilterFset; }
		}

		public double AfterFilterTorq {
			get { return _afterFilterTorq; }
		}

		public double DCurrentRegulatorProportionalPart {
			get { return _dCurrentRegulatorProportionalPart; }
		}

		public double QcurrentRegulatorProportionalPart {
			get { return _qcurrentRegulatorProportionalPart; }
		}

		public double SpeedRegulatorProportionalPart {
			get { return _speedRegulatorProportionalPart; }
		}

		public double FlowRegulatorProportionalPart {
			get { return _flowRegulatorProportionalPart; }
		}

		public double CalculatorDflowRegulatorOutput {
			get { return _calculatorDflowRegulatorOutput; }
		}

		public double CalculatorQflowRegulatorOutput {
			get { return _calculatorQflowRegulatorOutput; }
		}
	}
}