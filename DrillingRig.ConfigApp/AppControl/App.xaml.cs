﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using AlienJust.Adaptation.ConsoleLogger;
using AlienJust.Adaptation.WindowsPresentation;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.Text;
using AlienJust.Support.Text.Contracts;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
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

		private ICommandSenderHostSettable _cmdSenderHostSettable;
		private ICommandSenderHost _cmdSenderHost;

		private ITargetAddressHostSettable _targetAddressHostSettable;
		private ITargetAddressHost _targetAddressHost;

		private RelayMultiLoggerWithStackTraceSimple _debugLogger;

		private ILogger _commonLogger;
		private ILoggerRegistrationPoint _loggerRegPoint;

		private INotifySendingEnabledRaisable _notifySendingEnabledRaisable;
		private INotifySendingEnabled _notifySendingEnabled;

		private IParameterLogger _commonParamLogger;
		private IParamLoggerRegistrationPoint _paramLoggerRegPoint;

		private IAinsCounter _ainsCounter;
		private IAinsCounterRaisable _ainsCounterRaisable;

		private ICycleThreadHolder _cycleThreadHolder;

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

			var loggerAndRegPoint = new LoggerRegistrationPointThreadSafe();
			_commonLogger = loggerAndRegPoint;
			_loggerRegPoint = loggerAndRegPoint;

			var paramLoggerAndRegPoint = new ParamLoggerRegistrationPointThreadSafe();
			_commonParamLogger = paramLoggerAndRegPoint;
			_paramLoggerRegPoint = paramLoggerAndRegPoint;
			

			var cmdSenderHost = new CommandSenderHostThreadSafe();
			_cmdSenderHostSettable = cmdSenderHost;
			_cmdSenderHost = cmdSenderHost;

			var targetAddressHost = new TargetAddressHostThreadSafe(1);
			_targetAddressHostSettable = targetAddressHost;
			_targetAddressHost = targetAddressHost;
			
			var notifySendingEnabled = new NotifySendingEnabledThreadSafe(false);
			_notifySendingEnabledRaisable = notifySendingEnabled;
			_notifySendingEnabled = notifySendingEnabled;

			var ainsCounter = new AinsCounterThreadSafe(1);
			_ainsCounterRaisable = ainsCounter;
			_ainsCounter = ainsCounter;

			var cycleThreadHolder = new CycleThreadHolderThreadSafe(_debugLogger);

			_mainWindowCreationCompleteWaiter = new ManualResetEvent(false);
			var appThreadNotifier = new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher);

			MainWindow mainWindow = null;
			var mainWindowThread = new Thread(() => {
				var uiRoot = new SimpleUiRoot(new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher));
				var mainViewModel = new MainViewModel(uiRoot, new WpfWindowSystem(), colors, _cmdSenderHostSettable, _targetAddressHost, _debugLogger, _loggerRegPoint, _notifySendingEnabledRaisable, _commonParamLogger, _ainsCounterRaisable, cycleThreadHolder);
				/*var*/ mainWindow = new MainWindow(appThreadNotifier) { DataContext = mainViewModel };

				_mainWindowCreationCompleteWaiter.Set();
				mainWindow.Show();
				
				System.Windows.Threading.Dispatcher.Run();
			});
			mainWindowThread.SetApartmentState(ApartmentState.STA);
			mainWindowThread.IsBackground = true;
			mainWindowThread.Start();

			_mainWindowCreationCompleteWaiter.WaitOne();

			var cmdWindowThread = new Thread(() => {
				var uiRoot = new SimpleUiRoot(new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher));
				var cmdWindow = new CommandWindow(mainWindow) {
					DataContext = new CommandWindowViewModel(/*_mainViewModel.AinCommandAndCommonTelemetryVm*/
						new AinCommandAndCommonTelemetryViewModel(
							new AinCommandOnlyViewModel(_cmdSenderHost, _targetAddressHost, uiRoot, _commonLogger, _notifySendingEnabled, 0),
							new TelemetryCommonViewModel(_commonLogger, _debugLogger),
							_cmdSenderHost, _targetAddressHost, uiRoot, _commonLogger, _debugLogger, _notifySendingEnabled)
						)
				};
				cmdWindow.Show();

				System.Windows.Threading.Dispatcher.Run();
			});
			cmdWindowThread.SetApartmentState(ApartmentState.STA);
			cmdWindowThread.IsBackground = true;
			cmdWindowThread.Start();


			var rpdWindowThread = new Thread(() => {
				var uiRoot = new SimpleUiRoot(new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher));
				var chartVm = new ChartViewModel(uiRoot, colors);
				_paramLoggerRegPoint.RegisterLoggegr(chartVm);

				var chartWindow = new WindowChart(mainWindow) { DataContext = new WindowChartViewModel(chartVm) };
				chartWindow.Show();

				System.Windows.Threading.Dispatcher.Run();
			});

			rpdWindowThread.SetApartmentState(ApartmentState.STA);
			rpdWindowThread.IsBackground = true;
			rpdWindowThread.Start();

			var sciWindowThread = new Thread(() => {
				var oscilloscopeWindow = new OscilloscopeWindow(mainWindow, colors) { DataContext = new OscilloscopeWindowSciVm() };
				_paramLoggerRegPoint.RegisterLoggegr(oscilloscopeWindow);
				oscilloscopeWindow.Show();
				System.Windows.Threading.Dispatcher.Run();
			});
			sciWindowThread.SetApartmentState(ApartmentState.STA);
			sciWindowThread.IsBackground = true;
			sciWindowThread.Start();
			
		}
	}
}
