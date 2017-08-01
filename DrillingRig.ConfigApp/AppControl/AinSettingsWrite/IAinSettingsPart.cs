using AlienJust.Support.Collections;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	interface IAinSettingsPart {
		decimal? KpW { get; }
		decimal? KiW { get; }

		decimal? FiNom { get; }
		short? Imax { get; }

		short? UdcMax { get; }
		short? UdcMin { get; }

		decimal? Fnom { get; }
		decimal? Fmax { get; }

		decimal? DflLim { get; }
		decimal? FlMinMin { get; }

		short? IoutMax { get; }
		decimal? FiMin { get; }

		ushort? DacCh { get; }
		ushort? Imcw { get; }

		short? Ia0 { get; }
		short? Ib0 { get; }
		short? Ic0 { get; }

		short? Udc0 { get; }

		decimal? TauR { get; }
		decimal? Lm { get; }
		decimal? Lsl { get; }
		decimal? Lrl { get; }

		decimal? KpFi { get; }
		decimal? KiFi { get; }

		decimal? KpId { get; }
		decimal? KiId { get; }

		decimal? KpIq { get; }
		decimal? KiIq { get; }

		decimal? AccDfDt { get; }
		decimal? DecDfDt { get; }

		decimal? Unom { get; }

		decimal? TauFlLim { get; }

		decimal? Rs { get; }
		decimal? Fmin { get; }

		decimal? TauM { get; }
		decimal? TauF { get; }
		decimal? TauFSet { get; }
		decimal? TauFi { get; }

		short? IdSetMin { get; }
		short? IdSetMax { get; }

		BytesPair? UchMin { get; }
		BytesPair? UchMax { get; }


		int? Np { get; }
		int? NimpFloorCode { get; }
		AinTelemetryFanWorkmode? FanMode { get; }
		bool? DirectCurrentMagnetization { get; }

		decimal? UmodThr { get; }

		decimal? EmdecDfdt { get; }
		short? TextMax { get; }
		short? ToHl { get; }

		bool? Ain1LinkFault { get; }
		bool? Ain2LinkFault { get; }
		bool? Ain3LinkFault { get; }
	}
}