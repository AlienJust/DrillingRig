using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.AinCommand {
	class CommandWindowViewModel : ViewModelBase {
		public CommandWindowViewModel(AinCommandAndCommonTelemetryViewModel ainCommandOnlyVm) {
			AinCommandViewVm = ainCommandOnlyVm;
		}

		public AinCommandAndCommonTelemetryViewModel AinCommandViewVm { get; }
	}
}
