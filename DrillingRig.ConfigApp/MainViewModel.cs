using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.Text;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.CommandSenders.SerialPortBased;
using DrillingRig.CommandSenders.TestCommandSender;
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
using DrillingRig.ConfigApp.EngineAutoSetup;
using DrillingRig.ConfigApp.Logs;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using DrillingRig.ConfigApp.MnemonicCheme;
using DrillingRig.ConfigApp.NewLook.Archive;
using DrillingRig.ConfigApp.NewLook.OldLook;
using DrillingRig.ConfigApp.NewLook.Settings;
using DrillingRig.ConfigApp.NewLook.Telemetry;
using Colors = AlienJust.Adaptation.WindowsPresentation.Converters.Colors;

namespace DrillingRig.ConfigApp {
	internal class MainViewModel : ViewModelBase, ILinkContol {
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
		private readonly IMultiLoggerWithStackTrace<int> _debugLogger;
		private readonly ILoggerRegistrationPoint _loggerRegistrationPoint;
		private readonly INotifySendingEnabledRaisable _notifySendingEnabled;



		private readonly AutoSettingsReader _autoSettingsReader;
		private Colors _ain1StateColor;
		private Colors _ain2StateColor;
		private Colors _ain3StateColor;
		private bool _ain1IsUsed;
		private bool _ain2IsUsed;
		private bool _ain3IsUsed;

		public ChartViewModel ChartControlVm { get; set; }
		public IParameterLogger ExternalParamLogger { get; set; }

		private readonly IParameterLogger _paramLogger;
		private readonly IAinsCounterRaisable _ainsCounterRaisable;
		private readonly ICycleThreadHolder _cycleThreadHolder;
		private readonly IAinSettingsReader _ainSettingsReader;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsWriter _ainSettingsWriter;

		private AutoTimeSetter _autoTimeSetter;

		public AinCommandAndCommonTelemetryViewModel AinCommandAndCommonTelemetryVm { get; }

		private readonly IUserInterfaceRoot _uiRoot;
		public readonly List<Color> _colors;


		public MnemonicChemeViewModel MnemonicChemeVm { get; }
		public OldLookViewModel OldLookVm { get; }
		public ArchivesViewModel ArchiveVm { get; }
		public SettingsViewModel SettingsVm { get; }
		public TelemetryViewModel TelemtryVm { get; }
		public EngineAutoSetupViewModel EngineAutoSetupVm { get; }

