using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using AlienJust.Adaptation.WindowsPresentation;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope {
	public partial class OscilloscopeWindow : MetroWindow, IParameterLogger {
		private readonly MainWindow _mainWindow;
		private readonly List<Color> _colors;
		private readonly List<Color> _usedColors;
		//private int _currentColorIndex;

		private Timer _timer;
		private VerticalLineAnnotation _annotation;
		private double _linePosition;
		private readonly double _xmin;
		private readonly double _xmax;

		private double Ymin = -1.0;
		private double Ymax = 1.0;

		private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(10.0);
		private readonly TimeSpan _totalTime = TimeSpan.FromSeconds(20.0);

		private readonly Dictionary<string, Tuple<FastLineRenderableSeries, XyDataSeries<double, double>>> _namedSeries;
		private readonly WpfUiNotifierAsync _uiNotifier;

		
		public OscilloscopeWindow(MainWindow mainWindow, List<Color> colors) {
			_mainWindow = mainWindow;
			_colors = colors;
			_usedColors = new List<Color>();
			//_currentColorIndex = 0;
			InitializeComponent();

			_namedSeries = new Dictionary<string, Tuple<FastLineRenderableSeries, XyDataSeries<double, double>>>();
			_uiNotifier = new WpfUiNotifierAsync(Dispatcher);
			_xmin = -_totalTime.TotalSeconds / 2.0;
			_xmax = _totalTime.TotalSeconds / 2.0;
			_linePosition = _xmin;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			var xAxis = new NumericAxis { AxisTitle = "Развертка, сек" };
			xAxis.VisibleRange = new DoubleRange(_xmin, _xmax); // total range of 20 seconds

			var yAxis = new NumericAxis { AxisTitle = "Значания" };
			yAxis.AutoRange = AutoRange.Always;
			yAxis.VisibleRange = new DoubleRange(Ymin, Ymax); // total range of 20 seconds


			Surface.XAxis = xAxis;
			Surface.YAxis = yAxis;

			_annotation = new VerticalLineAnnotation();
			_annotation.Stroke = new SolidColorBrush(Colors.LawnGreen);
			_annotation.X1 = _linePosition;
			_annotation.X2 = _linePosition;
			_annotation.IsEditable = false;

			Surface.Annotations.Add(_annotation);

			_timer = new Timer(_updateInterval.TotalMilliseconds) {AutoReset = true};
			_timer.Elapsed += TimerOnTick;
			_timer.Start();

			Surface.InvalidateElement();

		}

		private static FastLineRenderableSeries CreateLineSeries(Color color) {
			return new FastLineRenderableSeries() {
				SeriesColor = color,
				StrokeThickness = 2,
				AntiAliasing = true,
				StrokeDashArray = new double[] { 2, 2 },
			};
		}

		private void TimerOnTick(object sender, EventArgs e) {
			// By nesting multiple updates inside a SuspendUpdates using block, you get one redraw at the end (c) Abt
			_linePosition += _updateInterval.TotalSeconds;

			_uiNotifier.Notify(() => {
				using (Surface.SuspendUpdates()) {


					if (_linePosition >= _xmax) {
						_linePosition = _xmin;
						foreach (var namedDataSeries in _namedSeries) {
							namedDataSeries.Value.Item2.Clear();
						}
					}
					_annotation.X1 = _linePosition;
				}
			});
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			_mainWindow.Close();
		}

		public void LogAnalogueParameter(string parameterName, double? value) {
			if (value.HasValue) { 
				_uiNotifier.Notify(() => {
					if (!_namedSeries.ContainsKey(parameterName)) {
						var color = _colors.First(c => _usedColors.All(uc => uc != c));
						_usedColors.Add(color);
						var rs = CreateLineSeries(color);

						var xy = new XyDataSeries<double,double>();
						rs.DataSeries = xy;
						_namedSeries.Add(parameterName, new Tuple<FastLineRenderableSeries, XyDataSeries<double, double>>(rs, xy));
						Surface.RenderableSeries.Add(rs);
					}
					_namedSeries[parameterName].Item2.Append(_linePosition, value.Value);
				});
			}
		}

		public void LogDiscreteParameter(string parameterName, bool? value) {
			if (value.HasValue) {
				_uiNotifier.Notify(() => {
					if (!_namedSeries.ContainsKey(parameterName)) {
						var color = _colors.First(c => _usedColors.All(uc => uc != c));
						_usedColors.Add(color);
						var rs = CreateLineSeries(color);

						var xy = new XyDataSeries<double, double>();
						rs.DataSeries = xy;
						_namedSeries.Add(parameterName, new Tuple<FastLineRenderableSeries, XyDataSeries<double, double>>(rs, xy));
						Surface.RenderableSeries.Add(rs);
					}
					_namedSeries[parameterName].Item2.Append(_linePosition, value.Value ? 1 : 0);
				});
			}
		}

		public void RemoveSeries(string parameterName) {
			if (_namedSeries.ContainsKey(parameterName)) {
				var s = _namedSeries[parameterName];
				_namedSeries.Remove(parameterName);

				Surface.RenderableSeries.Remove(s.Item1);
				_usedColors.Remove(s.Item1.SeriesColor);
				s.Item2.Clear();
				//_currentColorIndex--;
			}
		}
	}
}
