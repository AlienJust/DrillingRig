namespace DrillingRig.Commands.RtuModbus.Telemetry08 {
	/// <summary>
	/// 08.ХХ. СЛОВА СОСТОЯНИЯ.
	/// </summary>
	public interface ITelemetry08 {
		/// <summary>
		/// 01 MAIN STATUS WORD Главное слово состояния.
		/// </summary>
		ushort Msw { get; }

		/// <summary>
		/// 02 AUX STATUS WORD Вспомогательное слово состояния
		/// </summary>
		ushort Asw { get; } //

		/// <summary>
		/// 03 Этап работы с частотным приводом.
		/// </summary>
		ushort EngineState { get; }

		/// <summary>
		/// 04 MSW Ведомого привода.
		/// </summary>
		ushort FollowMsw { get; }

		/// <summary>
		/// 05 ASW Ведомого привода.
		/// </summary>
		ushort FollowAsw { get; }

		/// <summary>
		/// 06 (Ведомый привод) Этап работы с частотным приводом.
		/// </summary>
		ushort FollowEngineState { get; }
	}
}
