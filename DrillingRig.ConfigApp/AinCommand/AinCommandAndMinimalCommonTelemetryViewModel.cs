using System;
using System.Threading;
using System.Windows.Input;
using AlienJust.Support.Collections;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinCommand;
using DrillingRig.Commands.RtuModbus.CommonTelemetry;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;

namespace DrillingRig.ConfigApp.AinCommand {
	internal class AinCommandAndMinimalCommonTelemetryViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly INotifySendingEnabled _sendingEnabledControl;
		private readonly byte _zeroBasedAinNumber;
		private readonly IAinSettingsStorage _ainSettingsStorage;
		private readonly IAinSettingsStorageUpdatedNotify _storageUpdatedNotify;

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
		private ICommonTelemetry _telemetry;

		public AinCommandAndMinimalCommonTelemetryViewModel(ICommandSenderHost commandSenderHost,
			ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger,
			INotifySendingEnabled sendingEnabledControl, byte zeroBasedAinNumber, IAinSettingsStorage ainSettingsStorage,
			IAinSettingsStorageUpdatedNotify storageUpdatedNotify) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_sendingEnabledControl = sendingEnabledControl;
			_zeroBasedAinNumber = zeroBasedAinNumber;
			_ainSettingsStorage = ainSettingsStorage;
			_storageUpdatedNotify = storageUpdatedNotify;

			_fset = 0;
			_mset = 0;
			_set3 = 0;
			_mmin = -600;
			_mmax = 600;
			_telemetry = null;

			_sendAinCommandOff1 = new RelayCommand(SendAinCmdOff1, () => IsSendingEnabled);
			_sendAinCommandOff2 = new RelayCommand(SendAinCmdOff2, () => IsSendingEnabled);
			_sendAinCommandOff3 = new RelayCommand(SendAinCmdOff3, () => IsSendingEnabled);
			_sendAinCommandRun = new RelayCommand(SendAinCmdRun, () => IsSendingEnabled);
			_sendAinCommandInching1 = new RelayCommand(SendAinCmdInching1, () => IsSendingEnabled);
			_sendAinCommandInching2 = new RelayCommand(SendAinCmdInching2, () => IsSendingEnabled);
			_sendAinCommandReset = new RelayCommand(SendAinCmdReset, () => IsSendingEnabled);
			_sendAinCommandBits = new RelayCommand(SendAinCmdBits, () => IsSendingEnabled);

