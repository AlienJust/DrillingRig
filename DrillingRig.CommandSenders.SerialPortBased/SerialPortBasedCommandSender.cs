using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.Serial;
using AlienJust.Support.Text;
using DataAbstractionLevel.Low.InternalKitchen.Extensions;
using DrillingRid.Commands.Contracts;
using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.CommandSenders.SerialPortBased {
	public class SerialPortBasedCommandSender : ICommandSender {
		private readonly IMultiLoggerWithStackTrace _debugLogger;
		//private readonly SerialPort _serialPort;
		private readonly SerialPortExtender _portExtender;
		private readonly SingleThreadedRelayQueueWorkerProceedAllItemsBeforeStop<Action> _backWorker;

		public SerialPortBasedCommandSender(SerialPortExtender portExtender, IMultiLoggerWithStackTrace debugLogger) {
			_debugLogger = debugLogger;
			//_serialPort = new SerialPort(portName, 115200);
			//_serialPort.Open();
			//_portExtender = new SerialPortExtender(_serialPort, text => _debugLogger.GetLogger(3).Log(text, new StackTrace()));
			_portExtender = portExtender;

			_backWorker = new SingleThreadedRelayQueueWorkerProceedAllItemsBeforeStop<Action>("SpNotifyWorker", a => a(), ThreadPriority.BelowNormal, true, null, _debugLogger.GetLogger(0));
		}

		public void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete) {
			_backWorker.AddWork(() => {
				Exception backgroundException = null;
				byte[] resultBytes = null;
				try {
					var cmdBytes = command.Serialize();
					_debugLogger.GetLogger(4).Log("Command: " + command.Name, new StackTrace());
					_debugLogger.GetLogger(4).Log("Request: " + cmdBytes.ToText(), new StackTrace());

					var sendBytes = new byte[cmdBytes.Length + 4]; // 1 byte address + 2 bytes CRC16
					sendBytes[0] = address;
					sendBytes[1] = command.CommandCode;
					cmdBytes.CopyTo(sendBytes, 2);

					var sendCrc = MathExtensions.Crc16(sendBytes.ToList(), 0, sendBytes.Length - 2);
					sendBytes[sendBytes.Length - 2] = sendCrc.Low;
					sendBytes[sendBytes.Length - 1] = sendCrc.High;

					_portExtender.WriteBytes(sendBytes, 0, sendBytes.Length);
					var replyBytes = _portExtender.ReadBytes(command.ReplyLength + 4, timeout, true); // + 4 bytes are: addr, cmd, crc, crc

					// length is checked in port extender
					if (replyBytes[0] != address) {
						throw new Exception("Address is wrong");
					}
					if (replyBytes[1] != command.CommandCode) {
						throw new Exception("Command code is wrong, assumed the same as it was sended: " + command.CommandCode);
					}
					var crc = MathExtensions.Crc16(replyBytes.ToList(), 0, replyBytes.Length - 2);
					if (crc.Low != replyBytes[replyBytes.Length - 2])
						throw new Exception("Crc Low byte is wrong, assumed to be 0x" + crc.Low.ToString("x2") + " (" + crc.Low + " dec)");
					if (crc.High != replyBytes[replyBytes.Length - 1])
						throw new Exception("Crc High byte is wrong, assumed to be 0x" + crc.High.ToString("x2") + " (" + crc.High + " dec)");

					resultBytes = new byte[replyBytes.Length - 4];
					for (int i = 2; i < replyBytes.Length - 2; ++i)
						resultBytes[i - 2] = replyBytes[i];

					_debugLogger.GetLogger(4).Log("Reply: " + resultBytes.ToText(), new StackTrace());
				}
				catch (Exception ex) {
					backgroundException = ex;
					resultBytes = null;
				}
				finally {
					onComplete(backgroundException, resultBytes);
				}
			});
		}

		public void SendCommandAsyncNoLog(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete) {
			_backWorker.AddWork(() => {
				Exception backgroundException = null;
				byte[] resultBytes = null;
				try {
					var cmdBytes = command.Serialize();
					//_debugLogger.GetLogger(4).Log("Command: " + command.Name, new StackTrace());
					//_debugLogger.GetLogger(4).Log("Request: " + cmdBytes.ToText(), new StackTrace());

					var sendBytes = new byte[cmdBytes.Length + 4]; // 1 byte address + 2 bytes CRC16
					sendBytes[0] = address;
					sendBytes[1] = command.CommandCode;
					cmdBytes.CopyTo(sendBytes, 2);

					var sendCrc = MathExtensions.Crc16(sendBytes.ToList(), 0, sendBytes.Length - 2);
					sendBytes[sendBytes.Length - 2] = sendCrc.Low;
					sendBytes[sendBytes.Length - 1] = sendCrc.High;

					_portExtender.WriteBytes(sendBytes, 0, sendBytes.Length);
					var replyBytes = _portExtender.ReadBytes(command.ReplyLength + 4, timeout, true); // + 4 bytes are: addr, cmd, crc, crc

					// length is checked in port extender
					if (replyBytes[0] != address) {
						throw new Exception("Address is wrong");
					}
					if (replyBytes[1] != command.CommandCode) {
						throw new Exception("Command code is wrong, assumed the same as it was sended: " + command.CommandCode);
					}
					var crc = MathExtensions.Crc16(replyBytes.ToList(), 0, replyBytes.Length - 2);
					if (crc.Low != replyBytes[replyBytes.Length - 2])
						throw new Exception("Crc Low byte is wrong, assumed to be 0x" + crc.Low.ToString("x2") + " (" + crc.Low + " dec)");
					if (crc.High != replyBytes[replyBytes.Length - 1])
						throw new Exception("Crc High byte is wrong, assumed to be 0x" + crc.High.ToString("x2") + " (" + crc.High + " dec)");

					resultBytes = new byte[replyBytes.Length - 4];
					for (int i = 2; i < replyBytes.Length - 2; ++i)
						resultBytes[i - 2] = replyBytes[i];

					//_debugLogger.GetLogger(4).Log("Reply: " + resultBytes.ToText(), new StackTrace());
				}
				catch (Exception ex) {
					backgroundException = ex;
					resultBytes = null;
				}
				finally {
					onComplete(backgroundException, resultBytes);
				}
			});
		}

		public override string ToString() {
			return _serialPort.PortName;
		}

		public void EndWork() {
			_debugLogger.GetLogger(1).Log("EndWork called", new StackTrace());
			var portCloseWaiter = new ManualResetEvent(false);
			_backWorker.AddWork(() => {
				_debugLogger.GetLogger(4).Log("Closing port...", new StackTrace());
				_serialPort.Close();
				_debugLogger.GetLogger(4).Log("Port was closed", new StackTrace());
				portCloseWaiter.Set();
			});
			portCloseWaiter.WaitOne();
			_backWorker.StopAsync();

			//_backWorker.AddWork(()=>_backWorker.StopAsync());
			_backWorker.WaitStopComplete();
		}
	}
}
