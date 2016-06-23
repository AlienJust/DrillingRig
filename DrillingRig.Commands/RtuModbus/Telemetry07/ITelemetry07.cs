namespace DrillingRig.Commands.RtuModbus.Telemetry07 {
	/// <summary>
	/// 07.ХХ. УПРАВЛЯЮЩИЕ СЛОВА.
	/// </summary>
	public interface ITelemetry07 {
		/// <summary>
		/// 01 MAIN CONTROL WORD Главное управляющее слово
		/// </summary>
		ushort Mcw { get; }
	}
}
