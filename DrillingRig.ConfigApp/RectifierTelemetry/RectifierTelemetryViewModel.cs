using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinTelemetry;
using DrillingRig.Commands.Rectifier;

namespace DrillingRig.ConfigApp.RectifierTelemetry {
	internal class RectifierTelemetryViewModel : ViewModelBase {
		private readonly string _rectifierName;
		private IRectifierTelemetry _telemetry;

		public RectifierTelemetryViewModel(string rectifierName)
		{
			_rectifierName = rectifierName;
			_telemetry = null;
		}

		public short? VoltageInput1
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.VoltageInput1;
			}
		}

		public short? VoltageInput2
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.VoltageInput2;
			}
		}

		public short? VoltageInput3
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.VoltageInput3;
			}
		}

		public short? VoltageOutputDc
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.VoltageOutputDc;
			}
		}

		public short? Current1
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.Current1;
			}
		}

		public short? Current2
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.Current2;
			}
		}

		public short? Current3
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.Current3;
			}
		}

		public short? Temperature
		{
			get {
				if (_telemetry == null) return null;
				return _telemetry.Temperature;
			}
		}

		public string RectifierName {
			get { return _rectifierName; }
		}

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
