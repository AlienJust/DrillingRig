using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.FaultArchive;

namespace DrillingRig.ConfigApp.NewLook.Archive {
	class ArchiveViewModel : ViewModelBase, IArchiveViewModel {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly INotifySendingEnabled _sendingEnabledControl;

		private readonly byte _zeroBasedArchiveNumber;
		private IList<IArchiveRecordViewModel> _archiveRecords;
		private readonly RelayCommand _readArchive;
		

		public ArchiveViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, INotifySendingEnabled sendingEnabledControl, byte zeroBasedArchiveNumber) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_sendingEnabledControl = sendingEnabledControl;

			_zeroBasedArchiveNumber = zeroBasedArchiveNumber;
			
			_readArchive = new RelayCommand(DoReadArchive, ()=> _sendingEnabledControl.IsSendingEnabled);

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void DoReadArchive() {
			try {
				_logger.Log("Подготовка к отправке команды чтения архива № " + OneBasedArchiveNumber);
				var cmd = new ReadArchiveCommand(_zeroBasedArchiveNumber);
				_logger.Log("Команда чтения архива поставлена в очередь, номер архива: " + OneBasedArchiveNumber);
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
								_logger.Log("Команда чтения архива была отправлена, получен хороший ответ");
								ArchiveRecords = result.OrderBy(r=>r.Year).ThenBy(r=>r.Month).ThenBy(r=>r.Day).ThenBy(r=>r.Hour).ThenBy(r=>r.Minute).ThenBy(r=>r.Second).Select(r => (IArchiveRecordViewModel)new ArchiveRecordViewModel(r)).ToList();
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

		private void SendingEnabledControlOnSendingEnabledChanged(bool isSendingEnabled) {
			_readArchive.RaiseCanExecuteChanged();
		}

		public string OneBasedArchiveNumber => (_zeroBasedArchiveNumber + 1).ToString(CultureInfo.InvariantCulture);

		public IList<IArchiveRecordViewModel> ArchiveRecords {
			get { return _archiveRecords; }
			set {
				if (_archiveRecords != value) {
					_archiveRecords = value;
					RaisePropertyChanged(() => ArchiveRecords);
				}
			}
		}

		public ICommand ReadArchive => _readArchive;
	}

	interface IArchiveViewModel {
		string OneBasedArchiveNumber { get; }
		ICommand ReadArchive { get; }
		IList<IArchiveRecordViewModel> ArchiveRecords { get; set; }
	}
}
