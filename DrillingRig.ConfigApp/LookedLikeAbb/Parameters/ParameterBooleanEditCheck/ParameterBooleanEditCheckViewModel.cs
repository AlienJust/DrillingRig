using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanEditCheck {
	class ParameterBooleanEditCheckViewModel : ViewModelBase, ICheckableParameter {
		public string Name { get; }
		public string OffText { get; }
		public string OnText { get; }
		private bool? _value;
		private bool _isChecked;
		public ParameterBooleanEditCheckViewModel(string name, string offText,string onText) {
			Name = name;
			OffText = offText;
			OnText = onText;

			_isChecked = false;
		}


		public bool? Value {
			get => _value;
			set {
				if (_value != value) {
					_value = value;
					RaisePropertyChanged(() => Value);
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