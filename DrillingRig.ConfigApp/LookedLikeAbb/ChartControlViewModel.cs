using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Numerics;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class ChartControlViewModel : ViewModelBase {
		private bool _avoidCyclic;

		public ChartControlViewModel() {
			var startDateTime = DateTime.Now;


			var data = new XyDataSeries<DateTime, double> { SeriesName = "bobo1" };
			var dt = startDateTime;
			for (int i = 0; i < 10000; i++) {
				data.Append(dt, Math.Sin(2 * Math.PI * i / 1000));
				dt += TimeSpan.FromMilliseconds(20);
			}
			var ser = new FastLineRenderableSeries();
			ser.DataSeries = data;
			ChartSeries.Add(new ChartSeriesViewModel(data, ser));


			var data2 = new XyDataSeries<DateTime, double> { SeriesName = "bobo2" };
			var dt2 = startDateTime;
			for (int i = 0; i < 10000; i++) {
				data2.Append(dt2, -Math.Sin(2 * Math.PI * i / 1000));
				dt2 += TimeSpan.FromMilliseconds(20);
			}

			var ser2 = new FastLineRenderableSeries();
			ser2.DataSeries = data2;
			ChartSeries.Add(new ChartSeriesViewModel(data2, ser2));

		}

		#region Private Properties Fields

		private readonly ObservableCollection<IChartSeriesViewModel> _chartSeries = new ObservableCollection<IChartSeriesViewModel>();

		private bool _isAnnotationsVisible = true;
		private bool _isPanEnabled = true;
		private bool _isZoomEnabled = false;
		private bool _isToZoomXAxisOnly = false;
		private bool _isAntialiasingEnabled = true;
		private int _selectedStrokeThickness = 1;
		private ResamplingMode _selectedResamplingMode = ResamplingMode.MinMax;
		private AxisDragModes _axisDragMode = AxisDragModes.Pan;

		#endregion


		#region Public Properties

		public ObservableCollection<IChartSeriesViewModel> ChartSeries {
			get {
				return _chartSeries;
			}
		}

		public bool IsAnnotationsVisible {
			get { return _isAnnotationsVisible; }
			set
			{
				if (value != _isAnnotationsVisible) {
					_isAnnotationsVisible = value;
					RaisePropertyChanged(() => IsAnnotationsVisible);
				}
			}
		}

		public bool IsPanEnabled {
			get { return _isPanEnabled; }
			set {
				if (_isPanEnabled != value) {
					_isPanEnabled = value;
					RaisePropertyChanged(()=>IsPanEnabled);
				}
				AviodCyclic(() => IsZoomEnabled = !IsPanEnabled);
			}
		}

		public bool IsZoomEnabled {
			get { return _isZoomEnabled; }
			set {
				if (_isZoomEnabled != value) {
					_isZoomEnabled = value;
					RaisePropertyChanged(()=>IsZoomEnabled);
				}
				AviodCyclic(() => IsPanEnabled = !IsZoomEnabled);
			}
		}

		public bool IsToZoomXAxisOnly {
			get { return _isToZoomXAxisOnly; }
			set
			{
				if (_isToZoomXAxisOnly != value) {
					_isToZoomXAxisOnly = value;
					RaisePropertyChanged(()=>IsToZoomXAxisOnly);
				}
			}
		}

		public bool IsAntialiasingEnabled {
			get { return _isAntialiasingEnabled; }
			set {
				if (_isAntialiasingEnabled != value) {
					_isAntialiasingEnabled = value;
					RaisePropertyChanged(()=>IsAntialiasingEnabled);
				}
				foreach (var chartSeriesViewModel in ChartSeries) {
					chartSeriesViewModel.RenderSeries.AntiAliasing = value;
				}
			}
		}

		public int SelectedStrokeThickness {
			get { return _selectedStrokeThickness; }
			set {
				if (_selectedStrokeThickness != value) {
					_selectedStrokeThickness = value;
					RaisePropertyChanged(()=>SelectedStrokeThickness);
				}

				foreach (var chartSeriesViewModel in ChartSeries) {
					chartSeriesViewModel.RenderSeries.StrokeThickness = value;
				}
			}
		}

		public ResamplingMode SelectedResamplingMode {
			get { return _selectedResamplingMode; }
			set {
				if (_selectedResamplingMode != value) {
					_selectedResamplingMode = value;
					RaisePropertyChanged(()=>_selectedResamplingMode);
				}

				foreach (var chartSeriesViewModel in ChartSeries) {
					chartSeriesViewModel.RenderSeries.ResamplingMode = value;
				}
			}
		}

		public AxisDragModes AxisDragMode {
			get { return _axisDragMode; }
			set
			{
				if (_axisDragMode != value) {
					_axisDragMode = value;
					RaisePropertyChanged(()=>AxisDragMode);
				}
			}
		}

		public IEnumerable<string> AllThemes { get { return ThemeManager.AllThemes; } }

		public IEnumerable<int> StrokeThicknesses { get { return new[] { 1, 2, 3, 4, 5 }; } }

		#endregion

		#region Private Methods 

		void AviodCyclic(Action action) {
			if (_avoidCyclic)
				return;

			_avoidCyclic = true;

			action.Invoke();

			_avoidCyclic = false;
		}

		#endregion // Private Methods
	}
}
