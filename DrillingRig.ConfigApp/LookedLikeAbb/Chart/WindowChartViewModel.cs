using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	class WindowChartViewModel :ViewModelBase {
		public WindowChartViewModel(ChartViewModel chartVm) {
			ChartVm = chartVm;
		}

		public ChartViewModel ChartVm { get; }
	}
}
