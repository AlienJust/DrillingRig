using System;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class CycleReader : ICycleReader {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWorker<Action> _backWorker;

		private readonly TimeSpan _defaultTimeout;

		private readonly object _ain1TelemetryReadAsksCountSync;
		private readonly object _ain2TelemetryReadAsksCountSync;
		private readonly object _ain3TelemetryReadAsksCountSync;

		private int _ain1TelemetryReadAsksCount;
		private int _ain2TelemetryReadAsksCount;
		private int _ain3TelemetryReadAsksCount;

		public CycleReader(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;


			_ain1TelemetryReadAsksCountSync = new object();
			_ain1TelemetryReadAsksCount = 0;

			_ain2TelemetryReadAsksCountSync = new object();
			_ain2TelemetryReadAsksCount = 0;

			_ain3TelemetryReadAsksCountSync = new object();
			_ain3TelemetryReadAsksCount = 0;


			_backWorker = new SingleThreadedRelayQueueWorker<Action>("Read in background thread", a => a(), ThreadPriority.BelowNormal, true, null, new RelayActionLogger(logText => { }));
			_backWorker.AddWork(ReadCycleInBackground);
			_defaultTimeout = TimeSpan.FromMilliseconds(200.0);
		}

		private void ReadCycleInBackground() {
			var signal = new AutoResetEvent(false);
			while (true) {
				bool isAin1TelemtryReadNeeded;
				lock (_ain1TelemetryReadAsksCountSync) {
					isAin1TelemtryReadNeeded = _ain1TelemetryReadAsksCount > 0;
				}
				if (isAin1TelemtryReadNeeded) {
					var readTelemetryCmd = new ReadAinTelemetryCommand(0);
					_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress, readTelemetryCmd, _defaultTimeout,
						(exc, replyBytes) => {
							if (exc != null) {
								_logger.Log("Ошибка при чтении телеметрии АИН1");
								// TODO: show exception in console
								signal.Set();
								return;
							}


							try {
								var result = readTelemetryCmd.GetResult(replyBytes);
								_userInterfaceRoot.Notifier.Notify(() => {
									var evnt = Ain1TelemetryReaded;
									evnt?.Invoke(result);
								});
							}
							catch (Exception ex) {
								_logger.Log("Ошибка при разборе ответа команды чтения телеметрии АИН1");
								// TODO: show exception in console
							}
							signal.Set();
						});
					signal.WaitOne();
				}
				//-------------------------------------------------------------------
				bool isAin2TelemtryReadNeeded;
				lock (_ain2TelemetryReadAsksCountSync) {
					isAin2TelemtryReadNeeded = _ain2TelemetryReadAsksCount > 0;
				}
				if (isAin2TelemtryReadNeeded) {
					var readTelemetryCmd = new ReadAinTelemetryCommand(1);
					_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress, readTelemetryCmd, _defaultTimeout,
						(exc, replyBytes) => {
							if (exc != null) {
								_logger.Log("Ошибка при чтении телеметрии АИН2");
								// TODO: show exception in console
								signal.Set();
								return;
							}


							try {
								var result = readTelemetryCmd.GetResult(replyBytes);
								_userInterfaceRoot.Notifier.Notify(() => {
									var evnt = Ain2TelemetryReaded;
									evnt?.Invoke(result);
								});
							}
							catch (Exception ex) {
								_logger.Log("Ошибка при разборе ответа команды чтения телеметрии АИН2");
								// TODO: show exception in console
							}
							signal.Set();
						});
					signal.WaitOne();
				}
				//-------------------------------------------------------------------
				bool isAin3TelemtryReadNeeded;
				lock (_ain3TelemetryReadAsksCountSync) {
					isAin3TelemtryReadNeeded = _ain3TelemetryReadAsksCount > 0;
				}
				if (isAin3TelemtryReadNeeded) {
					var readTelemetryCmd = new ReadAinTelemetryCommand(2);
					_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress, readTelemetryCmd, _defaultTimeout,
						(exc, replyBytes) => {
							if (exc != null) {
								_logger.Log("Ошибка при чтении телеметрии АИН3");
									// TODO: show exception in console
									signal.Set();
								return;
							}


							try {
								var result = readTelemetryCmd.GetResult(replyBytes);
								_userInterfaceRoot.Notifier.Notify(() => {
									var evnt = Ain3TelemetryReaded;
									evnt?.Invoke(result);
								});
							}
							catch (Exception ex) {
								_logger.Log("Ошибка при разборе ответа команды чтения телеметрии АИН3");
									// TODO: show exception in console
								}
							signal.Set();
						});
					signal.WaitOne();
				}


				Thread.Sleep(50);
			}
		}

		public void AskToStartReadAin1TelemetryCycle() {
			lock (_ain1TelemetryReadAsksCountSync) {
				_ain1TelemetryReadAsksCount++;
			}
		}

		public void AskToStopReadAin1TelemetryCycle() {
			lock (_ain1TelemetryReadAsksCountSync) {
				if (_ain1TelemetryReadAsksCount > 0)
					_ain1TelemetryReadAsksCount--;
			}
		}

		public void AskToStartReadAin2TelemetryCycle() {
			lock (_ain2TelemetryReadAsksCountSync) {
				_ain2TelemetryReadAsksCount++;
			}
		}

		public void AskToStopReadAin2TelemetryCycle() {
			lock (_ain2TelemetryReadAsksCountSync) {
				if (_ain2TelemetryReadAsksCount > 0)
					_ain2TelemetryReadAsksCount--;
			}
		}

		public void AskToStartReadAin3TelemetryCycle() {
			lock (_ain3TelemetryReadAsksCountSync) {
				_ain3TelemetryReadAsksCount++;
			}
		}

		public void AskToStopReadAin3TelemetryCycle() {
			lock (_ain3TelemetryReadAsksCountSync) {
				if (_ain3TelemetryReadAsksCount > 0)
					_ain3TelemetryReadAsksCount--;
			}
		}

		public event AinTelemetryReadedDelegate Ain1TelemetryReaded;
		public event AinTelemetryReadedDelegate Ain2TelemetryReaded;
		public event AinTelemetryReadedDelegate Ain3TelemetryReaded;
	}

	internal interface ICycleReader {
		void AskToStartReadAin1TelemetryCycle();
		void AskToStopReadAin1TelemetryCycle();

		void AskToStartReadAin2TelemetryCycle();
		void AskToStopReadAin2TelemetryCycle();

		void AskToStartReadAin3TelemetryCycle();
		void AskToStopReadAin3TelemetryCycle();

		event AinTelemetryReadedDelegate Ain1TelemetryReaded;
		event AinTelemetryReadedDelegate Ain2TelemetryReaded;
		event AinTelemetryReadedDelegate Ain3TelemetryReaded;
	}

	delegate void AinTelemetryReadedDelegate(IAinTelemetry ainTelemetry);
}
