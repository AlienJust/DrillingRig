using System;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.EngineSettings;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
//using EngineSettingsSimple = DrillingRig.ConfigApp.AppControl.EngineSettingsStorage.EngineSettingsSimple;

namespace DrillingRig.ConfigApp.EngineSettings {
	class EngineSettingsViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;

		private readonly RelayCommand _readSettingsCommand;
		private readonly RelayCommand _writeSettingsCommand;

		private ushort? _inom;
		private ushort? _nnom;
		private ushort? _nmax;
		private ushort? _pnom;
		private ushort? _cosFi;
		private ushort? _eff;
		private ushort? _mass;
		private ushort? _mmM;
		private ushort? _height;

		private uint? _i2Tmax;
		private ushort? _icontinious;
		private ushort? _zeroF;

		public EngineSettingsViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;

			_readSettingsCommand = new RelayCommand(ReadSettings, () => _sendingEnabledControl.IsSendingEnabled);
			_writeSettingsCommand = new RelayCommand(WriteSettings, () => _sendingEnabledControl.IsSendingEnabled);

			ImportSettingsCommand = new RelayCommand(ImportSettings);
			ExportSettingsCommand = new RelayCommand(ExportSettings);


			_inom = null;
			_nnom = null;
			_nmax = null;
			_pnom = null;
			_cosFi = null;
			_eff = null;
			_mass = null;
			_mmM = null;
			_height = null;

			_i2Tmax = null;
			_icontinious = null;
			_zeroF = null;

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool issendingenabled) {
			_readSettingsCommand.RaiseCanExecuteChanged();
			_writeSettingsCommand.RaiseCanExecuteChanged();
		}

		private void ImportSettings() {
			_logger.Log("Начало импорта настроек");
			throw new NotImplementedException("Not implemented yet");
		}

		private void ExportSettings() {
			_logger.Log("Начало экспорта настроек");
			throw new NotImplementedException("Not implemented yet");
		}

		private void WriteSettings() {
			try {
				_logger.Log("Подготовка к записи настроек АИН");
				IEngineSettings engineSettings;
				try {
					engineSettings = new EngineSettingsSimple {
						Inom = Inom.Value,
						Nnom = Nnom.Value,
						Nmax = Nmax.Value,
						Pnom = Pnom.Value,
						CosFi = CosFi.Value,
						Eff = Eff.Value,
						Mass = Mass.Value,
						MmM = MmM.Value,
						Height = Height.Value,

						I2Tmax = I2Tmax.Value,
						Icontinious = Icontinious.Value,
						ZeroF = ZeroF.Value
					};
				}
				catch (Exception ex) {
					throw new Exception("убедитесь, что все значения настроек заполнены", ex);
				}
				var cmd = new WriteEngineSettingsCommand(engineSettings);

				_logger.Log("Команда записи настроек двигателя поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes);
								if (result) {
									_logger.Log("Настройки двигателя успешно записаны");
								}
								else {
									throw new Exception("странно, флаг записи результата = False");
								}
							}
							catch (Exception exx) {
								// TODO: log exception about error on answer parsing
								throw new Exception("ошибка при разборе ответа: " + exx.Message, exx);
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
				_logger.Log("Подготовка к чтению настроек АИН");

				var cmd = new ReadEngineSettingsCommand();

				_logger.Log("Команда чтения настроек двигателя поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes);
								_userInterfaceRoot.Notifier.Notify(() => {
									Inom = result.Inom;
									Nnom = result.Nnom;
									Nmax = result.Nmax;
									Pnom = result.Pnom;
									CosFi = result.CosFi;
									Eff = result.Eff;
									Mass = result.Mass;
									MmM = result.MmM;
									Height = result.Height;

									I2Tmax = result.I2Tmax;
									Icontinious = result.Icontinious;
									ZeroF = result.ZeroF;
								});
								_logger.Log("Настройки двигателя успешно прочитаны");
							}
							catch (Exception exx) {
								// TODO: log exception about error on answer parsing
								throw new Exception("ошибка при разборе ответа: " + exx.Message, exx);
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


		public ICommand ReadSettingsCommand => _readSettingsCommand;

		public ICommand WriteSettingsCommand => _writeSettingsCommand;

		public ICommand ImportSettingsCommand { get; }

		public ICommand ExportSettingsCommand { get; }

		public ushort? Inom {
			get { return _inom; }
			set { if (_inom != value) { _inom = value; RaisePropertyChanged(() => Inom); } }
		}

		public ushort? Nnom {
			get { return _nnom; }
			set { if (_nnom != value) { _nnom = value; RaisePropertyChanged(() => Nnom); } }
		}

		public ushort? Nmax {
			get { return _nmax; }
			set { if (_nmax != value) { _nmax = value; RaisePropertyChanged(() => Nmax); } }
		}

		public ushort? Pnom {
			get { return _pnom; }
			set { if (_pnom != value) { _pnom = value; RaisePropertyChanged(() => Pnom); } }
		}

		public ushort? CosFi {
			get { return _cosFi; }
			set { if (_cosFi != value) { _cosFi = value; RaisePropertyChanged(() => CosFi); } }
		}

		public ushort? Eff {
			get { return _eff; }
			set { if (_eff != value) { _eff = value; RaisePropertyChanged(() => Eff); } }
		}

		public ushort? Mass {
			get { return _mass; }
			set { if (_mass != value) { _mass = value; RaisePropertyChanged(() => Mass); } }
		}

		public ushort? MmM {
			get { return _mmM; }
			set { if (_mmM != value) { _mmM = value; RaisePropertyChanged(() => MmM); } }
		}

		public ushort? Height {
			get { return _height; }
			set { if (_height != value) { _height = value; RaisePropertyChanged(() => Height); } }
		}


		public uint? I2Tmax {
			get { return _i2Tmax; }
			set { if (_i2Tmax != value) { _i2Tmax = value; RaisePropertyChanged(() => I2Tmax); } }
		}
		
		public ushort? Icontinious {
			get { return _icontinious; }
			set { if (_icontinious != value) { _icontinious = value; RaisePropertyChanged(() => Icontinious); } }
		}
		
		public ushort? ZeroF {
			get { return _zeroF; }
			set { if (_zeroF != value) { _zeroF = value; RaisePropertyChanged(() => ZeroF); } }
		}
	}
}
