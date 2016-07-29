using System.Windows.Input;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AvaDock;

namespace DrillingRig.ConfigApp.AinCommand {
	internal class AinCommandViewModel : DockWindowViewModel, IAinTelemetriesCycleControl {
		public AinCommandViewModel(AinCommandOnlyViewModel ainCommandOnlyViewModel, TelemetryCommonViewModel commonTelemetryVm, AinTelemetryViewModel ainTelemetryVm, IAinTelemetriesCycleControl ainTelemetriesCycleControl) {
			AinCommandOnlyVm = ainCommandOnlyViewModel;
			CommonTelemetryVm = commonTelemetryVm;
			AinTelemetryVm = ainTelemetryVm;

			ReadCycleCommand = ainTelemetriesCycleControl.ReadCycleCommand;
			StopReadingCommand = ainTelemetriesCycleControl.StopReadingCommand;

		}
		
		public TelemetryCommonViewModel CommonTelemetryVm { get; }

		public AinTelemetryViewModel AinTelemetryVm { get; }

		public ICommand ReadCycleCommand { get; }

		public ICommand StopReadingCommand { get; }

		public AinCommandOnlyViewModel AinCommandOnlyVm { get; }
	}
}
