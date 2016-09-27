using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	public static class IainSettingsExtensions {
		public static void CompareSettingsAfterReReading(this IAinSettings settings, IAinSettings settingsReReaded, int zeroBasedAinNumber) {
			if (zeroBasedAinNumber == 0) {
				if (settingsReReaded.Ain1LinkFault) 
					throw new Exception("При повторном чтении (для подтверждения записи) настройки были вычитаны, однако связь с АИН1 отсутсвовала (взведен флаг ошибки связи)");
			}
			else if (zeroBasedAinNumber == 1) {
				if (settingsReReaded.Ain2LinkFault)
					throw new Exception("При повторном чтении (для подтверждения записи) настройки были вычитаны, однако связь с АИН2 отсутсвовала (взведен флаг ошибки связи)");
			}
			else if (zeroBasedAinNumber == 2) {
				if (settingsReReaded.Ain3LinkFault)
					throw new Exception("При повторном чтении (для подтверждения записи) настройки были вычитаны, однако связь с АИН3 отсутсвовала (взведен флаг ошибки связи)");
			}

			if (settings.FiNom != settingsReReaded.FiNom) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр FiNom)");
			if (settings.Imax != settingsReReaded.Imax) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Imax)");
			if (settings.UdcMax != settingsReReaded.UdcMax) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр UdcMax)");
			if (settings.UdcMin != settingsReReaded.UdcMin) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр UdcMin)");
			if (settings.Fnom != settingsReReaded.Fnom) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Fnom)");
			if (settings.Fmax != settingsReReaded.Fmax) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Fmax)");
			if (settings.DflLim != settingsReReaded.DflLim) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр DflLim)");
			if (settings.FlMinMin != settingsReReaded.FlMinMin) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр FlMinMin)");
			if (settings.IoutMax != settingsReReaded.IoutMax) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр IoutMax)");
			if (settings.FiMin != settingsReReaded.FiMin) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр FiMin)");
			if (settings.DacCh != settingsReReaded.DacCh) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр DacCh)");
			if (settings.Imcw != settingsReReaded.Imcw) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Imcw)");
			if (settings.Ia0 != settingsReReaded.Ia0) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Ia0), пришло значение: " + settingsReReaded.Ia0 + ", а ожидалось: " + settings.Ia0);
			if (settings.Ib0 != settingsReReaded.Ib0) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Ib0), пришло значение: " + settingsReReaded.Ib0 + ", а ожидалось: " + settings.Ib0);
			if (settings.Ic0 != settingsReReaded.Ic0) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Ic0), пришло значение: " + settingsReReaded.Ic0 + ", а ожидалось: " + settings.Ic0);
			if (settings.Udc0 != settingsReReaded.Udc0) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Udc0), пришло значение: " + settingsReReaded.Udc0 + ", а ожидалось: " + settings.Udc0);
			if (settings.TauR != settingsReReaded.TauR) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр TauR)");
			if (settings.Lm != settingsReReaded.Lm) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Lm)");
			if (settings.Lsl != settingsReReaded.Lsl) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Lsl)");
			if (settings.Lrl != settingsReReaded.Lrl) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Lrl)");
			if (settings.KpFi != settingsReReaded.KpFi) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр KpFi)");
			if (settings.KiFi != settingsReReaded.KiFi) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр KiFi)");
			if (settings.KpId != settingsReReaded.KpId) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр KpId)");
			if (settings.KiId != settingsReReaded.KiId) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр KiId)");
			if (settings.KpIq != settingsReReaded.KpIq) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр KpIq)");
			if (settings.KiIq != settingsReReaded.KiIq) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр KiIq)");
			if (settings.AccDfDt != settingsReReaded.AccDfDt) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр AccDfDt)");
			if (settings.DecDfDt != settingsReReaded.DecDfDt) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр DecDfDt)");
			if (settings.Unom != settingsReReaded.Unom) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Unom)");
			if (settings.TauFlLim != settingsReReaded.TauFlLim) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр TauFlLim)");
			if (settings.Rs != settingsReReaded.Rs) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Rs)");
			if (settings.Fmin != settingsReReaded.Fmin) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Fmin)");
			if (settings.TauM != settingsReReaded.TauM) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр TauM)");
			if (settings.TauF != settingsReReaded.TauF) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр TauF)");
			if (settings.TauFSet != settingsReReaded.TauFSet) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр TauFSet)");
			if (settings.TauFi != settingsReReaded.TauFi) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр TauFi)");
			if (settings.IdSetMin != settingsReReaded.IdSetMin) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр IdSetMin)");
			if (settings.IdSetMax != settingsReReaded.IdSetMax) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр IdSetMax)");
			if (settings.UchMin != settingsReReaded.UchMin) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр UchMin)");
			if (settings.UchMax != settingsReReaded.UchMax) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр UchMax)");

			if (settings.Np != settingsReReaded.Np) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Np)");
			if (settings.NimpFloorCode != settingsReReaded.NimpFloorCode) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр NimpFloorCode)");
			if (settings.FanMode != settingsReReaded.FanMode) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр FanMode)");

			if (settings.UmodThr != settingsReReaded.UmodThr) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр UmodThr)");
			if (settings.EmdecDfdt != settingsReReaded.EmdecDfdt) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр EmdecDfdt)");
			if (settings.TextMax != settingsReReaded.TextMax) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр TextMax)");
			if (settings.ToHl != settingsReReaded.ToHl) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр ToHl)");
		}
	}
}