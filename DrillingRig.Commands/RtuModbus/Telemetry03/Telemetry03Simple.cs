namespace DrillingRig.Commands.RtuModbus.Telemetry03 {
	class Telemetry03Simple : ITelemetry03 {
		public short Kpwm { get; set; }
		public short Ud { get; set; }
		public short Uq { get; set; }
		public short Id { get; set; }
		public short Iq { get; set; }
		public short UcompD { get; set; }
		public short UCompQ { get; set; }
		public short Aux1 { get; set; }
		public short Aux2 { get; set; }
		public short I2t { get; set; }
		public short FollowMout { get; set; }
	}
}