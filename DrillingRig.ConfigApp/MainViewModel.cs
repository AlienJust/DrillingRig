using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
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
using DrillingRig.ConfigApp.AinsSettings;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AvaDock;
using DrillingRig.ConfigApp.BsEthernetNominals;
using DrillingRig.ConfigApp.BsEthernetSettings;
using DrillingRig.ConfigApp.CoolerTelemetry;
using DrillingRig.ConfigApp.EngineSettings;
using DrillingRig.ConfigApp.LookedLikeAbb;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;
using DrillingRig.ConfigApp.NewLook.Archive;
using DrillingRig.ConfigApp.OldLook;
using DrillingRig.ConfigApp.RectifierTelemetry;
using DrillingRig.ConfigApp.Settings;
using DrillingRig.ConfigApp.SystemControl;
using DrillingRig.ConfigApp.Telemetry;

namespace DrillingRig.ConfigApp {
	internal class MainViewModel : ViewModelBase, ICommandSenderHost, ITargetAddressHost, IUserInterfaceRoot, INotifySendingEnabled, ILinkContol, ICycleThreadHolder, IAinsCounter {
		public IThreadNotifier Notifier { get; }
		private readonly IWindowSystem _windowSystem;

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

		private readonly object _isSendingEnabledSyncObject;
		private bool _isSendingEnabled;


		private readonly object _cyclePartsSync;
		private readonly List<ICyclePart> _cycleParts;
		private SingleThreadedRelayQueueWorker<Action> _backWorker;
		private int _selectedAinsCount;


		public ChartViewModel ChartControlVm { get; set; }

		public DockManagerViewModel DockManagerViewModel { get; private set; }

		public MainViewModel(IThreadNotifier notifier, IWindowSystem windowSystem) {
			Notifier = notifier;
			_windowSystem = windowSystem;

			_targetAddress = 1;

			_commandSender = null;
			_commandSenderController = null;

			_isPortOpened = false;

			_isSendingEnabled = false;
			_isSendingEnabledSyncObject = new object();

			_cyclePartsSync = new object();
			_cycleParts = new List<ICyclePart>();
			_backWorker = new SingleThreadedRelayQueueWorker<Action>("CycleBackWorker", a => a(), ThreadPriority.BelowNormal, true, null, new RelayActionLogger(Console.WriteLine, new ChainedFormatter(new List<ITextFormatter> { new PreffixTextFormatter("TelemetryBackWorker > "), new DateTimeFormatter(" > ") })));

			GetPortsAvailable();

			// Блоки АИН в системе:
			AinsCountInSystem = new List<int> { 1, 2, 3 };
			SelectedAinsCount = AinsCountInSystem.First();

			// Лог программы:
			_programLogVm = new ProgramLogViewModel(this) { Title = "Журнал" };
			_logger = new RelayLogger(_programLogVm, new DateTimeFormatter(" > "));



			_openPortCommand = new RelayCommand(OpenPort, () => !_isPortOpened);
			_closePortCommand = new RelayCommand(ClosePort, () => _isPortOpened);
			GetPortsAvailableCommand = new RelayCommand(GetPortsAvailable);


			// ABB way:
			ChartControlVm = new ChartViewModel(this) { Title = "Графики", CanClose = false };

			// var cycleReader = new CycleReader(this, this, this, _logger, this); // TODO: check if needed

			var ainSettingsReadedWriter = new AinSettingsReaderWriter(this, this, this, _logger, this); // TODO: move to field



			_logger.Log("Программа загружена");
			_backWorker.AddWork(CycleWork);

			var documents = new List<DockWindowViewModel> {
				new TelemetryViewModel(this, this, this, _logger, this, this, ChartControlVm) {Title = "Телеметрия", CanClose = false},
				new SettingsViewModel(this, _logger, ainSettingsReadedWriter) {Title = "Настройки", CanClose = false},
				new AinCommandOnlyViewModel(this, this, this, _logger, this, 0) {Title = "Команда", CanClose = false},
				new ArchivesViewModel(
					new ArchiveViewModel(this,this,this,_logger, this, 0),
					new ArchiveViewModel(this,this,this,_logger, this, 1)) {Title="Архив",CanClose = false},
				new OldLookViewModel(this, _windowSystem, this, this, this, this, _logger, this, this, ChartControlVm) {Title = "Дополнительно", CanClose = false}
			};
			var anchorables = new List<DockWindowViewModel> { ChartControlVm, _programLogVm };
			DockManagerViewModel = new DockManagerViewModel(documents, anchorables);
		}


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
			try {
				if (_isPortOpened) ClosePort();
				_logger.Log("Открытие порта " + _selectedComName + "...");


				if (_selectedComName == TestComPortName) // TODO: extract constant
				{
					var sender = new NothingBasedCommandSender();
					_commandSender = sender;
					_commandSenderController = sender;

				}
				else {
					var sender = new SerialPortBasedCommandSender(SelectedComName);
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
			ComPortsAvailable = ports;
			if (ComPortsAvailable.Count > 0) SelectedComName = ComPortsAvailable[0];
			ports.Add(TestComPortName); // TODO: extract constant);
		}

		public event SendingEnabledChangedDelegate SendingEnabledChanged;

		public bool IsSendingEnabled {
			get { lock (_isSendingEnabledSyncObject) return _isSendingEnabled; }
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


	}
}
