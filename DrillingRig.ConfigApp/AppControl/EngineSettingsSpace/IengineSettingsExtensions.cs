using System;
using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace {
	public static class IengineSettingsExtensions {
		public static void CompareSettingsAfterReReading(this IEngineSettings settings, IEngineSettings settingsReReaded, int zeroBasedAinNumber) {
			if (settings.Inom != settingsReReaded.Inom) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр Inom)");
			if (settings.Nnom != settingsReReaded.Nnom) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр Nnom)");
			if (settings.Nmax != settingsReReaded.Nmax) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр Nmax)");
			if (settings.Pnom != settingsReReaded.Pnom) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр Pnom)");
			if (settings.CosFi != settingsReReaded.CosFi) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр CosFi)");
			if (settings.Eff != settingsReReaded.Eff) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр Eff)");
			if (settings.Mass != settingsReReaded.Mass) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр Mass)");
			if (settings.MmM != settingsReReaded.MmM) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр MmM)");
			if (settings.Height != settingsReReaded.Height) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр Height)");
			if (settings.I2Tmax != settingsReReaded.I2Tmax) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр I2Tmax)");
			if (settings.Icontinious != settingsReReaded.Icontinious) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр Icontinious)");
			if (settings.ZeroF != settingsReReaded.ZeroF) throw new Exception("ѕри повторном чтении вычитанные настройки не совпали с записываемыми (параметр ZeroF)");
		}
	}
}