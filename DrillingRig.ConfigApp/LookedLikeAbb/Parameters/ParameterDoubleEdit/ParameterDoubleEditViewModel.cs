using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleEdit {
	class ParameterDoubleEditViewModel : ViewModelBase, ICheckableParameter {
		public string Name { get; }
		public string Format { get; }
		public double MinimumValue { get; }
		public double MaximumValue { get; }
		private double? _currentValue;
		private bool _isChecked;

		public double Increment { get; set; }

		public ParameterDoubleEditViewModel(string name, string format, double minimumValue, double maximumValue, double? currentValue) {
			Name = name;
			Format = format;
			MinimumValue = minimumValue;
			MaximumValue = maximumValue;

			_isChecked = false;
			_currentValue = currentValue;
			Increment = 1.0;
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