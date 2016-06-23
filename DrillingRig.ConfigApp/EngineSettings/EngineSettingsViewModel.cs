using System;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.EngineSettings;

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

		private ushort? _icontinious;
		private uint? _i2Tmax;
		private ushort? _mnom;
		private uint? _pnom;
		private ushort? _zeroF;
		private byte? _ks;
		
		public EngineSettingsViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl)
		{
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


			_icontinious = null;
			_i2Tmax = null;
			_mnom = null;
			_pnom = null;
			_zeroF = null;
			_ks = null;

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
						Icontinious = Icontinious.Value,
						I2Tmax = I2Tmax.Value,
						Mnom = Mnom.Value,
						Pnom = Pnom.Value,
						ZeroF = ZeroF.Value,
						Ks = Ks.Value,
					};
				}
				catch (Exception ex)
				{
					throw new Exception("убедитесь, что все значения настроек заполнены", ex);
				}
				var cmd = new WriteEngineSettingsCommand(engineSettings);

				_logger.Log("Команда записи настроек двигателя поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() =>
					{
						try
						{
							if (exception != null)
							{
								throw new Exception("ошибка при передаче данных: " + exception.Message, exception);
							}

							try
							{
								var result = cmd.GetResult(bytes);
								if (result) {
									_logger.Log("Настройки двигателя успешно записаны");
								}
								else {
									throw new Exception("странно, флаг записи результата = False");
								}
							}
							catch (Exception exx)
							{
								// TODO: log exception about error on answer parsing
								throw new Exception("ошибка при разборе ответа: " + exx.Message, exx);
							}
						}
						catch (Exception ex)
						{
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду записи настроек БС-Ethernet в очередь: " + ex.Message);
			}
		}

		private void ReadSettings() {
			try
			{
				_logger.Log("Подготовка к чтению настроек АИН");

				var cmd = new ReadEngineSettingsCommand();

				_logger.Log("Команда чтения настроек двигателя поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() =>
					{
						try
						{
							if (exception != null)
							{
								throw new Exception("ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes);
								_userInterfaceRoot.Notifier.Notify(() => {
									Icontinious = result.Icontinious;
									I2Tmax = result.I2Tmax;
									Mnom = result.Mnom;
									Pnom = result.Pnom;
									ZeroF = result.ZeroF;
									Ks = result.Ks;
								});
								_logger.Log("Настройки двигателя успешно прочитаны");
							}
							catch (Exception exx)
							{
								// TODO: log exception about error on answer parsing
								throw new Exception("ошибка при разборе ответа: " + exx.Message, exx);
							}
						}
						catch (Exception ex)
						{
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex)
			{
				_logger.Log("Не удалось поставить команду чтения настроек БС-Ethernet в очередь: " + ex.Message);
			}
		}


		public ICommand ReadSettingsCommand => _readSettingsCommand;

		public ICommand WriteSettingsCommand => _writeSettingsCommand;

		public ICommand ImportSettingsCommand { get; }

		public ICommand ExportSettingsCommand { get; }

		public ushort? Icontinious {
			get { return _icontinious; }
			set { if (_icontinious != value) { _icontinious = value; RaisePropertyChanged(() => Icontinious); } }
		}

		public uint? I2Tmax {
			get { return _i2Tmax; }
			set { if (_i2Tmax != value) { _i2Tmax = value; RaisePropertyChanged(() => I2Tmax); } }
		}

		public ushort? Mnom {
			get { return _mnom; }
			set { if (_mnom != value) { _mnom = value; RaisePropertyChanged(() => Mnom); } }
		}

		public uint? Pnom {
			get { return _pnom; }
			set { if (_pnom != value) { _pnom = value; RaisePropertyChanged(() => Pnom); } }
		}

		public ushort? ZeroF {
			get { return _zeroF; }
			set { if (_zeroF != value) { _zeroF = value; RaisePropertyChanged(() => ZeroF); } }
		}

		public byte? Ks {
			get { return _ks; }
			set { if (_ks != value) { _ks = value; RaisePropertyChanged(() => Ks); } }
		}
	}
}
