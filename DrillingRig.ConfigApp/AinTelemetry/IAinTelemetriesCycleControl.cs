using System.Windows.Input;

namespace DrillingRig.ConfigApp.AinTelemetry {
	internal interface IAinTelemetriesCycleControl {
		ICommand ReadCycleCommand { get; }
		ICommand StopReadingCommand { get; }
	}
}