using System;

namespace DrillingRig.CommandSenders.Contracts {
	public interface IRtuModbusCommandSender {
		void ReadHoldingRegisters(byte address, IRtuModbusCommandWithReply command, TimeSpan timeout, Action<Exception, byte[]> onComplete);
	}
}