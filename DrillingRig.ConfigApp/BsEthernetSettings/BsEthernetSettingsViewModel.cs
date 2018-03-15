using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.BsEthernetSettings;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.BsEthernetSettings {
	class BsEthernetSettingsViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;

		private readonly RelayCommand _readSettingsCommand;
		private readonly RelayCommand _writeSettingsCommand;
		private readonly ICommand _importSettingCommand;
		private readonly ICommand _exportSettingsCommand;

		private string _ipAddress;
		private string _mask;
		private string _gateway;
		private string _dnsServer;
		private string _macAddress;
		private byte _modbusAddress;
		private byte _driveNumber;
		private byte _addressCan;

		private readonly List<FtRoleViewModel> _ftRoles;
		private FtRoleViewModel _selectedFtRole;

		public BsEthernetSettingsViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;

			_ipAddress = string.Empty;
			_mask = string.Empty;
			_gateway = string.Empty;
			_dnsServer = string.Empty;
			_macAddress = string.Empty;
			_modbusAddress = 1;
			_driveNumber = 0;
			_addressCan = 0;
			_ftRoles = new List<FtRoleViewModel> {
				new FtRoleViewModel(FriquencyTransformerRole.Single),
				new FtRoleViewModel(FriquencyTransformerRole.Master),
				new FtRoleViewModel(FriquencyTransformerRole.Slave)
			};
			_selectedFtRole = _ftRoles.First();

			_readSettingsCommand = new RelayCommand(ReadSettings, () => _sendingEnabledControl.IsSendingEnabled);
			_writeSettingsCommand = new RelayCommand(WriteSettings, () => _sendingEnabledControl.IsSendingEnabled);

			_importSettingCommand = new RelayCommand(ImportSettings);
			_exportSettingsCommand = new RelayCommand(ExportSettings);

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool issendingenabled) {
			_readSettingsCommand.RaiseCanExecuteChanged();
			_writeSettingsCommand.RaiseCanExecuteChanged();
		}

		private void ImportSettings() {
			_logger.Log("Начало импорта настроек");
			var dialogResult = _windowSystem.ShowOpenFileDialog("Выберите файл с настройками БС-Ethernet", "XML files|*.xml|All files|*.*");
			if (!string.IsNullOrEmpty(dialogResult)) {
				try {
					var importer = new BsEthernetSettingsXmlWorker(dialogResult);
					var settings = importer.ImportSettings();

					IpAddress = settings.IpAddress.ToString();
					Mask = settings.Mask.ToString();
					Gateway = settings.Gateway.ToString();
					DnsServer = settings.DnsServer.ToString();

					var mac = settings.MacAddress.GetAddressBytes().Aggregate(string.Empty, (current, b) => current + (b.ToString("X2") + "."));
					mac = mac.Substring(0, mac.Length - 1);
					MacAddress = mac;

					ModbusAddress = settings.ModbusAddress;
					DriveNumber = settings.DriveNumber;
					AddressCan = settings.AddressCan;
					SelectedFtRole = _ftRoles.First(r => r.Role == settings.FtRole);


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
					var exporter = new BsEthernetSettingsXmlWorker(dialogResult);
					var mac = new PhysicalAddress(MacAddress.Split('.').Select(s => byte.Parse(s, NumberStyles.HexNumber)).ToArray());
					var ip = new IPAddress(IpAddress.Split('.').Select(byte.Parse).ToArray());
					var mask = new IPAddress(Mask.Split('.').Select(byte.Parse).ToArray());
					var gate = new IPAddress(Gateway.Split('.').Select(byte.Parse).ToArray());
					var dns = new IPAddress(DnsServer.Split('.').Select(byte.Parse).ToArray());
					var ftRole = SelectedFtRole.Role;

					exporter.ExportSettings(new BsEthernetSettingsSimple(mac, ip, mask, gate, dns, _modbusAddress, _driveNumber, _addressCan, ftRole));
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
				var mac = new PhysicalAddress(MacAddress.Split('.').Select(s => byte.Parse(s, NumberStyles.HexNumber)).ToArray());
				var ip = new IPAddress(IpAddress.Split('.').Select(byte.Parse).ToArray());
				var mask = new IPAddress(Mask.Split('.').Select(byte.Parse).ToArray());
				var gate = new IPAddress(Gateway.Split('.').Select(byte.Parse).ToArray());
				var dns = new IPAddress(DnsServer.Split('.').Select(byte.Parse).ToArray());
				var ftRole = SelectedFtRole.Role;


				var cmd = new WriteBsEthernetSettingsCommand(new BsEthernetSettingsSimple(mac, ip, mask, gate, dns, ModbusAddress, DriveNumber, AddressCan, ftRole));

				_logger.Log("Команда записи настроек БС-Ethernet поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(0.2), 2
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes); // result is unused but GetResult can throw exception
								_logger.Log("Настройки успешно записаны в БС-Ethernet");
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
			try {
				_logger.Log("Подготовка к чтению настроек БС-Ethernet");

				var cmd = new ReadBsEthernetSettingsCommand();

				_logger.Log("Команда чтения настроек БС-Ethernet поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(0.2), 2
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes);
								var mac = string.Empty;
								mac = result.MacAddress.GetAddressBytes().Aggregate(mac, (current, b) => current + b.ToString("X2") + ".");
								mac = mac.Substring(0, mac.Length - 1);
								_userInterfaceRoot.Notifier.Notify(() => {
									MacAddress = mac;
									IpAddress = result.IpAddress.ToString();
									Mask = result.Mask.ToString();
									Gateway = result.Gateway.ToString();
									DnsServer = result.DnsServer.ToString();
									ModbusAddress = result.ModbusAddress;
									DriveNumber = result.DriveNumber;
									AddressCan = result.AddressCan;
									SelectedFtRole = FtRoles.First(ftr => ftr.Role == result.FtRole);
								});
								_logger.Log("Настройки БС-Ethernet успешно прочитаны");
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
				_logger.Log("Не удалось поставить команду чтения настроек БС-Ethernet в очередь: " + ex.Message);
			}
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
					RaisePropertyChanged(() => Gateway);
				}
			}
		}

		public string DnsServer {
			get { return _dnsServer; }
			set {
				if (_dnsServer != value) {
					_dnsServer = value;
					RaisePropertyChanged(() => DnsServer);
				}
			}
		}

		public string MacAddress {
			get { return _macAddress; }
			set {
				if (_macAddress != value) {
					_macAddress = value;
					RaisePropertyChanged(() => MacAddress);
				}
			}
		}

		public byte ModbusAddress {
			get { return _modbusAddress; }
			set {
				if (_modbusAddress != value) {
					_modbusAddress = value;
					RaisePropertyChanged(() => ModbusAddress);
				}
			}
		}

		public byte DriveNumber {
			get { return _driveNumber; }
			set {
				if (_driveNumber != value) {
					_driveNumber = value;
					RaisePropertyChanged(() => DriveNumber);
				}
			}
		}

		public ICommand ReadSettingsCommand {
			get { return _readSettingsCommand; }
		}

		public ICommand WriteSettingCommand {
			get { return _writeSettingsCommand; }
		}

		public ICommand ImportSettingCommand {
			get { return _importSettingCommand; }
		}

		public ICommand ExportSettingCommand {
			get { return _exportSettingsCommand; }
		}

		public byte AddressCan {
			get { return _addressCan; }
			set {
				if (_addressCan != value) {
					_addressCan = value;
					RaisePropertyChanged(() => AddressCan);
				}
			}
		}

		public IEnumerable<FtRoleViewModel> FtRoles {
			get {
				return _ftRoles;
			}
		}

		public FtRoleViewModel SelectedFtRole {
			get { return _selectedFtRole; }
			set {
				if (_selectedFtRole != value) {
					_selectedFtRole = value;
					RaisePropertyChanged(() => SelectedFtRole);
					//Console.WriteLine("Selected role changed, now: " + _selectedFtRole.Text);
				}
			}
		}
	}
}
