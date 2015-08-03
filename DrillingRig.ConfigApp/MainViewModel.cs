using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.CommandSenders.Contracts;
using DrillingRig.CommandSenders.SerialPortBased;

namespace DrillingRig.ConfigApp {
	internal class MainViewModel : ViewModelBase {
		private List<string> _comPortsAvailable;
		private string _selectedComName;

		private IRrModbusCommandSender _commandSender;
		private SerialPort _serialPort;

		private readonly RelayCommand _openPortCommand;
		private readonly RelayCommand _closePortCommand;
		private readonly RelayCommand _getPortsAvailableCommand;

		private bool _isPortOpened;


		public MainViewModel() {
			_commandSender = null;
			_openPortCommand = new RelayCommand(OpenPort, () => !_isPortOpened);
			_closePortCommand = new RelayCommand(ClosePort, () => _isPortOpened);
			_getPortsAvailableCommand = new RelayCommand(GetPortsAvailable);
			GetPortsAvailable();
			_isPortOpened = false;
		}

		private void ClosePort() {
			_serialPort.Close();
			_commandSender = null;
			_isPortOpened = false;
			_openPortCommand.RaiseCanExecuteChanged();
			_closePortCommand.RaiseCanExecuteChanged();
		}

		private void OpenPort() {
			_serialPort = new SerialPort(SelectedComName, 115200);
			_commandSender = new SerialPortBasedCommandSender(_serialPort);
			_isPortOpened = true;
			_openPortCommand.RaiseCanExecuteChanged();
			_closePortCommand.RaiseCanExecuteChanged();
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
	}
}
