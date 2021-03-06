﻿using System;
using System.Diagnostics;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.Text;
using DrillingRid.Commands.Contracts;
using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.CommandSenders.TestCommandSender {
	public class NothingBasedCommandSender : ICommandSender {
		private readonly IMultiLoggerWithStackTrace<int> _debugLogger;
		private readonly IThreadNotifier _uiNotifier;
		private readonly IStoppableWorker _backWorkerStoppable;
		private readonly IWorker<Action> _backWorker;

		public NothingBasedCommandSender(IWorker<Action> backWorker, IStoppableWorker stoppableBackWorker, IMultiLoggerWithStackTrace<int> debugLogger, IThreadNotifier uiNotifier) {

			_debugLogger = debugLogger;
			_uiNotifier = uiNotifier;
			_backWorker = backWorker;
			_backWorkerStoppable = stoppableBackWorker;
		}

		public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, int maxAttemptsCount, Action<Exception, byte[]> onComplete) {
			_backWorker.AddWork(() => {
				try {
					var request = command.Serialize();
					_debugLogger.GetLogger(4).Log("Command: " + command.Name, new StackTrace(Thread.CurrentThread, true));
					_debugLogger.GetLogger(4).Log("Request: " + request.ToText(), new StackTrace(Thread.CurrentThread, true));
					
					//Thread.Sleep(TimeSpan.FromMilliseconds(timeout.TotalMilliseconds/10.0)); // 1/10 of timeout waiting :)
					Thread.Sleep(TimeSpan.FromMilliseconds(timeout.TotalMilliseconds)); // sleeping for full timeout :)
					Exception exception = null;
					byte[] reply;
					try {
						if (command is IRrModbusCommandWithTestReply testCmd) {
							reply = testCmd.GetTestReply();
							_debugLogger.GetLogger(4).Log("Test reply: " + reply.ToText(), new StackTrace(Thread.CurrentThread, true));
						}
						else throw new Exception("Cannot cast command to IRrModbusCommandWithTestReply");
					}
					catch (Exception ex) {
						exception = ex;
						reply = null;
					}
					_uiNotifier.Notify(() => onComplete(exception, reply));
				}
				catch (Exception ex) {
					_debugLogger.GetLogger(2).Log("Ошибка при сериализации команды" + Environment.NewLine + ex, new StackTrace(Thread.CurrentThread, true));
				}
			});
		}

		public void EndWork() {
			_debugLogger.GetLogger(1).Log("EndWork called", new StackTrace(Thread.CurrentThread, true));
			_backWorkerStoppable.StopAsync();
			_debugLogger.GetLogger(1).Log("backworker stopasync was called", new StackTrace(Thread.CurrentThread, true));

			_backWorkerStoppable.WaitStopComplete();
			_debugLogger.GetLogger(1).Log("backworker has been stopped", new StackTrace(Thread.CurrentThread, true));
		}

		public override string ToString() {
			return "Test";
		}
	}

	public class SilentNothingBasedCommandSender : ICommandSender {
		private readonly IMultiLoggerWithStackTrace<int> _debugLogger;
		private readonly IThreadNotifier _uiNotifier;
		private readonly IStoppableWorker _backWorkerStoppable;
		private readonly IWorker<Action> _backWorker;

		public SilentNothingBasedCommandSender(IWorker<Action> backWorker, IStoppableWorker stoppableBackWorker, IMultiLoggerWithStackTrace<int> debugLogger, IThreadNotifier uiNotifier) {

			_debugLogger = debugLogger;
			_uiNotifier = uiNotifier;
			_backWorker = backWorker;
			_backWorkerStoppable = stoppableBackWorker;
		}

		public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, int maxAttemptsCount, Action<Exception, byte[]> onComplete) {
			_backWorker.AddWork(() => {
				command.Serialize();

				Thread.Sleep(TimeSpan.FromMilliseconds(timeout.TotalMilliseconds / 10.0)); // 1/10 of timeout waiting :)
				Exception exception = null;
				byte[] reply;
				try {
					var testCmd = command as IRrModbusCommandWithTestReply;
					if (testCmd != null) {
						reply = testCmd.GetTestReply();
					}
					else throw new Exception("Cannot cast command to IRrModbusCommandWithTestReply");
				}
				catch (Exception ex) {
					exception = ex;
					reply = null;
				}
				_uiNotifier.Notify(() => onComplete(exception, reply));
			});
		}

		public void EndWork() {
			_debugLogger.GetLogger(1).Log("EndWork called", new StackTrace(Thread.CurrentThread, true));
			_backWorkerStoppable.StopAsync();
			_debugLogger.GetLogger(1).Log("backworker stopasync was called", new StackTrace(Thread.CurrentThread, true));

			_backWorkerStoppable.WaitStopComplete();
			_debugLogger.GetLogger(1).Log("backworker has been stopped", new StackTrace(Thread.CurrentThread, true));
		}

		public override string ToString() {
			return "Test";
		}
	}
}
