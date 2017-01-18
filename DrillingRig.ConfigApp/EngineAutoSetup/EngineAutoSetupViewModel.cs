using System;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.Commands.BsEthernetLogs;
using DrillingRig.Commands.EngineTests;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.BsEthernetLogs;
using DrillingRig.ConfigApp.LookedLikeAbb.EngingeTest;

namespace DrillingRig.ConfigApp.EngineAutoSetup {
	class EngineAutoSetupViewModel : ViewModelBase {
		private readonly INotifySendingEnabled _notifySendingEnabled;
		private readonly IAinSettingsReader _ainSettingsReader;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsWriter _ainSettingsWriter;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targetAddressHost;
		private readonly ReadCycleModel _bsEthernetReadCycleModel;

		private readonly RelayCommand _launchAutoSetupCmd;
		private readonly RelayCommand _readTestResultCmd;

		private readonly RelayCommand _writeLeftTestResultCmd;
		private readonly RelayCommand _writeRightTestResultCmd;

		private readonly IEngineTestParams _engineTestParams;


		public TableViewModel LeftTable { get; }
		public TableViewModel RightTable { get; }

		private bool _needToUpdateLeftTable;

		private bool _isDcTestChecked;
		private bool _isTrTestChecked;
		private bool _isLeakTestChecked;
		private bool _isXxTestChecked;
		private bool _isInertionTestChecked;
		private string _lastLogLineText;

		private IBsEthernetLogLine _lastLogLine;
		private int _errorsCount;

		public EngineAutoSetupViewModel(TableViewModel leftTable, TableViewModel rightTable, INotifySendingEnabled notifySendingEnabled,
			IAinSettingsReader ainSettingsReader, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsWriter ainSettingsWriter,
			IUserInterfaceRoot uiRoot, ILogger logger, ICommandSenderHost commandSenderHost, ITargetAddressHost targetAddressHost, ReadCycleModel bsEthernetReadCycleModel) {
			LeftTable = leftTable;
			RightTable = rightTable;
			_notifySendingEnabled = notifySendingEnabled;
			_ainSettingsReader = ainSettingsReader;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_ainSettingsWriter = ainSettingsWriter;
			_uiRoot = uiRoot;
			_logger = logger;
			_commandSenderHost = commandSenderHost;
			_targetAddressHost = targetAddressHost;
			_bsEthernetReadCycleModel = bsEthernetReadCycleModel;

			_needToUpdateLeftTable = true; // on app start we have no settings:

			_isDcTestChecked = false;
			_isTrTestChecked = false;
			_isLeakTestChecked = false;
			_isXxTestChecked = false;
			_isInertionTestChecked = false;

			_lastLogLine = null;
			_errorsCount = 0;
			_lastLogLineText = String.Empty;

			_engineTestParams = new EngineTestParamsBuilderAciIdentifyIni("aci_identify.ini").Build();

			_launchAutoSetupCmd = new RelayCommand(LaunchAutoSetup, CheckLaunchAutoSetupPossible);
			_readTestResultCmd = new RelayCommand(ReadTestResult, () => _notifySendingEnabled.IsSendingEnabled);

			_writeLeftTestResultCmd = new RelayCommand(WriteLeftTestResult, () => _notifySendingEnabled.IsSendingEnabled);
			_writeRightTestResultCmd = new RelayCommand(WriteRightTestResult, () => _notifySendingEnabled.IsSendingEnabled);
			// finally subscribing events:
			_notifySendingEnabled.SendingEnabledChanged += NotifySendingEnabledOnSendingEnabledChanged;
			_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete; //.AinSettingsUpdated += AinSettingsStorageUpdatedNotifyOnAinSettingsUpdated;

			_bsEthernetReadCycleModel.AnotherLogLineWasReaded += BsEthernetReadCycleModelOnAnotherLogLineWasReaded;
		}


