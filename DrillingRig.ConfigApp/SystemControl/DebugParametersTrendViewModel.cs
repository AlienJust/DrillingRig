using System;
using System.Collections.Generic;
using AlienJust.Support.ModelViewViewModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DrillingRig.ConfigApp.SystemControl {
	class DebugParametersTrendViewModel : ViewModelBase, INamedTrendsControl {
		private IList<byte> _debugBytes;
		private readonly LineSeries _points1;
		private readonly LineSeries _points2;
		private readonly LineSeries _points3;
		private readonly LineSeries _points4;

		private bool _addPoints1AsSigned;
		private bool _addPoints2AsSigned;
		private bool _addPoints3AsSigned;
		private bool _addPoints4AsSigned;

		private readonly PlotModel _plotVm;
		private readonly PlotController _plotCr;

		public DebugParametersTrendViewModel() {
			_plotVm = new PlotModel { IsLegendVisible = false };
			_plotVm.Axes.Add(new DateTimeAxis());
			_plotVm.Axes.Add(new LinearAxis());
			//_plotVm.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

			_plotCr = new PlotController();
			_plotCr.UnbindAll();
			_plotCr.BindMouseDown(OxyMouseButton.Left, PlotCommands.Track);
			_plotCr.Bind(new OxyMouseDownGesture(OxyMouseButton.Right), PlotCommands.PanAt);
			_plotCr.Bind(new OxyMouseDownGesture(OxyMouseButton.Left), PlotCommands.ZoomRectangle);
			_plotCr.Bind(new OxyMouseEnterGesture(OxyModifierKeys.None), PlotCommands.HoverPointsOnlyTrack);
			_plotCr.BindMouseWheel(PlotCommands.ZoomWheel);
			_plotCr.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.None, 2, PlotCommands.ResetAt);

			_points1 = new LineSeries { Color = OxyColor.FromRgb(255, 0, 0) };
			_points2 = new LineSeries { Color = OxyColor.FromRgb(0, 128, 0) };
			_points3 = new LineSeries { Color = OxyColor.FromRgb(0, 0, 128) };
			_points4 = new LineSeries { Color = OxyColor.FromRgb(128, 0, 128) };

			_plotVm.Series.Add(_points1);
			_plotVm.Series.Add(_points2);
			_plotVm.Series.Add(_points3);
			_plotVm.Series.Add(_points4);

			TrendControlVm1 = new TrendControlViewModel("Параметр 1", this);
			TrendControlVm2 = new TrendControlViewModel("Параметр 2", this);
			TrendControlVm3 = new TrendControlViewModel("Параметр 3", this);
			TrendControlVm4 = new TrendControlViewModel("Параметр 4", this);

			CommandPanLeftFast = new RelayCommand(() => {
				_plotVm.PanAllAxes(_plotVm.PlotArea.Width / 4.0, 0);
				_plotVm.InvalidatePlot(false);
			});

			CommandPanLeft = new RelayCommand(() => {
				_plotVm.PanAllAxes(_plotVm.PlotArea.Width / 20.0, 0);
				_plotVm.InvalidatePlot(false);
			});

			CommandPanRight = new RelayCommand(() => {
				_plotVm.PanAllAxes(_plotVm.PlotArea.Width / -20.0, 0);
				_plotVm.InvalidatePlot(false);
			});

			CommandPanRightFast = new RelayCommand(() => {
				_plotVm.PanAllAxes(_plotVm.PlotArea.Width / -4.0, 0);
				_plotVm.InvalidatePlot(false);
			});

			CommandZoomOut = new RelayCommand(() => {
				_plotVm.ZoomAllAxes(0.8);
				_plotVm.InvalidatePlot(false);
			});

			CommandZoomIn = new RelayCommand(() => {
				_plotVm.ZoomAllAxes(1.25);
				_plotVm.InvalidatePlot(false);
			});

			CommandZoomAll = new RelayCommand(() => {
				_plotVm.ResetAllAxes();
				_plotVm.InvalidatePlot(false);
			});



			CommandPanUpFast = new RelayCommand(() => {
				_plotVm.PanAllAxes(0, _plotVm.PlotArea.Height / 4.0);
				_plotVm.InvalidatePlot(false);
			});

			CommandPanUp = new RelayCommand(() => {
				_plotVm.PanAllAxes(0, _plotVm.PlotArea.Height / 20.0);
				_plotVm.InvalidatePlot(false);
			});

			CommandPanDown = new RelayCommand(() => {
				_plotVm.PanAllAxes(0, _plotVm.PlotArea.Height / -20.0);
				_plotVm.InvalidatePlot(false);
			});

			CommandPanDownFast = new RelayCommand(() => {
				_plotVm.PanAllAxes(0, _plotVm.PlotArea.Height / -4.0);
				_plotVm.InvalidatePlot(false);
			});
		}

		public void ShowBytes(IList<byte> bytes) {
			_debugBytes = bytes;
			// TODO: rework for big endian and little endian archs

			double value1 = _addPoints1AsSigned ? ConvertHelp.ToInt16(_debugBytes[0], _debugBytes[1]) * 1.0 : ConvertHelp.ToUInt16(_debugBytes[0], _debugBytes[1]) * 1.0;
			double value2 = _addPoints2AsSigned ? ConvertHelp.ToInt16(_debugBytes[2], _debugBytes[3]) * 1.0 : ConvertHelp.ToUInt16(_debugBytes[2], _debugBytes[3]) * 1.0;
			double value3 = _addPoints3AsSigned ? ConvertHelp.ToInt16(_debugBytes[4], _debugBytes[5]) * 1.0 : ConvertHelp.ToUInt16(_debugBytes[4], _debugBytes[5]) * 1.0;
			double value4 = _addPoints4AsSigned ? ConvertHelp.ToInt16(_debugBytes[6], _debugBytes[7]) * 1.0 : ConvertHelp.ToUInt16(_debugBytes[6], _debugBytes[7]) * 1.0;

			_points1.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value1));
			_points2.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value2));
			_points3.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value3));
			_points4.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value4));

			_plotVm.InvalidatePlot(true);
		}


		public PlotModel PlotVm => _plotVm;

		public PlotController PlotCr => _plotCr;

		public void ClearTrendData(string name) {
			if (name == null) throw new ArgumentNullException(nameof(name));
			switch (name) {
				case "Параметр 1":
					_points1.Points.Clear();
					_plotVm.InvalidatePlot(true);
					break;
				case "Параметр 2":
					_points2.Points.Clear();
					_plotVm.InvalidatePlot(true);
					break;
				case "Параметр 3":
					_points3.Points.Clear();
					_plotVm.InvalidatePlot(true);
					break;
				case "Параметр 4":
					_points4.Points.Clear();
					_plotVm.InvalidatePlot(true);
					break;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public bool GetTrendVisibility(string name) {
			if (name == null) throw new ArgumentNullException(nameof(name));
			switch (name) {
				case "Параметр 1":
					return _points1.IsVisible;
				case "Параметр 2":
					return _points2.IsVisible;
				case "Параметр 3":
					return _points3.IsVisible;
				case "Параметр 4":
					return _points4.IsVisible;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public void SetTrendVisibility(string name, bool value) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name) {
				case "Параметр 1":
					_points1.IsVisible = value;
					break;
				case "Параметр 2":
					_points2.IsVisible = value;
					break;
				case "Параметр 3":
					_points3.IsVisible = value;
					break;
				case "Параметр 4":
					_points4.IsVisible = value;
					break;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public bool GetSignedFlag(string name) {
			if (name == null) throw new ArgumentNullException(nameof(name));
			switch (name) {
				case "Параметр 1":
					return _addPoints1AsSigned;
				case "Параметр 2":
					return _addPoints2AsSigned;
				case "Параметр 3":
					return _addPoints3AsSigned;
				case "Параметр 4":
					return _addPoints4AsSigned;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public void SetSignedFlag(string name, bool isSigned) {
			if (name == null) throw new ArgumentNullException(nameof(name));
			switch (name) {
				case "Параметр 1":
					_addPoints1AsSigned = isSigned;
					break;
				case "Параметр 2":
					_addPoints2AsSigned = isSigned;
					break;
				case "Параметр 3":
					_addPoints3AsSigned = isSigned;
					break;
				case "Параметр 4":
					_addPoints4AsSigned = isSigned;
					break;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public TrendControlViewModel TrendControlVm1 { get; }

		public TrendControlViewModel TrendControlVm2 { get; }

		public TrendControlViewModel TrendControlVm3 { get; }

		public TrendControlViewModel TrendControlVm4 { get; }

		public RelayCommand CommandPanLeft { get; }

		public RelayCommand CommandPanRight { get; }

		public RelayCommand CommandZoomOut { get; }

		public RelayCommand CommandZoomIn { get; }

		public RelayCommand CommandPanLeftFast { get; }

		public RelayCommand CommandPanRightFast { get; }

		public RelayCommand CommandZoomAll { get; }

		public RelayCommand CommandPanUpFast { get; }

		public RelayCommand CommandPanUp { get; }

		public RelayCommand CommandPanDown { get; }

		public RelayCommand CommandPanDownFast { get; }
	}
}
