namespace DrillingRig.Commands.Rectifier
{
	public interface IRectifierTelemetry {
		short VoltageInput1 { get; }
		short VoltageInput2 { get; }
		short VoltageInput3 { get; }
		short VoltageOutputDc { get; }
		short Current1 { get; }
		short Current2 { get; }
		short Current3 { get; }
		short Temperature { get; }
	}
}
