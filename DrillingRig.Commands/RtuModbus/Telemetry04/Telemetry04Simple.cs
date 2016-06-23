namespace DrillingRig.Commands.RtuModbus.Telemetry04 {
	class Telemetry04Simple : ITelemetry04 {
		public short Pver { get; set; }
		public ushort PvDate { get; set; }
		public short BsVer { get; set; }
	}
}