		private void BsEthernetReadCycleModelOnAnotherLogLineWasReaded(IBsEthernetLogLine logLine) {
			_uiRoot.Notifier.Notify(() => {
				if (logLine == null) {
					if (_errorsCount <= 5) _errorsCount++;
					if (_errorsCount == 5) LastLogLineText = "[ER]";
				}
				else {
					_errorsCount = 0;
					if (_lastLogLine == null || _lastLogLine.Number != logLine.Number) {
						LastLogLineText = "[OK] " + logLine.Number.ToString("d5") + " > " + logLine.Content;
						_lastLogLine = logLine;
					}
				}
			});
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
			
			var testMask = BuildTestMask();
			var cmd = new EngineTestLaunchCommand(testMask, _engineTestParams);
			// TODO: may be each time test launching update _engineTestParams from aci_identify.ini?
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
					catch {
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
						return;
					}
					_ainSettingsReader.ReadSettingsAsync(0, true, (exception, settings) => {
						IEngineTestResult testResult;
						try {
							testResult = cmd.GetResult(reply);
							_logger.Log("Результаты тестирования получены");
						}
						catch {
							testResult = null;
							_logger.Log("Произошла ошибка во время разбора прочитаных результатов тестирования");
						}

						_logger.Log(exception != null ? "Не удалось прочитать настройки АИН №1 после тестирования двигателя" : "Настройки АИН №1 успешно прочитаны после тестирования двигателя");
						_uiRoot.Notifier.Notify(() => { RightTable.Update(testResult, settings, _engineTestParams.F0); });
					});
				});
		}

		private void WriteLeftTestResult() {
			_logger.Log("Запись результатов тестирования (откат на начальные значения)");
			try {
				var settingsPart = WriteTestResult(LeftTable);
				_ainSettingsWriter.WriteSettingsAsync(settingsPart, exception => { _logger.Log(exception != null ? "Не удалось записать настройки АИН (откат на начальные значения)" : "Настройки АИН успешно записаны (откат на начальные значения)"); });
			}
			catch {
				_logger.Log("Не удалось начать запись результатов тестирования (откат на начальные значения)");
			}
		}

		private void WriteRightTestResult() {
			_logger.Log("Запись результатов тестирования");
			try {
				var settingsPart = WriteTestResult(RightTable);
				_ainSettingsWriter.WriteSettingsAsync(settingsPart, exception => { _logger.Log(exception != null ? "Не удалось записать настройки АИН" : "Настройки АИН успешно записаны"); });
			}
			catch {
				_logger.Log("Не удалось начать запись результатов тестирования");
			}
		}

		private IAinSettingsPart WriteTestResult(TableViewModel table) {
			return new AinSettingsPartWritable {
				Rs = table.Rs,
				//Rr 
				Lsl = table.LslAndLrl,
				Lrl = table.LslAndLrl,
				Lm = table.Lm,

				FiNom = table.FlNom,
				// J
				TauR = table.Tr,
				// RoverL

				KpId = table.IdIqKp,
				KpIq = table.IdIqKp,
				KiId = table.IdIqKi,
				KiIq = table.IdIqKi,
				KpFi = table.FluxKp,
				KpW = table.SpeedKp,
				KiW = table.SpeedKi
			};
		}

		private void AinSettingsReadNotifyOnAinSettingsReadComplete(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings) {
			// Сработает в том числе при отключении от com-port'a; будут переданы settings = null
			if (zeroBasedAinNumber == 0) {
				if (_needToUpdateLeftTable && settings != null) {
					_uiRoot.Notifier.Notify(() => {
						_needToUpdateLeftTable = false;
						LeftTable.Update(null, settings, _engineTestParams.F0);
						LeftTable.J = 1;
						LeftTable.RoverL = 0;
					});
				}
				RightTable.Update(null, settings, _engineTestParams.F0);
			}
		}

		private void NotifySendingEnabledOnSendingEnabledChanged(bool isSendingEnabled) {
			_uiRoot.Notifier.Notify(() => {
				_needToUpdateLeftTable = true;
				_launchAutoSetupCmd.RaiseCanExecuteChanged();
				_readTestResultCmd.RaiseCanExecuteChanged();
				_writeLeftTestResultCmd.RaiseCanExecuteChanged();
				_writeRightTestResultCmd.RaiseCanExecuteChanged();
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

		public string LastLogLineText {
			get { return _lastLogLineText; }
			set {
				if (_lastLogLineText != value) {
					_lastLogLineText = value;
					RaisePropertyChanged(() => LastLogLineText);
				}
			}
		}

		public ICommand ReadTestResultCmd => _readTestResultCmd;

		public ICommand WriteLeftTestResultCmd => _writeLeftTestResultCmd;

		public ICommand WriteRightTestResultCmd => _writeRightTestResultCmd;
	}
}
