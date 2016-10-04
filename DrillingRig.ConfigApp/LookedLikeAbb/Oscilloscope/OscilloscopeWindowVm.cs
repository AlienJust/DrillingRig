using System;
using System.Windows.Threading;
using AlienJust.Support.ModelViewViewModel;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope {
	class OscilloscopeWindowVm : ViewModelBase {
		public OscilloscopeWindowVm() {
			plotModel1 = new PlotModel();
			plotModel1.Title = "Oscilloscope";
			//plotModel1.PlotAreaBorderThickness = new OxyThickness(0, 0, 0, 0);
			//plotModel1.PlotMargins = new OxyThickness(10, 10, 10, 10);
			plotModel1.Background = OxyColors.Black;
			plotModel1.PlotAreaBorderColor = OxyColors.Lime;
			plotModel1.TextColor = OxyColors.Lime;

			var linearAxis1 = new LinearAxis();
			linearAxis1.MajorGridlineStyle = LineStyle.Solid;
			linearAxis1.MajorGridlineColor = OxyColor.FromArgb(40, 0, 255, 0);

			linearAxis1.MinorGridlineStyle = LineStyle.Dot;
			linearAxis1.MinorGridlineColor = OxyColor.FromArgb(20, 0, 255, 0);

			linearAxis1.AxislineStyle = LineStyle.Solid;
			linearAxis1.AxislineColor = OxyColors.Lime;
			linearAxis1.TicklineColor = OxyColors.Lime;

			linearAxis1.Maximum = 50;
			linearAxis1.Minimum = -50;

			linearAxis1.IsZoomEnabled = false;
			linearAxis1.IsPanEnabled = false;

			linearAxis1.PositionAtZeroCrossing = true;
			linearAxis1.TickStyle = TickStyle.Crossing;

			plotModel1.Axes.Add(linearAxis1);



			var linearAxis2 = new LinearAxis();
			linearAxis2.MajorGridlineStyle = LineStyle.Solid;
			linearAxis2.MajorGridlineColor = OxyColor.FromArgb(40, 0, 255, 0);

			linearAxis2.MinorGridlineStyle = LineStyle.Dot;
			linearAxis2.MinorGridlineColor = OxyColor.FromArgb(20, 0, 255, 0);

			linearAxis2.AxislineStyle = LineStyle.Solid;
			linearAxis2.AxislineColor = OxyColors.Lime;
			linearAxis2.TicklineColor = OxyColors.Lime;

			linearAxis2.Maximum = 100;
			linearAxis2.Minimum = -100;
			linearAxis2.AbsoluteMaximum = 100;
			linearAxis2.AbsoluteMinimum = -100;

			linearAxis2.IsZoomEnabled = false;
			linearAxis2.IsPanEnabled = false;

			linearAxis2.Position = AxisPosition.Bottom; // means that it X axis
			linearAxis2.PositionAtZeroCrossing = true;

			linearAxis2.TickStyle = TickStyle.Crossing;

			plotModel1.Axes.Add(linearAxis2);



			var lineAnnotation1 = new LineAnnotation();
			lineAnnotation1.Type = LineAnnotationType.Vertical;
			lineAnnotation1.X = 4;
			plotModel1.Annotations.Add(lineAnnotation1);

			_timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(50)};
			_timer.Tick += (sender, args) => {
				lineAnnotation1.X += 1.0;
				if (lineAnnotation1.X >= linearAxis2.AbsoluteMaximum)
					lineAnnotation1.X = linearAxis2.AbsoluteMinimum;
				plotModel1.InvalidatePlot(false);
			};
			_timer.Start();
		}

		public PlotModel plotModel1 { get; }

		private readonly DispatcherTimer _timer;
	}
}
