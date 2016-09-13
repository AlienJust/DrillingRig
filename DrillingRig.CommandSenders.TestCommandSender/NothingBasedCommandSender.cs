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
	public class NothingBasedCommandSender : IRrModbusCommandSender, ICommandSenderController {
		private readonly IMultiLoggerWithStackTrace _debugLogger;
		private readonly IStoppableWorker _backWorkerStoppable;
		private readonly IWorker<Action> _backWorker;
		//private readonly IStoppableWorker<Action> _notifyWorker;

		public NothingBasedCommandSender(IMultiLoggerWithStackTrace debugLogger) {
			_debugLogger = debugLogger;
			var backWorker = new SingleThreadedRelayQueueWorker<Action>("NbBackWorker", a => a(), ThreadPriority.BelowNormal, true, null, debugLogger.GetLogger(0));
			_backWorker = backWorker;
			_backWorkerStoppable = backWorker;
			//_notifyWorker = new SingleThreadedRelayQueueWorker<Action>("NbNotifyWorker", a => a(), ThreadPriority.BelowNormal, true, null, new RelayActionLogger(Console.WriteLine, new ChainedFormatter(new List<ITextFormatter> { new PreffixTextFormatter("NtfyWorker > "), new DateTimeFormatter(" > ") })));
		}


		public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete) {
			_backWorker.AddWork(() => {
				var request = command.Serialize();
				_debugLogger.GetLogger(4).Log("Command: " + command.Name, new StackTrace());
				_debugLogger.GetLogger(4).Log("Request: " + request.ToText(), new StackTrace());

				Thread.Sleep(TimeSpan.FromMilliseconds(timeout.TotalMilliseconds/10.0)); // 1/10 of timeout waiting :)
				Exception exception = null;
				byte[] reply;
				try
				{
					var testCmd = command as IRrModbusCommandWithTestReply;
					if (testCmd != null)
					{
						reply = testCmd.GetTestReply();
						_debugLogger.GetLogger(4).Log("Test reply: " + reply.ToText(), new StackTrace());
					}
					else throw new Exception("Cannot cast command to IRrModbusCommandWithTestReply");
				}
				catch (Exception ex)
				{
					exception = ex;
					reply = null;
				}
				onComplete(exception, reply);
			});
		}

		public void EndWork() {
			// Порядок завершения потоков имеет значение, в противном случае не вызывается onComplete (особенности замыканий)?
			// Тогда почему бы не сделать исключение доступа к уничтоженному объекту (в данном случае Action onComplete)?
			_debugLogger.GetLogger(1).Log("EndWork called", new StackTrace());

			//_notifyWorker.StopSynchronously();
			//Console.WriteLine("notify worker stopped OK");

			_backWorkerStoppable.StopAsync();
			_debugLogger.GetLogger(1).Log("backworker stopasync was called", new StackTrace());
			_backWorkerStoppable.WaitStopComplete();
			_debugLogger.GetLogger(1).Log("backworker has been stopped", new StackTrace());
		}

		public override string ToString() {
			return "Test";
		}
	}
}
