namespace DrillingRig.Commands.RtuModbus.CommonTelemetry {
	class CommonTelemetrySimple : ICommonTelemetry {
		public CommonTelemetrySimple(ushort commonEngineState, ushort commonFaultState, bool ain1LinkFault, bool ain2LinkFault, bool ain3LinkFault, ushort ain1Status) {
			CommonEngineState = commonEngineState;
			CommonFaultState = commonFaultState;
			Ain1LinkFault = ain1LinkFault;
			Ain2LinkFault = ain2LinkFault;
			Ain3LinkFault = ain3LinkFault;
			Ain1Status = ain1Status;
		}

		public ushort CommonEngineState { get; }
		public ushort CommonFaultState { get; }
		public bool Ain1LinkFault { get; }
		public bool Ain2LinkFault { get; }
		public bool Ain3LinkFault { get; }
		public ushort Ain1Status { get; }
	}
}