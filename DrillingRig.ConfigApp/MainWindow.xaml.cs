using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using AlienJust.Adaptation.WindowsPresentation;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope;
using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			var colors = new List<Color> {
				Colors.LawnGreen,
				Colors.Red,
				Colors.Cyan,
				Colors.Yellow,
				Colors.Coral,
				Colors.LightGreen,
				Colors.HotPink,
				Colors.DeepSkyBlue,
				Colors.Gold,
				Colors.Orange,
				Colors.Violet,
				Colors.White,
				Colors.Fuchsia,
				Colors.LightSkyBlue,
				Colors.LightGray,
				Colors.Khaki,
				Colors.SpringGreen,
				Colors.Tomato,
				Colors.LightCyan,
				Colors.Goldenrod,
				Colors.SlateBlue,
				Colors.Cornsilk,
				Colors.MediumPurple,
				Colors.RoyalBlue,
				Colors.MediumVioletRed,
				Colors.MediumTurquoise,

			};
			var mainViewModel = new MainViewModel(new WpfUiNotifierAsync(Dispatcher), new WpfWindowSystem(), colors);
			DataContext = mainViewModel;

			var cmdWindow = new CommandWindow(this) {DataContext = new CommandWindowViewModel(mainViewModel.AinCommandAndCommonTelemetryVm)};
			cmdWindow.Show();

			var chartWindow = new WindowChart(this) {DataContext = new WindowChartViewModel(mainViewModel.ChartControlVm)};
			chartWindow.Show();

			var oscilloscopeWindow = new OscilloscopeWindow(this, colors) { DataContext = new OscilloscopeWindowSciVm()};
			mainViewModel.ParamLoggerContainer.AddParamLogger(oscilloscopeWindow);

			oscilloscopeWindow.Show();
		}
	}
}
