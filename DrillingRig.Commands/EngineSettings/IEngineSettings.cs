namespace DrillingRig.Commands.EngineSettings {
	/// <summary>
	/// Настройки двигателя (названия свойств сгенерированы на основе названий из кода Марата (нижний уровень)
	/// </summary>
	public interface IEngineSettings {
		/// <summary>
		/// ГРАНИЦА ПЕРЕГРЕВА, [АМПЕР^2 *0.1сек]
		/// </summary>
		uint I2Tmax { get; }

		/// <summary>
		/// Номинальная Мощность двигателя [Вт]
		/// </summary>
		uint Pnom { get; }

		/// <summary>
		/// Номинальный ток, при котором остывание равно нагреву (RMS) [А]
		/// </summary>
		ushort Icontinious { get; }

		/// <summary>
		/// Номинальный моментный ток [Ампер]
		/// </summary>
		ushort Mnom { get; }

		/// <summary>
		/// *0.1Гц Скорость вращения (электрическая) ниже нулевого предела (ZERO_SPEED)
		/// </summary>
		ushort ZeroF { get; }
	}
}
