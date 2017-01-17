using AlienJust.Support.Collections;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	interface IAinSettingsPart {
		double? KpW { get; }
		double? KiW { get; }

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

		double? KpFi { get; }
		double? KiFi { get; }

		double? KpId { get; }
		double? KiId { get; }

		double? KpIq { get; }
		double? KiIq { get; }

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


		int? Np { get; }
		int? NimpFloorCode { get; }
		AinTelemetryFanWorkmode? FanMode { get; }


		short? UmodThr { get; }

		short? EmdecDfdt { get; }
		short? TextMax { get; }
		short? ToHl { get; }

		bool? Ain1LinkFault { get; }
		bool? Ain2LinkFault { get; }
		bool? Ain3LinkFault { get; }
	}
}