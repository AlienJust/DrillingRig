namespace DrillingRig.Commands.Cooler
{
	public interface ICoolerTelemetry {
		ushort Diagnostic { get; }
		short CoolingLiquidPressure { get; }
		short FanSpeed { get; }
		short CoolingLiquidTemperature { get; }
		ushort Reserve1 { get; }
		ushort Reserve2 { get; }
	}
}
