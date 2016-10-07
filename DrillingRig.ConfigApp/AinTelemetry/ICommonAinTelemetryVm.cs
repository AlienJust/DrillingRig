namespace DrillingRig.ConfigApp.AinTelemetry {
	internal interface ICommonAinTelemetryVm {
		void UpdateCommonEngineState(ushort? value);
		void UpdateCommonFaultState(ushort? value);
		void UpdateAinsLinkState(bool? ain1LinkFault, bool? ain2LinkFault, bool? ain3LinkFault);
		void UpdateAinStatuses(ushort? status1, ushort? status2, ushort? status3);
	}
}