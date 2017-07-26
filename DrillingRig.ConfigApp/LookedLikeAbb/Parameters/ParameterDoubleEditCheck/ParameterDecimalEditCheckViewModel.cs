using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleEditCheck
{
	class ParameterDecimalEditCheckViewModel : ViewModelBase, ICheckableParameter {
		public string Name { get; }
		public string Format { get; }
		public decimal MinimumValue { get; }
		public decimal MaximumValue { get; }
		private decimal? _currentValue;
		private bool _isChecked;

		public decimal Increment { get; set; }

		public ParameterDecimalEditCheckViewModel(string name, string format, decimal minimumValue, decimal maximumValue) {
			Name = name;
			Format = format;
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;

			_isChecked = false;
			_currentValue = null;
			Increment = 1.0m;
		}


		public decimal? CurrentValue {
			get => _currentValue;
			set {
				if (_currentValue != value) {
					_currentValue = value;
					RaisePropertyChanged(() => CurrentValue);
				}
			}
		}

		public bool IsChecked {
			get => _isChecked;
			set {
				if (value != _isChecked) {
					_isChecked = value;
					RaisePropertyChanged(() => IsChecked);
				}
			}
		}
	}
}