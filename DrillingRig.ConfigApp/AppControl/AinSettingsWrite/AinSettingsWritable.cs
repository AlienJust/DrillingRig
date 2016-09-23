using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw {
	class AinSettingsWritable : IAinSettings {
		public int KpW { get; set; }
		public int KiW { get; set; }
		public short FiNom { get; set; }
		public short Imax { get; set; }
		public short UdcMax { get; set; }
		public short UdcMin { get; set; }
		public short Fnom { get; set; }
		public short Fmax { get; set; }
		public short DflLim { get; set; }
		public short FlMinMin { get; set; }
		public short IoutMax { get; set; }
		public short FiMin { get; set; }
		public short DacCh { get; set; }
		public short Imcw { get; set; }
		public short Ia0 { get; set; }
		public short Ib0 { get; set; }
		public short Ic0 { get; set; }
		public short Udc0 { get; set; }
		public short TauR { get; set; }
		public short Lm { get; set; }
		public short Lsl { get; set; }
		public short Lrl { get; set; }
		public int KpFi { get; set; }
		public int KiFi { get; set; }
		public int KpId { get; set; }
		public int KiId { get; set; }
		public int KpIq { get; set; }
		public int KiIq { get; set; }
		public short AccDfDt { get; set; }
		public short DecDfDt { get; set; }
		public short Unom { get; set; }
		public short TauFlLim { get; set; }
		public short Rs { get; set; }
		public short Fmin { get; set; }
		public short TauM { get; set; }
		public short TauF { get; set; }
		public short TauFSet { get; set; }
		public short TauFi { get; set; }
		public short IdSetMin { get; set; }
		public short IdSetMax { get; set; }
		public int KpFe { get; set; }
		public int KiFe { get; set; }
		public short Np { get; set; }
		public short UmodThr { get; set; }
		public short EmdecDfdt { get; set; }
		public short TextMax { get; set; }
		public short ToHl { get; set; }
		public bool Ain1LinkFault { get; set; }
		public bool Ain2LinkFault { get; set; }
		public bool Ain3LinkFault { get; set; }


		public AinSettingsWritable(IAinSettings settings) {
			KpW = settings.KpW;
			KiW = settings.KiW;
			FiNom = settings.FiNom;
			Imax = settings.Imax;
			UdcMax = settings.UdcMax;
			UdcMin = settings.UdcMin;
			Fnom = settings.Fnom;
			Fmax = settings.Fmax;
			DflLim = settings.DflLim;
			FlMinMin = settings.FlMinMin;
			IoutMax = settings.IoutMax;
			FiMin = settings.FiMin;
			DacCh = settings.DacCh;
			Imcw = settings.Imcw;
			Ia0 = settings.Ia0;
			Ib0 = settings.Ib0;
			Ic0 = settings.Ic0;
			Udc0 = settings.Udc0;
			TauR = settings.TauR;
			Lm = settings.Lm;
			Lsl = settings.Lsl;
			Lrl = settings.Lrl;
			KpFi = settings.KpFi;
			KiFi = settings.KiFi;
			KpId = settings.KpId;
			KiId = settings.KiId;
			KpIq = settings.KpIq;
			KiIq = settings.KiIq;
			AccDfDt = settings.AccDfDt;
			DecDfDt = settings.DecDfDt;
			Unom = settings.Unom;
			TauFlLim = settings.TauFlLim;
			Rs = settings.Rs;
			Fmin = settings.Fmin;
			TauM = settings.TauM;
			TauF = settings.TauF;
			TauFSet = settings.TauFSet;
			TauFi = settings.TauFi;
			IdSetMin = settings.IdSetMin;
			IdSetMax = settings.IdSetMax;
			KpFe = settings.KpFe;
			KiFe = settings.KiFe;
			Np = settings.Np;
			UmodThr = settings.UmodThr;
			EmdecDfdt = settings.EmdecDfdt;
			TextMax = settings.TextMax;
			ToHl = settings.ToHl;
			Ain1LinkFault = settings.Ain1LinkFault;
			Ain2LinkFault = settings.Ain2LinkFault;
			Ain3LinkFault = settings.Ain3LinkFault;
		}

		public void ModifyFromPart(IAinSettingsPart part) {
			if (part.FiNom.HasValue) FiNom = part.FiNom.Value;
			if (part.Imax.HasValue) Imax = part.Imax.Value;
			if (part.UdcMax.HasValue) UdcMax = part.UdcMax.Value;
			if (part.UdcMin.HasValue) UdcMin = part.UdcMin.Value;
			if (part.Fnom.HasValue) Fnom = part.Fnom.Value;
			if (part.Fmax.HasValue) Fmax = part.Fmax.Value;
			if (part.DflLim.HasValue) DflLim = part.DflLim.Value;
			if (part.FlMinMin.HasValue) FlMinMin = part.FlMinMin.Value;
			if (part.IoutMax.HasValue) IoutMax = part.IoutMax.Value;
			if (part.FiMin.HasValue) FiMin = part.FiMin.Value;
			if (part.DacCh.HasValue) DacCh = part.DacCh.Value;
			if (part.Imcw.HasValue) Imcw = part.Imcw.Value;
			if (part.Ia0.HasValue) Ia0 = part.Ia0.Value;
			if (part.Ib0.HasValue) Ib0 = part.Ib0.Value;
			if (part.Ic0.HasValue) Ic0 = part.Ic0.Value;
			if (part.Udc0.HasValue) Udc0 = part.Udc0.Value;
			if (part.TauR.HasValue) TauR = part.TauR.Value;
			if (part.Lm.HasValue) Lm = part.Lm.Value;
			if (part.Lsl.HasValue) Lsl = part.Lsl.Value;
			if (part.Lrl.HasValue) Lrl = part.Lrl.Value;
			if (part.KpFi.HasValue) KpFi = part.KpFi.Value;
			if (part.KiFi.HasValue) KiFi = part.KiFi.Value;
			if (part.KpId.HasValue) KpId = part.KpId.Value;
			if (part.KiId.HasValue) KiId = part.KiId.Value;
			if (part.KpIq.HasValue) KpIq = part.KpIq.Value;
			if (part.KiIq.HasValue) KiIq = part.KiIq.Value;
			if (part.AccDfDt.HasValue) AccDfDt = part.AccDfDt.Value;
			if (part.DecDfDt.HasValue) DecDfDt = part.DecDfDt.Value;
			if (part.Unom.HasValue) Unom = part.Unom.Value;
			if (part.TauFlLim.HasValue) TauFlLim = part.TauFlLim.Value;
			if (part.Rs.HasValue) Rs = part.Rs.Value;
			if (part.Fmin.HasValue) Fmin = part.Fmin.Value;
			if (part.TauM.HasValue) TauM = part.TauM.Value;
			if (part.TauF.HasValue) TauF = part.TauF.Value;
			if (part.TauFSet.HasValue) TauFSet = part.TauFSet.Value;
			if (part.TauFi.HasValue) TauFi = part.TauFi.Value;
			if (part.IdSetMin.HasValue) IdSetMin = part.IdSetMin.Value;
			if (part.IdSetMax.HasValue) IdSetMax = part.IdSetMax.Value;
			if (part.KpFe.HasValue) KpFe = part.KpFe.Value;
			if (part.KiFe.HasValue) KiFe = part.KiFe.Value;
			if (part.Np.HasValue) Np = part.Np.Value;
			if (part.UmodThr.HasValue) UmodThr = part.UmodThr.Value;
			if (part.EmdecDfdt.HasValue) EmdecDfdt = part.EmdecDfdt.Value;
			if (part.TextMax.HasValue) TextMax = part.TextMax.Value;
			if (part.ToHl.HasValue) ToHl = part.ToHl.Value;

			if (part.Ain1LinkFault.HasValue) Ain1LinkFault = part.Ain1LinkFault.Value;
			if (part.Ain2LinkFault.HasValue) Ain2LinkFault = part.Ain2LinkFault.Value;
			if (part.Ain3LinkFault.HasValue) Ain3LinkFault = part.Ain3LinkFault.Value;
		}
	}
}