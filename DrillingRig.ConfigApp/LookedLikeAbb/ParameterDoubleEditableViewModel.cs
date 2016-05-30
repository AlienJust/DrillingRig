using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class ParameterDoubleEditableViewModel : ViewModelBase, ICheckableParameter {
		public string Name { get; }
		public string Format { get; }
		public double MinimumValue { get; }
		public double MaximumValue { get; }
		private double? _currentValue;
		private bool _isChecked;

		public ParameterDoubleEditableViewModel(string name, string format, double minimumValue, double maximumValue, double? currentValue) {
			Name = name;
			Format = format;
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;

			_isChecked = false;
			_currentValue = currentValue;
		}


		public double? CurrentValue
		{
			get { return _currentValue; }
			set
			{
				if (_currentValue != value) {
					_currentValue = value;
					RaisePropertyChanged(() => CurrentValue);
				}
			}
		}

		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
				if (value != _isChecked) {
					_isChecked = value;
					RaisePropertyChanged(()=>IsChecked);
				}
			}
		}
	}
}