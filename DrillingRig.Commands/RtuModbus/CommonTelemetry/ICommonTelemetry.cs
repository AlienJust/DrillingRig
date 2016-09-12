using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrillingRig.Commands.RtuModbus.CommonTelemetry {
	public interface ICommonTelemetry {
		ushort CommonEngineState { get; }
		ushort CommonFaultState { get; }
		bool Ain1LinkFault { get; }
		bool Ain2LinkFault { get; }
		bool Ain3LinkFault { get; }
		ushort Ain1Status { get; }
	}
}
