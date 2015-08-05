using System;
using System.Threading;
using AlienJust.Support.Concurrent;
using DrillingRid.Commands.Contracts;
using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.CommandSenders.TestCommandSender
{
    public class NothingBasedCommandSender :IRrModbusCommandSender
    {
	    private readonly SingleThreadedRelayQueueWorker<Action> _backWorker;
	    private readonly SingleThreadedRelayQueueWorker<Action> _notifyWorker;

	    public NothingBasedCommandSender() {
		    _backWorker = new SingleThreadedRelayQueueWorker<Action>(a => a(), ThreadPriority.BelowNormal, true, null);
		    _notifyWorker = new SingleThreadedRelayQueueWorker<Action>(a => a(), ThreadPriority.BelowNormal, true, null);
	    }


	    public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete) {
		    _backWorker.AddWork(() => {
			    Thread.Sleep(TimeSpan.FromMilliseconds(timeout.TotalMilliseconds/10.0)); // 1/10 of timeout waiting :)
				_notifyWorker.AddWork(() => {
					Exception exception = null;
					byte[] reply = null;
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
					onComplete(exception, reply);
				});
		    });
	    }

		public void Dispose() {
		    _backWorker.StopSynchronously();
			_notifyWorker.StopSynchronously();
	    }

	    public override string ToString() {
		    return "Test";
	    }
    }
}
