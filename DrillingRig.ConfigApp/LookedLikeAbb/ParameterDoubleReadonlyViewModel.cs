using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class ParameterDoubleReadonlyViewModel : ViewModelBase, ICheckableParameter {
		public string Name { get; }
		public string Format { get; }
		private double? _currentValue;
		private bool _isChecked;

		public ParameterDoubleReadonlyViewModel(string name, string format, double? currentValue) {
			Name = name;
			Format = format;

			_isChecked = false;
			_currentValue = currentValue;
		}

		public double? CurrentValue {
			get { return _currentValue; }
			set {
				if (_currentValue != value) {
					_currentValue = value;
					RaisePropertyChanged(() => CurrentValue);
					RaisePropertyChanged(()=>FormattedValue);
				}
			}
		}

		public string FormattedValue => _currentValue?.ToString(Format) ?? "?";

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