namespace DrillingRig.Commands.RtuModbus.Telemetry02 {
	class Telemetry02Simple : ITelemetry02 {
		public short Wout { get; set; }
		public short WsetF { get; set; }
		public short FIset { get; set; }
		public short FImag { get; set; }
		public short FImagF { get; set; }
		public short IqSet { get; set; }
		public short IdSet { get; set; }
		public short Ed { get; set; }
		public short Eq { get; set; }
		public short Ef { get; set; }
		public short Efi { get; set; }
	}
}