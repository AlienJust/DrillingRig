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

		private readonly RelayCommand _sendAinCommand;
		
		private readonly List<IModeSetVariantForAinCommandViewModel> _availableModesetVariants;
		private IModeSetVariantForAinCommandViewModel _selectedModesetVariant;

		private ushort _fset;
		private ushort _mset;
		private ushort _set3;
		private ushort _mmin;
		private ushort _mmax;

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
			_availableModesetVariants = new List<IModeSetVariantForAinCommandViewModel> {
				new ModeSetVariantForAinCommandOff1ViewModel(),
				new ModeSetVariantForAinCommandOff2ViewModel(),
				new ModeSetVariantForAinCommandOff3ViewModel(),
				new ModeSetVariantForAinCommandRunViewModel(),
				new ModeSetVariantForAinCommandInching1ViewModel(),
				new ModeSetVariantForAinCommandInching2ViewModel(),
				new ModeSetVariantForAinCommandResetViewModel()
			};

			// TODO: fill modeset available variants

			_selectedModesetVariant = _availableModesetVariants.First();

			_sendAinCommand = new RelayCommand(SendAinCmd, () => _sendingEnabledControl.IsSendingEnabled);

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool issendingenabled) {
			_sendAinCommand.RaiseCanExecuteChanged();
		}

		private void SendAinCmd() {
			try {
				_logger.Log("Подготовка к отправке команды для АИН");
				var cmd = new FirstAinCommand(_zeroBasedAinNumber, _selectedModesetVariant.Value, _fset, _mset, _set3, _mmin, _mmax);

				_logger.Log("Команда для АИН поставлена в очередь");
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


		public ushort Fset {
			get { return _fset; }
			set {
				if (_fset != value) {
					_fset = value;
					RaisePropertyChanged(() => Fset);
				}
			}
		}

		public ushort Mset {
			get { return _mset; }
			set {
				if (_mset != value) {
					_mset = value;
					RaisePropertyChanged(() => Mset);
				}
			}
		}

		public ushort Set3 {
			get { return _set3; }
			set {
				if (_set3 != value) {
					_set3 = value;
					RaisePropertyChanged(() => Set3);
				}
			}
		}

		public ushort Mmin {
			get { return _mmin; }
			set {
				if (_mmin != value) {
					_mmin = value;
					RaisePropertyChanged(() => Mmin);
				}
			}
		}

		public ushort Mmax {
			get { return _mmax; }
			set {
				if (_mmax != value) {
					_mmax = value;
					RaisePropertyChanged(() => Mmax);
				}
			}
		}


		public ICommand SendAinCommand {
			get { return _sendAinCommand; }
		}

		public IEnumerable<IModeSetVariantForAinCommandViewModel> AvailableModesetVariants {
			get { return _availableModesetVariants; }
		}

		public IModeSetVariantForAinCommandViewModel SelectedModesetVariant {
			get { return _selectedModesetVariant; }
			set {
				if (_selectedModesetVariant != value) {
					_selectedModesetVariant = value;
					RaisePropertyChanged(() => SelectedModesetVariant);
				}
			}
		}
	}
}
