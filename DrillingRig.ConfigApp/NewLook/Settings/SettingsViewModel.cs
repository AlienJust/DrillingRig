using AlienJust.Support.Loggers.Contracts;
using DrillingRig.ConfigApp.AvaDock;
using DrillingRig.ConfigApp.LookedLikeAbb;

namespace DrillingRig.ConfigApp.Settings {
	class SettingsViewModel : DockWindowViewModel {
		public Group20SettingsViewModel Group20SettingsVm { get; }
		public Group22SettingsViewModel Group22SettingsVm { get; }
		public Group23SettingsViewModel Group23SettingsVm { get; }
		public Group24SettingsViewModel Group24SettingsVm { get; }
		public Group25SettingsViewModel Group25SettingsVm { get; }
		public Group26SettingsViewModel Group26SettingsVm { get; }
		public Group27SettingsViewModel Group27SettingsVm { get; }

		public Group99SettingsViewModel Group99SettingsVm { get; }
		public Group100SettingsViewModel Group100SettingsVm { get; }
		public Group101SettingsViewModel Group101SettingsVm { get; }
		public Group102SettingsViewModel Group102SettingsVm { get; }
		public Group103SettingsViewModel Group103SettingsVm { get; }
		public Group104SettingsViewModel Group104SettingsVm { get; }
		public Group105SettingsViewModel Group105SettingsVm { get; }
		public Group106SettingsViewModel Group106SettingsVm { get; }

		public SettingsViewModel(IUserInterfaceRoot userInterfaceRoot, ILogger logger, IAinSettingsReaderWriter ainSettingsReadedWriter) {
			Group20SettingsVm = new Group20SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group22SettingsVm = new Group22SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group23SettingsVm = new Group23SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group24SettingsVm = new Group24SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group25SettingsVm = new Group25SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group26SettingsVm = new Group26SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group27SettingsVm = new Group27SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group99SettingsVm = new Group99SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group100SettingsVm = new Group100SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group101SettingsVm = new Group101SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group102SettingsVm = new Group102SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group103SettingsVm = new Group103SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group104SettingsVm = new Group104SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group105SettingsVm = new Group105SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
			Group106SettingsVm = new Group106SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter);
		}
	}
}
