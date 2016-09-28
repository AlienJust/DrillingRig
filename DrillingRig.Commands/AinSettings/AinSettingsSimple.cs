using AlienJust.Support.Collections;

namespace DrillingRig.Commands.AinSettings {
	public class AinSettingsSimple : IAinSettings {
		public AinSettingsSimple(
			BytesPair reserved00,
			double kpW, 
			int kiW, 
			short fiNom, 
			short imax, 
			short udcMax, 
			short udcMin, 
			short fnom, 
			short fmax,

			short dflLim,
			short flMinMin,

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
			BytesPair reserved24,
			double kpFi, 
			int kiFi,
			BytesPair reserved28,
			double kpId, 
			int kiId,
			BytesPair reserved32,
			double kpIq, 
			int kiIq, 
			short accDfDt, 
			short decDfDt, 
			short unom,

			short tauFlLim,

			short rs, 
			short fmin, 
			short tauM, 
			short tauF, 
			short tauFSet, 
			short tauFi, 
			short idSetMin, 
			short idSetMax,
			BytesPair uchMin,
			BytesPair uchMax,

			BytesPair reserved50,
			BytesPair reserved51,

			int np,
			int nimpFloorCode,
			AinTelemetryFanWorkmode fanMode,

			short umodThr,

			short emdecDfdt, 
			short textMax, 
			short toHl, bool ain1LinkFault, bool ain2LinkFault, bool ain3LinkFault) {

			Reserved00 = reserved00;
			KpW = kpW;
			KiW = kiW;
			FiNom = fiNom;
			Imax = imax;
			UdcMax = udcMax;
			UdcMin = udcMin;
			Fnom = fnom;
			Fmax = fmax;
			
			DflLim = dflLim;
			FlMinMin = flMinMin;

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

			Reserved24 = reserved24;
			KpFi = kpFi;
			KiFi = kiFi;

			Reserved28 = reserved28;
			KpId = kpId;
			KiId = kiId;

			Reserved32 = reserved32;
			KpIq = kpIq;
			KiIq = kiIq;
			AccDfDt = accDfDt;
			DecDfDt = decDfDt;
			Unom = unom;

			TauFlLim = tauFlLim;

			Rs = rs;
			Fmin = fmin;
			TauM = tauM;
			TauF = tauF;
			TauFSet = tauFSet;
			TauFi = tauFi;
			IdSetMin = idSetMin;
			IdSetMax = idSetMax;
			UchMin = uchMin;
			UchMax = uchMax;

			Reserved50 = reserved50;
			Reserved51 = reserved51;

			Np = np;
			NimpFloorCode = nimpFloorCode;
			FanMode = fanMode;

			UmodThr = umodThr;

			EmdecDfdt = emdecDfdt;
			TextMax = textMax;
			ToHl = toHl;

			Ain1LinkFault = ain1LinkFault;
			Ain2LinkFault = ain2LinkFault;
			Ain3LinkFault = ain3LinkFault;
		}
		public BytesPair Reserved00 { get; }

		public double KpW { get; }

		public int KiW { get; }

		public short FiNom { get; }

		public short Imax { get; }

		public short UdcMax { get; }

		public short UdcMin { get; }

		public short Fnom { get; }

		public short Fmax { get; }

		public short DflLim { get; }

		public short FlMinMin { get; }

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
		public BytesPair Reserved24 { get; }

		public double KpFi { get; }

		public int KiFi { get; }
		public BytesPair Reserved28 { get; }

		public double KpId { get; }

		public int KiId { get; }
		public BytesPair Reserved32 { get; }

		public double KpIq { get; }

		public int KiIq { get; }

		public short AccDfDt { get; }

		public short DecDfDt { get; }

		public short Unom { get; }

		public short TauFlLim { get; }

		public short Rs { get; }

		public short Fmin { get; }

		public short TauM { get; }

		public short TauF { get; }

		public short TauFSet { get; }

		public short TauFi { get; }

		public short IdSetMin { get; }

		public short IdSetMax { get; }

		public BytesPair UchMin { get; }

		public BytesPair UchMax { get; }
		public BytesPair Reserved50 { get; }
		public BytesPair Reserved51 { get; }

		public int Np { get; }
		public int NimpFloorCode { get; }
		public AinTelemetryFanWorkmode FanMode { get; }

		public short UmodThr { get; }

		public short EmdecDfdt { get; }

		public short TextMax { get; }

		public short ToHl { get; }

		public bool Ain1LinkFault { get; }
		public bool Ain2LinkFault { get; }
		public bool Ain3LinkFault { get; }
	}
}