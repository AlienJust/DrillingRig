using System;
using System.Collections.Generic;
using System.IO.Ports;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.Text;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.CommandSenders.Contracts;
using DrillingRig.CommandSenders.SerialPortBased;
using DrillingRig.CommandSenders.TestCommandSender;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.BsEthernetNominals;
using DrillingRig.ConfigApp.BsEthernetSettings;

namespace DrillingRig.ConfigApp {
	internal class MainViewModel : ViewModelBase, ICommandSenderHost, ITargetAddressHost, IUserInterfaceRoot, INotifySendingEnabled {
		private List<string> _comPortsAvailable;
		private string _selectedComName;

		private IRrModbusCommandSender _commandSender;

		private readonly IThreadNotifier _notifier;
		private readonly IWindowSystem _windowSystem;


		private readonly ProgramLogViewModel _programLogVm;
		private readonly BsEthernetSettingsViewModel _bsEthernetSettingsVm;
		private readonly BsEthernetNominalsViewModel _bsEthernetNominalsVm;
		private readonly AinTelemetriesViewModel _ainTelemetriesVm;
		private readonly AinCommandViewModel _ain1CommandVm;
		private readonly AinCommandViewModel _ain2CommandVm;
		private readonly AinCommandViewModel _ain3CommandVm;

		private readonly RelayCommand _openPortCommand;
		private readonly RelayCommand _closePortCommand;
		private readonly RelayCommand _getPortsAvailableCommand;

		private bool _isPortOpened;
		private byte _targetAddress;

		private readonly ILogger _logger;
		private bool _isSendingEnabled;

		public MainViewModel(IThreadNotifier notifier, IWindowSystem windowSystem) {
			_commandSender = null;
			_isPortOpened = false;
			_notifier = notifier;
			_windowSystem = windowSystem;

			GetPortsAvailable();

			_programLogVm = new ProgramLogViewModel(this);

			_logger = new RelayLogger(_programLogVm, new DateTimeFormatter(" > "));

			_bsEthernetSettingsVm = new BsEthernetSettingsViewModel(this, this, this, _logger, _windowSystem, this);
			_bsEthernetNominalsVm = new BsEthernetNominalsViewModel(this, this, this, _logger, _windowSystem, this);
			_ainTelemetriesVm = new AinTelemetriesViewModel(this, this, this, _logger, _windowSystem);
			
			_ain1CommandVm = new AinCommandViewModel(this, this, this, _logger, _windowSystem, this, 0);
			_ain2CommandVm = new AinCommandViewModel(this, this, this, _logger, _windowSystem, this, 1);
			_ain3CommandVm = new AinCommandViewModel(this, this, this, _logger, _windowSystem, this, 2);


			_openPortCommand = new RelayCommand(OpenPort, () => !_isPortOpened);
			_closePortCommand = new RelayCommand(ClosePort, () => _isPortOpened);
			_getPortsAvailableCommand = new RelayCommand(GetPortsAvailable);

			_logger.Log("Программа загружена");
		}

		private void ClosePort() {
			try {
				IsSendingEnabled = false;
				RaiseSendingEnabledChanged(IsSendingEnabled);

				_logger.Log("Закрытие ранее открытого порта " + _commandSender + "...");
				_commandSender.Dispose(); // TODO: make async
				_commandSender = null;
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

				if (_selectedComName == "Test") // TODO: extract constant
					_commandSender = new NothingBasedCommandSender();
				else
					_commandSender = new SerialPortBasedCommandSender(SelectedComName);

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
			var ports = new List<string> {"Test"}; // TODO: extract constant
			ports.AddRange(SerialPort.GetPortNames());
			ComPortsAvailable = ports;
			if (ComPortsAvailable.Count > 0) SelectedComName = ComPortsAvailable[0];
		}

		public event SendingEnabledChangedDelegate SendingEnabledChanged;

		// TODO: thread safity
		public bool IsSendingEnabled {
			get { return _isSendingEnabled; }
			set { _isSendingEnabled = value; }
		}

		private void RaiseSendingEnabledChanged(bool isSendingEnabled)
		{
			var eve = SendingEnabledChanged;
			if (eve != null)
				eve.Invoke(isSendingEnabled);
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

		public RelayCommand OpenPortCommand {
			get { return _openPortCommand; }
		}

		public RelayCommand ClosePortCommand {
			get { return _closePortCommand; }
		}

		public RelayCommand GetPortsAvailableCommand {
			get { return _getPortsAvailableCommand; }
		}

		public IRrModbusCommandSender Sender {
			get { return _commandSender; }
		}

		public byte TargetAddress {
			get { return _targetAddress; }
			set {
				if (_targetAddress != value) {
					_targetAddress = value;
					RaisePropertyChanged(() => TargetAddress);
				}
			}
		}

		public IThreadNotifier Notifier {
			get { return _notifier; }
		}

		public ProgramLogViewModel ProgramLogVm {
			get { return _programLogVm; }
		}

		public BsEthernetSettingsViewModel BsEthernetSettingsVm {
			get { return _bsEthernetSettingsVm; }
		}

		public BsEthernetNominalsViewModel BsEthernetNominalsVm {
			get { return _bsEthernetNominalsVm; }
		}

		public AinTelemetriesViewModel AinTelemetriesVm {
			get { return _ainTelemetriesVm; }
		}

		public AinCommandViewModel Ain1CommandVm {
			get { return _ain1CommandVm; }
		}

		public AinCommandViewModel Ain2CommandVm {
			get { return _ain2CommandVm; }
		}

		public AinCommandViewModel Ain3CommandVm {
			get { return _ain3CommandVm; }
		}
	}
}
