namespace DrillingRig.Commands.AikTelemetry {
	class AiksTelemetrySimple : IAiksTelemetry {
		private readonly IAikTelemetry _aik1;
		private readonly IAikTelemetry _aik2;
		private readonly IAikTelemetry _aik3;
		public AiksTelemetrySimple(IAikTelemetry aik1, IAikTelemetry aik2, IAikTelemetry aik3) {
			_aik1 = aik1;
			_aik2 = aik2;
			_aik3 = aik3;
		}

		public IAikTelemetry Aik1 {
			get { return _aik1; }
		}

		public IAikTelemetry Aik2 {
			get { return _aik2; }
		}

		public IAikTelemetry Aik3 {
			get { return _aik3; }
		}
	}
}