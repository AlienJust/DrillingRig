using DrillingRig.CommandSenders.Contracts;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;

namespace DrillingRig.ConfigApp.CommandSenderHost {
	internal interface ICommandSenderHostSettable : ICommandSenderHost {
		void SetCommandSender(ICommandSender sender);
		
	}
}