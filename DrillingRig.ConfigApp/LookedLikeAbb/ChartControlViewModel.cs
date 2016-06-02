using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using RPD.SciChartControl;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	public class ChartViewModel {
		private const int MaxPoints = 10000;

		public ChartViewModel() {
			AnalogSeries = new ObservableCollection<IChartSeriesViewModel>();
			DiscreteSeries = new ObservableCollection<IChartSeriesViewModel>();

			AnalogSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();
			DiscreteSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();
		}

		#region View Model Public Properties


		public ObservableCollection<IChartSeriesViewModel> AnalogSeries { get; set; }
		public ObservableCollection<IChartSeriesViewModel> DiscreteSeries { get; set; }

		public ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData { get; set; }
		public ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData { get; set; }

		#endregion


		public static ChartSeriesViewModel GenerateExampleSeries(DateTime startDateTime, double koeff, string seriesName) {
			var dataSeries = new XyDataSeries<DateTime, double> { SeriesName = seriesName };

			var dt2 = startDateTime;
			for (int i = 0; i < MaxPoints; i++) {
				dataSeries.Append(dt2, Math.Sin(2 * Math.PI * i / 1000 + koeff));
				dt2 += TimeSpan.FromMilliseconds(20);
			}

			var series = new FastLineRenderableSeries { DataSeries = dataSeries };
			series.SeriesColor = Colors.Green;

			return new ChartSeriesViewModel(dataSeries, series);
		}

		public static ChartSeriesViewModel GenerateExampleDiscreteSeries(DateTime startDateTime, double koeff, string seriesName) {
			var dataSeries = new XyDataSeries<DateTime, double> { SeriesName = seriesName };
			var dt2 = startDateTime;
			var r = new Random();

			for (int i = 0; i < MaxPoints; i++) {
				var val = Math.Sin(2 * Math.PI * i / 1000 + koeff);
				dataSeries.Append(dt2, Math.Round(val > 0 ? val : 0));
				dt2 += TimeSpan.FromMilliseconds(20);
			}

			var series = new FastLineRenderableSeries { DataSeries = dataSeries };
			return new ChartSeriesViewModel(dataSeries, series);
		}
	}
}
