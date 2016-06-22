namespace DrillingRig.Commands.RtuModbus.Telemetry01 {
	class Telemetry01Simple : ITelemetry01 {
		public short We { get; set; }
		public short Wm { get; set; }
		public short WfbF { get; set; }
		public short Isum { get; set; }
		public short Uout { get; set; }
		public short Udc { get; set; }
		public short T1 { get; set; }
		public short T2 { get; set; }
		public short T3 { get; set; }
		public short Text1 { get; set; }
		public short Text2 { get; set; }
		public short Text3 { get; set; }
		public short Torq { get; set; }
		public short TorqF { get; set; }
		public short Mout { get; set; }
		public short P { get; set; }
		public ushort Din { get; set; }
		public ushort Dout { get; set; }
		public ushort SelTorq { get; set; }
	}
}