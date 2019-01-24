using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.ParamLogger;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleReadonly {
	class ParameterDoubleReadonlyViewModel : ViewModelBase, ICheckableParameter {
		public string Name { get; }
		public string Format { get; }
		private double? _currentValue;
		private readonly IParameterLogger _parameterLogger;
		private bool _isChecked;

		public ParameterDoubleReadonlyViewModel(string name, string format, double? currentValue, IParameterLogger parameterLogger) {
			Name = name;
			Format = format;

			_isChecked = false;
			_currentValue = currentValue;
			_parameterLogger = parameterLogger;
		}

		public double? CurrentValue {
			get => _currentValue;
			set {
				if (_currentValue != value) {
					_currentValue = value;
					RaisePropertyChanged(() => CurrentValue);
					RaisePropertyChanged(()=>FormattedValue);
				}
				if (_isChecked) {
					_parameterLogger.LogAnalogueParameter(Name, value);
				}
			}
		}

		public string FormattedValue => _currentValue?.ToString(Format) ?? "-";

		public bool IsChecked {
			get => _isChecked;
			set {
				if (value != _isChecked) {
					_isChecked = value;
					RaisePropertyChanged(() => IsChecked);
					if (!_isChecked) _parameterLogger.RemoveSeries(Name);
				}
			}
		}
	}
}