			_sendingEnabledControl.SendingEnabledChanged += sendingEnabled => { SendingEnabledControlOnSendingEnabledChanged(); };
			_storageUpdatedNotify.AinSettingsUpdated += (ainNumber, settings) => {
				if (ainNumber == 0) SendingEnabledControlOnSendingEnabledChanged();
			};
		}

		public bool IsSendingEnabled => _sendingEnabledControl.IsSendingEnabled && _ainSettingsStorage.GetSettings(0) != null;

		private void SendingEnabledControlOnSendingEnabledChanged() {
			_userInterfaceRoot.Notifier.Notify(() => {
				_sendAinCommandOff1.RaiseCanExecuteChanged();
				_sendAinCommandOff2.RaiseCanExecuteChanged();
				_sendAinCommandOff3.RaiseCanExecuteChanged();
				_sendAinCommandRun.RaiseCanExecuteChanged();
				_sendAinCommandInching1.RaiseCanExecuteChanged();
				_sendAinCommandInching2.RaiseCanExecuteChanged();
				_sendAinCommandReset.RaiseCanExecuteChanged();
				_sendAinCommandBits.RaiseCanExecuteChanged();
				RaisePropertyChanged(() => FsetHz);
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

		public void UpdateCommonTelemetry(ICommonTelemetry telemetry) {
			_telemetry = telemetry;
			// TODO: Raise prop changes
			RaisePropertyChanged(() => McwReceived0);
			RaisePropertyChanged(() => McwReceived1);
			RaisePropertyChanged(() => McwReceived2);
			RaisePropertyChanged(() => McwReceived3);
			RaisePropertyChanged(() => McwReceived4);
			RaisePropertyChanged(() => McwReceived5);
			RaisePropertyChanged(() => McwReceived6);
			RaisePropertyChanged(() => McwReceived7);
			RaisePropertyChanged(() => McwReceived8);
			RaisePropertyChanged(() => McwReceived9);
			RaisePropertyChanged(() => McwReceived10);

			RaisePropertyChanged(() => MswReceived0);
			RaisePropertyChanged(() => MswReceived1);
			RaisePropertyChanged(() => MswReceived2);
			RaisePropertyChanged(() => MswReceived3);
			RaisePropertyChanged(() => MswReceived4);
			RaisePropertyChanged(() => MswReceived5);
			RaisePropertyChanged(() => MswReceived6);
			RaisePropertyChanged(() => MswReceived7);
			RaisePropertyChanged(() => MswReceived8);
			RaisePropertyChanged(() => MswReceived9);
			RaisePropertyChanged(() => MswReceived10);
			RaisePropertyChanged(() => MswReceived11);
			RaisePropertyChanged(() => MswReceived12);
			RaisePropertyChanged(() => MswReceived13);
			RaisePropertyChanged(() => MswReceived14);

			RaisePropertyChanged(() => FsetReceived);
			RaisePropertyChanged(() => MsetReceived);
			RaisePropertyChanged(() => Reserve3Received);
			RaisePropertyChanged(() => MMinReceived);
			RaisePropertyChanged(() => MMaxReceived);
		}

		private void SendCmdWithCommandMode(ushort commandMode) {
			try {
				_logger.Log("Подготовка к отправке команды для АИН");
				if (FsetHz == null)
					throw new Exception("Нет настроек АИН1, необходимо их прочитать, чтобы знать число пар полюсов");
				var fsetToSend = (short)(FsetHz.Value * 10.0);
				var cmd = new FirstAinCommand(_zeroBasedAinNumber, commandMode, fsetToSend, _mset, _set3, _mmin, _mmax);
				_logger.Log("Команда для АИН поставлена в очередь, режим работы: " +
										ModeSetVariantForAinCommandExtensions.FromUshortToText(commandMode));
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
					RaisePropertyChanged(() => FsetHz);
				}
			}
		}

		public double? FsetHz {
			get {
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				if (ain1Settings != null)
					return (int)(_fset / 6.0 * ain1Settings.Np) / 10.0;
				return null;
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

		public bool? McwReceived0 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x01) != 0x00;
		public bool? McwReceived1 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x02) != 0x00;
		public bool? McwReceived2 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x04) != 0x00;
		public bool? McwReceived3 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x08) != 0x00;
		public bool? McwReceived4 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x10) != 0x00;
		public bool? McwReceived5 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x20) != 0x00;
		public bool? McwReceived6 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x40) != 0x00;
		public bool? McwReceived7 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x80) != 0x00;
		public bool? McwReceived8 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x01) != 0x00;
		public bool? McwReceived9 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x02) != 0x00;
		public bool? McwReceived10 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x04) != 0x00;


		public bool? MswReceived0 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x01) != 0x00;
		public bool? MswReceived1 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x02) != 0x00;
		public bool? MswReceived2 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x04) != 0x00;
		public bool? MswReceived3 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x08) != 0x00;
		public bool? MswReceived4 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x10) != 0x00;
		public bool? MswReceived5 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x20) != 0x00;
		public bool? MswReceived6 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x40) != 0x00;
		public bool? MswReceived7 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.Second & 0x80) != 0x00;

		public bool? MswReceived8 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x01) != 0x00;
		public bool? MswReceived9 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x02) != 0x00;
		public bool? MswReceived10 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x04) != 0x00;
		public bool? MswReceived11 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x08) != 0x00;
		public bool? MswReceived12 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x10) != 0x00;
		public bool? MswReceived13 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x20) != 0x00;
		public bool? MswReceived14 => _telemetry == null ? (bool?)null : (_telemetry.Mcw.First & 0x40) != 0x00;

		public double? FsetReceived {
			get {
				if (_telemetry == null) return null;
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				return _telemetry.Fset.HighFirstSignedValue * 60.0 / ain1Settings?.Np;
			}
		}

		public double? MsetReceived => _telemetry?.Mset.HighFirstSignedValue;

		public double? MMinReceived => _telemetry?.MMin.HighFirstSignedValue;

		public double? MMaxReceived => _telemetry?.MMax.HighFirstSignedValue;

		public double? Reserve3Received => _telemetry?.Reserve3.HighFirstSignedValue;
	}
}
