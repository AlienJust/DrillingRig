using System;
using AlienJust.Support.Reflection;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	public static class IainSettingsExtensions {
		public static void CompareSettingsAfterReReading(this IAinSettings settings, IAinSettings settingsReReaded, int zeroBasedAinNumber) {
			if (zeroBasedAinNumber == 0) {
				if (settingsReReaded.Ain1LinkFault) 
					throw new Exception("ѕри повторном чтении (дл€ подтверждени€ записи) настройки были вычитаны, однако св€зь с ј»Ќ1 отсутсвовала (взведен флаг ошибки св€зи)");
			}
			else if (zeroBasedAinNumber == 1) {
				if (settingsReReaded.Ain2LinkFault)
					throw new Exception("ѕри повторном чтении (дл€ подтверждени€ записи) настройки были вычитаны, однако св€зь с ј»Ќ2 отсутсвовала (взведен флаг ошибки св€зи)");
			}
			else if (zeroBasedAinNumber == 2) {
				if (settingsReReaded.Ain3LinkFault)
					throw new Exception("ѕри повторном чтении (дл€ подтверждени€ записи) настройки были вычитаны, однако св€зь с ј»Ќ3 отсутсвовала (взведен флаг ошибки св€зи)");
			}

			string paramsText = string.Empty;

			if (settings.KpW != settingsReReaded.KpW) paramsText += $"{Environment.NewLine}параметр KpW был {settings.KpW:f10}; стал {settingsReReaded.KpW:f10}";
			if (settings.KiW != settingsReReaded.KiW) paramsText += $"{Environment.NewLine}параметр KiW был {settings.KiW:f10}; стал {settingsReReaded.KiW:f10}";

			if (settings.FiNom != settingsReReaded.FiNom) paramsText += $"{Environment.NewLine}параметр FiNom был {settings.FiNom}; стал {settingsReReaded.FiNom}";
			if (settings.Imax != settingsReReaded.Imax) paramsText += $"{Environment.NewLine}параметр Imax был {settings.Imax}; стал {settingsReReaded.Imax}";
			if (settings.UdcMax != settingsReReaded.UdcMax) paramsText += $"{Environment.NewLine}параметр UdcMax был {settings.UdcMax}; стал {settingsReReaded.UdcMax}";
			if (settings.UdcMin != settingsReReaded.UdcMin) paramsText += $"{Environment.NewLine}параметр UdcMin был {settings.UdcMin}; стал {settingsReReaded.UdcMin}";
			if (settings.Fnom != settingsReReaded.Fnom) paramsText += $"{Environment.NewLine}параметр Fnom был {settings.Fnom:f10}; стал {settingsReReaded.Fnom:f10}";
			if (settings.Fmax != settingsReReaded.Fmax) paramsText += $"{Environment.NewLine}параметр Fmax был {settings.Fmax:f10}; стал {settingsReReaded.Fmax:f10}";
			if (settings.DflLim != settingsReReaded.DflLim) paramsText += $"{Environment.NewLine}параметр DflLim был {settings.DflLim:f10}; стал {settingsReReaded.DflLim:f10}";
			if (settings.FlMinMin != settingsReReaded.FlMinMin) paramsText += $"{Environment.NewLine}параметр FlMinMin был {settings.FlMinMin}; стал {settingsReReaded.FlMinMin}";
			if (settings.IoutMax != settingsReReaded.IoutMax) paramsText += $"{Environment.NewLine}параметр IoutMax был {settings.IoutMax}; стал {settingsReReaded.IoutMax}";
			if (settings.FiMin != settingsReReaded.FiMin) paramsText += $"{Environment.NewLine}параметр FiMin был {settings.FiMin}; стал {settingsReReaded.FiMin}";
			if (settings.DacCh != settingsReReaded.DacCh) paramsText += $"{Environment.NewLine}параметр DacCh был {settings.DacCh}; стал {settingsReReaded.DacCh}";
			if (settings.Imcw != settingsReReaded.Imcw) paramsText += $"{Environment.NewLine}параметр Imcw был {settings.Imcw}; стал {settingsReReaded.Imcw}";
			if (settings.Ia0 != settingsReReaded.Ia0) paramsText += $"{Environment.NewLine}параметр Ia0 был {settings.Ia0}; стал {settingsReReaded.Ia0}";
			if (settings.Ib0 != settingsReReaded.Ib0) paramsText += $"{Environment.NewLine}параметр Ib0 был {settings.Ib0}; стал {settingsReReaded.Ib0}";
			if (settings.Ic0 != settingsReReaded.Ic0) paramsText += $"{Environment.NewLine}параметр Ic0 был {settings.Ic0}; стал {settingsReReaded.Ic0}";
			if (settings.Udc0 != settingsReReaded.Udc0) paramsText += $"{Environment.NewLine}параметр Udc0 был {settings.Udc0}; стал {settingsReReaded.Udc0}";
			if (settings.TauR != settingsReReaded.TauR) paramsText += $"{Environment.NewLine}параметр TauR был {settings.TauR:f10}; стал {settingsReReaded.TauR:f10}";
			if (settings.Lm != settingsReReaded.Lm) paramsText += $"{Environment.NewLine}параметр Lm был {settings.Lm:f10}; стал {settingsReReaded.Lm:f10}";
			if (settings.Lsl != settingsReReaded.Lsl) paramsText += $"{Environment.NewLine}параметр Lsl был {settings.Lsl:f10}; стал {settingsReReaded.Lsl:f10}";
			if (settings.Lrl != settingsReReaded.Lrl) paramsText += $"{Environment.NewLine}параметр Lrl был {settings.Lrl:f10}; стал {settingsReReaded.Lrl:f10}";

			if (settings.KpFi != settingsReReaded.KpFi) paramsText += $"{Environment.NewLine}параметр KpFi был {settings.KpFi:f10}; стал {settingsReReaded.KpFi:f10}";
			if (settings.KiFi != settingsReReaded.KiFi) paramsText += $"{Environment.NewLine}параметр KiFi был {settings.KiFi:f10}; стал {settingsReReaded.KiFi:f10}";

			if (settings.KpId != settingsReReaded.KpId) paramsText += $"{Environment.NewLine}параметр KpId был {settings.KpId:f10}; стал {settingsReReaded.KpId:f10}";
			if (settings.KiId != settingsReReaded.KiId) paramsText += $"{Environment.NewLine}параметр KiId был {settings.KiId:f10}; стал {settingsReReaded.KiId:f10}";
			if (settings.KpIq != settingsReReaded.KpIq) paramsText += $"{Environment.NewLine}параметр KpIq был {settings.KpIq:f10}; стал {settingsReReaded.KpIq:f10}";
			if (settings.KiIq != settingsReReaded.KiIq) paramsText += $"{Environment.NewLine}параметр KiIq был {settings.KiIq:f10}; стал {settingsReReaded.KiIq:f10}";

			if (settings.AccDfDt != settingsReReaded.AccDfDt) paramsText += $"{Environment.NewLine}параметр AccDfDt был {settings.AccDfDt}; стал {settingsReReaded.AccDfDt}";
			if (settings.DecDfDt != settingsReReaded.DecDfDt) paramsText += $"{Environment.NewLine}параметр DecDfDt был {settings.DecDfDt}; стал {settingsReReaded.DecDfDt}";

			if (settings.Unom != settingsReReaded.Unom) paramsText += $"{Environment.NewLine}параметр Unom был {settings.Unom:f10}; стал {settingsReReaded.Unom:f10}";
			if (settings.TauFlLim != settingsReReaded.TauFlLim) paramsText += $"{Environment.NewLine}параметр TauFlLim был {settings.TauFlLim:f10}; стал {settingsReReaded.TauFlLim:f10}";
			if (settings.Rs != settingsReReaded.Rs) paramsText += $"{Environment.NewLine}параметр Rs был {settings.Rs:f10}; стал {settingsReReaded.Rs:f10}";
			if (settings.Fmin != settingsReReaded.Fmin) paramsText += $"{Environment.NewLine}параметр Fmin был {settings.Fmin:f10}; стал {settingsReReaded.Fmin:f10}";

			if (settings.TauM != settingsReReaded.TauM) paramsText += $"{Environment.NewLine}параметр TauM был {settings.TauM}; стал {settingsReReaded.TauM}";
			if (settings.TauF != settingsReReaded.TauF) paramsText += $"{Environment.NewLine}параметр TauF был {settings.TauF}; стал {settingsReReaded.TauF}";
			if (settings.TauFSet != settingsReReaded.TauFSet) paramsText += $"{Environment.NewLine}параметр TauFSet был {settings.TauFSet}; стал {settingsReReaded.TauFSet}";
			if (settings.TauFi != settingsReReaded.TauFi) paramsText += $"{Environment.NewLine}параметр TauFi был {settings.TauFi}; стал {settingsReReaded.TauFi}";
			if (settings.IdSetMin != settingsReReaded.IdSetMin) paramsText += $"{Environment.NewLine}параметр IdSetMin был {settings.IdSetMin}; стал {settingsReReaded.IdSetMin}";
			if (settings.IdSetMax != settingsReReaded.IdSetMax) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(()=>settings.IdSetMax)} был {settings.IdSetMax}; стал {settingsReReaded.IdSetMax}";
			if (settings.UchMin != settingsReReaded.UchMin) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.UchMin)} был {settings.UchMin}; стал {settingsReReaded.UchMin}";
			if (settings.UchMax != settingsReReaded.UchMax) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.UchMax)} был {settings.UchMax}; стал {settingsReReaded.UchMax}";

			if (settings.Np != settingsReReaded.Np) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.Np)} был {settings.Np}; стал {settingsReReaded.Np}";
			if (settings.NimpFloorCode != settingsReReaded.NimpFloorCode) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.NimpFloorCode)} был {settings.NimpFloorCode}; стал {settingsReReaded.NimpFloorCode}";
			if (settings.FanMode != settingsReReaded.FanMode) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.FanMode)} был {settings.FanMode.ToIoBits()}; стал {settingsReReaded.FanMode.ToIoBits()}";
			if (settings.DirectCurrentMagnetization != settingsReReaded.DirectCurrentMagnetization) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.DirectCurrentMagnetization)} был {settings.DirectCurrentMagnetization}; стал {settingsReReaded.DirectCurrentMagnetization}";

			if (settings.UmodThr != settingsReReaded.UmodThr) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.UmodThr)} был {settings.UmodThr:f10}; стал {settingsReReaded.UmodThr:f10}";
			if (settings.EmdecDfdt != settingsReReaded.EmdecDfdt) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.EmdecDfdt)} был {settings.EmdecDfdt}; стал {settingsReReaded.EmdecDfdt}";
			if (settings.TextMax != settingsReReaded.TextMax) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.TextMax)} был {settings.TextMax}; стал {settingsReReaded.TextMax}";
			if (settings.ToHl != settingsReReaded.ToHl) paramsText += $"{Environment.NewLine}параметр {ReflectedProperty.GetName(() => settings.ToHl)} был {settings.ToHl}; стал {settingsReReaded.ToHl}";

			if (paramsText != string.Empty) throw new Exception("ќшибка при сравнении имеющихс€ настроек и прочитанных заново настроек: " + paramsText);
		}
	}
}