		public MainViewModel(IUserInterfaceRoot uiRoot, IWindowSystem windowSystem, List<Color> colors, ICommandSenderHostSettable commandSenderHostSettable, ITargetAddressHost targetAddressHost, IMultiLoggerWithStackTrace<int> debugLogger, ILoggerRegistrationPoint loggerRegistrationPoint, INotifySendingEnabledRaisable notifySendingEnabled, IParameterLogger paramLogger, IAinsCounterRaisable ainsCounterRaisable, ICycleThreadHolder cycleThreadHolder, IAinSettingsReader ainSettingsReader, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsReadNotifyRaisable ainSettingsReadNotifyRaisable, IAinSettingsWriter ainSettingsWriter, IAinSettingsStorage ainSettingsStorage, IAinSettingsStorageSettable ainSettingsStorageSettable, IAinSettingsStorageUpdatedNotify storageUpdatedNotify) {
			_uiRoot = uiRoot;
			_colors = colors;

			_commandSenderHostSettable = commandSenderHostSettable;
			_commandSenderHost = commandSenderHostSettable;
			_targetAddressHost = targetAddressHost;

			_isPortOpened = false;

			// Лог программы:
			_debugLogger = debugLogger;
			_loggerRegistrationPoint = loggerRegistrationPoint;

			// разрешение к отправке (COM-порт открыт/закрыт)
			_notifySendingEnabled = notifySendingEnabled;

			_programLogVm = new ProgramLogViewModel(_uiRoot, _debugLogger, new DateTimeFormatter(" > "));
			_logger = new RelayLogger(_programLogVm);
			_loggerRegistrationPoint.RegisterLoggegr(_logger);

			GetPortsAvailable();

			_openPortCommand = new RelayCommand(OpenPort, () => !_isPortOpened);
			_closePortCommand = new RelayCommand(ClosePort, () => _isPortOpened);
			GetPortsAvailableCommand = new RelayCommand(GetPortsAvailable);

			_paramLogger = paramLogger;


			_ainsCounterRaisable = ainsCounterRaisable;
			_cycleThreadHolder = cycleThreadHolder;
			_ainSettingsReader = ainSettingsReader;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_ainSettingsWriter = ainSettingsWriter;
			// Блоки АИН в системе:
			AinsCountInSystem = new List<int> { 1, 2, 3 };
			SelectedAinsCount = AinsCountInSystem.First();

			var ainSettingsReadedWriter = new AinSettingsReaderWriter(_ainSettingsReader, _ainSettingsWriter);


			AinCommandAndCommonTelemetryVm = new AinCommandAndCommonTelemetryViewModel(
				new AinCommandAndMinimalCommonTelemetryViewModel(_commandSenderHost, _targetAddressHost, _uiRoot, _logger, _notifySendingEnabled, 0, ainSettingsStorage, storageUpdatedNotify),
				new TelemetryCommonViewModel(), _commandSenderHost, _targetAddressHost, _uiRoot, _notifySendingEnabled);

			_cycleThreadHolder.RegisterAsCyclePart(AinCommandAndCommonTelemetryVm);

			TelemtryVm = new TelemetryViewModel(_uiRoot, _commandSenderHost, _targetAddressHost, _logger, _cycleThreadHolder, _ainsCounterRaisable, _paramLogger, _notifySendingEnabled);

			SettingsVm = new SettingsViewModel(_uiRoot, _logger, ainSettingsReadedWriter, _ainSettingsReadNotify, ainSettingsReadNotifyRaisable, ainSettingsStorage, ainSettingsStorageSettable, storageUpdatedNotify, _ainsCounterRaisable, _commandSenderHost, _targetAddressHost, _notifySendingEnabled); // TODO: can be moved to app.xaml.cs if needed

			ArchiveVm = new ArchivesViewModel(
				new ArchiveViewModel(_commandSenderHost, _targetAddressHost, _uiRoot, _logger, _notifySendingEnabled, 0),
				new ArchiveViewModel(_commandSenderHost, _targetAddressHost, _uiRoot, _logger, _notifySendingEnabled, 1));

			MnemonicChemeVm = new MnemonicChemeViewModel(Path.Combine(Environment.CurrentDirectory, "mnemoniccheme.png"));
			OldLookVm = new OldLookViewModel(_uiRoot, windowSystem, _commandSenderHost, _targetAddressHost, _notifySendingEnabled, this, _logger, _debugLogger, _cycleThreadHolder, _ainsCounterRaisable, _paramLogger, ainSettingsStorage, storageUpdatedNotify);

			_ain1StateColor = Colors.Gray;
			_ain2StateColor = Colors.Gray;
			_ain3StateColor = Colors.Gray;

			_ain1IsUsed = true;
			_ain2IsUsed = false;
			_ain3IsUsed = false;

			_ainsCounterRaisable.AinsCountInSystemHasBeenChanged += ainsCount => {
				switch (ainsCount) {
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

			EngineAutoSetupVm = new EngineAutoSetupViewModel(
				new TableViewModel("Начальные значения:"),
				new TableViewModel("После тестирования:"),
				_notifySendingEnabled, _ainSettingsReadNotify, _ainSettingsWriter, _uiRoot, _logger, _commandSenderHost, _targetAddressHost);

			_logger.Log("Программа загружена");
		}

		private void ClosePort() {
			try {
				_notifySendingEnabled.SetIsSendingEnabledAndRaiseChange(false);
				var currentSender = _commandSenderHost.Sender;
				_logger.Log("Закрытие ранее открытого порта " + currentSender + "...");

				// Вызов SilentSender.EndWork не производится!
				currentSender.EndWork(); // TODO: make async
				_commandSenderHostSettable.SetCommandSender(null, null);

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

				if (_selectedComName == TestComPortName) {
					var backWorker = new SingleThreadedRelayQueueWorkerProceedAllItemsBeforeStopNoLog<Action>("NbBackWorker", a => a(), ThreadPriority.BelowNormal, true, null);
					var sender = new NothingBasedCommandSender(backWorker, backWorker, _debugLogger, _uiRoot.Notifier);
					var silentSender = new SilentNothingBasedCommandSender(backWorker, backWorker, _debugLogger, _uiRoot.Notifier);
					_commandSenderHostSettable.SetCommandSender(sender, silentSender);

				}
				else {
					var serialPort = new SerialPort(SelectedComName, 115200);
					serialPort.Open();

					var backWorker = new SingleThreadedRelayQueueWorkerProceedAllItemsBeforeStopNoLog<Action>("SerialPortBackWorker", a => a(), ThreadPriority.BelowNormal, true, null);
					var sender = new SerialPortBasedCommandSender(backWorker, backWorker, serialPort, _debugLogger);
					var silentSender = new SilentSerialPortBasedCommandSender(backWorker, backWorker, serialPort, _debugLogger);
					_commandSenderHostSettable.SetCommandSender(sender, silentSender);
				}


				_isPortOpened = true;
				_openPortCommand.RaiseCanExecuteChanged();
				_closePortCommand.RaiseCanExecuteChanged();
				_logger.Log("Порт " + _selectedComName + " открыт");

				_notifySendingEnabled.SetIsSendingEnabledAndRaiseChange(true);
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


		public List<int> AinsCountInSystem { get; }

		public int SelectedAinsCount {
			get { return _ainsCounterRaisable.SelectedAinsCount; }
			set {
				if (value != 1 && value != 2 && value != 3) throw new ArgumentOutOfRangeException("Поддерживаемое число блоков АИН в системе может быть только 1, 2 или 3, получено ошибочное число: " + value);
				_ainsCounterRaisable.SetAinsCountAndRaiseChange(value);
				RaisePropertyChanged(() => SelectedAinsCount);
			}
		}

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
