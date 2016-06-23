namespace DrillingRig.Commands.RtuModbus.Telemetry09 {
	/// <summary>
	/// 09.ХХ. СЛОВА ОТКАЗОВ.
	/// </summary>
	public interface ITelemetry09 {
		/// <summary>
		/// 01 Биты ошибок АИН1
		/// </summary>
		ushort Status1 { get; }

		/// <summary>
		/// 02 Биты ошибок АИН2
		/// </summary>
		ushort Status2 { get; } //

		/// <summary>
		/// 03 Биты ошибок АИН3
		/// </summary>
		ushort Status3 { get; }

		/// <summary>
		/// 04 Текущий код аварии
		/// </summary>
		ushort FaultState { get; }

		/// <summary>
		/// 05 Код последнего сигнала предупреждения.
		/// </summary>
		ushort Warning { get; }

		/// <summary>
		/// 06 Ошибки связи с блоками АИН.
		/// </summary>
		ushort ErrLinkAin { get; }

		/// <summary>
		/// 07 (Ведомый привод) Биты ошибок АИН
		/// </summary>
		ushort FollowStatus { get; }
	}
}
