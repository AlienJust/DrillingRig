using AlienJust.Support.Collections;

namespace DrillingRig.Commands.RtuModbus.CommonTelemetry {
	public interface ICommonTelemetry {
		ushort CommonEngineState { get; }
		ushort CommonFaultState { get; }
		bool Ain1LinkFault { get; }
		bool Ain2LinkFault { get; }
		bool Ain3LinkFault { get; }
		ushort Ain1Status { get; }
		ushort Ain2Status { get; }
		ushort Ain3Status { get; }

		BytesPair Mcw { get; }
		BytesPair Msw { get; }
		BytesPair Asw { get; }
		BytesPair Fset { get; }
		BytesPair Mset { get; }
		BytesPair Reserve3 { get; }

		BytesPair MMin { get; }
		BytesPair MMax { get; }
	}
}
