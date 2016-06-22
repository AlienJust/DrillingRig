namespace DrillingRig.Commands.RtuModbus.Telemetry02 {
	/// <summary>
	/// 02.ХХ. ТЕКУЩИЕ СИГНАЛЫ скорость и момент.
	/// </summary>
	public interface ITelemetry02 {
		/// <summary>
		/// 01 Выход задатчика интенсивности частоты [об/мин]
		/// </summary>
		short Wout { get; }

		/// <summary>
		/// 02 Выход задатчика интенсивности после фильтра [об/мин]
		/// </summary>
		short WsetF { get; } //

		/// <summary>
		/// 03 Уставка потока [%]
		/// </summary>
		short FIset { get; }

		/// <summary>
		/// 04 Измеренный поток [%]
		/// </summary>
		short FImag { get; }

		/// <summary>
		/// 05 Измеренный поток после фильтра [%]
		/// </summary>
		short FImagF { get; }

		/// <summary>
		/// 06 Задание моментного тока [А]
		/// </summary>
		short IqSet { get; }

		/// <summary>
		/// 07 Задание тока возбуждения [А]
		/// </summary>
		short IdSet { get; }

		/// <summary>
		/// 08 Пропорциональная часть регулятора тока D [А]
		/// </summary>
		short Ed { get; }

		/// <summary>
		/// 09 Пропорциональная часть регулятора тока Q [А]
		/// </summary>
		short Eq { get; }

		/// <summary>
		/// 10 Пропорциональная часть регулятора скорости [об/мин]
		/// </summary>
		short Ef { get; }

		/// <summary>
		/// 11 Пропорциональная часть регулятора потока [%]
		/// </summary>
		short Efi { get; }
	}
}
