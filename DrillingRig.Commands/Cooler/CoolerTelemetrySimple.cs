namespace DrillingRig.Commands.Cooler {
	class CoolerTelemetrySimple : ICoolerTelemetry {
		private readonly ushort _diagnostic;
		private readonly short _coolingLiquidPressure;
		private readonly short _fanSpeed;
		private readonly short _coolingLiquidTemperature;
		private readonly ushort _reserve1;
		private readonly ushort _reserve2;

		public CoolerTelemetrySimple(ushort diagnostic, short coolingLiquidPressure, short fanSpeed, short coolingLiquidTemperature, ushort reserve1, ushort reserve2)
		{
			_diagnostic = diagnostic;
			_coolingLiquidPressure = coolingLiquidPressure;
			_fanSpeed = fanSpeed;
			_coolingLiquidTemperature = coolingLiquidTemperature;
			_reserve1 = reserve1;
			_reserve2 = reserve2;
		}

		public ushort Diagnostic {
			get { return _diagnostic; }
		}

		public short CoolingLiquidPressure
		{
			get { return _coolingLiquidPressure; }
		}

		public short FanSpeed
		{
			get { return _fanSpeed; }
		}

		public short CoolingLiquidTemperature
		{
			get { return _coolingLiquidTemperature; }
		}

		public ushort Reserve1 {
			get { return _reserve1; }
		}

		public ushort Reserve2 {
			get { return _reserve2; }
		}
	}
}