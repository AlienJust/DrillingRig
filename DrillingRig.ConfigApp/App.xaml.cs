using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using AlienJust.Adaptation.WindowsPresentation;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope;

namespace DrillingRig.ConfigApp
{
	public partial class App : Application
	{
		private MainViewModel _mainViewModel;
		private MainWindow _mainWindow;
		private ManualResetEvent _mainWindowCreationCompleteWaiter;

		private void App_OnStartup(object sender, StartupEventArgs e) {
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
			_mainWindowCreationCompleteWaiter = new ManualResetEvent(false);

			Thread newWindowThread = new Thread(() => {
				_mainViewModel = new MainViewModel(new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher), new WpfWindowSystem(), colors);
				_mainWindow = new MainWindow { DataContext = _mainViewModel };
				_mainWindowCreationCompleteWaiter.Set();
				_mainWindow.Show();
				
				System.Windows.Threading.Dispatcher.Run();
			});
			newWindowThread.SetApartmentState(ApartmentState.STA);
			newWindowThread.IsBackground = true;
			newWindowThread.Start();

			_mainWindowCreationCompleteWaiter.WaitOne();

			var cmdWindow = new CommandWindow(_mainWindow) { DataContext = new CommandWindowViewModel(_mainViewModel.AinCommandAndCommonTelemetryVm) };
			cmdWindow.Show();

			var chartWindow = new WindowChart(_mainWindow) { DataContext = new WindowChartViewModel(_mainViewModel.ChartControlVm) };
			chartWindow.Show();

			var oscilloscopeWindow = new OscilloscopeWindow(_mainWindow, colors) { DataContext = new OscilloscopeWindowSciVm() };
			_mainViewModel.ParamLoggerContainer.AddParamLogger(oscilloscopeWindow);

			oscilloscopeWindow.Show();
		}
	}
}
