using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.ConfigApp {
	internal interface ICommandSenderHost {
		IRrModbusCommandSender Sender { get; }
	}
}