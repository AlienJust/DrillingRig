using System;
using System.Collections.Generic;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Text;
using AlienJust.Support.Text.Contracts;
using DrillingRid.Commands.Contracts;
using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.CommandSenders.TestCommandSender {
	public class NothingBasedCommandSender : IRrModbusCommandSender, ICommandSenderController {
		private readonly IStoppableWorker<Action> _backWorker;
		//private readonly IStoppableWorker<Action> _notifyWorker;

		public NothingBasedCommandSender() {
			_backWorker = new SingleThreadedRelayQueueWorker<Action>("NbBackWorker", a => a(), ThreadPriority.BelowNormal, true, null, new RelayActionLogger(Console.WriteLine, new ChainedFormatter(new List<ITextFormatter> {new PreffixTextFormatter("BackWorker > "), new DateTimeFormatter(" > ")})));
			//_notifyWorker = new SingleThreadedRelayQueueWorker<Action>("NbNotifyWorker", a => a(), ThreadPriority.BelowNormal, true, null, new RelayActionLogger(Console.WriteLine, new ChainedFormatter(new List<ITextFormatter> { new PreffixTextFormatter("NtfyWorker > "), new DateTimeFormatter(" > ") })));
		}


		public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete) {
			_backWorker.AddWork(() => {
				command.Serialize();

				Thread.Sleep(TimeSpan.FromMilliseconds(timeout.TotalMilliseconds/10.0)); // 1/10 of timeout waiting :)
				Exception exception = null;
				byte[] reply;
				try
				{
					var testCmd = command as IRrModbusCommandWithTestReply;
					if (testCmd != null)
					{
						reply = testCmd.GetTestReply();
					}
					else throw new Exception("Cannot cast command to IRrModbusCommandWithTestReply");
				}
				catch (Exception ex)
				{
					exception = ex;
					reply = null;
				}
				onComplete(exception, reply);

				//_notifyWorker.AddWork(() => onComplete(exception, reply));
			});
		}

		public void EndWork() {
			// Порядок завершения потоков имеет значение, в противном случае не вызывается onComplete (особенности замыканий)?
			// Тогда почему бы не сделать исключение доступа к уничтоженному объекту (в данном случае Action onComplete)?
			Console.WriteLine("EndWork called");

			//_notifyWorker.StopSynchronously();
			//Console.WriteLine("notify worker stopped OK");

			_backWorker.Stop();
			Console.WriteLine("backworker stopped OK");
		}

		public override string ToString() {
			return "Test";
		}
	}
}
