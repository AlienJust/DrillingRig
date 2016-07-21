using System;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	public class PointsSeriesAndAdditionalData {
		public PointsSeriesAndAdditionalData(ChartSeriesViewModel seriesViewModel, SeriesAdditionalData additionalData, XyDataSeries<DateTime, double> dataSeries, FastLineRenderableSeries renderSeries) {
			SeriesVm = seriesViewModel;
			Metadata = additionalData;
			DataSeries = dataSeries;
			RenderSeries = renderSeries;
		}

		public ChartSeriesViewModel SeriesVm { get; }
		public SeriesAdditionalData Metadata { get; }
		public XyDataSeries<DateTime, double> DataSeries { get; set; }
		public FastLineRenderableSeries RenderSeries { get; }
	}
}