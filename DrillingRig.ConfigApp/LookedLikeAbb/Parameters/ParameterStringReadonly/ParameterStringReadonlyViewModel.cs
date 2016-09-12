using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly {
	class ParameterStringReadonlyViewModel : ViewModelBase {
		public string Name { get; }
		private string _currentValue;

		public ParameterStringReadonlyViewModel(string name, string currentValue) {
			Name = name;

			_currentValue = currentValue;
		}

		public string CurrentValue {
			get { return _currentValue; }
			set {
				if (_currentValue != value) {
					_currentValue = value;
					RaisePropertyChanged(() => CurrentValue);
					RaisePropertyChanged(() => FormattedValue);
				}
			}
		}

		public string FormattedValue => _currentValue;
	}
}