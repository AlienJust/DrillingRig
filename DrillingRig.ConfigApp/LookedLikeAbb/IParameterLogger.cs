using Abt.Controls.SciChart;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	public interface IParameterLogger {
		void LogAnalogueParameter(string parameterName, double? value);
		void LogDiscreteParameter(string parameterName, bool? value);

		void RemoveSeries(IChartSeriesViewModel seriesViewModel);
	}
}