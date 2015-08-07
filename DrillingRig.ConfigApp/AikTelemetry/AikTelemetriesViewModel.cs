using System;
using System.Collections.Generic;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;

namespace DrillingRig.ConfigApp.AikTelemetry {
	internal class AikTelemetriesViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;

		private readonly RelayCommand _readCycleCommand;
		private readonly RelayCommand _stopReadingCommand;

		private readonly List<AikTelemetryViewModel> _aikTelemetryVms;

		public AikTelemetriesViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;

			_readCycleCommand = new RelayCommand(ReadCycle);
			_stopReadingCommand = new RelayCommand(StopReading);

			_aikTelemetryVms = new List<AikTelemetryViewModel> {
				new AikTelemetryViewModel("АИК №1"),
				new AikTelemetryViewModel("АИК №2"),
				new AikTelemetryViewModel("АИК №3")
			};
		}

		private void StopReading() {
			throw new NotImplementedException();
		}

		private void ReadCycle() {
			throw new NotImplementedException();
		}

		public IEnumerable<AikTelemetryViewModel> AikTelemetryVms {
			get { return _aikTelemetryVms; }
		}

		public ICommand ReadCycleCommand {
			get { return _readCycleCommand; }
		}

		public ICommand StopReadingCommand {
			get { return _stopReadingCommand; }
		}
	}
}
