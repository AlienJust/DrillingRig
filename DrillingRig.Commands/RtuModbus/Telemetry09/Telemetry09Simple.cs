namespace DrillingRig.Commands.RtuModbus.Telemetry09 {
	class Telemetry09Simple : ITelemetry09 {
		public ushort Status1 { get; set; }
		public ushort Status2 { get; set; }
		public ushort Status3 { get; set; }

		public ushort FaultState { get; set; }
		public ushort Warning { get; set; }
		public ushort ErrLinkAin { get; set; }
		public ushort FollowStatus { get; set; }
	}
}