using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.AinTelemetry {
	internal interface ICommonAinTelemetryVm {
		void UpdateCommonEngineState(EngineState value);
		void UpdateCommonFaultState(EngineState value);
		void UpdateAinsLinkState(bool ain1Linkfault, bool ain2LinkFault, bool ain3LinkFault);
	}
}