using System;
using System.IO.Ports;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Serial;
using DrillingRid.Commands.Contracts;
using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.CommandSenders.SerialPortBased
{
    public class SerialPortBasedCommandSender: IRrModbusCommandSender {
	    private readonly SerialPortExtender _portExtender;
	    private readonly IWorker<Action> _backWorker;
	    private readonly IWorker<Action> _notifyWorker;

	    public SerialPortBasedCommandSender(SerialPort port) {
		    _portExtender = new SerialPortExtender(port);
		    _backWorker = new SingleThreadedRelayQueueWorker<Action>(a => a(), ThreadPriority.BelowNormal, true, null);
			_notifyWorker = new SingleThreadedRelayQueueWorker<Action>(a => a(), ThreadPriority.BelowNormal, true, null);
	    }

	    public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete) {
		    _backWorker.AddWork(() => {
			    Exception backgroundException = null;
			    byte[] replyBytes = null;
			    try {
				    var cmdBytes = command.Serialize();
				    var sendBytes = new byte[cmdBytes.Length + 3]; // 1 byte address + 2 bytes CRC16
				    //TODO: get all bytes
				    _portExtender.WriteBytes(sendBytes, 0, sendBytes.Length);
				    replyBytes = _portExtender.ReadBytes(command.ReplyLength, (int) timeout.TotalSeconds);
			    }
			    catch (Exception ex) {
				    backgroundException = ex;
			    }
			    finally {
				    _notifyWorker.AddWork(() => onComplete(backgroundException, replyBytes));
			    }
		    });
	    }
    }
}
