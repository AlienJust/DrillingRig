﻿using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
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
			get { return _currentValue; }
			set {
				if (_currentValue != value) {
					_currentValue = value;
					RaisePropertyChanged(() => CurrentValue);
					RaisePropertyChanged(()=>FormattedValue);
				}
				if (_isChecked) {
					_parameterLogger.LogParameter(Name, value);
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