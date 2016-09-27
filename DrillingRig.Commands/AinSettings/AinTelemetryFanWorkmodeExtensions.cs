using System;

namespace DrillingRig.Commands.AinSettings {
	static class AinTelemetryFanWorkmodeExtensions {
		public static AinTelemetryFanWorkmode FromIoBits(int bits) {
			switch (bits) {
				case 0:
					return AinTelemetryFanWorkmode.AllwaysOff;
				case 1:
					return AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffTwoMinutesLaterAfterPwmOff;
				case 2:
					return AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffAfterPwmOffAndTempGoesDownBelow45C;
				case 3:
					return AinTelemetryFanWorkmode.AllwaysOn;
				default:
					throw new Exception("Cannot convert " + typeof(int).FullName + " value " + bits + " to " + typeof(AinTelemetryFanWorkmode).FullName);
			}
		}

		public static int ToIoBits(this AinTelemetryFanWorkmode fanMode) {
			switch (fanMode) {
				case AinTelemetryFanWorkmode.AllwaysOff:
					return 0;
				case AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffTwoMinutesLaterAfterPwmOff:
					return 1;
				case AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffAfterPwmOffAndTempGoesDownBelow45C:
					return 2;
				case AinTelemetryFanWorkmode.AllwaysOn:
					return 3;
				default:
					throw new Exception("Cannot convert " + typeof(AinTelemetryFanWorkmode).FullName + " value to " + typeof(int).FullName);
			}
		}

		public static string ToHumanString(this AinTelemetryFanWorkmode fanMode) {
			switch (fanMode) {
				case AinTelemetryFanWorkmode.AllwaysOff:
					return "Всегда выключен";
				case AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffTwoMinutesLaterAfterPwmOff:
					return "Включение вместе с ШИМ, выключение через 2 минуты после снятия ШИМ";
				case AinTelemetryFanWorkmode.SwitchOnSyncToPwmSwtichOffAfterPwmOffAndTempGoesDownBelow45C:
					return "Включение вместе с ШИМ, выключение при снижении температуры ниже 45 градусов после снятия ШИМ";
				case AinTelemetryFanWorkmode.AllwaysOn:
					return "Всегда включен";
				default:
					throw new Exception("Cannot convert " + typeof(AinTelemetryFanWorkmode).FullName + " value to " + typeof(string).FullName);
			}
		}
	}
}