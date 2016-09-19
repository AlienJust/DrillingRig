using AlienJust.Support.Concurrent.Contracts;

namespace DrillingRig.ConfigApp.AppControl {
	class SimpleUiRoot : IUserInterfaceRoot {
		public SimpleUiRoot(IThreadNotifier notifier) {
			Notifier = notifier;
		}

		public IThreadNotifier Notifier { get; }
	}
}