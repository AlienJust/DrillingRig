using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanReadonly {
	class ParameterBooleanReadonlyViewModel : ViewModelBase, ICheckableParameter {
		public string Name { get; }

		private bool? _currentValue;
		private readonly IParameterLogger _parameterLogger;
		private bool _isChecked;

		public ParameterBooleanReadonlyViewModel(string name, bool? currentValue, IParameterLogger parameterLogger) {
			Name = name;

			_isChecked = false;
			_currentValue = currentValue;
			_parameterLogger = parameterLogger;
		}

		public bool? CurrentValue {
			get { return _currentValue; }
			set {
				if (_currentValue != value) {
					_currentValue = value;
					RaisePropertyChanged(() => CurrentValue);
					RaisePropertyChanged(()=>FormattedValue);
				}
				if (_isChecked) {
					_parameterLogger.LogDiscreteParameter(Name, value);
				}
			}
		}

		public string FormattedValue => _currentValue.HasValue ? _currentValue.Value ? "1" : "0" : "?";

		public bool IsChecked {
			get { return _isChecked; }
			set {
				if (value != _isChecked) {
					_isChecked = value;
					RaisePropertyChanged(() => IsChecked);
				}
			}
		}
	}
}