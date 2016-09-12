using System.Windows;
using AlienJust.Adaptation.WindowsPresentation;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
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
			var mainViewModel = new MainViewModel(new WpfUiNotifier(Dispatcher), new WpfWindowSystem());
			DataContext = mainViewModel;

			var ainCommandAndCommonTelemetryVm = new AinCommandAndCommonTelemetryViewModel(new AinCommandOnlyViewModel(mainViewModel,
				mainViewModel,
				mainViewModel,
				mainViewModel.Logger,
				mainViewModel, 0), new TelemetryCommonViewModel(mainViewModel.Logger), mainViewModel, mainViewModel, mainViewModel, mainViewModel.Logger, mainViewModel);

			var cmdWindowVm = new CommandWindowViewModel(ainCommandAndCommonTelemetryVm);

			var cmdWindow = new CommandWindow {DataContext = cmdWindowVm};
			cmdWindow.Show();

			var chartWindow = new WindowChart{DataContext = new WindowChartViewModel(mainViewModel.ChartControlVm)};
			chartWindow.Show();

			mainViewModel.RegisterAsCyclePart(ainCommandAndCommonTelemetryVm);
		}
	}
}
