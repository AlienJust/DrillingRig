using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.AinCommand {
	class CommandWindowViewModel : ViewModelBase {
		private bool _isTopMost;

		public CommandWindowViewModel(AinCommandAndCommonTelemetryViewModel ainCommandOnlyVm) {
			AinCommandViewVm = ainCommandOnlyVm;
			_isTopMost = true;
		}

		public AinCommandAndCommonTelemetryViewModel AinCommandViewVm { get; }

		public bool IsTopMost {
			get { return _isTopMost; }
			set {
				if (_isTopMost != value) {
					_isTopMost = value;
					RaisePropertyChanged(() => IsTopMost);
				}
			}
		}
	}
}
