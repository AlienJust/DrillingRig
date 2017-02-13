namespace DrillingRig.ConfigApp.AppControl.EngineSettingsStorage {
	/// <summary>
	/// Часть настроек двигателя (названия свойств сгенерированы на основе названий из кода Марата (нижний уровень)
	/// </summary>
	public interface IEngineSettingsPart {
		/// <summary>
		/// Номинальный ток двигателя, А
		/// </summary>
		ushort? Inom { get; }

		/// <summary>
		/// Номинальная скорость двигателя, об/мин
		/// </summary>
		ushort? Nnom { get; }

		/// <summary>
		/// Максимальная скорость двигателя, об/мин
		/// </summary>
		ushort? Nmax { get; }

		/// <summary>
		/// Номинальная Мощность двигателя [кВт]
		/// </summary>
		ushort? Pnom { get; }

		/// <summary>
		/// Косинус Фи, ед. измерения?
		/// </summary>
		ushort? CosFi { get; }

		/// <summary>
		/// КПД двигателя, %
		/// </summary>
		ushort? Eff { get; }

		/// <summary>
		/// Масса двигателя, кг
		/// </summary>
		ushort? Mass { get; }

		/// <summary>
		/// Кратность момента
		/// </summary>
		ushort? MmM { get; }

		/// <summary>
		/// Высота двигателя, мм
		/// </summary>
		ushort? Height { get; }

		/// <summary>
		/// ГРАНИЦА ПЕРЕГРЕВА, [АМПЕР^2 *0.1сек]
		/// </summary>
		uint? I2Tmax { get; }
		
		/// <summary>
		/// Номинальный ток, при котором остывание равно нагреву (RMS) [А]
		/// </summary>
		ushort? Icontinious { get; }

		/// <summary>
		/// *0.1Гц Скорость вращения (электрическая) ниже нулевого предела (ZERO_SPEED)
		/// </summary>
		ushort? ZeroF { get; }
	}
}
