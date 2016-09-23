using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.ConfigApp.CommandSenderHost {
	internal interface ICommandSenderHost {
		ICommandSender Sender { get; }
		
	}
}