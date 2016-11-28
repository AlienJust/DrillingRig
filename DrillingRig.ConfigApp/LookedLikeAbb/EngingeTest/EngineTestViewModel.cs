using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.LookedLikeAbb.EngingeTest {
	class EngineTestViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targetAddressHost;
		private readonly INotifySendingEnabled _notifySendingEnabled;
		private readonly RelayCommand _launchSelectedTest;

		private TestViewModel _selectedTest;

		public ICommand LaunchSelectedTest => _launchSelectedTest;
		public IEnumerable<TestViewModel> Tests { get; }

		public EngineTestViewModel(IUserInterfaceRoot uiRoot, ILogger logger, ICommandSenderHost commandSenderHost, ITargetAddressHost targetAddressHost, INotifySendingEnabled notifySendingEnabled) {
			_uiRoot = uiRoot;
			_logger = logger;
			_commandSenderHost = commandSenderHost;
			_targetAddressHost = targetAddressHost;
			_notifySendingEnabled = notifySendingEnabled;

			Tests = new List<TestViewModel> { new TestViewModel(EngineTestId.Test1), new TestViewModel(EngineTestId.Test2) };
			SelectedTest = Tests.First();

			_launchSelectedTest = new RelayCommand(LaunchTest, () => _notifySendingEnabled.IsSendingEnabled);
			_notifySendingEnabled.SendingEnabledChanged += se => _launchSelectedTest.RaiseCanExecuteChanged();
		}

		private void LaunchTest() {
			throw new NotImplementedException();
		}

		public TestViewModel SelectedTest {
			get { return _selectedTest; }
			set {
				if (_selectedTest != value) {
					_selectedTest = value;
					RaisePropertyChanged(() => SelectedTest);
				}
			}
		}
	}
}
