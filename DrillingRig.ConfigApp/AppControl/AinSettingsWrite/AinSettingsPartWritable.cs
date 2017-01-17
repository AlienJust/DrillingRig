using AlienJust.Support.Collections;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	class AinSettingsPartWritable : IAinSettingsPart {
		public double? KpW { get; set; }
		public double? KiW { get; set; }
		public short? FiNom { get; set; }
		public short? Imax { get; set; }
		public short? UdcMax { get; set; }
		public short? UdcMin { get; set; }
		public short? Fnom { get; set; }
		public short? Fmax { get; set; }
		public short? DflLim { get; set; }
		public short? FlMinMin { get; set; }
		public short? IoutMax { get; set; }
		public short? FiMin { get; set; }
		public short? DacCh { get; set; }
		public short? Imcw { get; set; }
		public short? Ia0 { get; set; }
		public short? Ib0 { get; set; }
		public short? Ic0 { get; set; }
		public short? Udc0 { get; set; }
		public short? TauR { get; set; }
		public short? Lm { get; set; }
		public short? Lsl { get; set; }
		public short? Lrl { get; set; }
		public double? KpFi { get; set; }
		public double? KiFi { get; set; }
		public double? KpId { get; set; }
		public double? KiId { get; set; }
		public double? KpIq { get; set; }
		public double? KiIq { get; set; }
		public short? AccDfDt { get; set; }
		public short? DecDfDt { get; set; }
		public short? Unom { get; set; }
		public short? TauFlLim { get; set; }
		public short? Rs { get; set; }
		public short? Fmin { get; set; }
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

		public short? UmodThr { get; set; }
		public short? EmdecDfdt { get; set; }
		public short? TextMax { get; set; }
		public short? ToHl { get; set; }

		public bool? Ain1LinkFault { get; set; }
		public bool? Ain2LinkFault { get; set; }
		public bool? Ain3LinkFault { get; set; }
	}
}