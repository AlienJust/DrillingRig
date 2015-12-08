namespace DrillingRig.Commands.Rectifier {
	class RectifierTelemetrySimple : IRectifierTelemetry {
		private readonly short _voltageInput1;
		private readonly short _voltageInput2;
		private readonly short _voltageInput3;
		private readonly short _voltageOutputDc;
		private readonly short _current1;
		private readonly short _current2;
		private readonly short _current3;
		private readonly short _temperature;

		public RectifierTelemetrySimple(short voltageInput1, short voltageInput2, short voltageInput3, short voltageOutputDc, short current1, short current2, short current3, short temperature) {
			_voltageInput1 = voltageInput1;
			_voltageInput2 = voltageInput2;
			_voltageInput3 = voltageInput3;
			_voltageOutputDc = voltageOutputDc;
			_current1 = current1;
			_current2 = current2;
			_current3 = current3;
			_temperature = temperature;
		}

		public short VoltageInput1 {
			get { return _voltageInput1; }
		}

		public short VoltageInput2 {
			get { return _voltageInput2; }
		}

		public short VoltageInput3 {
			get { return _voltageInput3; }
		}

		public short VoltageOutputDc {
			get { return _voltageOutputDc; }
		}

		public short Current1 {
			get { return _current1; }
		}

		public short Current2 {
			get { return _current2; }
		}

		public short Current3 {
			get { return _current3; }
		}

		public short Temperature {
			get { return _temperature; }
		}
	}
}