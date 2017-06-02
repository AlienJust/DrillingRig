using AlienJust.Support.Collections;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	class AinSettingsPartWritable : IAinSettingsPart {
		public double? KpW { get; set; }
		public double? KiW { get; set; }
		public double? FiNom { get; set; }
		public short? Imax { get; set; }
		public short? UdcMax { get; set; }
		public short? UdcMin { get; set; }
		public double? Fnom { get; set; }
		public double? Fmax { get; set; }
		public double? DflLim { get; set; }
		public double? FlMinMin { get; set; }
		public short? IoutMax { get; set; }
		public double? FiMin { get; set; }
		public short? DacCh { get; set; }
		public short? Imcw { get; set; }
		public short? Ia0 { get; set; }
		public short? Ib0 { get; set; }
		public short? Ic0 { get; set; }
		public short? Udc0 { get; set; }

		public double? TauR { get; set; }
		public double? Lm { get; set; }
		public double? Lsl { get; set; }
		public double? Lrl { get; set; }

		public double? KpFi { get; set; }
		public double? KiFi { get; set; }
		public double? KpId { get; set; }
		public double? KiId { get; set; }
		public double? KpIq { get; set; }
		public double? KiIq { get; set; }
		public short? AccDfDt { get; set; }
		public short? DecDfDt { get; set; }
		public double? Unom { get; set; }
		public double? TauFlLim { get; set; }
		public double? Rs { get; set; }
		public double? Fmin { get; set; }
		public short? TauM { get; set; }
		public short? TauF { get; set; }
		public short? TauFSet { get; set; }
		public short? TauFi { get; set; }
		public short? IdSetMin { get; set; }
		public short? IdSetMax { get; set; }
		public BytesPair? UchMin { get; set; }
		public BytesPair? UchMax { get; set; }

		public int? Np { get; set; }
		public int? NimpFloorCode { get; set; }
		public AinTelemetryFanWorkmode? FanMode { get; set; }

		public double? UmodThr { get; set; }
		public short? EmdecDfdt { get; set; }
		public short? TextMax { get; set; }
		public short? ToHl { get; set; }

		public bool? Ain1LinkFault { get; set; }
		public bool? Ain2LinkFault { get; set; }
		public bool? Ain3LinkFault { get; set; }
	}
}