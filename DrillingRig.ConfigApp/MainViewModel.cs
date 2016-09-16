using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using AlienJust.Adaptation.ConsoleLogger;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.Text;
using AlienJust.Support.Text.Contracts;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.CommandSenders.SerialPortBased;
using DrillingRig.CommandSenders.TestCommandSender;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.Logs;
using DrillingRig.ConfigApp.LookedLikeAbb;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using DrillingRig.ConfigApp.MnemonicCheme;
using DrillingRig.ConfigApp.NewLook.Archive;
using DrillingRig.ConfigApp.NewLook.OldLook;
using DrillingRig.ConfigApp.NewLook.Settings;
using DrillingRig.ConfigApp.NewLook.Telemetry;
using Colors = AlienJust.Adaptation.WindowsPresentation.Converters.Colors;

namespace DrillingRig.ConfigApp {
	internal class MainViewModel : ViewModelBase
		, IUserInterfaceRoot
		, ILinkContol
		, ICycleThreadHolder
		, IAinsCounter /*, IAinsLinkControlViewModel*/ {
		public IThreadNotifier Notifier { get; }

		private const string TestComPortName = "ТЕСТ";

		private List<string> _comPortsAvailable;
		private string _selectedComName;

		private readonly ICommandSenderHostSettable _commandSenderHostSettable;
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targetAddressHost;

		private readonly ProgramLogViewModel _programLogVm;


		private readonly RelayCommand _openPortCommand;
		private readonly RelayCommand _closePortCommand;

		private bool _isPortOpened;
		
		private readonly ILogger _logger;
		private readonly IMultiLoggerWithStackTrace _debugLogger;
		private readonly ILoggerRegistrationPoint _loggerRegistrationPoint;
		private readonly INotifySendingEnabledRaisable _notifySendingEnabled;

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
		private AutoTimeSetter _autoTimeSetter;
		public IParameterLoggerContainer ParamLoggerContainer { get; }

		public AinCommandAndCommonTelemetryViewModel AinCommandAndCommonTelemetryVm { get; }

		public readonly List<Color> _colors;
		

		public MainViewModel(IThreadNotifier notifier, IWindowSystem windowSystem, List<Color> colors, ICommandSenderHostSettable commandSenderHostSettable, ITargetAddressHost targetAddressHost, IMultiLoggerWithStackTrace debugLogger, ILoggerRegistrationPoint loggerRegistrationPoint, INotifySendingEnabledRaisable notifySendingEnabled) {
			Notifier = notifier;
			_colors = colors;

			_commandSenderHostSettable = commandSenderHostSettable;
			_commandSenderHost = commandSenderHostSettable;
			_targetAddressHost = targetAddressHost;

			_isPortOpened = false;
			
			// Лог программы:
			_debugLogger = debugLogger;
			_loggerRegistrationPoint = loggerRegistrationPoint;
			_notifySendingEnabled = notifySendingEnabled;

			_programLogVm = new ProgramLogViewModel(this, _debugLogger);
			_logger = new RelayLogger(_programLogVm, new DateTimeFormatter(" > "));
			_loggerRegistrationPoint.RegisterLoggegr(_logger);

			// циклический опрос
			_cyclePartsSync = new object();
			_cycleParts = new List<ICyclePart>();
			_backWorker = new SingleThreadedRelayQueueWorker<Action>("CycleBackWorker", a => a(), ThreadPriority.Lowest, true, null, _debugLogger.GetLogger(0));

			GetPortsAvailable();

			// Блоки АИН в системе:
			AinsCountInSystem = new List<int> { 1, 2, 3 };
			SelectedAinsCount = AinsCountInSystem.First();
			

			_openPortCommand = new RelayCommand(OpenPort, () => !_isPortOpened);
			_closePortCommand = new RelayCommand(ClosePort, () => _isPortOpened);
			GetPortsAvailableCommand = new RelayCommand(GetPortsAvailable);


			// ABB way:
			
			ChartControlVm = new ChartViewModel(this, _colors);
			var paramLogger = new ParameterLoggerRelay(new List<IParameterLogger> { ChartControlVm});
			_relayParamLogger = paramLogger;
			ParamLoggerContainer = paramLogger;

			// var cycleReader = new CycleReader(this, this, this, _logger, this); // TODO: check if needed

			var ainSettingsReader = new AinSettingsReader(_commandSenderHost, _targetAddressHost, _logger);
			var ainSettingsWriter = new AinSettingsWriter(_commandSenderHost, _targetAddressHost, this, ainSettingsReader);
			var ainSettingsReadedWriter = new AinSettingsReaderWriter(ainSettingsReader, ainSettingsWriter);

			_autoTimeSetter = new AutoTimeSetter(_commandSenderHostSettable, _notifySendingEnabled, _targetAddressHost, _logger); // TODO: can I convert it to local variable (woudn't it be GCed)?
			_autoSettingsReader = new AutoSettingsReader(_notifySendingEnabled, this, ainSettingsReader, _logger); // TODO: can I convert it to local variable (woudn't it be GCed)?
			

			AinCommandAndCommonTelemetryVm = new AinCommandAndCommonTelemetryViewModel(
				new AinCommandOnlyViewModel(_commandSenderHost, _targetAddressHost, this, _logger, _notifySendingEnabled, 0),
				new TelemetryCommonViewModel(_logger, _debugLogger), _commandSenderHost, _targetAddressHost, this, _logger, _debugLogger, _notifySendingEnabled);
			RegisterAsCyclePart(AinCommandAndCommonTelemetryVm);

			TelemtryVm = new TelemetryViewModel(this, _commandSenderHost, _targetAddressHost, _logger, this, this, _relayParamLogger);
			SettingsVm = new SettingsViewModel(this, _logger, ainSettingsReadedWriter, ainSettingsReader);
			ArchiveVm = new ArchivesViewModel(new ArchiveViewModel(_commandSenderHost, _targetAddressHost, this, _logger, _notifySendingEnabled, 0), new ArchiveViewModel(_commandSenderHost, _targetAddressHost, this, _logger, _notifySendingEnabled, 1));
			MnemonicChemeVm = new MnemonicChemeViewModel(Path.Combine(Environment.CurrentDirectory, "mnemoniccheme.png"));
			OldLookVm = new OldLookViewModel(this, windowSystem, _commandSenderHost, _targetAddressHost, _notifySendingEnabled, this, _logger, _debugLogger, this, this, _relayParamLogger);
			
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

			_notifySendingEnabled.SendingEnabledChanged += isEnabled => {
				// TODO: execution in ui thread
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
				lock (_cyclePartsSync) {
					foreach (var cyclePart in _cycleParts) {
						if (!cyclePart.Cancel) {
							try {
								cyclePart.InCycleAction();
								Thread.Sleep(10);
							}
							catch {
								Thread.Sleep(10);
							}
						}
						else Thread.Sleep(5);
					}
				}
			}
		}

		private void ClosePort() {
			try {
				_notifySendingEnabled.SetIsSendingEnabledAndRaiseChange(false);
				var currentSender = _commandSenderHost.Sender;
				_logger.Log("Закрытие ранее открытого порта " + currentSender + "...");
				currentSender.EndWork(); // TODO: make async
				_commandSenderHostSettable.SetCommandSender(null);

				_isPortOpened = false;
				_openPortCommand.RaiseCanExecuteChanged();
				_closePortCommand.RaiseCanExecuteChanged();
				_logger.Log("Ранее открытый порт " + currentSender + " закрыт");

			}
			catch (Exception ex) {
				_logger.Log("Не удалось закрыть открытый ранее порт. " + ex.Message);
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
					_commandSenderHostSettable.SetCommandSender(sender);

				}
				else {
					var sender = new SerialPortBasedCommandSender(SelectedComName, _debugLogger);
					_commandSenderHostSettable.SetCommandSender(sender);
				}


				_isPortOpened = true;
				_openPortCommand.RaiseCanExecuteChanged();
				_closePortCommand.RaiseCanExecuteChanged();
				_logger.Log("Порт " + _selectedComName + " открыт");

				_notifySendingEnabled.SetIsSendingEnabledAndRaiseChange(false);
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
	}
}
