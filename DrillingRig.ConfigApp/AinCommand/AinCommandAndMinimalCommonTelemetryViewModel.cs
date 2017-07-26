using System;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinCommand;
using DrillingRig.Commands.RtuModbus.CommonTelemetry;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

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

		private readonly RelayCommand _cmdSub10FsetHz;
		private readonly RelayCommand _cmdSub01FsetHz;
		private readonly RelayCommand _cmdSetFsetHzToZero;
		private readonly RelayCommand _cmdAdd01FsetHz;
		private readonly RelayCommand _cmdAdd10FsetHz;


		/// <summary>
		/// Частота в оборотах в минуту, которую пользователь вводит через окно
		/// </summary>
		private decimal? _fset;
		private short _mset;
		private short _set3;
		private short _mmin;
		private short _mmax;
		private ICommonTelemetry _telemetry;
		private short? _mMinMaxAbs;

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


			_cmdSub10FsetHz = new RelayCommand(() => {
				if (FsetHz.HasValue) FsetHz -= 1;
			}, () => IsSendingEnabled);
			_cmdSub01FsetHz = new RelayCommand(() => {
				if (FsetHz.HasValue) FsetHz -= 0.1m;
			}, () => IsSendingEnabled);
			_cmdSetFsetHzToZero = new RelayCommand(() => FsetHz = 0.0m, () => IsSendingEnabled);
			_cmdAdd01FsetHz = new RelayCommand(() => {
				if (FsetHz.HasValue) FsetHz += 0.1m;
			}, () => IsSendingEnabled);
			_cmdAdd10FsetHz = new RelayCommand(() => {
				if (FsetHz.HasValue) FsetHz += 1.0m;
			}, () => IsSendingEnabled);



			_sendingEnabledControl.SendingEnabledChanged += sendingEnabled => { SendingEnabledControlOnSendingEnabledChanged(); };

			_storageUpdatedNotify.AinSettingsUpdated += (ainNumber, settings) => {
				if (ainNumber == 0) {
					SendingEnabledControlOnSendingEnabledChanged();
					RaisePropertyChanged(() => Fset);
					RaisePropertyChanged(() => FsetHz);
					RaisePropertyChanged(() => FsetSmallChange);
					RaisePropertyChanged(() => FsetSmallChangeOrOne);
					RaisePropertyChanged(() => FsetMax);
					RaisePropertyChanged(() => FsetMin);
					RaisePropertyChanged(() => NegativeMaximumFreqSet);
					RaisePropertyChanged(() => PositiveMaximumFreqSet);
					RaisePropertyChanged(() => TickFreqSet);
				}
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

				_cmdSub10FsetHz.RaiseCanExecuteChanged();
				_cmdSub01FsetHz.RaiseCanExecuteChanged();
				_cmdSetFsetHzToZero.RaiseCanExecuteChanged();
				_cmdAdd01FsetHz.RaiseCanExecuteChanged();
				_cmdAdd10FsetHz.RaiseCanExecuteChanged();

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
				var fsetToSend = (short)(FsetHz.Value * 10.0m);
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
								cmd.GetResult(bytes); // result is unused but GetResult can throw exception
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

		/// <summary>
		/// Частота в об/мин, перед отправкой преобразуется электрическую с точностью до дГц (0.1 Гц), 0.1 Гц = 0.3 об/мин (когда две пары полюсов)
		/// </summary>
		public decimal? Fset {
			get => _fset;
			set {
				if (_fset != value) {
					_fset = value;
					RaisePropertyChanged(() => Fset);
					RaisePropertyChanged(() => FsetHz);
				}
			}
		}

		public decimal? FsetHz {
			get {
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				if (ain1Settings != null && _fset.HasValue)
					return (int)(_fset * ain1Settings.Np / 0.6m) / 100.0m; // т.к. могу задавать частоту с точностью 1 сГц (0.01 Гц) - происходит округление до ближайшего минимального значения кратного 0.01
				return null;
			}
			set {
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				if (ain1Settings != null && value != null) {
					_fset = Math.Round(value.Value * 60.0m / ain1Settings.Np);
					RaisePropertyChanged(() => Fset);
					RaisePropertyChanged(() => FsetHz);
				}
				else Fset = null;
			}
		}

		/// <summary>
		/// Полученная скорость в об/мин
		/// </summary>
		public double? FsetReceived {
			get {
				if (_telemetry == null) return null;
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				return _telemetry.Fset.HighFirstSignedValue * 0.6 / ain1Settings?.Np; // полученная из телеметрии электрическая частота указана в 0.01Гц
			}
		}

		/// <summary>
		/// Минимальное изменение числа оборотов, которое соответсвует изменению частоты на 0.01Гц, однако если оно меньше 1.0, то будет возвращена единичка
		/// </summary>
		public double? FsetSmallChangeOrOne => FsetSmallChange < 1.0 ? 1.0 : FsetSmallChange;

		/// <summary>
		/// Минимальное изменение числа оборотов/мин, которое соответсвует изменению частоты на 0.01Гц
		/// </summary>
		public double? FsetSmallChange {
			get {
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				return 0.6 / ain1Settings?.Np;
			}
		}

		/// <summary>
		/// Положительное ограничение на скорость в герцах (20.01)
		/// </summary>
		public decimal? PositiveMaximumFreqSet {
			get {
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				return ain1Settings?.Fmax;
			}
		}

		public decimal? NegativeMaximumFreqSet {
			get {
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				return -1.0m * ain1Settings?.Fmax;
			}
		}

		public decimal? TickFreqSet => PositiveMaximumFreqSet / 5.0m;

		/// <summary>
		/// Положительное ограничение на скорость в об/мин (20.01)
		/// </summary>
		public decimal? FsetMax {
			get {
				var ain1Settings = _ainSettingsStorage.GetSettings(0);

				return PositiveMaximumFreqSet * 60.0m / ain1Settings?.Np;
			}
		}



		public decimal? FsetMin {
			get {
				var ain1Settings = _ainSettingsStorage.GetSettings(0);
				return PositiveMaximumFreqSet * -60.0m / ain1Settings?.Np;
			}
		}


		public short Mset {
			get => _mset;
			set {
				if (_mset != value) {
					_mset = value;
					RaisePropertyChanged(() => Mset);
				}
			}
		}

		public short Set3 {
			get => _set3;
			set {
				if (_set3 != value) {
					_set3 = value;
					RaisePropertyChanged(() => Set3);
				}
			}
		}

		public short Mmin {
			get => _mmin;
			set {
				if (_mmin != value) {
					_mmin = value;
					RaisePropertyChanged(() => Mmin);
					RaisePropertyChanged(() => MMinMaxAbs);
				}
			}
		}

		public short Mmax {
			get => _mmax;
			set {
				if (_mmax != value) {
					_mmax = value;
					RaisePropertyChanged(() => Mmax);
					RaisePropertyChanged(() => MMinMaxAbs);
				}
			}
		}

		public short? MMinMaxAbs {
			get {
				if (_mmin < 0 && _mmax > 0 && _mmin + _mmax == 0 || _mmin == 0 && _mmax == 0) {
					return _mmax;
				}
				return null;
			}
			set {
				if (value.HasValue) {
					Mmin = (short)-value.Value;
					Mmax = value.Value;
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

		public ICommand CmdSub10FsetHz => _cmdSub10FsetHz;
		public ICommand CmdSub01FsetHz => _cmdSub01FsetHz;
		public ICommand CmdSetFsetHzToZero => _cmdSetFsetHzToZero;
		public ICommand CmdAdd01FsetHz => _cmdAdd01FsetHz;
		public ICommand CmdAdd10FsetHz => _cmdAdd10FsetHz;

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


		public bool? MswReceived0 => _telemetry == null ? (bool?)null : (_telemetry.Msw.Second & 0x01) != 0x00;
		public bool? MswReceived1 => _telemetry == null ? (bool?)null : (_telemetry.Msw.Second & 0x02) != 0x00;
		public bool? MswReceived2 => _telemetry == null ? (bool?)null : (_telemetry.Msw.Second & 0x04) != 0x00;
		public bool? MswReceived3 => _telemetry == null ? (bool?)null : (_telemetry.Msw.Second & 0x08) != 0x00;
		public bool? MswReceived4 => _telemetry == null ? (bool?)null : (_telemetry.Msw.Second & 0x10) != 0x00;
		public bool? MswReceived5 => _telemetry == null ? (bool?)null : (_telemetry.Msw.Second & 0x20) != 0x00;
		public bool? MswReceived6 => _telemetry == null ? (bool?)null : (_telemetry.Msw.Second & 0x40) != 0x00;
		public bool? MswReceived7 => _telemetry == null ? (bool?)null : (_telemetry.Msw.Second & 0x80) != 0x00;

		public bool? MswReceived8 => _telemetry == null ? (bool?)null : (_telemetry.Msw.First & 0x01) != 0x00;
		public bool? MswReceived9 => _telemetry == null ? (bool?)null : (_telemetry.Msw.First & 0x02) != 0x00;
		public bool? MswReceived10 => _telemetry == null ? (bool?)null : (_telemetry.Msw.First & 0x04) != 0x00;
		public bool? MswReceived11 => _telemetry == null ? (bool?)null : (_telemetry.Msw.First & 0x08) != 0x00;
		public bool? MswReceived12 => _telemetry == null ? (bool?)null : (_telemetry.Msw.First & 0x10) != 0x00;
		public bool? MswReceived13 => _telemetry == null ? (bool?)null : (_telemetry.Msw.First & 0x20) != 0x00;
		public bool? MswReceived14 => _telemetry == null ? (bool?)null : (_telemetry.Msw.First & 0x40) != 0x00;


		public double? MsetReceived => _telemetry?.Mset.HighFirstSignedValue;
		public double? MMinReceived => _telemetry?.MMin.HighFirstSignedValue;
		public double? MMaxReceived => _telemetry?.MMax.HighFirstSignedValue;
		public double? Reserve3Received => _telemetry?.Reserve3.HighFirstSignedValue;
	}
}
