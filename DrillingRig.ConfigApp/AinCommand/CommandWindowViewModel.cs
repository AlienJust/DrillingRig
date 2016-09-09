using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.AinCommand {
	class CommandWindowViewModel : ViewModelBase {
		public CommandWindowViewModel(AinCommandOnlyViewModel ainCommandOnlyVm) {
			AinCommandViewVm = ainCommandOnlyVm;
		}

		public AinCommandOnlyViewModel AinCommandViewVm { get; }
	}
}
