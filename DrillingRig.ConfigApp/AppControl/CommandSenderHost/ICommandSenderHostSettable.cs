using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.ConfigApp.CommandSenderHost {
	internal interface ICommandSenderHostSettable : ICommandSenderHost {
		void SetCommandSender(ICommandSender sender);
		
	}
}