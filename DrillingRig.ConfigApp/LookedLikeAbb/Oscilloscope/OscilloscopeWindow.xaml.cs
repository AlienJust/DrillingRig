using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using AlienJust.Adaptation.WindowsPresentation;
using AlienJust.Support.Concurrent.Contracts;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope {
	public partial class OscilloscopeWindow : MetroWindow, IParameterLogger, IUpdatable {
		private readonly List<Color> _colors;
		private readonly List<Color> _usedColors;

		private Timer _timer;
		private VerticalLineAnnotation _annotation;
		private double _linePosition;
		private readonly double _xmin;
		private readonly double _xmax;

		private double Ymin = -1.0;
		private double Ymax = 1.0;

		private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(30); //33.33fps
		private readonly TimeSpan _totalTime = TimeSpan.FromSeconds(20.0);

		private readonly Dictionary<string, Tuple<FastLineRenderableSeries, XyDataSeries<double, double>>> _namedSeries;
		private readonly IThreadNotifier _uiNotifier;

		private bool _isPaused;
		private readonly object _isPausedSyncObj;

		public OscilloscopeWindow(List<Color> colors) {
			_colors = colors;
			_usedColors = new List<Color>();

			InitializeComponent();

			_namedSeries = new Dictionary<string, Tuple<FastLineRenderableSeries, XyDataSeries<double, double>>>();
			_uiNotifier = new WpfUiNotifierAsyncWithPriority(Dispatcher, DispatcherPriority.ContextIdle);
			_xmin = -_totalTime.TotalSeconds / 2.0;
			_xmax = _totalTime.TotalSeconds / 2.0;
			_linePosition = _xmin;

			_isPausedSyncObj = new object();
			_isPaused = false;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			CheckBoxAutoScale.IsChecked = true;

			var xAxis = new NumericAxis {
				AxisTitle = "Развертка, сек",
				VisibleRange = new DoubleRange(_xmin, _xmax)
			};
			// total range of 20 seconds

			var yAxis = new NumericAxis {
				AxisTitle = "Значания",
				AutoRange = AutoRange.Always,
				VisibleRange = new DoubleRange(Ymin, Ymax)
			};
			// total range of 20 seconds


			Surface.XAxis = xAxis;
			Surface.YAxis = yAxis;

			_annotation = new VerticalLineAnnotation {
				Stroke = new SolidColorBrush(Colors.LawnGreen),
				StrokeThickness = 1,
				X1 = _linePosition,
				X2 = _linePosition,
				IsEditable = false
			};

			Surface.Annotations.Add(_annotation);

			_timer = new Timer(_updateInterval.TotalMilliseconds) { AutoReset = true };
			_timer.Elapsed += TimerOnTick;
			_timer.Start();

			Surface.InvalidateElement();
			Surface.SetMouseCursor(Cursors.Cross);
			Surface.ChartModifier = new PanModifierOnResume();
		}

		private static FastLineRenderableSeries CreateLineSeries(Color color) {
			return new FastLineRenderableSeries {
				SeriesColor = color,
				StrokeThickness = 1,
				AntiAliasing = false
			};
		}

		private void TimerOnTick(object sender, EventArgs e) {
			// By nesting multiple updates inside a SuspendUpdates using block, you get one redraw at the end (c) Abt
			if (!IsPaused) {
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
		}

		public void LogAnalogueParameter(string parameterName, double? value) {
			if (!IsPaused) {
				if (value.HasValue) {
					_uiNotifier.Notify(() => {
						if (!_namedSeries.ContainsKey(parameterName)) {
							var color = _colors.First(c => _usedColors.All(uc => uc != c));
							_usedColors.Add(color);
							var rs = CreateLineSeries(color);

							var xy = new XyDataSeries<double, double>();
							rs.DataSeries = xy;
							_namedSeries.Add(parameterName,
									new Tuple<FastLineRenderableSeries, XyDataSeries<double, double>>(rs, xy));
							Surface.RenderableSeries.Add(rs);
						}
						_namedSeries[parameterName].Item2.Append(_linePosition, value.Value);
						Update();
					});
				}
			}
		}

		public void LogDiscreteParameter(string parameterName, bool? value) {
			if (!IsPaused) {
				if (value.HasValue) {
					_uiNotifier.Notify(() => {
						if (!_namedSeries.ContainsKey(parameterName)) {
							var color = _colors.First(c => _usedColors.All(uc => uc != c));
							_usedColors.Add(color);
							var rs = CreateLineSeries(color);

							var xy = new XyDataSeries<double, double>();
							rs.DataSeries = xy;
							_namedSeries.Add(parameterName,
									new Tuple<FastLineRenderableSeries, XyDataSeries<double, double>>(rs, xy));
							Surface.RenderableSeries.Add(rs);
						}
						_namedSeries[parameterName].Item2.Append(_linePosition, value.Value ? 1 : 0);
						Update();
					});
				}
			}
		}

		public void RemoveSeries(string parameterName) {
			if (_namedSeries.ContainsKey(parameterName)) {
				var s = _namedSeries[parameterName];
				_namedSeries.Remove(parameterName);

				Surface.RenderableSeries.Remove(s.Item1);
				_usedColors.Remove(s.Item1.SeriesColor);
				s.Item2.Clear();
			}
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e) {
			if (Surface?.YAxis != null)
				Surface.YAxis.AutoRange = AutoRange.Always;
		}

		private void CheckBoxAutoScale_Unchecked(object sender, RoutedEventArgs e) {
			if (Surface?.YAxis != null)
				Surface.YAxis.AutoRange = AutoRange.Never;
		}


		public void Update() {
			_uiNotifier.Notify(() => {
				if (CheckBoxAutoScale.IsChecked.HasValue && CheckBoxAutoScale.IsChecked.Value) {
					Surface?.ZoomExtentsY();
				}
			});
		}

		private void ButtonZoomOut_Click(object sender, RoutedEventArgs e) {
			if (!CheckBoxAutoScale.IsChecked.HasValue)
				return;
			if (!CheckBoxAutoScale.IsChecked.Value) {
				Surface?.YAxis.ZoomBy(0.2, 0.2);
			}
		}

		private void ButtonZoomIn_Click(object sender, RoutedEventArgs e) {
			if (!CheckBoxAutoScale.IsChecked.HasValue)
				return;
			if (!CheckBoxAutoScale.IsChecked.Value) {
				Surface?.YAxis.ZoomBy(-0.2, -0.2);
			}
		}

		private void ButtonPause_Click(object sender, RoutedEventArgs e) {
			IsPaused = true;
			Surface.ChartModifier = new PanModifierOnPause();
			// TODO: should I extract modifier as private class member?
		}

		private void ButtonResume_Click(object sender, RoutedEventArgs e) {
			Surface.ChartModifier = new PanModifierOnResume();
			// TODO: should I extract modifier as private class member?
			Surface.XAxes.Default.VisibleRange = new DoubleRange(_xmin, _xmax);
			IsPaused = false;
		}

		private bool IsPaused {
			get {
				lock (_isPausedSyncObj)
					return _isPaused;
			}
			set {
				lock (_isPausedSyncObj)
					_isPaused = value;
			}
		}
	}
}
