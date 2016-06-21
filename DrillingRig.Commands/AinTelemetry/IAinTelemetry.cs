using System;

namespace DrillingRig.Commands.AinTelemetry {
	public interface IAinTelemetry {
		ushort CommonFaultState { get; }
		ushort CommonEngineState { get; }

		/// <summary>
		/// Fe, Вычисленная частота вращения (электрическая)
		/// </summary>
		double RotationFriquencyCalculated { get; }

		/// <summary>
		///Uout, Коэффициент модуляции ШИМ
		/// </summary>
		double PwmModulationCoefficient { get; }

		/// <summary>
		/// IQset, Задание моментного тока
		/// </summary>
		double MomentumCurrentSetting { get; }

		/// <summary>
		/// T, Температура радиатора
		/// </summary>
		double RadiatorTemperature { get; }

		/// <summary>
		/// Udc, Напряжение шины DC
		/// </summary>
		double DcBusVoltage { get; }

		/// <summary>
		/// Isum, Амплитуда огибающей тока по всем фазам
		/// </summary>
		double AllPhasesCurrentAmplitudeEnvelopeCurve { get; }

		/// <summary>
		/// Ud, Выход регулятора тока D
		/// </summary>
		double RegulatorCurrentDoutput { get; }

		/// <summary>
		/// Uq, Выход регулятора тока Q
		/// </summary>
		double RegulatorCurrentQoutput { get; }

		/// <summary>
		/// Fout, Выход задатчика интенсивности частоты (электрической)
		/// </summary>
		double FriquencyIntensitySetpointOutput { get; }

		/// <summary>
		/// Flset, Уставка потока
		/// </summary>
		double FlowSetting { get; }

		/// <summary>
		/// Torq, Измеренный момент
		/// </summary>
		double MeasuredMoment { get; }

		/// <summary>
		/// Mout, Выход рерулятора скорости или уставка моментного тока
		/// </summary>
		double SpeedRegulatorOutputOrMomentSetting { get; }

		/// <summary>
		/// Flmag, Измеренный поток
		/// </summary>
		double MeasuredFlow { get; }


		double SettingExcitationCurrent { get; }

		ModeSetRunModeBits12 RunModeBits12 { get; }
		bool ResetZiToZero { get; }
		bool ResetFault { get; }
		bool LimitRegulatorId { get; }
		bool LimitRegulatorIq { get; }
		bool LimitRegulatorSpeed { get; }
		bool LimitRegulatorFlow { get; }
		ModeSetMomentumSetterSelector MomentumSetterSelector { get; }


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