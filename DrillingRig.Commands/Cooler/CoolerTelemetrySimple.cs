namespace DrillingRig.Commands.Cooler {
	class CoolerTelemetrySimple : ICoolerTelemetry {
		private readonly ushort _diagnostic;
		private readonly double _coolingLiquidPressure;
		private readonly double _fanSpeed;
		private readonly double _coolingLiquidTemperature;
		private readonly ushort _reserve1;
		private readonly ushort _reserve2;

		public CoolerTelemetrySimple(ushort diagnostic, double coolingLiquidPressure, double fanSpeed, double coolingLiquidTemperature, ushort reserve1, ushort reserve2) {
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

		public double CoolingLiquidPressure {
			get { return _coolingLiquidPressure; }
		}

		public double FanSpeed {
			get { return _fanSpeed; }
		}

		public double CoolingLiquidTemperature {
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