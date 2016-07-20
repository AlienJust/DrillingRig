using System;
using System.Collections.Generic;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	internal class ParameterLogger : IParameterLogger {
		private readonly Dictionary<string, PointsSeriesAndAdditionalData> _logs;
		private readonly ChartViewModel _chart;

		public ParameterLogger(ChartViewModel charVm) {
			_logs = new Dictionary<string, PointsSeriesAndAdditionalData>();
			_chart = charVm;
		}

		public void LogParameter(string parameterName, double? value) {
			if (value.HasValue) {
				if (!_logs.ContainsKey(parameterName)) {
					var dataSeries = new XyDataSeries<DateTime, double> { SeriesName = parameterName };

					var renderSeries = new FastLineRenderableSeries { DataSeries = dataSeries/*, SeriesColor = Colors.Green*/};
					var vm = new ChartSeriesViewModel(dataSeries, renderSeries);
					var metadata = new SeriesAdditionalData(vm);

					_chart.AnalogSeries.Add(vm);
					_chart.AnalogSeriesAdditionalData.Add(metadata);

					_logs.Add(parameterName, new PointsSeriesAndAdditionalData(vm, metadata, dataSeries, renderSeries));
				}
				_logs[parameterName].DataSeries.Append(DateTime.Now, value.Value);
			}
		}
	}
}