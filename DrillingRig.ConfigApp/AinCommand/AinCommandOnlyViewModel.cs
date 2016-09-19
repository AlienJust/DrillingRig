using System;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinCommand;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;

namespace DrillingRig.ConfigApp.AinCommand {
	internal class AinCommandOnlyViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly INotifySendingEnabled _sendingEnabledControl;
		private readonly byte _zeroBasedAinNumber;

		private readonly RelayCommand _sendAinCommandOff1;
		private readonly RelayCommand _sendAinCommandOff2;
		private readonly RelayCommand _sendAinCommandOff3;
		private readonly RelayCommand _sendAinCommandRun;
		private readonly RelayCommand _sendAinCommandInching1;
		private readonly RelayCommand _sendAinCommandInching2;
		private readonly RelayCommand _sendAinCommandReset;
		private readonly RelayCommand _sendAinCommandBits;

		private short _fset;
		private short _mset;
		private short _set3;
		private short _mmin;
		private short _mmax;

		public AinCommandOnlyViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, INotifySendingEnabled sendingEnabledControl, byte zeroBasedAinNumber) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_sendingEnabledControl = sendingEnabledControl;
			_zeroBasedAinNumber = zeroBasedAinNumber;

			_fset = 0;
			_mset = 0;
			_set3 = 0;
			_mmin = -600;
			_mmax = 600;

			_sendAinCommandOff1 = new RelayCommand(SendAinCmdOff1, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandOff2 = new RelayCommand(SendAinCmdOff2, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandOff3 = new RelayCommand(SendAinCmdOff3, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandRun = new RelayCommand(SendAinCmdRun, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandInching1 = new RelayCommand(SendAinCmdInching1, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandInching2 = new RelayCommand(SendAinCmdInching2, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandReset = new RelayCommand(SendAinCmdReset, () => _sendingEnabledControl.IsSendingEnabled);
			_sendAinCommandBits = new RelayCommand(SendAinCmdBits, () => _sendingEnabledControl.IsSendingEnabled);

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool isSendingEnabled) {
			_userInterfaceRoot.Notifier.Notify(() => {
				_sendAinCommandOff1.RaiseCanExecuteChanged();
				_sendAinCommandOff2.RaiseCanExecuteChanged();
				_sendAinCommandOff3.RaiseCanExecuteChanged();
				_sendAinCommandRun.RaiseCanExecuteChanged();
				_sendAinCommandInching1.RaiseCanExecuteChanged();
				_sendAinCommandInching2.RaiseCanExecuteChanged();
				_sendAinCommandReset.RaiseCanExecuteChanged();
				_sendAinCommandBits.RaiseCanExecuteChanged();
			});
		}

		private void SendAinCmdOff1() {
			SendCmdWithCommandMode(ModeSetVariantForAinCommand.Off1.ToUshort());
		}

		private void SendAinCmdOff2() {
			SendCmdWithCommandMode(ModeSetVariantForAinCommand.Off2.ToUshort());
		}

		private void SendAinCmdOff3() {
			SendCmdWithCommandMode(ModeSetVariantForAinCommand.Off3.ToUshort());
		}

		private void SendAinCmdRun() {
			SendCmdWithCommandMode(ModeSetVariantForAinCommand.Run.ToUshort());
		}

		private void SendAinCmdInching1() {
			SendCmdWithCommandMode(ModeSetVariantForAinCommand.Inching1.ToUshort());
		}

		private void SendAinCmdInching2() {
			SendCmdWithCommandMode(ModeSetVariantForAinCommand.Inching2.ToUshort());
		}

		private void SendAinCmdReset() {
			SendCmdWithCommandMode(ModeSetVariantForAinCommand.Reset.ToUshort());
		}
		private void SendAinCmdBits() {
			ushort commandMode = 0;
			if (Off1) commandMode += 0x0001;
			if (Off2) commandMode += 0x0002;
			if (Off3) commandMode += 0x0004;
			if (Run) commandMode += 0x0008;
			if (RampOutZero) commandMode += 0x0010;
			if (RampHold) commandMode += 0x0020;
			if (RampInZero) commandMode += 0x0040;
			if (Reset) commandMode += 0x0080;
			if (Inching1) commandMode += 0x0100;
			if (Inching2) commandMode += 0x0200;
			if (Remote) commandMode += 0x0400;
			_logger.Log("Режим работы в составе команды АИН = 0x" + commandMode.ToString("X4"));
			SendCmdWithCommandMode(commandMode);
		}

		private void SendCmdWithCommandMode(ushort commandMode)
		{
			try {
				_logger.Log("Подготовка к отправке команды для АИН");
				var cmd = new FirstAinCommand(_zeroBasedAinNumber, commandMode, _fset, _mset, _set3, _mmin, _mmax);
				_logger.Log("Команда для АИН поставлена в очередь, режим работы: " + ModeSetVariantForAinCommandExtensions.FromUshortToText(commandMode));
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


		public bool Off1 { get; set; }
		public bool Off2 { get; set; }
		public bool Off3 { get; set; }
		public bool Run { get; set; }
		public bool RampOutZero { get; set; }
		public bool RampHold { get; set; }
		public bool RampInZero { get; set; }
		public bool Reset { get; set; }
		public bool Inching1 { get; set; }
		public bool Inching2 { get; set; }
		public bool Remote { get; set; }
		
		public ICommand SendAinCommandOff1 => _sendAinCommandOff1;

		public ICommand SendAinCommandOff2 => _sendAinCommandOff2;

		public ICommand SendAinCommandOff3 => _sendAinCommandOff3;

		public ICommand SendAinCommandRun => _sendAinCommandRun;


		public ICommand SendAinCommandInching1 => _sendAinCommandInching1;

		public ICommand SendAinCommandInching2 => _sendAinCommandInching2;


		public ICommand SendAinCommandReset => _sendAinCommandReset;

		public RelayCommand SendAinCommandBits => _sendAinCommandBits;
	}
}
