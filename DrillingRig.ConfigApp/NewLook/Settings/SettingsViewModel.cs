using AlienJust.Support.Loggers.Contracts;
using DrillingRig.ConfigApp.LookedLikeAbb;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Group106Settings;

namespace DrillingRig.ConfigApp.NewLook.Settings {
	class SettingsViewModel {
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

		public SettingsViewModel(IUserInterfaceRoot userInterfaceRoot, ILogger logger, IAinSettingsReaderWriter ainSettingsReadedWriter, IAinSettingsReadNotify ainSettingsReadNotify) {
			Group20SettingsVm = new Group20SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group22SettingsVm = new Group22SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group23SettingsVm = new Group23SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group24SettingsVm = new Group24SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group25SettingsVm = new Group25SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group26SettingsVm = new Group26SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group27SettingsVm = new Group27SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group99SettingsVm = new Group99SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group100SettingsVm = new Group100SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group101SettingsVm = new Group101SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group102SettingsVm = new Group102SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group103SettingsVm = new Group103SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group104SettingsVm = new Group104SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group105SettingsVm = new Group105SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
			Group106SettingsVm = new Group106SettingsViewModel(userInterfaceRoot, logger, ainSettingsReadedWriter, ainSettingsReadNotify);
		}
	}
}
