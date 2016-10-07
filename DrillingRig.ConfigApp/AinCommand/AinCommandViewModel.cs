using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AinTelemetry;

namespace DrillingRig.ConfigApp.AinCommand {
	internal class AinCommandViewModel : ViewModelBase, IAinTelemetriesCycleControl {
		public AinCommandViewModel(AinCommandAndMinimalCommonTelemetryViewModel ainCommandAndMinimalCommonTelemetryViewModel, TelemetryCommonViewModel commonTelemetryVm, AinTelemetryViewModel ainTelemetryVm, IAinTelemetriesCycleControl ainTelemetriesCycleControl) {
			AinCommandAndMinimalCommonTelemetryVm = ainCommandAndMinimalCommonTelemetryViewModel;
			CommonTelemetryVm = commonTelemetryVm;
			AinTelemetryVm = ainTelemetryVm;

			ReadCycleCommand = ainTelemetriesCycleControl.ReadCycleCommand;
			StopReadingCommand = ainTelemetriesCycleControl.StopReadingCommand;

		}
		
		public TelemetryCommonViewModel CommonTelemetryVm { get; }

		public AinTelemetryViewModel AinTelemetryVm { get; }

		public ICommand ReadCycleCommand { get; }

		public ICommand StopReadingCommand { get; }

		public AinCommandAndMinimalCommonTelemetryViewModel AinCommandAndMinimalCommonTelemetryVm { get; }
	}
}
