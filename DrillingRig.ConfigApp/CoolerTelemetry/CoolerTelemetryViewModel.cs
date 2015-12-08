using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.Cooler;

namespace DrillingRig.ConfigApp.CoolerTelemetry {
	internal class CoolerTelemetryViewModel : ViewModelBase {
		private ICoolerTelemetry _telemetry;

		public CoolerTelemetryViewModel()
		{
			_telemetry = null;
		}

		public ushort? Diagnostic
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.Diagnostic;
			}
		}

		public double? CoolingLiquidPressure
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.CoolingLiquidPressure;
			}
		}

		public double? FanSpeed
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.FanSpeed;
			}
		}

		public double? CoolingLiquidTemperature
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.CoolingLiquidTemperature;
			}
		}

		public ushort? Reserve1
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.Reserve1;
			}
		}

		public ushort? Reserve2
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.Reserve2;
			}
		}


		public void UpdateTelemetry(ICoolerTelemetry telemetry) {
			_telemetry = telemetry;

			RaisePropertyChanged(() => Diagnostic);
			RaisePropertyChanged(() => CoolingLiquidPressure);
			RaisePropertyChanged(() => FanSpeed);
			RaisePropertyChanged(() => CoolingLiquidTemperature);
			RaisePropertyChanged(() => Reserve1);
			RaisePropertyChanged(() => Reserve2);
		}
	}
}
