using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.EngineTests;
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

			Tests = new List<TestViewModel>
			{
				new TestViewModel(EngineTestId.DcTest),
				new TestViewModel(EngineTestId.RlTest),
				new TestViewModel(EngineTestId.LslLrlTest),
				new TestViewModel(EngineTestId.TestXx),
				new TestViewModel(EngineTestId.Inertion),
				new TestViewModel(EngineTestId.Tests1And6And8),
				new TestViewModel(EngineTestId.Tests1And6And8And10),
				new TestViewModel(EngineTestId.Tests1And21And6And8),
				new TestViewModel(EngineTestId.Tests1And21And6And8And10),
				new TestViewModel(EngineTestId.NoTestAutoSetupOnly)
			};
			SelectedTest = Tests.First();

			_launchSelectedTest = new RelayCommand(LaunchTest, () => _notifySendingEnabled.IsSendingEnabled);
			_notifySendingEnabled.SendingEnabledChanged += se => _launchSelectedTest.RaiseCanExecuteChanged();
		}

		private void LaunchTest()
		{
			var engineTestParams = new EngineTestParamsBuilderAciIdentifyIni("aci_identify.ini").Build();
			_commandSenderHost.Sender.SendCommandAsync(_targetAddressHost.TargetAddress,
				new EngineTestLaunchCommand(_selectedTest.TestId, engineTestParams),
				TimeSpan.FromMilliseconds(200),
				(ex, reply) =>
				{
					// TODO: get values Rs, Rr, LsI, LrI, Lm, Fl_nom, J, Tr, RoverL
					
					
				});
		}
		private void ReadTestResult() {
			
			_commandSenderHost.Sender.SendCommandAsync(_targetAddressHost.TargetAddress,
				new EngineTestReadResultCommand(), 
				TimeSpan.FromMilliseconds(200),
				(ex, reply) => {
					// TODO: get values Rs, Rr, LsI, LrI, Lm, Fl_nom, J, Tr, RoverL
				});
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
