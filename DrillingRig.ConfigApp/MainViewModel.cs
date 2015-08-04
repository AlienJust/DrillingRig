using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AlienJust.Adaptation.WindowsPresentation;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.Text;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.CommandSenders.Contracts;
using DrillingRig.CommandSenders.SerialPortBased;
using DrillingRig.ConfigApp.BsEthernetSettings;

namespace DrillingRig.ConfigApp {
	internal class MainViewModel : ViewModelBase, ICommandSenderHost, ITargetAddressHost, IUserInterfaceRoot {
		private List<string> _comPortsAvailable;
		private string _selectedComName;

		private IRrModbusCommandSender _commandSender;
		private SerialPort _serialPort;

		private readonly IThreadNotifier _notifier;
		private readonly IWindowSystem _windowSystem;
		private readonly ProgramLogViewModel _programLogVm;
		private readonly BsEthernetSettingsViewModel _bsEthernetSettingsVm;

		private readonly RelayCommand _openPortCommand;
		private readonly RelayCommand _closePortCommand;
		private readonly RelayCommand _getPortsAvailableCommand;

		private bool _isPortOpened;
		private byte _targetAddress;

		private readonly ILogger _logger;


		public MainViewModel(IThreadNotifier notifier, IWindowSystem windowSystem) {
			_commandSender = null;
			_isPortOpened = false;
			_notifier = notifier;
			_windowSystem = windowSystem;

			GetPortsAvailable();

			_programLogVm = new ProgramLogViewModel(this);
			
			_logger = new RelayLogger(_programLogVm, new DateTimeFormatter(" > "));
			
			_bsEthernetSettingsVm = new BsEthernetSettingsViewModel(this, this, this, _logger, _windowSystem);
			

			_openPortCommand = new RelayCommand(OpenPort, () => !_isPortOpened);
			_closePortCommand = new RelayCommand(ClosePort, () => _isPortOpened);
			_getPortsAvailableCommand = new RelayCommand(GetPortsAvailable);

			_logger.Log("Программа загружена");
		}

		private void ClosePort() {
			try {
				_logger.Log("Закрытие порта " + _serialPort.PortName + "...");
				_serialPort.Close();
				_commandSender = null;
				_isPortOpened = false;
				_openPortCommand.RaiseCanExecuteChanged();
				_closePortCommand.RaiseCanExecuteChanged();
				_logger.Log("Порт " + _serialPort.PortName + " закрыт");
			}
			catch (Exception ex) {
				_logger.Log("Не удалось закрыть порт " + _serialPort.PortName + ". " + ex.Message);
			}
		}

		private void OpenPort() {
			try {
				if (_isPortOpened) ClosePort();
				_logger.Log("Открытие порта " + _selectedComName + "...");
				_serialPort = new SerialPort(SelectedComName, 115200);
				_serialPort.Open();
				_commandSender = new SerialPortBasedCommandSender(_serialPort);
				_isPortOpened = true;
				_openPortCommand.RaiseCanExecuteChanged();
				_closePortCommand.RaiseCanExecuteChanged();
				_logger.Log("Порт " + _serialPort.PortName + " открыт");
			}
			catch (Exception ex) {
				_logger.Log("Не удалось открыть порт " + _serialPort.PortName + ". " + ex.Message);
			}
		}

		private void GetPortsAvailable() {
			ComPortsAvailable = SerialPort.GetPortNames().ToList();
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

		public BsEthernetSettingsViewModel BsEthernetSettingsVm { get { return _bsEthernetSettingsVm; } }

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
	}
}
