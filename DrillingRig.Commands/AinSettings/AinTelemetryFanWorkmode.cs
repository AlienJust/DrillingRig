namespace DrillingRig.Commands.AinSettings {
	public enum AinTelemetryFanWorkmode {
		/// <summary>
		/// Всегда выключен
		/// </summary>
		AllwaysOff,

		/// <summary>
		/// Включение вместе с ШИМ, выключение через 2 минуты после снятия ШИМ
		/// </summary>
		SwitchOnSyncToPwmSwtichOffTwoMinutesLaterAfterPwmOff,

		/// <summary>
		/// Выключение при снижении температуры ниже 45 градусов после снятия ШИМ
		/// </summary>
		SwitchOnSyncToPwmSwtichOffAfterPwmOffAndTempGoesDownBelow45C,

		/// <summary>
		/// Всегда включен
		/// </summary>
		AllwaysOn
	}
}