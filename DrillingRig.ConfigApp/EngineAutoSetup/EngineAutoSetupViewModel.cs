using System;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.Commands.EngineTests;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.LookedLikeAbb.EngingeTest;

namespace DrillingRig.ConfigApp.EngineAutoSetup {
	class EngineAutoSetupViewModel : ViewModelBase {
		private readonly INotifySendingEnabled _notifySendingEnabled;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsWriter _ainSettingsWriter;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targetAddressHost;

		private readonly RelayCommand _launchAutoSetupCmd;
		private readonly RelayCommand _readTestResultCmd;

		public TableViewModel LeftTable { get; }
		public TableViewModel RightTable { get; }

		private bool _needToUpdateLeftTable;

		private bool _isDcTestChecked;
		private bool _isTrTestChecked;
		private bool _isLeakTestChecked;
		private bool _isXxTestChecked;
		private bool _isInertionTestChecked;

		public EngineAutoSetupViewModel(TableViewModel leftTable, TableViewModel rightTable, INotifySendingEnabled notifySendingEnabled,
			IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsWriter ainSettingsWriter, IUserInterfaceRoot uiRoot,
			ILogger logger, ICommandSenderHost commandSenderHost, ITargetAddressHost targetAddressHost) {
			LeftTable = leftTable;
			RightTable = rightTable;
			_notifySendingEnabled = notifySendingEnabled;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_ainSettingsWriter = ainSettingsWriter;
			_uiRoot = uiRoot;
			_logger = logger;
			_commandSenderHost = commandSenderHost;
			_targetAddressHost = targetAddressHost;

			_needToUpdateLeftTable = true; // on app start we have no settings:

			_isDcTestChecked = false;
			_isTrTestChecked = false;
			_isLeakTestChecked = false;
			_isXxTestChecked = false;
			_isInertionTestChecked = false;

			_launchAutoSetupCmd = new RelayCommand(LaunchAutoSetup, CheckLaunchAutoSetupPossible);
			_readTestResultCmd = new RelayCommand(ReadTestResult, ()=>_notifySendingEnabled.IsSendingEnabled);

			// finally subscribing events:
			_notifySendingEnabled.SendingEnabledChanged += NotifySendingEnabledOnSendingEnabledChanged;
			_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete; //.AinSettingsUpdated += AinSettingsStorageUpdatedNotifyOnAinSettingsUpdated;
		}

		private bool CheckLaunchAutoSetupPossible() {
			// TODO: check tests are selected


			return _notifySendingEnabled.IsSendingEnabled;
		}

		private EngineTestId BuildTestMask() {
			var testId = EngineTestId.AutoSetupOnly;

			if (_isDcTestChecked) testId = testId | EngineTestId.DcTest;
			if (_isTrTestChecked) testId = testId | EngineTestId.RlTest;
			if (_isLeakTestChecked) testId = testId | EngineTestId.LrlTest | EngineTestId.LslTest;
			if (_isXxTestChecked) testId = testId | EngineTestId.XxTest;
			if (_isInertionTestChecked) testId = testId | EngineTestId.InertionTest;

			return testId;
		}

		private void LaunchAutoSetup() {
			var engineTestParams = new EngineTestParamsBuilderAciIdentifyIni("aci_identify.ini").Build();
			var testMask = BuildTestMask();
			var cmd = new EngineTestLaunchCommand(testMask, engineTestParams);

			_logger.Log("Запуск тестирования двигателя (" + ((byte)testMask).ToString("X2") + ")");
			_commandSenderHost.Sender.SendCommandAsync(_targetAddressHost.TargetAddress,
				cmd,
				TimeSpan.FromMilliseconds(200),
				(ex, reply) => {
					if (ex != null) {
						_logger.Log("Во время запуска тестирования произошли ошибки");
						return;
					}
					try {
						var result = cmd.GetResult(reply);
						_logger.Log(result ? "Получено подтверждение от БС-Ethernet об успешном запуске тестирования" : "БС-Ethernet сообщило о невозможности запуска тестирования");
					}
					catch (Exception e) {
						_logger.Log("Ошибка при разборе ответа на команду запуска тестирования");
					}
				});
		}

		private void ReadTestResult() {
			var cmd = new EngineTestReadResultCommand();
			_commandSenderHost.Sender.SendCommandAsync(_targetAddressHost.TargetAddress,
				cmd,
				TimeSpan.FromMilliseconds(200),
				(ex, reply) => {
					if (ex != null) {
						_logger.Log("Ошибка при получении результатов тестирования");
						// log, return
						return;
					}
					var result = cmd.GetResult(reply);
					_logger.Log("Результаты тестирования получены");
				});
		}

		private void AinSettingsReadNotifyOnAinSettingsReadComplete(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings) {
			// Сработает в том числе при отключении от com-port'a; будут переданы settings = null
			if (zeroBasedAinNumber == 0) {
				if (_needToUpdateLeftTable && settings != null) {
					_uiRoot.Notifier.Notify(() => {
						_needToUpdateLeftTable = false;
						LeftTable.Update(settings);
					});

				}
				RightTable.Update(settings);
			}
		}

		private void NotifySendingEnabledOnSendingEnabledChanged(bool isSendingEnabled) {
			_uiRoot.Notifier.Notify(() => {
				_needToUpdateLeftTable = true;
				_launchAutoSetupCmd.RaiseCanExecuteChanged();
				_readTestResultCmd.RaiseCanExecuteChanged();
			});
		}

		public ICommand LaunchAutoSetupCmd => _launchAutoSetupCmd;

		public bool IsDcTestChecked {
			get { return _isDcTestChecked; }
			set {
				if (_isDcTestChecked != value) {
					_isDcTestChecked = value; RaisePropertyChanged(() => IsDcTestChecked);
					if (!_isDcTestChecked) {
						_isTrTestChecked = false;
						RaisePropertyChanged(() => IsTrTestChecked);
					}
				}
			}
		}

		public bool IsTrTestChecked {
			get { return _isTrTestChecked; }
			set {
				if (_isTrTestChecked != value) {
					if (_isDcTestChecked) {
						_isTrTestChecked = value;
					}
					RaisePropertyChanged(() => IsTrTestChecked);
				}
			}
		}

		public bool IsLeakTestChecked {
			get { return _isLeakTestChecked; }
			set { if (_isLeakTestChecked != value) { _isLeakTestChecked = value; RaisePropertyChanged(() => IsLeakTestChecked); } }
		}

		public bool IsXxTestChecked {
			get { return _isXxTestChecked; }
			set { if (_isXxTestChecked != value) { _isXxTestChecked = value; RaisePropertyChanged(() => IsXxTestChecked); } }
		}

		public bool IsInertionTestChecked {
			get { return _isInertionTestChecked; }
			set { if (_isInertionTestChecked != value) { _isInertionTestChecked = value; RaisePropertyChanged(() => IsInertionTestChecked); } }
		}

		public ICommand ReadTestResultCmd => _readTestResultCmd;
	}
}
