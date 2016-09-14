using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abt.Controls.SciChart;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope {
	class OscilloscopeLogger : ViewModelBase, IParameterLogger {
		public void LogAnalogueParameter(string parameterName, double? value) {
			throw new NotImplementedException();
		}

		public void LogDiscreteParameter(string parameterName, bool? value) {
			throw new NotImplementedException(); // cannot register, or can?
		}

		public void RemoveSeries(IChartSeriesViewModel seriesViewModel) {
			throw new NotImplementedException();
		}
	}
}
