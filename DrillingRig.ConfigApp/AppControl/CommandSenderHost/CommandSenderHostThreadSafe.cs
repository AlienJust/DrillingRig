using DrillingRig.CommandSenders.Contracts;

namespace DrillingRig.ConfigApp.CommandSenderHost {
	class CommandSenderHostThreadSafe : ICommandSenderHostSettable {
		private readonly object _senderSync;
		private ICommandSender _sender;

		public CommandSenderHostThreadSafe() {
			_senderSync = new object();
		}

		public void SetCommandSender(ICommandSender sender) {
			lock (_senderSync) {
				_sender = sender;
			}
		}

		public ICommandSender Sender {
			get {
				lock (_senderSync) return _sender;
			}
		}
	}
}