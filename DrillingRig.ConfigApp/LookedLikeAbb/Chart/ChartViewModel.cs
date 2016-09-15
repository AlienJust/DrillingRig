using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using AlienJust.Support.ModelViewViewModel;
using RPD.SciChartControl;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	public class ChartViewModel : ViewModelBase, IParameterLogger {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly Dictionary<string, PointsSeriesAndAdditionalData> _logs;
		private readonly Random _colorRandom;
		private readonly List<Color> _predefinedColors;
		private IUpdatable _updatable;

		public ChartViewModel(IUserInterfaceRoot uiRoot) {
			_uiRoot = uiRoot;
			_logs = new Dictionary<string, PointsSeriesAndAdditionalData>();

			_colorRandom = new Random();
			_predefinedColors = new List<Color> { Colors.Red, Colors.Coral, Colors.DeepSkyBlue, Colors.Aquamarine, Colors.DarkTurquoise, Colors.GreenYellow, Colors.HotPink, Colors.Violet};

			AnalogSeries = new ObservableCollection<IChartSeriesViewModel>();
			DiscreteSeries = new ObservableCollection<IChartSeriesViewModel>();

			AnalogSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();
			DiscreteSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();

			AnalogSeries.CollectionChanged += AnalogSeriesOnCollectionChanged;
			DiscreteSeries.CollectionChanged += DiscreteSeriesOnCollectionChanged;
		}

		private void DiscreteSeriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
			var additionalDataToRemove = DiscreteSeriesAdditionalData.Where(adata => DiscreteSeries.All(data => data != adata.ChartSeries)).ToList();
			foreach (var seriesAdditionalData in additionalDataToRemove) {
				DiscreteSeriesAdditionalData.Remove(seriesAdditionalData);
				RemoveSeries(seriesAdditionalData.ChartSeries);
			}
		}

		private void AnalogSeriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
			var additionalDataToRemove = AnalogSeriesAdditionalData.Where(adata => AnalogSeries.All(data => data != adata.ChartSeries)).ToList();
			foreach (var seriesAdditionalData in additionalDataToRemove) {
				AnalogSeriesAdditionalData.Remove(seriesAdditionalData);
				RemoveSeries(seriesAdditionalData.ChartSeries);
			}
		}

		public void LogAnalogueParameter(string parameterName, double? value) {
			if (value.HasValue) {
				if (!_logs.ContainsKey(parameterName)) {
					var dataSeries = new XyDataSeries<DateTime, double> { SeriesName = parameterName };

					var renderSeries = new FastLineRenderableSeries { DataSeries = dataSeries, SeriesColor = _predefinedColors[_colorRandom.Next(_predefinedColors.Count)]};
					var vm = new ChartSeriesViewModel(dataSeries, renderSeries);
					var metadata = new SeriesAdditionalData(vm);

					AnalogSeries.Add(vm);
					AnalogSeriesAdditionalData.Add(metadata);

					//uiRoot.Notifier.Notify(() => AnalogSeries.Add(vm));
					//uiRoot.Notifier.Notify(() => AnalogSeriesAdditionalData.Add(metadata));

					_logs.Add(parameterName, new PointsSeriesAndAdditionalData(vm, metadata, dataSeries, renderSeries));
				}
				//_uiRoot.Notifier.Notify(()=> _logs[parameterName].DataSeries.Append(DateTime.Now, value.Value));
				_logs[parameterName].DataSeries.Append(DateTime.Now, value.Value);
				_updatable?.Update();
			}
		}


		public void LogDiscreteParameter(string parameterName, bool? value) {
			if (value.HasValue) {
				if (!_logs.ContainsKey(parameterName)) {
					var dataSeries = new XyDataSeries<DateTime, double> { SeriesName = parameterName };

					var renderSeries = new FastLineRenderableSeries { DataSeries = dataSeries, SeriesColor = Colors.Red};
					var vm = new ChartSeriesViewModel(dataSeries, renderSeries);
					var metadata = new SeriesAdditionalData(vm);

					DiscreteSeries.Add(vm);
					DiscreteSeriesAdditionalData.Add(metadata);

					_logs.Add(parameterName, new PointsSeriesAndAdditionalData(vm, metadata, dataSeries, renderSeries));
				}
				_logs[parameterName].DataSeries.Append(DateTime.Now, value.Value ? 1.0 : 0.0);
				_updatable?.Update();
			}
		}

		public void RemoveSeries(string parameterName) {
			if (_logs.ContainsKey(parameterName)) {
				RemoveSeries(_logs[parameterName].SeriesVm);
			}
		}

		public void RemoveSeries(IChartSeriesViewModel seriesViewModel) {
			var logsToRemove = _logs.Where(log => log.Value.SeriesVm == seriesViewModel).ToList();
			foreach (var keyValuePair in logsToRemove) {
				_logs.Remove(keyValuePair.Key);
			}
		}

		public ObservableCollection<IChartSeriesViewModel> AnalogSeries { get; set; }
		public ObservableCollection<IChartSeriesViewModel> DiscreteSeries { get; set; }

		public ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData { get; set; }
		public ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData { get; set; }


		public void SetUpdatable(IUpdatable updatable) {
			_updatable = updatable;
		}
	}
}
