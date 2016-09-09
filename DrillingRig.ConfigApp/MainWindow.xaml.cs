using System.Windows;
using AlienJust.Adaptation.WindowsPresentation;
using DrillingRig.ConfigApp.AinCommand;
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
			var cmdWindowVm = new CommandWindowViewModel(
				new AinCommandOnlyViewModel(
					mainViewModel, 
					mainViewModel, 
					mainViewModel, 
					mainViewModel.Logger, 
					mainViewModel, 0)); // TODO: get proper logger

			var cmdWindow = new CommandWindow {DataContext = cmdWindowVm};
			cmdWindow.Show();

			var chartWindow = new WindowChart{DataContext = new WindowChartViewModel(mainViewModel.ChartControlVm)};
			chartWindow.Show();
		}
	}
}
