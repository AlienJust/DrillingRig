using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.SystemControl;

namespace DrillingRig.ConfigApp.SystemControl
{
	class SystemControlViewModel : ViewModelBase, IDebugInformationShower
	{
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;
		private readonly ILinkContol _linkControl;

		private readonly RelayCommand _cmdSetBootloader;
		private readonly RelayCommand _cmdRestart;
		private readonly RelayCommand _cmdFlash;

		private IList<byte> _debugBytes;

		public SystemControlViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl, ILinkContol linkControl)
		{
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;
			_linkControl = linkControl;

			_cmdSetBootloader = new RelayCommand(SetBootloader, () => _sendingEnabledControl.IsSendingEnabled);
			_cmdRestart = new RelayCommand(Restart, () => _sendingEnabledControl.IsSendingEnabled);
			_cmdFlash = new RelayCommand(Flash, () => _sendingEnabledControl.IsSendingEnabled);

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool issendingenabled)
		{
			_cmdSetBootloader.RaiseCanExecuteChanged();
			_cmdRestart.RaiseCanExecuteChanged();
			_cmdFlash.RaiseCanExecuteChanged();
		}

		private void SetBootloader()
		{
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


		private void Flash() {
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
								//throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
								_logger.Log("Произошла ошибка при передаче данных, но это нормально");
							}
							_logger.Log("Команда перехода в режим bootloader была отправлена, отключаемся от COM-порта");
							_linkControl.CloseComPort();
							var psi = new ProcessStartInfo("flash.bat");
							var process = new Process {StartInfo = psi};
							process.Start();
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

		private void Restart()
		{
			var cmd = new RestartCommand();
			try
			{
				_logger.Log(cmd.Name);
				_logger.Log("Команда <" + cmd.Name + "> поставлена в очередь");
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
							_logger.Log("Команда <" + cmd.Name + "> была отправлена");
						}
						catch (Exception ex)
						{
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex)
			{
				_logger.Log("Не удалось поставить команду <" + cmd.Name + "> в очередь: " + ex.Message);
			}
		}


		public ICommand CmdSetBootloader
		{
			get { return _cmdSetBootloader; }
		}

		public RelayCommand CmdRestart {
			get { return _cmdRestart; }
		}

		public void ShowBytes(IList<byte> bytes) {
			_debugBytes = bytes;
			RaisePropertyChanged(() => B01);
			RaisePropertyChanged(() => B02);
			RaisePropertyChanged(() => B03);
			RaisePropertyChanged(() => B04);

			RaisePropertyChanged(() => B11);
			RaisePropertyChanged(() => B12);
			RaisePropertyChanged(() => B13);
			RaisePropertyChanged(() => B14);

			RaisePropertyChanged(() => B21);
			RaisePropertyChanged(() => B22);
			RaisePropertyChanged(() => B23);
			RaisePropertyChanged(() => B24);

			RaisePropertyChanged(() => B31);
			RaisePropertyChanged(() => B32);
			RaisePropertyChanged(() => B33);
			RaisePropertyChanged(() => B34);

			RaisePropertyChanged(() => B41);
			RaisePropertyChanged(() => B42);
			RaisePropertyChanged(() => B43);
			RaisePropertyChanged(() => B44);

			RaisePropertyChanged(() => B51);
			RaisePropertyChanged(() => B52);
			RaisePropertyChanged(() => B53);
			RaisePropertyChanged(() => B54);

			RaisePropertyChanged(() => B61);
			RaisePropertyChanged(() => B62);
			RaisePropertyChanged(() => B63);
			RaisePropertyChanged(() => B64);

			RaisePropertyChanged(() => B71);
			RaisePropertyChanged(() => B72);
			RaisePropertyChanged(() => B73);
			RaisePropertyChanged(() => B74);
		}

		private string GetByteText(int zeroBasedRow, int oneBasedCol) {
			try {
				var b = _debugBytes[zeroBasedRow*4 + oneBasedCol - 1];
				return b.ToString("X2") + " (" + b + ")";
			}
			catch {
				return "----";
			}
		}

		public string B01 { get { return GetByteText(0, 1); } }
		public string B02 { get { return GetByteText(0, 2); } }
		public string B03 { get { return GetByteText(0, 3); } }
		public string B04 { get { return GetByteText(0, 4); } }

		public string B11 { get { return GetByteText(1, 1); } }
		public string B12 { get { return GetByteText(1, 2); } }
		public string B13 { get { return GetByteText(1, 3); } }
		public string B14 { get { return GetByteText(1, 4); } }

		public string B21 { get { return GetByteText(2, 1); } }
		public string B22 { get { return GetByteText(2, 2); } }
		public string B23 { get { return GetByteText(2, 3); } }
		public string B24 { get { return GetByteText(2, 4); } }

		public string B31 { get { return GetByteText(3, 1); } }
		public string B32 { get { return GetByteText(3, 2); } }
		public string B33 { get { return GetByteText(3, 3); } }
		public string B34 { get { return GetByteText(3, 4); } }

		public string B41 { get { return GetByteText(4, 1); } }
		public string B42 { get { return GetByteText(4, 2); } }
		public string B43 { get { return GetByteText(4, 3); } }
		public string B44 { get { return GetByteText(4, 4); } }

		public string B51 { get { return GetByteText(5, 1); } }
		public string B52 { get { return GetByteText(5, 2); } }
		public string B53 { get { return GetByteText(5, 3); } }
		public string B54 { get { return GetByteText(5, 4); } }

		public string B61 { get { return GetByteText(6, 1); } }
		public string B62 { get { return GetByteText(6, 2); } }
		public string B63 { get { return GetByteText(6, 3); } }
		public string B64 { get { return GetByteText(6, 4); } }

		public string B71 { get { return GetByteText(7, 1); } }
		public string B72 { get { return GetByteText(7, 2); } }
		public string B73 { get { return GetByteText(7, 3); } }
		public string B74 { get { return GetByteText(7, 4); } }

		public RelayCommand CmdFlash {
			get { return _cmdFlash; }
		}
	}
}
