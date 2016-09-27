using AlienJust.Support.Collections;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	interface IAinSettingsPart {
		BytesPair? KpW { get; }
		int? KiW { get; }
		short? FiNom { get; }
		short? Imax { get; }
		short? UdcMax { get; }
		short? UdcMin { get; }
		short? Fnom { get; }
		short? Fmax { get; }

		short? DflLim { get; }
		short? FlMinMin { get; }

		short? IoutMax { get; }
		short? FiMin { get; }
		short? DacCh { get; }

		short? Imcw { get; }

		short? Ia0 { get; }
		short? Ib0 { get; }
		short? Ic0 { get; }

		short? Udc0 { get; }
		short? TauR { get; }
		short? Lm { get; }

		short? Lsl { get; }
		short? Lrl { get; }

		BytesPair? KpFi { get; }
		int? KiFi { get; }

		BytesPair? KpId { get; }
		int? KiId { get; }
		BytesPair? KpIq { get; }
		int? KiIq { get; }

		short? AccDfDt { get; }
		short? DecDfDt { get; }

		short? Unom { get; }

		short? TauFlLim { get; }

		short? Rs { get; }
		short? Fmin { get; }

		short? TauM { get; }
		short? TauF { get; }
		short? TauFSet { get; }
		short? TauFi { get; }

		short? IdSetMin { get; }
		short? IdSetMax { get; }

		BytesPair? UchMin { get; }
		BytesPair? UchMax { get; }

		short? Np { get; }

		short? UmodThr { get; }

		short? EmdecDfdt { get; }
		short? TextMax { get; }
		short? ToHl { get; }

		bool? Ain1LinkFault { get; }
		bool? Ain2LinkFault { get; }
		bool? Ain3LinkFault { get; }
	}
}