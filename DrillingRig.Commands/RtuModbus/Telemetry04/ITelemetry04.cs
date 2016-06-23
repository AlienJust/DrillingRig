namespace DrillingRig.Commands.RtuModbus.Telemetry04 {
	/// <summary>
	/// 04.ХХ. ИНФОРМАЦИЯ о прошивках.
	/// </summary>
	public interface ITelemetry04 {
		/// <summary>
		/// 01 Версия ПО (АИН)
		/// </summary>
		short Pver { get; }

		/// <summary>
		/// 02 Дата билда ПО (АИН)
		/// </summary>
		ushort PvDate { get; } //

		/// <summary>
		/// 03 Версия ПО (БС-Ethernet)
		/// </summary>
		short BsVer { get; }
	}
}
