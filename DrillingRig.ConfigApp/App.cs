using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using AlienJust.Adaptation.WindowsPresentation;

namespace DrillingRig.ConfigApp {
	public class EntryPoint {
		[STAThread]
		public static void Main(string[] args) {
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


			Thread newWindowThread1 = new Thread(() => {
				var mainViewModel = new MainViewModel(new WpfUiNotifierAsync(Dispatcher.CurrentDispatcher), new WpfWindowSystem(), colors);
				var mainWindow = new MainWindow { DataContext = mainViewModel };
				mainWindow.Show();
				Dispatcher.Run();
			});
			newWindowThread1.SetApartmentState(ApartmentState.STA);
			newWindowThread1.IsBackground = true;
			newWindowThread1.Start();

			/*

			Thread newWindowThread2 = new Thread(() => {
				var cmdWindow = new CommandWindow(mainWindow) { DataContext = new CommandWindowViewModel(mainViewModel.AinCommandAndCommonTelemetryVm) };
				cmdWindow.Show();
				System.Windows.Threading.Dispatcher.Run();
			});
			newWindowThread2.SetApartmentState(ApartmentState.STA);
			newWindowThread2.IsBackground = true;
			newWindowThread2.Start();
			




			Thread newWindowThread3 = new Thread(() => {
				var chartWindow = new WindowChart(mainWindow) { DataContext = new WindowChartViewModel(mainViewModel.ChartControlVm) };
				chartWindow.Show();
				System.Windows.Threading.Dispatcher.Run();
			});
			newWindowThread3.SetApartmentState(ApartmentState.STA);
			newWindowThread3.IsBackground = true;
			newWindowThread3.Start();




			Thread newWindowThread4 = new Thread(() => {
				var oscilloscopeWindow = new OscilloscopeWindow(mainWindow, colors) { DataContext = new OscilloscopeWindowSciVm() };
				mainViewModel.ParamLoggerContainer.AddParamLogger(oscilloscopeWindow);
				oscilloscopeWindow.Show();
				System.Windows.Threading.Dispatcher.Run();
			});
			newWindowThread4.SetApartmentState(ApartmentState.STA);
			newWindowThread4.IsBackground = true;
			newWindowThread4.Start();
			base.OnStartup(e);*/

			Application app = new Application();
			//App app = new App();
			//app.InitializeComponent();
			//other stuff
			app.Run();
			//Cef.Shutdown(); //Exception  :(
		}
	}
}
