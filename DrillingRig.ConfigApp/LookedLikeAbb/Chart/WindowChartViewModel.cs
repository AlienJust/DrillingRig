using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	class WindowChartViewModel :ViewModelBase {
		public WindowChartViewModel(ChartViewModel chartVm) {
			ChartVm = chartVm;
		}

		public ChartViewModel ChartVm { get; }
	}
}
