namespace DrillingRig.Commands.AinSettings {
	public class AinSettingsSimple : IAinSettings {
		public AinSettingsSimple(
			int kpW, 
			int kiW, 
			short fiNom, 
			short imax, 
			short udcMax, 
			short udcMin, 
			short fnom, 
			short fmax,

			short empty10,
			short empty11,

			short ioutMax, 
			short fiMin, 
			short dacCh, 
			short imcw, 
			short ia0, 
			short ib0, 
			short ic0, 
			short udc0, 
			short tauR, 
			short lm, 
			short lsl, 
			short lrl, 
			int kpFi, 
			int kiFi, 
			int kpId, 
			int kiId, 
			int kpIq, 
			int kiIq, 
			short accDfDt, 
			short decDfDt, 
			short unom,

			short empty39,

			short rs, 
			short fmin, 
			short tauM, 
			short tauF, 
			short tauFSet, 
			short tauFi, 
			short idSetMin, 
			short idSetMax, 
			int kpFe, 
			int kiFe, 
			short np,

			short empty53,

			short emdecDfdt, 
			short textMax, 
			short toHl, bool ain1LinkFault, bool ain2LinkFault, bool ain3LinkFault) {
			KpW = kpW;
			KiW = kiW;
			FiNom = fiNom;
			Imax = imax;
			UdcMax = udcMax;
			UdcMin = udcMin;
			Fnom = fnom;
			Fmax = fmax;
			
			Empty10 = empty10;
			Empty11 = empty11;

			IoutMax = ioutMax;
			FiMin = fiMin;
			DacCh = dacCh;
			Imcw = imcw;
			Ia0 = ia0;
			Ib0 = ib0;
			Ic0 = ic0;
			Udc0 = udc0;
			TauR = tauR;
			Lm = lm;
			Lsl = lsl;
			Lrl = lrl;
			KpFi = kpFi;
			KiFi = kiFi;
			KpId = kpId;
			KiId = kiId;
			KpIq = kpIq;
			KiIq = kiIq;
			AccDfDt = accDfDt;
			DecDfDt = decDfDt;
			Unom = unom;

			Empty39 = empty39;

			Rs = rs;
			Fmin = fmin;
			TauM = tauM;
			TauF = tauF;
			TauFSet = tauFSet;
			TauFi = tauFi;
			IdSetMin = idSetMin;
			IdSetMax = idSetMax;
			KpFe = kpFe;
			KiFe = kiFe;
			
			Np = np;

			Empty53 = empty53;

			EmdecDfdt = emdecDfdt;
			TextMax = textMax;
			ToHl = toHl;
			Ain1LinkFault = ain1LinkFault;
			Ain2LinkFault = ain2LinkFault;
			Ain3LinkFault = ain3LinkFault;
		}

		public int KpW { get; }

		public int KiW { get; }

		public short FiNom { get; }

		public short Imax { get; }

		public short UdcMax { get; }

		public short UdcMin { get; }

		public short Fnom { get; }

		public short Fmax { get; }

		public short Empty10 { get; }

		public short Empty11 { get; }

		public short IoutMax { get; }

		public short FiMin { get; }

		public short DacCh { get; }

		public short Imcw { get; }

		public short Ia0 { get; }

		public short Ib0 { get; }

		public short Ic0 { get; }

		public short Udc0 { get; }

		public short TauR { get; }

		public short Lm { get; }

		public short Lsl { get; }

		public short Lrl { get; }

		public int KpFi { get; }

		public int KiFi { get; }

		public int KpId { get; }

		public int KiId { get; }

		public int KpIq { get; }

		public int KiIq { get; }

		public short AccDfDt { get; }

		public short DecDfDt { get; }

		public short Unom { get; }

		public short Empty39 { get; }

		public short Rs { get; }

		public short Fmin { get; }

		public short TauM { get; }

		public short TauF { get; }

		public short TauFSet { get; }

		public short TauFi { get; }

		public short IdSetMin { get; }

		public short IdSetMax { get; }

		public int KpFe { get; }

		public int KiFe { get; }

		public short Np { get; }

		public short Empty53 { get; }

		public short EmdecDfdt { get; }

		public short TextMax { get; }

		public short ToHl { get; }

		public bool Ain1LinkFault { get; }
		public bool Ain2LinkFault { get; }
		public bool Ain3LinkFault { get; }
	}
}