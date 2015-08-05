using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Serial;
using DataAbstractionLevel.Low.InternalKitchen.Extensions;
using DrillingRid.Commands.Contracts;
using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.CommandSenders.SerialPortBased
{
    public class SerialPortBasedCommandSender: IRrModbusCommandSender {
		private readonly SerialPort _serialPort;
	    private readonly SerialPortExtender _portExtender;
	    private readonly IWorker<Action> _backWorker;
	    private readonly IWorker<Action> _notifyWorker;

	    public SerialPortBasedCommandSender(string portName) {
			_serialPort = new SerialPort(portName, 115200);
			_serialPort.Open();
		    _portExtender = new SerialPortExtender(_serialPort);
			
		    _backWorker = new SingleThreadedRelayQueueWorker<Action>(a => a(), ThreadPriority.BelowNormal, true, null);
			_notifyWorker = new SingleThreadedRelayQueueWorker<Action>(a => a(), ThreadPriority.BelowNormal, true, null);
	    }

	    public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete) {
		    _backWorker.AddWork(() => {
			    Exception backgroundException = null;
			    byte[] resultBytes = null;
			    try {
				    var cmdBytes = command.Serialize();
				    var sendBytes = new byte[cmdBytes.Length + 3]; // 1 byte address + 2 bytes CRC16
				    
				    _portExtender.WriteBytes(sendBytes, 0, sendBytes.Length);
				    var replyBytes = _portExtender.ReadBytes(command.ReplyLength + 4, (int) timeout.TotalSeconds); // + 4 bytes are: addr, cmd, crc, crc
				    
					// length is checked in port extender
				    if (replyBytes[0] != address) {
					    throw new Exception("Address is wrong");
				    }
					if (replyBytes[1] != command.CommandCode)
					{
						throw new Exception("Command code is wrong, assumed the same as it was sended: " + command.CommandCode);
					}
				    var crc = MathExtensions.Crc16(replyBytes.ToList(), 0, replyBytes.Length - 2);
					if (crc.High != replyBytes[replyBytes.Length - 2])
						throw new Exception("Crc Hi byte is wrong, assumed to be 0x" + crc.High.ToString("x2") + " (" + crc.High +" dec)");
					if (crc.Low != replyBytes[replyBytes.Length - 1])
						throw new Exception("Crc Lo byte is wrong, assumed to be 0x" + crc.Low.ToString("x2") + " (" + crc.Low + " dec)");

				    resultBytes = new byte[replyBytes.Length - 4];
				    for (int i = 2; i < replyBytes.Length - 2; ++i)
					    resultBytes[i - 2] = replyBytes[i];
			    }
			    catch (Exception ex) {
				    backgroundException = ex;
				    resultBytes = null;
			    }
			    finally {
					_notifyWorker.AddWork(() => onComplete(backgroundException, resultBytes));
			    }
		    });
	    }

	    public void Dispose() {
			_backWorker.StopSynchronously();
			_notifyWorker.StopSynchronously();
			_serialPort.Close();
	    }

	    public override string ToString() {
		    return _serialPort.PortName;
	    }
    }
}
