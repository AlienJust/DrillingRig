using System;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	class AinSettingsPartWritable : IAinSettingsPart {
		public int? KpW { get; set; }
		public int? KiW { get; set; }
		public short? FiNom { get; set; }
		public short? Imax { get; set; }
		public short? UdcMax { get; set; }
		public short? UdcMin { get; set; }
		public short? Fnom { get; set; }
		public short? Fmax { get; set; }
		public short? DflLim { get; set; }
		public short? FlMinMin { get; set; }
		public short? IoutMax { get; set; }
		public short? FiMin { get; set; }
		public short? DacCh { get; set; }
		public short? Imcw { get; set; }
		public short? Ia0 { get; set; }
		public short? Ib0 { get; set; }
		public short? Ic0 { get; set; }
		public short? Udc0 { get; set; }
		public short? TauR { get; set; }
		public short? Lm { get; set; }
		public short? Lsl { get; set; }
		public short? Lrl { get; set; }
		public int? KpFi { get; set; }
		public int? KiFi { get; set; }
		public int? KpId { get; set; }
		public int? KiId { get; set; }
		public int? KpIq { get; set; }
		public int? KiIq { get; set; }
		public short? AccDfDt { get; set; }
		public short? DecDfDt { get; set; }
		public short? Unom { get; set; }
		public short? TauFlLim { get; set; }
		public short? Rs { get; set; }
		public short? Fmin { get; set; }
		public short? TauM { get; set; }
		public short? TauF { get; set; }
		public short? TauFSet { get; set; }
		public short? TauFi { get; set; }
		public short? IdSetMin { get; set; }
		public short? IdSetMax { get; set; }
		public int? KpFe { get; set; }
		public int? KiFe { get; set; }
		public short? Np { get; set; }
		public short? UmodThr { get; set; }
		public short? EmdecDfdt { get; set; }
		public short? TextMax { get; set; }
		public short? ToHl { get; set; }

		public bool? Ain1LinkFault { get; set; }
		public bool? Ain2LinkFault { get; set; }
		public bool? Ain3LinkFault { get; set; }
	}

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
			if (settings.KpFe != settingsReReaded.KpFe) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр KpFe)");
			if (settings.KiFe != settingsReReaded.KiFe) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр KiFe)");
			if (settings.Np != settingsReReaded.Np) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр Np)");
			if (settings.UmodThr != settingsReReaded.UmodThr) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр UmodThr)");
			if (settings.EmdecDfdt != settingsReReaded.EmdecDfdt) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр EmdecDfdt)");
			if (settings.TextMax != settingsReReaded.TextMax) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр TextMax)");
			if (settings.ToHl != settingsReReaded.ToHl) throw new Exception("При повторном чтении вычитанные настройки не совпали с записываемыми (параметр ToHl)");
		}
	}
}