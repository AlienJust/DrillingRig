using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.AinCommand;

namespace DrillingRig.ConfigApp.AinCommand {
	internal class AinCommandViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;
		private readonly byte _zeroBasedAinNumber;

		private readonly RelayCommand _sendAinCommandOff1;
		private readonly RelayCommand _sendAinCommandOff2;
		private readonly RelayCommand _sendAinCommandOff3;
		private readonly RelayCommand _sendAinCommandRun;
		private readonly RelayCommand _sendAinCommandInching1;
		private readonly RelayCommand _sendAinCommandInching2;
		private readonly RelayCommand _sendAinCommandReset;


		private short _fset;
		private short _mset;
		private short _set3;
		private short _mmin;
		private short _mmax;

		public AinCommandViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl, byte zeroBasedAinNumber) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;
			_zeroBasedAinNumber = zeroBasedAinNumber;

			_fset = 0;
			_mset = 0;
			_set3 = 0;
			_mmin = 0;
			_mmax = 0;

			_sendAinCommandOff1 = new RelayCommand(SendAinCmdOff1, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandOff2 = new RelayCommand(SendAinCmdOff2, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandOff3 = new RelayCommand(SendAinCmdOff3, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandRun = new RelayCommand(SendAinCmdRun, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandInching1 = new RelayCommand(SendAinCmdInching1, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandInching2 = new RelayCommand(SendAinCmdInching2, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandReset = new RelayCommand(SendAinCmdReset, () => _sendingEnabledControl.IsSendingEnabled);

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool issendingenabled) {
			_sendAinCommandOff1.RaiseCanExecuteChanged();
			_sendAinCommandOff2.RaiseCanExecuteChanged();
			_sendAinCommandOff3.RaiseCanExecuteChanged();
			_sendAinCommandRun.RaiseCanExecuteChanged();
			_sendAinCommandInching1.RaiseCanExecuteChanged();
			_sendAinCommandInching2.RaiseCanExecuteChanged();
			_sendAinCommandReset.RaiseCanExecuteChanged();
		}

		private void SendAinCmdOff1() {
			SendCmdWithCommandMode(new ModeSetVariantForAinCommandOff1ViewModel());
		}

		private void SendAinCmdOff2() {
			SendCmdWithCommandMode(new ModeSetVariantForAinCommandOff2ViewModel());
		}

		private void SendAinCmdOff3() {
			SendCmdWithCommandMode(new ModeSetVariantForAinCommandOff3ViewModel());
		}

		private void SendAinCmdRun() {
			SendCmdWithCommandMode(new ModeSetVariantForAinCommandRunViewModel());
		}

		private void SendAinCmdInching1() {
			SendCmdWithCommandMode(new ModeSetVariantForAinCommandInching1ViewModel());
		}

		private void SendAinCmdInching2() {
			SendCmdWithCommandMode(new ModeSetVariantForAinCommandInching2ViewModel());
		}

		private void SendAinCmdReset() {
			SendCmdWithCommandMode(new ModeSetVariantForAinCommandResetViewModel());
		}

		private void SendCmdWithCommandMode(IModeSetVariantForAinCommandViewModel commandMode) {
			try {
				_logger.Log("Подготовка к отправке команды для АИН");
				var cmd = new FirstAinCommand(_zeroBasedAinNumber, commandMode.Value, _fset, _mset, _set3, _mmin, _mmax);

				_logger.Log("Команда для АИН поставлена в очередь, режим работы: " + commandMode.Name);
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
								var result = cmd.GetResult(bytes); // result is unused but GetResult can throw exception
								_logger.Log("Команда для АИН была отправлена, получен хороший ответ");
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
				_logger.Log("Не удалось поставить команду для АИН в очередь: " + ex.Message);
			}
		}

		public short Fset {
			get { return _fset; }
			set {
				if (_fset != value) {
					_fset = value;
					RaisePropertyChanged(() => Fset);
				}
			}
		}

		public short Mset {
			get { return _mset; }
			set {
				if (_mset != value) {
					_mset = value;
					RaisePropertyChanged(() => Mset);
				}
			}
		}

		public short Set3 {
			get { return _set3; }
			set {
				if (_set3 != value) {
					_set3 = value;
					RaisePropertyChanged(() => Set3);
				}
			}
		}

		public short Mmin {
			get { return _mmin; }
			set {
				if (_mmin != value) {
					_mmin = value;
					RaisePropertyChanged(() => Mmin);
				}
			}
		}

		public short Mmax {
			get { return _mmax; }
			set {
				if (_mmax != value) {
					_mmax = value;
					RaisePropertyChanged(() => Mmax);
				}
			}
		}


		public ICommand SendAinCommandOff1 {
			get { return _sendAinCommandOff1; }
		}

		public ICommand SendAinCommandOff2 {
			get { return _sendAinCommandOff2; }
		}

		public ICommand SendAinCommandOff3 {
			get { return _sendAinCommandOff3; }
		}

		public ICommand SendAinCommandRun {
			get { return _sendAinCommandRun; }
		}


		public ICommand SendAinCommandInching1 {
			get { return _sendAinCommandInching1; }
		}

		public ICommand SendAinCommandInching2 {
			get { return _sendAinCommandInching2; }
		}


		public ICommand SendAinCommandReset {
			get { return _sendAinCommandReset; }
		}
	}
}
