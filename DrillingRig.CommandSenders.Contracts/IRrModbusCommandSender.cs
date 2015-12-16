using System;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.CommandSenders.Contracts {
	public interface IRrModbusCommandSender {
		void SendCommandAsync(byte address, IRrModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete);
	}
}
