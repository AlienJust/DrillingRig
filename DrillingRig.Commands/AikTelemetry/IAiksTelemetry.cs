namespace DrillingRig.Commands.AikTelemetry {
	public interface IAiksTelemetry {
		IAinTelemetry Aik1 { get; }
		IAinTelemetry Aik2 { get; }
		IAinTelemetry Aik3 { get; }
	}
}