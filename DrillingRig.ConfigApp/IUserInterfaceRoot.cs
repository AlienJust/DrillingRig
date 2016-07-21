using AlienJust.Support.Concurrent.Contracts;

namespace DrillingRig.ConfigApp {
	public interface IUserInterfaceRoot {
		IThreadNotifier Notifier { get; }
	}
}