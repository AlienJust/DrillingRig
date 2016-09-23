using System;
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
		private readonly IMultiLoggerWithStackTrace _debugLogger;
		private readonly IThreadNotifier _uiNotifier;
		private readonly IStoppableWorker _backWorkerStoppable;
		private readonly IWorker<Action> _backWorker;

		public NothingBasedCommandSender(IMultiLoggerWithStackTrace debugLogger, IThreadNotifier uiNotifier) {

			_debugLogger = debugLogger;
			_uiNotifier = uiNotifier;
			var backWorker = new SingleThreadedRelayQueueWorker<Action>("NbBackWorker", a => a(), ThreadPriority.BelowNormal, true, null, debugLogger.GetLogger(0));
			_backWorker = backWorker;
			_backWorkerStoppable = backWorker;
		}

		public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete) {
			_backWorker.AddWork(() => {
				var request = command.Serialize();
				_debugLogger.GetLogger(4).Log("Command: " + command.Name, new StackTrace(Thread.CurrentThread, true));
				_debugLogger.GetLogger(4).Log("Request: " + request.ToText(), new StackTrace(Thread.CurrentThread, true));

				Thread.Sleep(TimeSpan.FromMilliseconds(timeout.TotalMilliseconds/10.0)); // 1/10 of timeout waiting :)
				Exception exception = null;
				byte[] reply;
				try
				{
					var testCmd = command as IRrModbusCommandWithTestReply;
					if (testCmd != null)
					{
						reply = testCmd.GetTestReply();
						_debugLogger.GetLogger(4).Log("Test reply: " + reply.ToText(), new StackTrace(Thread.CurrentThread, true));
					}
					else throw new Exception("Cannot cast command to IRrModbusCommandWithTestReply");
				}
				catch (Exception ex)
				{
					exception = ex;
					reply = null;
				}
				_uiNotifier.Notify(()=>onComplete(exception, reply));
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
