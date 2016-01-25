namespace DrillingRig.ConfigApp.AinTelemetry {
	internal interface ICommonAinTelemetryVm {
		void UpdateCommonEngineState(ushort? value);
		void UpdateCommonFaultState(ushort? value);
		void UpdateAinsLinkState(bool? ain1LinkFault, bool? ain2LinkFault, bool? ain3LinkFault);
	}
}