using System;
using System.Collections.Generic;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	internal class ParameterLogger : IParameterLogger {
		private readonly Random _rand = new Random();
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly Dictionary<string, PointsSeriesAndAdditionalData> _logs;
		private readonly ChartViewModel _chart;

		public ParameterLogger(IUserInterfaceRoot uiRoot, ChartViewModel charVm) {
			_uiRoot = uiRoot;
			_logs = new Dictionary<string, PointsSeriesAndAdditionalData>();
			_chart = charVm;
		}

		public void LogParameter(string parameterName, double? value) {
			_uiRoot.Notifier.Notify(() => {
				if (value.HasValue) {
					if (!_logs.ContainsKey(parameterName)) {
						var dataSeries = new XyDataSeries<DateTime, double> {SeriesName = parameterName};
						dataSeries.Append(DateTime.Today, 0.0);
						dataSeries.Append(DateTime.Now, value.Value);

						var renderSeries = new FastLineRenderableSeries {DataSeries = dataSeries, SeriesColor = Colors.Green};
						var vm = new ChartSeriesViewModel(dataSeries, renderSeries);

						var metadata = new SeriesAdditionalData(vm);

						//_chart.AddDataCommandExecute(0.3, parameterName);
						_chart.AnalogSeries.Add(vm);
						_chart.AnalogSeriesAdditionalData.Add(metadata);
						_logs.Add(parameterName, new PointsSeriesAndAdditionalData(vm, metadata, dataSeries, renderSeries));
					}
					//_logs[parameterName].DataSeries.Append(DateTime.Now, value.Value);
				}
			});
		}
	}
}