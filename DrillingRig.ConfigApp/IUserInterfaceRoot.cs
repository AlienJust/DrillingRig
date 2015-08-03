using AlienJust.Support.Concurrent.Contracts;

namespace DrillingRig.ConfigApp {
	internal interface IUserInterfaceRoot {
		IThreadNotifier Notifier { get; }
	}
}