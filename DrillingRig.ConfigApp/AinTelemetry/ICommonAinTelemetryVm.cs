using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.AinTelemetry {
	internal interface ICommonAinTelemetryVm {
		void UpdateCommonEngineState(EngineState? value);
		void UpdateCommonFaultState(FaultState? value);
		void UpdateAinsLinkState(bool? ain1LinkFault, bool? ain2LinkFault, bool? ain3LinkFault);
	}
}