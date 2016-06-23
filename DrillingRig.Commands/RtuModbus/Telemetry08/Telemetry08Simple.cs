namespace DrillingRig.Commands.RtuModbus.Telemetry08 {
	class Telemetry08Simple : ITelemetry08 {
		public ushort Msw { get; set; }
		public ushort Asw { get; set; }
		public ushort EngineState { get; set; }

		public ushort FollowMsw { get; set; }
		public ushort FollowAsw { get; set; }
		public ushort FollowEngineState { get; set; }
	}
}