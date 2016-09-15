using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using AlienJust.Adaptation.ConsoleLogger;
using AlienJust.Adaptation.WindowsPresentation.Converters;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.Text;
using AlienJust.Support.Text.Contracts;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.CommandSenders.Contracts;
using DrillingRig.CommandSenders.SerialPortBased;
using DrillingRig.CommandSenders.TestCommandSender;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.Logs;
using DrillingRig.ConfigApp.LookedLikeAbb;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using DrillingRig.ConfigApp.MnemonicCheme;
using DrillingRig.ConfigApp.NewLook.Archive;
using DrillingRig.ConfigApp.NewLook.OldLook;
using DrillingRig.ConfigApp.NewLook.Settings;
using DrillingRig.ConfigApp.NewLook.Telemetry;

namespace DrillingRig.ConfigApp {
	internal class MainViewModel : ViewModelBase
		, ICommandSenderHost
		, ITargetAddressHost
		, IUserInterfaceRoot
		, INotifySendingEnabled
		, ILinkContol
		, ICycleThreadHolder
		, IAinsCounter /*, IAinsLinkControlViewModel*/ {
		public IThreadNotifier Notifier { get; }

		private const string TestComPortName = "ТЕСТ";

		private List<string> _comPortsAvailable;
		private string _selectedComName;

		private IRrModbusCommandSender _commandSender;
		private ICommandSenderController _commandSenderController;

		private readonly ProgramLogViewModel _programLogVm;


		private readonly RelayCommand _openPortCommand;
		private readonly RelayCommand _closePortCommand;

		private bool _isPortOpened;
		private byte _targetAddress;

		private readonly ILogger _logger;
		private readonly IMultiLoggerWithStackTrace _debugLogger;

		private readonly object _isSendingEnabledSyncObject;
		private bool _isSendingEnabled;


		private readonly object _cyclePartsSync;
		private readonly List<ICyclePart> _cycleParts;
		private readonly SingleThreadedRelayQueueWorker<Action> _backWorker;

		private int _selectedAinsCount;

		private readonly AutoSettingsReader _autoSettingsReader;
		private Colors _ain1StateColor;
		private Colors _ain2StateColor;
		private Colors _ain3StateColor;
		private bool _ain1IsUsed;
		private bool _ain2IsUsed;
		private bool _ain3IsUsed;

		public ChartViewModel ChartControlVm { get; set; }
		public IParameterLogger ExternalParamLogger { get; set; }

		private readonly IParameterLogger _relayParamLogger;
		public IParameterLoggerContainer ParamLoggerContainer { get; }

		public AinCommandAndCommonTelemetryViewModel AinCommandAndCommonTelemetryVm { get; }

		public MainViewModel(IThreadNotifier notifier, IWindowSystem windowSystem) {
			Notifier = notifier;

			_targetAddress = 1;

			_commandSender = null;
			_commandSenderController = null;

			_isPortOpened = false;

			_isSendingEnabled = false;
			_isSendingEnabledSyncObject = new object();
			
			// Лог программы:
			_debugLogger = new RelayMultiLoggerWithStackTraceSimple(
				new RelayLoggerWithStackTrace(
					new RelayLogger(
						new ColoredConsoleLogger(ConsoleColor.DarkRed, ConsoleColor.Black), 
						new ChainedFormatter(new List<ITextFormatter>{new ThreadFormatter(" > ", true, false, false), new DateTimeFormatter(" > ")})), 
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

			_programLogVm = new ProgramLogViewModel(this, _debugLogger);
			_logger = new RelayLogger(_programLogVm, new DateTimeFormatter(" > "));

			// циклический опрос
			_cyclePartsSync = new object();
			_cycleParts = new List<ICyclePart>();
			_backWorker = new SingleThreadedRelayQueueWorker<Action>("CycleBackWorker", a => a(), ThreadPriority.BelowNormal, true, null, _debugLogger.GetLogger(0));

			GetPortsAvailable();

			// Блоки АИН в системе:
			AinsCountInSystem = new List<int> { 1, 2, 3 };
			SelectedAinsCount = AinsCountInSystem.First();
			

			_openPortCommand = new RelayCommand(OpenPort, () => !_isPortOpened);
			_closePortCommand = new RelayCommand(ClosePort, () => _isPortOpened);
			GetPortsAvailableCommand = new RelayCommand(GetPortsAvailable);


			// ABB way:
			ChartControlVm = new ChartViewModel(this);
			var paramLogger = new ParameterLoggerRelay(new List<IParameterLogger> { ChartControlVm});
			_relayParamLogger = paramLogger;
			ParamLoggerContainer = paramLogger;

			// var cycleReader = new CycleReader(this, this, this, _logger, this); // TODO: check if needed

			var ainSettingsReader = new AinSettingsReader(this, this, _logger);
			var ainSettingsWriter = new AinSettingsWriter(this, this, this, ainSettingsReader);
			var ainSettingsReadedWriter = new AinSettingsReaderWriter(ainSettingsReader, ainSettingsWriter);

			_autoSettingsReader = new AutoSettingsReader(this, this, ainSettingsReader, _logger); // TODO: can I convert it to local variable (woudn't it be GCed)?

			AinCommandAndCommonTelemetryVm = new AinCommandAndCommonTelemetryViewModel(
				new AinCommandOnlyViewModel(this, this, this, _logger, this, 0),
				new TelemetryCommonViewModel(_logger, _debugLogger), this, this, this, _logger, _debugLogger, this);
			RegisterAsCyclePart(AinCommandAndCommonTelemetryVm);

			TelemtryVm = new TelemetryViewModel(this, this, this, _logger, this, this, _relayParamLogger);
			SettingsVm = new SettingsViewModel(this, _logger, ainSettingsReadedWriter, ainSettingsReader);
			ArchiveVm = new ArchivesViewModel(new ArchiveViewModel(this, this, this, _logger, this, 0), new ArchiveViewModel(this, this, this, _logger, this, 1));
			MnemonicChemeVm = new MnemonicChemeViewModel(Path.Combine(Environment.CurrentDirectory, "mnemoniccheme.png"));
			OldLookVm = new OldLookViewModel(this, windowSystem, this, this, this, this, _logger, _debugLogger, this, this, _relayParamLogger);
			
			_ain1StateColor = Colors.Gray;
			_ain2StateColor = Colors.Gray;
			_ain3StateColor = Colors.Gray;

			_ain1IsUsed = true;
			_ain2IsUsed = false;
			_ain3IsUsed = false;

			AinsCountInSystemHasBeenChanged += () => {
				switch (SelectedAinsCount) {
					case 1:
						Ain1IsUsed = true;
						Ain2IsUsed = false;
						Ain3IsUsed = false;
						break;
					case 2:
						Ain1IsUsed = true;
						Ain2IsUsed = true;
						Ain3IsUsed = false;
						break;
					case 3:
						Ain1IsUsed = true;
						Ain2IsUsed = true;
						Ain3IsUsed = true;
						break;
					default:
						throw new Exception("Такое число АИН в системе не поддерживается");
				}
			};
			
			AinCommandAndCommonTelemetryVm.AinsLinkInformationHasBeenUpdated += (ain1Error, ain2Error, ain3Error) => {
				Ain1StateColor = ain1Error.HasValue ? ain1Error.Value ? Colors.Red : Colors.YellowGreen : Colors.Gray;
				Ain2StateColor = ain2Error.HasValue ? ain2Error.Value ? Colors.Red : Colors.YellowGreen : Colors.Gray;
				Ain3StateColor = ain3Error.HasValue ? ain3Error.Value ? Colors.Red : Colors.YellowGreen : Colors.Gray;
			};

			SendingEnabledChanged += isEnabled => {
				Ain1StateColor = Colors.Gray;
				Ain2StateColor = Colors.Gray;
				Ain3StateColor = Colors.Gray;
			};

			_backWorker.AddWork(CycleWork);
			_logger.Log("Программа загружена");
		}

		public MnemonicChemeViewModel MnemonicChemeVm { get; }

		public OldLookViewModel OldLookVm { get; }

		public ArchivesViewModel ArchiveVm { get; }

		public SettingsViewModel SettingsVm { get; }

		public TelemetryViewModel TelemtryVm { get; }


		private void CycleWork() {
			while (true) {
				//int currentCycleActionsCount = 0;
				lock (_cyclePartsSync) {
					foreach (var cyclePart in _cycleParts) {
						if (!cyclePart.Cancel) {
							try {
								cyclePart.InCycleAction();
								Thread.Sleep(50);
							}
							catch {
								continue; /*can show exception in log*/
							}
							//finally {
							//currentCycleActionsCount++;
							//}
						}
					}
				}
				//Console.WriteLine("currentCycleActionsCount=" + currentCycleActionsCount);
			}
		}

		private void ClosePort() {
			// must be called from UI
			try {
				IsSendingEnabled = false;
				RaiseSendingEnabledChanged(IsSendingEnabled);

				_logger.Log("Закрытие ранее открытого порта " + _commandSender + "...");
				_commandSenderController.EndWork(); // TODO: make async
				_commandSender = null;
				_commandSenderController = null;
				_isPortOpened = false;
				_openPortCommand.RaiseCanExecuteChanged();
				_closePortCommand.RaiseCanExecuteChanged();
				_logger.Log("Ранее открытый порт " + _commandSender + " закрыт");

			}
			catch (Exception ex) {
				_logger.Log("Не удалось закрыть открытый ранее порт " + _commandSender + ". " + ex.Message);
			}
		}

		private void OpenPort() {
			// must be called only from UI
			try {
				if (_isPortOpened) ClosePort();
				_logger.Log("Открытие порта " + _selectedComName + "...");


				if (_selectedComName == TestComPortName)
				{
					var sender = new NothingBasedCommandSender(_debugLogger, Notifier);
					_commandSender = sender;
					_commandSenderController = sender;

				}
				else {
					var sender = new SerialPortBasedCommandSender(SelectedComName, _debugLogger);
					_commandSender = sender;
					_commandSenderController = sender;
				}


				_isPortOpened = true;
				_openPortCommand.RaiseCanExecuteChanged();
				_closePortCommand.RaiseCanExecuteChanged();
				_logger.Log("Порт " + _selectedComName + " открыт");

				IsSendingEnabled = true;
				RaiseSendingEnabledChanged(IsSendingEnabled);
			}
			catch (Exception ex) {
				_logger.Log("Не удалось открыть порт " + _selectedComName + ". " + ex.Message);
			}
		}

		private void GetPortsAvailable() {
			var ports = new List<string>();
			ports.AddRange(SerialPort.GetPortNames());
			ports.Add(TestComPortName); // TODO: extract constant);
			ComPortsAvailable = ports;
			if (ComPortsAvailable.Count > 0) SelectedComName = ComPortsAvailable[0];
		}

		public event SendingEnabledChangedDelegate SendingEnabledChanged;

		public bool IsSendingEnabled {
			get {
				lock (_isSendingEnabledSyncObject) return _isSendingEnabled;
			}
			set {
				lock (_isSendingEnabledSyncObject) {
					if (_isSendingEnabled != value) {
						_isSendingEnabled = value;
						RaisePropertyChanged(() => IsSendingEnabled);
					}
				}
			}
		}

		private void RaiseSendingEnabledChanged(bool isSendingEnabled) {
			var eve = SendingEnabledChanged;
			eve?.Invoke(isSendingEnabled);
		}

		public List<string> ComPortsAvailable {
			get { return _comPortsAvailable; }
			set {
				if (_comPortsAvailable != value) {
					_comPortsAvailable = value;
					RaisePropertyChanged(() => ComPortsAvailable);
				}
			}
		}

		public string SelectedComName {
			get { return _selectedComName; }
			set {
				if (value != _selectedComName) {
					_selectedComName = value;
					RaisePropertyChanged(() => SelectedComName);
				}
			}
		}

		public RelayCommand OpenPortCommand => _openPortCommand;

		public RelayCommand ClosePortCommand => _closePortCommand;

		public RelayCommand GetPortsAvailableCommand { get; }

		public IRrModbusCommandSender Sender => _commandSender;

		public byte TargetAddress {
			get { return _targetAddress; }
			set {
				if (_targetAddress != value) {
					_targetAddress = value;
					RaisePropertyChanged(() => TargetAddress);
				}
			}
		}



		public ProgramLogViewModel ProgramLogVm => _programLogVm;



		public void CloseComPort() {
			ClosePort();
		}

		public void RegisterAsCyclePart(ICyclePart part) {
			lock (_cyclePartsSync) {
				_cycleParts.Add(part);
			}
		}

		public List<int> AinsCountInSystem { get; }

		public int SelectedAinsCount {
			get { return _selectedAinsCount; }
			set {
				if (value != 1 && value != 2 && value != 3) throw new ArgumentOutOfRangeException("Поддерживаемое число блоков АИН в системе может быть только 1, 2 или 3, получено ошибочное число: " + value);
				if (value != _selectedAinsCount) {
					_selectedAinsCount = value;
					RaisePropertyChanged(() => SelectedAinsCount);
					var evnt = AinsCountInSystemHasBeenChanged;
					evnt?.Invoke();
				}
			}
		}

		public event AinsCountInSystemHasBeenChangedDelegate AinsCountInSystemHasBeenChanged;

		public Colors Ain1StateColor {
			get { return _ain1StateColor; }
			set {
				if (_ain1StateColor != value) {
					_ain1StateColor = value;
					RaisePropertyChanged(() => Ain1StateColor);
				}
			}
		}

		public Colors Ain2StateColor {
			get { return _ain2StateColor; }
			set {
				if (_ain2StateColor != value) {
					_ain2StateColor = value;
					RaisePropertyChanged(() => Ain2StateColor);
				}
			}
		}

		public Colors Ain3StateColor {
			get { return _ain3StateColor; }
			set {
				if (_ain3StateColor != value) {
					_ain3StateColor = value;
					RaisePropertyChanged(() => Ain3StateColor);
				}
			}
		}
		
		public bool Ain1IsUsed {
			get { return _ain1IsUsed; }
			set {
				if (_ain1IsUsed != value) {
					_ain1IsUsed = value;
					RaisePropertyChanged(() => Ain1IsUsed);
				}
			}
		}

		public bool Ain2IsUsed {
			get { return _ain2IsUsed; }
			set {
				if (_ain2IsUsed != value) {
					_ain2IsUsed = value;
					RaisePropertyChanged(() => Ain2IsUsed);
				}
			}
		}

		public bool Ain3IsUsed {
			get { return _ain3IsUsed; }
			set {
				if (_ain3IsUsed != value) {
					_ain3IsUsed = value;
					RaisePropertyChanged(() => Ain3IsUsed);
				}
			}
		}

		public ILogger Logger => _logger;
	}
}
