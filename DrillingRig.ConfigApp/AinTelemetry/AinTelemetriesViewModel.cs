using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.Text;
using AlienJust.Support.Text.Contracts;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.AinTelemetry {
	internal class AinTelemetriesViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;

		private readonly RelayCommand _readCycleCommand;
		private readonly RelayCommand _stopReadingCommand;

		private readonly List<AinTelemetryViewModel> _ainTelemetryVms;

		private readonly IWorker<Action> _backWorker;

		private readonly object _syncCancel;
		private bool _cancel;

		private bool _readingInProgress;


		public AinTelemetriesViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;

			_readCycleCommand = new RelayCommand(ReadCycle, () => !_readingInProgress);
			_stopReadingCommand = new RelayCommand(StopReading, () => _readingInProgress);

			_ainTelemetryVms = new List<AinTelemetryViewModel> {
				new AinTelemetryViewModel("АИН №1"),
				new AinTelemetryViewModel("АИН №2"),
				new AinTelemetryViewModel("АИН №3")
			};

			_backWorker = new SingleThreadedRelayQueueWorker<Action>("AinTelemetryBackWorker", a => a(), ThreadPriority.BelowNormal, true, null, new RelayActionLogger(Console.WriteLine, new ChainedFormatter(new List<ITextFormatter> { new PreffixTextFormatter("TelemetryBackWorker > "), new DateTimeFormatter(" > ") })));
			_syncCancel = new object();
			_cancel = false;
			_readingInProgress = false;
		}

		private void StopReading() {
			Cancel = true;
			_logger.Log("Взведен внутренний флаг прерывания циклического опроса");
		}

		private void ReadCycle() {
			_logger.Log("Запуск циклического опроса телеметрии");
			Cancel = false;
			_readingInProgress = true;
			_readCycleCommand.RaiseCanExecuteChanged();
			_stopReadingCommand.RaiseCanExecuteChanged();


			_backWorker.AddWork(() => {
				try {
					var waiter = new ManualResetEvent(false);
					while (!Cancel) {
						for (byte zbAinNumber = 0; zbAinNumber < 3; ++zbAinNumber) {
							var cmd = new ReadAinTelemetryCommand(zbAinNumber);
							byte ainNumber = zbAinNumber;
							_commandSenderHost.Sender.SendCommandAsync(0x01,
								cmd, TimeSpan.FromSeconds(1.0),
								(exception, bytes) => {
									IAinTelemetry ainTelemetry = null;
									try {
										if (exception != null) {
											throw new Exception("Произошла ошибка во время обмена", exception);
										}
										var result = cmd.GetResult(bytes);
										ainTelemetry = result;
									}
									catch (Exception ex) {
										// TODO: log exception, null values
										_logger.Log("Ошибка: " + ex.Message);
										Console.WriteLine(ex);
									}
									finally {
										byte number = ainNumber;
										_userInterfaceRoot.Notifier.Notify(() => {
											Console.WriteLine("UserInterface thread begin action =============================");
											_ainTelemetryVms[number].UpdateTelemetry(ainTelemetry);
											Console.WriteLine("UserInterface thread end action ===============================");
										});
										waiter.Set();
									}
								});
							waiter.WaitOne();
							waiter.Reset();
							Console.WriteLine("Pause 100ms");
							Thread.Sleep(100); // TODO: interval must be setted by user
						}
					}
				}
				catch (Exception ex) {
					_logger.Log("Ошибка фонового потока очереди отправки: " + ex.Message);
				}
				finally {
					_logger.Log("Циклический опрос окончен");
					_userInterfaceRoot.Notifier.Notify(() => {
						_readingInProgress = false;
						_readCycleCommand.RaiseCanExecuteChanged();
						_stopReadingCommand.RaiseCanExecuteChanged();
					});
				}
			});
		}

		public IEnumerable<AinTelemetryViewModel> AinTelemetryVms {
			get { return _ainTelemetryVms; }
		}

		public ICommand ReadCycleCommand {
			get { return _readCycleCommand; }
		}

		public ICommand StopReadingCommand {
			get { return _stopReadingCommand; }
		}

		private bool Cancel {
			get {
				lock (_syncCancel) {
					return _cancel;
				}
			}
			set {
				lock (_syncCancel) {
					_cancel = value;
				}
			}
		}
	}
}
