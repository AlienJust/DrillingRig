using AlienJust.Support.Collections;

namespace DrillingRig.Commands.RtuModbus.CommonTelemetry {
	class CommonTelemetrySimple : ICommonTelemetry {
		public CommonTelemetrySimple(ushort commonEngineState, ushort commonFaultState, bool ain1LinkFault, bool ain2LinkFault, bool ain3LinkFault, ushort ain1Status, BytesPair mcw, BytesPair msw, BytesPair asw, BytesPair fset, BytesPair mset, BytesPair reserve3, BytesPair mMin, BytesPair max) {
			CommonEngineState = commonEngineState;
			CommonFaultState = commonFaultState;
			Ain1LinkFault = ain1LinkFault;
			Ain2LinkFault = ain2LinkFault;
			Ain3LinkFault = ain3LinkFault;
			Ain1Status = ain1Status;
			Mcw = mcw;
			Msw = msw;
			Asw = asw;
			Fset = fset;
			Mset = mset;
			Reserve3 = reserve3;
			MMin = mMin;
			Max = max;
		}

		public ushort CommonEngineState { get; }
		public ushort CommonFaultState { get; }
		public bool Ain1LinkFault { get; }
		public bool Ain2LinkFault { get; }
		public bool Ain3LinkFault { get; }
		public ushort Ain1Status { get; }
		public BytesPair Mcw { get; }
		public BytesPair Msw { get; }
		public BytesPair Asw { get; }
		public BytesPair Fset { get; }
		public BytesPair Mset { get; }
		public BytesPair Reserve3 { get; }
		public BytesPair MMin { get; }
		public BytesPair Max { get; }
	}
}