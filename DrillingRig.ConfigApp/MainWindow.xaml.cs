using System.Windows;
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

			var mainViewModel = new MainViewModel(new WpfUiNotifierAsync(Dispatcher), new WpfWindowSystem());
			DataContext = mainViewModel;

			var cmdWindow = new CommandWindow(this) {DataContext = new CommandWindowViewModel(mainViewModel.AinCommandAndCommonTelemetryVm)};
			cmdWindow.Show();

			var chartWindow = new WindowChart(this) {DataContext = new WindowChartViewModel(mainViewModel.ChartControlVm)};
			chartWindow.Show();

			var oscilloscopeWindow = new OscilloscopeWindow(this) { DataContext = new OscilloscopeWindowSciVm()};
			mainViewModel.ParamLoggerContainer.AddParamLogger(oscilloscopeWindow);

			oscilloscopeWindow.Show();
		}
	}
}
