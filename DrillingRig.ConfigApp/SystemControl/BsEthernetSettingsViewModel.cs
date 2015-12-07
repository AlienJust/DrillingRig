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
using DrillingRig.Commands.SystemControl;
using DrillingRig.ConfigApp.BsEthernetSettings;

namespace DrillingRig.ConfigApp.SystemControl
{
	class SystemControlViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;

		private readonly RelayCommand _cmdSetBootloader;

		public SystemControlViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl)
		{
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;

			_cmdSetBootloader = new RelayCommand(ReadSettings, () => _sendingEnabledControl.IsSendingEnabled);
			
			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool issendingenabled) {
			_cmdSetBootloader.RaiseCanExecuteChanged();
		}

		private void ReadSettings() {
			try
			{
				_logger.Log("Переход в режим bootloader");

				var cmd = new SetBootloaderCommand();

				_logger.Log("Команда перехода в режим bootloader поставлена в очередь");
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
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
							}
							_logger.Log("Команда перехода в режим bootloader была отправлена");
						}
						catch (Exception ex)
						{
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex)
			{
				_logger.Log("Не удалось поставить команду перехода в режим bootloader в очередь: " + ex.Message);
			}
		}


		public ICommand CmdSetBootloader {
			get { return _cmdSetBootloader; }
		}
	}
}
