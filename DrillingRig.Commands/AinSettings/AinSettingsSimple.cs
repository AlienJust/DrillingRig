using AlienJust.Support.Collections;

namespace DrillingRig.Commands.AinSettings {
	public class AinSettingsSimple : IAinSettings {
		public AinSettingsSimple(
			BytesPair reserved00,
			decimal kpW,
			decimal kiW,

			decimal fiNom,
			short imax,
			short udcMax,
			short udcMin,
			decimal fnom,
			decimal fmax,

			decimal dflLim,
			decimal flMinMin,

			short ioutMax,
			decimal fiMin,
			ushort dacCh,
			ushort imcw,
			short ia0,
			short ib0,
			short ic0,
			short udc0,

			decimal tauR,
			decimal lm,
			decimal lsl,
			decimal lrl,
			BytesPair reserved24,

			decimal kpFi,
			decimal kiFi,

			BytesPair reserved28,

			decimal kpId,
			decimal kiId,

			BytesPair reserved32,

			decimal kpIq,
			decimal kiIq,

			decimal accDfDt,
			decimal decDfDt,
			decimal unom,

			decimal tauFlLim,

			decimal rs,
			decimal fmin,

			decimal tauM,
			decimal tauF,
			decimal tauFSet,
			decimal tauFi,

			short idSetMin,
			short idSetMax,
			BytesPair uchMin,
			BytesPair uchMax,

			BytesPair reserved50,
			BytesPair reserved51,

			int np,
			int nimpFloorCode,
			AinTelemetryFanWorkmode fanMode,

			decimal umodThr,

			decimal emdecDfdt,
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

		public decimal KpW { get; }
		public decimal KiW { get; }

		public decimal FiNom { get; }

		public short Imax { get; }

		public short UdcMax { get; }
		public short UdcMin { get; }

		public decimal Fnom { get; }
		public decimal Fmax { get; }

		public decimal DflLim { get; }

		public decimal FlMinMin { get; }

		public short IoutMax { get; }

		public decimal FiMin { get; }

		public ushort DacCh { get; }
		public ushort Imcw { get; }

		public short Ia0 { get; }
		public short Ib0 { get; }
		public short Ic0 { get; }

		public short Udc0 { get; }

		public decimal TauR { get; }
		public decimal Lm { get; }
		public decimal Lsl { get; }
		public decimal Lrl { get; }

		public BytesPair Reserved24 { get; }

		public decimal KpFi { get; }
		public decimal KiFi { get; }

		public BytesPair Reserved28 { get; }

		public decimal KpId { get; }
		public decimal KiId { get; }

		public BytesPair Reserved32 { get; }

		public decimal KpIq { get; }
		public decimal KiIq { get; }

		public decimal AccDfDt { get; }
		public decimal DecDfDt { get; }

		public decimal Unom { get; }

		public decimal TauFlLim { get; }

		public decimal Rs { get; }

		public decimal Fmin { get; }

		public decimal TauM { get; }
		public decimal TauF { get; }
		public decimal TauFSet { get; }
		public decimal TauFi { get; }

		public short IdSetMin { get; }
		public short IdSetMax { get; }

		public BytesPair UchMin { get; }

		public BytesPair UchMax { get; }
		public BytesPair Reserved50 { get; }
		public BytesPair Reserved51 { get; }

		public int Np { get; }
		public int NimpFloorCode { get; }
		public AinTelemetryFanWorkmode FanMode { get; }

		public decimal UmodThr { get; }

		public decimal EmdecDfdt { get; }

		public short TextMax { get; }

		public short ToHl { get; }

		public bool Ain1LinkFault { get; }
		public bool Ain2LinkFault { get; }
		public bool Ain3LinkFault { get; }
	}
}