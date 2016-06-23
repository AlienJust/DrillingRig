namespace DrillingRig.Commands.RtuModbus.Telemetry03 {
	/// <summary>
	/// 03.ХХ. ТЕКУЩИЕ СИГНАЛЫ внутренние параметры.
	/// </summary>
	public interface ITelemetry03 {
		/// <summary>
		/// 01 Коэффициент модуляции ШИМ [%]
		/// </summary>
		short Kpwm { get; }

		/// <summary>
		/// 02 Выход регулятора тока D [%]
		/// </summary>
		short Ud { get; } //

		/// <summary>
		/// 03 Выход регулятора тока Q [%]
		/// </summary>
		short Uq { get; }

		/// <summary>
		/// 04 Измеренная составляющая тока D [%]
		/// </summary>
		short Id { get; }

		/// <summary>
		/// 05 Измеренная составляющая тока Q [%]
		/// </summary>
		short Iq { get; }

		/// <summary>
		/// 06 Выход регулятора компенсатора вычислителя потока D [В]
		/// </summary>
		short UcompD { get; }

		/// <summary>
		/// 07 Выход регулятора компенсатора вычислителя потока Q [В]
		/// </summary>
		short UCompQ { get; }

		/// <summary>
		/// 08 Вспомогательная ячейка №1 АИН1
		/// </summary>
		short Aux1 { get; }

		/// <summary>
		/// 09 Вспомогательная ячейка №2 АИН1
		/// </summary>
		short Aux2 { get; }

		/// <summary>
		/// 10 Вычисленное текущее значение теплового показателя двигателя [А^2*c]
		/// </summary>
		short I2t { get; }

		/// <summary>
		/// 11 (Ведомый привод) Уставка моментного тока (Выход регулятора скорости) [%]
		/// </summary>
		short FollowMout { get; }
	}
}
