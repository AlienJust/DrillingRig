using Abt.Controls.SciChart;
using RPD.SciChartControl;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	internal class SeriesAdditionalData : ISeriesAdditionalData {

		public SeriesAdditionalData(IChartSeriesViewModel chartSeriesViewModel) {
			ChartSeries = chartSeriesViewModel;
		}

		public IChartSeriesViewModel ChartSeries { get; set; }

		public PointMetadata GetPointMetadata(int pointIndex) {
			return new PointMetadata() { DataPosition = 1, IsValid = true };
		}
	}
}