using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.Rectifier;

namespace DrillingRig.ConfigApp.RectifierTelemetry {
	internal class RectifierTelemetryViewModel : ViewModelBase {
		private IRectifierTelemetry _telemetry;

		public RectifierTelemetryViewModel(string rectifierName)
		{
			RectifierName = rectifierName;
			_telemetry = null;
		}

		public short? VoltageInput1 => _telemetry?.VoltageInput1;

		public short? VoltageInput2 => _telemetry?.VoltageInput2;

		public short? VoltageInput3 => _telemetry?.VoltageInput3;

		public short? VoltageOutputDc => _telemetry?.VoltageOutputDc;

		public short? Current1 => _telemetry?.Current1;

		public short? Current2 => _telemetry?.Current2;

		public short? Current3 => _telemetry?.Current3;

		public short? Temperature => _telemetry?.Temperature;

		public string RectifierName { get; }

		public void UpdateTelemetry(IRectifierTelemetry telemetry) {
			_telemetry = telemetry;

			RaisePropertyChanged(() => VoltageInput1);
			RaisePropertyChanged(() => VoltageInput2);
			RaisePropertyChanged(() => VoltageInput3);
			RaisePropertyChanged(() => VoltageOutputDc);
			RaisePropertyChanged(() => Current1);
			RaisePropertyChanged(() => Current2);
			RaisePropertyChanged(() => Current3);
			RaisePropertyChanged(() => Temperature);
		}
	}
}
