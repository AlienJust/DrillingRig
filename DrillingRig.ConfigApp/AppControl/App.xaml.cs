using System;
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
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope;

namespace DrillingRig.ConfigApp.AppControl {
	public partial class App : Application {
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

		private IAinSettingsReader _ainSettingsReader;
		private IAinSettingsReadNotify _ainSettingsReadNotify;
		private IAinSettingsWriter _ainSettingsWriter;

		private AutoTimeSetter _autoTimeSetter;
		private AutoSettingsReader _autoSettingsReader;

		private IAinSettingsStorage _ainSettingsStorage;
		private IAinSettingsStorageSettable _ainSettingsStorageSettable;
		private IAinSettingsStorageUpdatedNotify _ainSettingsStorageUpdatedNotify;


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
					new RelayActionLogger(s => { }),
					//new RelayLogger(
					//new ColoredConsoleLogger(ConsoleColor.DarkRed, ConsoleColor.Black),
					//new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.Red, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter>
						{
							new ThreadFormatter(" > ", true, false, false),
							new DateTimeFormatter(" > ")
						})),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.Yellow, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter>
						{
							new ThreadFormatter(" > ", true, false, false),
							new DateTimeFormatter(" > ")
						})),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayActionLogger(s => { }),
					//new RelayLogger(
					//new ColoredConsoleLogger(ConsoleColor.DarkCyan, ConsoleColor.Black),
					//new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.Cyan, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter>
						{
							new ThreadFormatter(" > ", true, false, false),
							new DateTimeFormatter(" > ")
						})),
					//new StackTraceFormatterWithNullSuport(" > ", "[NO STACK INFO]")),
					new StackTraceFormatterNothing()),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.Green, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterNothing()),
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.White, ConsoleColor.Black),
						new ChainedFormatter(new List<ITextFormatter> { new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ") })),
					new StackTraceFormatterNothing()));

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

			_cycleThreadHolder = new CycleThreadHolderThreadSafe();

			var ainSettingsStorage = new AinSettingsStorageThreadSafe();
			_ainSettingsStorage = ainSettingsStorage;
			_ainSettingsStorageSettable = ainSettingsStorage;
			_ainSettingsStorageUpdatedNotify = ainSettingsStorage;

			var ainSettingsReader = new AinSettingsReader(_cmdSenderHost, _targetAddressHost, _commonLogger, _ainSettingsStorageSettable, _debugLogger);
			_ainSettingsReader = ainSettingsReader;
			_ainSettingsReadNotify = ainSettingsReader;

			_ainSettingsWriter = new AinSettingsWriter(_cmdSenderHost, _targetAddressHost, _ainsCounterRaisable, _ainSettingsReader);
			_autoTimeSetter = new AutoTimeSetter(_cmdSenderHost, _notifySendingEnabled, _targetAddressHost, _commonLogger);
			_autoSettingsReader = new AutoSettingsReader(_notifySendingEnabled, _ainsCounterRaisable, _ainSettingsReader, _ainSettingsStorageSettable, _commonLogger);


			// обнуление хранилища настроек при отключении
			_notifySendingEnabled.SendingEnabledChanged += enabled => {
				if (!enabled) {
					for (byte i = 0; i < _ainsCounter.SelectedAinsCount; ++i)
						_ainSettingsStorageSettable.SetSettings(i, null);
				}
			};
			// обнуление хранилища настроек при изменении числа АИНов
			_ainsCounter.AinsCountInSystemHasBeenChanged += count => {
				for (byte i = (byte)count; i < 3; ++i) {
					_ainSettingsStorageSettable.SetSettings(i, null);
				}
			};



			_mainWindowCreationCompleteWaiter = new ManualResetEvent(false);
			var appThreadNotifier = new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher);



			MainWindow mainWindow = null;
			var mainWindowThread = new Thread(() => {

				var mainViewModel = new MainViewModel(
						new SimpleUiRoot(new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher)),
						new WpfWindowSystem(),
						colors,
						_cmdSenderHostSettable,
						_targetAddressHost,
						_debugLogger,
						_loggerRegPoint,
						_notifySendingEnabledRaisable,
						_commonParamLogger,
						_ainsCounterRaisable,
						_cycleThreadHolder,
						_ainSettingsReader,
						_ainSettingsReadNotify,
						_ainSettingsWriter, _ainSettingsStorage, _ainSettingsStorageUpdatedNotify);

				/*var*/
				mainWindow = new MainWindow(appThreadNotifier) { DataContext = mainViewModel };

				_mainWindowCreationCompleteWaiter.Set();
				mainWindow.Show();

				System.Windows.Threading.Dispatcher.Run();
			});
			mainWindowThread.SetApartmentState(ApartmentState.STA);
			mainWindowThread.Priority = ThreadPriority.AboveNormal;
			mainWindowThread.IsBackground = true;
			mainWindowThread.Start();

			_mainWindowCreationCompleteWaiter.WaitOne();



			var cmdWindowThread = new Thread(() => {
				var uiRoot = new SimpleUiRoot(new WpfUiNotifierAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher));

				var ainCommandAndCommonTelemetryVm = new AinCommandAndCommonTelemetryViewModel(
					new AinCommandAndMinimalCommonTelemetryViewModel(_cmdSenderHost, _targetAddressHost, uiRoot, _commonLogger, _notifySendingEnabled, 0, _ainSettingsStorage, _ainSettingsStorageUpdatedNotify),
					new TelemetryCommonViewModel(),
					_cmdSenderHost, _targetAddressHost, uiRoot, _notifySendingEnabled);
				_cycleThreadHolder.RegisterAsCyclePart(ainCommandAndCommonTelemetryVm);

				var cmdWindow = new CommandWindow(mainWindow) { DataContext = new CommandWindowViewModel(ainCommandAndCommonTelemetryVm) };
				cmdWindow.Show();

				System.Windows.Threading.Dispatcher.Run();
			});
			cmdWindowThread.SetApartmentState(ApartmentState.STA);
			cmdWindowThread.IsBackground = true;
			cmdWindowThread.Priority = ThreadPriority.AboveNormal;
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
			rpdWindowThread.Priority = ThreadPriority.AboveNormal;
			rpdWindowThread.Start();



			var sciWindowThread = new Thread(() => {
				var oscilloscopeWindow = new OscilloscopeWindow(mainWindow, colors) { DataContext = new OscilloscopeWindowSciVm() };
				_paramLoggerRegPoint.RegisterLoggegr(oscilloscopeWindow);
				oscilloscopeWindow.Show();
				System.Windows.Threading.Dispatcher.Run();
			});
			sciWindowThread.SetApartmentState(ApartmentState.STA);
			sciWindowThread.IsBackground = true;
			sciWindowThread.Priority = ThreadPriority.BelowNormal;
			sciWindowThread.Start();

		}
	}
}
