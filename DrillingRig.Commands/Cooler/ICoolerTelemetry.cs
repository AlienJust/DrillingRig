namespace DrillingRig.Commands.Cooler
{
	public interface ICoolerTelemetry {
		ushort Diagnostic { get; }
		double CoolingLiquidPressure { get; }
		double FanSpeed { get; }
		double CoolingLiquidTemperature { get; }
		ushort Reserve1 { get; }
		ushort Reserve2 { get; }
	}
}
