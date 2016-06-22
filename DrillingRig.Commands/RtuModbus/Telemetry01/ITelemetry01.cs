namespace DrillingRig.Commands.RtuModbus.Telemetry01 {
	/// <summary>
	/// 01.ХХ. ТЕКУЩИЕ СИГНАЛЫ основные.
	/// </summary>
	public interface ITelemetry01 {
		/// <summary>
		/// 01 Вычисленная частота вращения [об/мин]
		/// </summary>
		short We { get; }

		/// <summary>
		/// 02 Частота вращения, измеренная ДЧВ [об/мин]
		/// </summary>
		short Wm { get; } //

		/// <summary>
		/// 03 Частота на ОС регулятора скорости после фильтра [об/мин]
		/// </summary>
		short WfbF { get; }

		/// <summary>
		/// 04 Измеренный ток двигателя [А]
		/// </summary>
		short Isum { get; }

		/// <summary>
		/// 05 Вычисленное выходное напряжение на двигателе [В]
		/// </summary>
		short Uout { get; }

		/// <summary>
		/// 06 Напряжение шины DC [В]
		/// </summary>
		short Udc { get; }

		/// <summary>
		/// 07 Температура радиатора АИН1 [град С]
		/// </summary>
		short T1 { get; }

		/// <summary>
		/// 08 Температура радиатора АИН2 [град С]
		/// </summary>
		short T2 { get; }

		/// <summary>
		/// 09 Температура радиатора АИН3 [град С]
		/// </summary>
		short T3 { get; }

		/// <summary>
		/// 10 Температура внешняя АИН1 [град С]
		/// </summary>
		short Text1 { get; }

		/// <summary>
		/// 11 Температура внешняя АИН2 [град С]
		/// </summary>
		short Text2 { get; }

		/// <summary>
		/// 12 Температура внешняя АИН3 [град С]
		/// </summary>
		short Text3 { get; }

		/// <summary>
		/// 13 Измеренный момент [Нм]
		/// </summary>
		short Torq { get; }

		/// <summary>
		/// 14 Измеренный момент после фильтра [Нм]
		/// </summary>
		short TorqF { get; }

		/// <summary>
		/// 15 Уставка моментного тока (Выход регулятора скорости) [%]
		/// </summary>
		short Mout { get; }

		/// <summary>
		/// 16 Мощность, подаваемая на двигатель
		/// </summary>
		short P { get; }

		/// <summary>
		/// 17 Состояние цифровых входов
		/// </summary>
		ushort Din { get; }

		/// <summary>
		/// 18 Состояние релейных выходов
		/// </summary>
		ushort Dout { get; }

		/// <summary>
		/// 19 Активный режим регулирования (Управление по скорости/Управление крутящим моментом)
		/// </summary>
		ushort SelTorq { get; }
	}
}
