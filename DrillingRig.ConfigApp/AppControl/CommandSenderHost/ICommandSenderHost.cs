using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.ConfigApp.AppControl.CommandSenderHost {
	internal interface ICommandSenderHost {
		ICommandSender Sender { get; }
		ICommandSender SilentSender { get; }
	}
}