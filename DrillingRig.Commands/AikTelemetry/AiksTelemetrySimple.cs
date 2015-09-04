namespace DrillingRig.Commands.AikTelemetry {
	class AiksTelemetrySimple : IAiksTelemetry {
		private readonly IAinTelemetry _aik1;
		private readonly IAinTelemetry _aik2;
		private readonly IAinTelemetry _aik3;
		public AiksTelemetrySimple(IAinTelemetry aik1, IAinTelemetry aik2, IAinTelemetry aik3) {
			_aik1 = aik1;
			_aik2 = aik2;
			_aik3 = aik3;
		}

		public IAinTelemetry Aik1 {
			get { return _aik1; }
		}

		public IAinTelemetry Aik2 {
			get { return _aik2; }
		}

		public IAinTelemetry Aik3 {
			get { return _aik3; }
		}
	}
}