using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using AlienJust.Adaptation.ConsoleLogger;
using AlienJust.Adaptation.WindowsPresentation;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Text;
using AlienJust.Support.Text.Contracts;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope;

namespace DrillingRig.ConfigApp.AppControl
{
	public partial class App : Application
	{
		//private MainViewModel _mainViewModel;
		//private MainWindow _mainWindow;
		private ManualResetEvent _mainWindowCreationCompleteWaiter;
		private CommandSenderHostThreadSafe _cmdSenderHost;
		private TargetAddressHostThreadSafe _targetAddressHost;
		private RelayMultiLoggerWithStackTraceSimple _debugLogger;
		private LoggerRegistrationPointThreadSafe _logger;
		private NotifySendingEnabledThreadSafe _notifySendingEnabled;
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
				Colors.MediumTurquoise };

			_debugLogger = new RelayMultiLoggerWithStackTraceSimple(
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.DarkRed, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.Red, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.Yellow, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.DarkCyan, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.Cyan, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.Green, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.White, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")));

			_logger = new LoggerRegistrationPointThreadSafe();


			_cmdSenderHost = new CommandSenderHostThreadSafe();
			_targetAddressHost = new TargetAddressHostThreadSafe(1);
			_notifySendingEnabled = new NotifySendingEnabledThreadSafe(false);

			_mainWindowCreationCompleteWaiter = new ManualResetEvent(false);

			MainWindow mainWindow = null;
			Thread mainWindowThread = new Thread(() => {
				var mainViewModel = new MainViewModel(new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher), new WpfWindowSystem(), colors, _cmdSenderHost, _targetAddressHost, _debugLogger, _logger, _notifySendingEnabled);
				/*var*/ mainWindow = new MainWindow { DataContext = mainViewModel };

				_mainWindowCreationCompleteWaiter.Set();
				mainWindow.Show();
				
				System.Windows.Threading.Dispatcher.Run();
			});
			mainWindowThread.SetApartmentState(ApartmentState.STA);
			mainWindowThread.IsBackground = true;
			mainWindowThread.Start();

			_mainWindowCreationCompleteWaiter.WaitOne();

			Thread cndWindowThread = new Thread(() => {
				var uiRoot = new SimpleUiRoot(new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher));
				var cmdWindow = new CommandWindow(mainWindow) {
					DataContext = new CommandWindowViewModel(/*_mainViewModel.AinCommandAndCommonTelemetryVm*/
						new AinCommandAndCommonTelemetryViewModel(
							new AinCommandOnlyViewModel(_cmdSenderHost, _targetAddressHost, uiRoot, _logger, _notifySendingEnabled, 0),
							new TelemetryCommonViewModel(_logger, _debugLogger),
							_cmdSenderHost, _targetAddressHost, uiRoot,_logger, _debugLogger, _notifySendingEnabled)
						)
				};
				cmdWindow.Show();

				System.Windows.Threading.Dispatcher.Run();
			});
			cndWindowThread.SetApartmentState(ApartmentState.STA);
			cndWindowThread.IsBackground = true;
			cndWindowThread.Start();

			Thread rpdWindowThread = new Thread(() => {
				var chartWindow = new WindowChart(mainWindow) { DataContext = new WindowChartViewModel(_mainViewModel.ChartControlVm) };
				chartWindow.Show();

				System.Windows.Threading.Dispatcher.Run();
			});
			rpdWindowThread.SetApartmentState(ApartmentState.STA);
			rpdWindowThread.IsBackground = true;
			rpdWindowThread.Start();

			Thread sciWindowThread = new Thread(() => {
				var oscilloscopeWindow = new OscilloscopeWindow(mainWindow, colors) { DataContext = new OscilloscopeWindowSciVm() };
				_mainViewModel.ParamLoggerContainer.AddParamLogger(oscilloscopeWindow);
				oscilloscopeWindow.Show();

				System.Windows.Threading.Dispatcher.Run();
			});
			sciWindowThread.SetApartmentState(ApartmentState.STA);
			sciWindowThread.IsBackground = true;
			sciWindowThread.Start();
			
		}
	}

	class SimpleUiRoot : IUserInterfaceRoot {
		public SimpleUiRoot(IThreadNotifier notifier) {
			Notifier = notifier;
		}

		public IThreadNotifier Notifier { get; }
	}
}
