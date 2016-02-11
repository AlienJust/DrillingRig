namespace DrillingRig.ConfigApp.AinTelemetry
{
	class AinTelemetryExpandedViewModel {
		private string _ainName;
		private AinTelemetryViewModel _ainTelemetryVm;
		public AinTelemetryExpandedViewModel(string ainName, AinTelemetryViewModel ainTelemetryVm) {
			AinName = ainName;
			AinTelemetryVm = ainTelemetryVm;
		}

		public string AinName {
			get { return _ainName; }
			set { _ainName = value; }
		}

		public AinTelemetryViewModel AinTelemetryVm {
			get { return _ainTelemetryVm; }
			set { _ainTelemetryVm = value; }
		}
	}
}
