
namespace DrillingRig.Commands.AinSettings {
	public interface IAinSettings {
		int KpW { get; }
		int KiW { get; }
		short FiNom { get; }
		short Imax { get; }
		short UdcMax { get; }
		short UdcMin { get; }
		short Fnom { get; }
		short Fmax { get; }
		short IoutMax { get; }
		short FiMin { get; }
		short DacCh { get; }

		short Imcw { get; }

		short Ia0 { get; }
		short Ib0 { get; }
		short Ic0 { get; }

		short Udc0 { get; }
		short TauR { get; }
		short Lm { get; }

		short Lsl { get; }
		short Lrl { get; }

		int KpFi { get; }
		int KiFi { get; }

		int KpId { get; }
		int KiId { get; }
		int KpIq { get; }
		int KiIq { get; }

		short AccDfDt { get; }
		short DecDfDt { get; }

		short Unom { get; }

		short Rs { get; }
		short Fmin { get; }

		short TauM { get; }
		short TauF { get; }
		short TauFSet { get; }
		short TauFi { get; }

		short IdSetMin { get; }
		short IdSetMax { get; }

		int KpFe { get; }
		int KiFe { get; }

		short Np { get; }
		short EmdecDfdt { get; }
		short TextMax { get; }
		short ToHl { get; }
	}
}