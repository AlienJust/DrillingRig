using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands;

namespace DrillingRig.ConfigApp.BsEthernetSettings
{
	class BsEthernetSettingsViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;

		private readonly ICommand _readSettingCommand;
		private readonly ICommand _writeSettingsCommand;
		private readonly ICommand _importSettingCommand;
		private readonly ICommand _exportSettingsCommand;

		
		private string _ipAddress;
		private string _mask;
		private string _gateway;
		private string _dnsServer;
		private string _macAddress;
		private byte _modbusAddress;
		private ushort _driveNumber;

		public BsEthernetSettingsViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;

			_ipAddress = string.Empty;
			_mask = string.Empty;
			_gateway = string.Empty;
			_dnsServer = string.Empty;
			_macAddress = string.Empty;
			_modbusAddress = 1;
			_driveNumber = 0;

			_readSettingCommand = new RelayCommand(ReadSettings);
			_writeSettingsCommand = new RelayCommand(WriteSettings);

			_importSettingCommand = new RelayCommand(ImportSettings);
			_exportSettingsCommand = new RelayCommand(ExportSettings);
		}

		private void ImportSettings() {
			_logger.Log("Начало импорта настроек");
			var dialogResult = _windowSystem.ShowOpenFileDialog("Выберите файл с настройками БС-Ethernet", "XML files|*.xml|All files|*.*");
			if (!string.IsNullOrEmpty(dialogResult)) {
				try {
					var importer = new BsEthernetSettingsImporter(dialogResult);
					var settings = importer.ImportSettings();
					IpAddress = settings.IpAddress;
					Mask = settings.Mask;
					Gateway = settings.Gateway;
					DnsServer = settings.DnsServer;
					MacAddress = settings.MacAddress;
					ModbusAddress = settings.ModbusAddress;
					DriveNumber = settings.DriveNumber;
					_logger.Log("Настройки успешно импортированы");
				}
				catch (Exception ex) {
					_logger.Log("Произошла ошибка во время импорта настроек. " + ex.Message);
				}
			}
			else {
				_logger.Log("Импорт отменен пользователем");
			}
		}

		private void ExportSettings() {
			_logger.Log("Начало экспорта настроек");
			var dialogResult = _windowSystem.ShowSaveFileDialog("Выберите файл для сохранения настроек БС-Ethernet", "XML files|*.xml|All files|*.*");
			if (!string.IsNullOrEmpty(dialogResult)) {
				try {
					var exporter = new BsEthernetSettingsExporterXml(dialogResult);
					exporter.ExportSettings(new BsEthernetSettingsSimple(IpAddress, Mask, Gateway, DnsServer, MacAddress, ModbusAddress, DriveNumber));
					_logger.Log("Настройки успешно экспортированы");
				}
				catch (Exception ex) {
					_logger.Log("Произошла ошибка во время экспорта настроек. " + ex.Message);
				}
			}
			else {
				_logger.Log("Экспорт отменен пользователем");
			}
		}

		private void WriteSettings() {
			try {
				_logger.Log("Подготовка к записи настроек БС-Ethernet");
				var ip = new IPAddress(IpAddress.Split('.').Select(byte.Parse).ToArray());
				var mask = new IPAddress(Mask.Split('.').Select(byte.Parse).ToArray());
				var gate = new IPAddress(Gateway.Split('.').Select(byte.Parse).ToArray());
				var dns = new IPAddress(DnsServer.Split('.').Select(byte.Parse).ToArray());
				var mac = new PhysicalAddress(MacAddress.Split('.').Select(s => byte.Parse(s, NumberStyles.HexNumber)).ToArray());

				var cmd = new WriteBsEthernetSettingsCommand(ip, mask, gate, dns, mac, ModbusAddress, DriveNumber);

				_logger.Log("Команда записи настроек БС-Ethernet поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(5)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes);
							}
							catch (Exception exx) {
								// TODO: log exception about error on answer parsing
								throw new Exception("Ошибка при разборе ответа: " + exx.Message, exx);
							}
						}
						catch (Exception ex) {
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду записи настроек БС-Ethernet в очередь: " + ex.Message);
			}
		}

		private void ReadSettings() {
			throw new NotImplementedException();
		}

		public string IpAddress {
			get { return _ipAddress; }
			set {
				if (_ipAddress != value) {
					_ipAddress = value;
					RaisePropertyChanged(() => IpAddress);
				}
			}
		}

		public string Mask {
			get { return _mask; }
			set {
				if (_mask != value) {
					_mask = value;
					RaisePropertyChanged(() => Mask);
				}
			}
		}

		public string Gateway {
			get { return _gateway; }
			set {
				if (_gateway != value) {
					_gateway = value;
					RaisePropertyChanged(()=>Gateway);
				} }
		}

		public string DnsServer {
			get { return _dnsServer; }
			set {
				if (_dnsServer != value) {
					_dnsServer = value;
					RaisePropertyChanged(()=>DnsServer);
				} }
		}

		public string MacAddress {
			get { return _macAddress; }
			set {
				if (_macAddress != value) {
					_macAddress = value;
					RaisePropertyChanged(()=>MacAddress);
				} }
		}

		public byte ModbusAddress {
			get { return _modbusAddress; }
			set {
				if (_modbusAddress != value) {
					_modbusAddress = value;
					RaisePropertyChanged(()=>ModbusAddress);
				} }
		}

		public ushort DriveNumber {
			get { return _driveNumber; }
			set {
				if (_driveNumber != value) {
					_driveNumber = value;
					RaisePropertyChanged(() => DriveNumber);
				}
			}
		}

		public ICommand ReadSettingCommand {
			get { return _readSettingCommand; }
		}

		public ICommand WriteSettingCommand {
			get { return _writeSettingsCommand; }
		}

		public ICommand ImportSettingCommand {
			get { return _importSettingCommand; }
		}

		public ICommand ExportSettingCommand
		{
			get { return _exportSettingsCommand; }
		}
	}
}
