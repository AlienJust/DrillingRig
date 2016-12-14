using DrillingRig.CommandSenders.Contracts;
using DrillingRig.ConfigApp.CommandSenderHost;

namespace DrillingRig.ConfigApp.AppControl.CommandSenderHost {
	class CommandSenderHostThreadSafe : ICommandSenderHostSettable {
		private readonly object _sendersSync;
		private ICommandSender _sender;
		private ICommandSender _senderSilent;

		public CommandSenderHostThreadSafe() {
			_sendersSync = new object();
		}

		public void SetCommandSender(ICommandSender sender, ICommandSender silentSender) {
			lock (_sendersSync) {
				_sender = sender;
				_senderSilent = silentSender;
			}
		}

		public ICommandSender Sender {
			get {
				lock (_sendersSync) return _sender;
			}
		}

		public ICommandSender SilentSender {
			get {
				lock (_sendersSync) return _senderSilent;
			}
		}
	}
}