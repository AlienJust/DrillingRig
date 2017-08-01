using AlienJust.Support.Collections;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	class AinSettingsPartWritable : IAinSettingsPart {
		public decimal? KpW { get; set; }
		public decimal? KiW { get; set; }
		public decimal? FiNom { get; set; }
		public short? Imax { get; set; }
		public short? UdcMax { get; set; }
		public short? UdcMin { get; set; }
		public decimal? Fnom { get; set; }
		public decimal? Fmax { get; set; }
		public decimal? DflLim { get; set; }
		public decimal? FlMinMin { get; set; }
		public short? IoutMax { get; set; }
		public decimal? FiMin { get; set; }
		public ushort? DacCh { get; set; }
		public ushort? Imcw { get; set; }
		public short? Ia0 { get; set; }
		public short? Ib0 { get; set; }
		public short? Ic0 { get; set; }
		public short? Udc0 { get; set; }

		public decimal? TauR { get; set; }
		public decimal? Lm { get; set; }
		public decimal? Lsl { get; set; }
		public decimal? Lrl { get; set; }

		public decimal? KpFi { get; set; }
		public decimal? KiFi { get; set; }
		public decimal? KpId { get; set; }
		public decimal? KiId { get; set; }
		public decimal? KpIq { get; set; }
		public decimal? KiIq { get; set; }
		public decimal? AccDfDt { get; set; }
		public decimal? DecDfDt { get; set; }
		public decimal? Unom { get; set; }
		public decimal? TauFlLim { get; set; }
		public decimal? Rs { get; set; }
		public decimal? Fmin { get; set; }
		public decimal? TauM { get; set; }
		public decimal? TauF { get; set; }
		public decimal? TauFSet { get; set; }
		public decimal? TauFi { get; set; }
		public short? IdSetMin { get; set; }
		public short? IdSetMax { get; set; }
		public BytesPair? UchMin { get; set; }
		public BytesPair? UchMax { get; set; }

		public int? Np { get; set; }
		public int? NimpFloorCode { get; set; }
		public AinTelemetryFanWorkmode? FanMode { get; set; }
		public bool? DirectCurrentMagnetization { get; set; }

		public decimal? UmodThr { get; set; }
		public decimal? EmdecDfdt { get; set; }
		public short? TextMax { get; set; }
		public short? ToHl { get; set; }

		public bool? Ain1LinkFault { get; set; }
		public bool? Ain2LinkFault { get; set; }
		public bool? Ain3LinkFault { get; set; }
	}
}