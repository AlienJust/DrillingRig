using System;
using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;

namespace DrillingRig.ConfigApp.EngineAutoSetup {
	class EngineAutoSetupViewModel : ViewModelBase {
		private readonly INotifySendingEnabled _notifySendingEnabled;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsWriter _ainSettingsWriter;
		private readonly IUserInterfaceRoot _uiRoot;

		private readonly RelayCommand _launchAutoSetupCmd;
		public TableViewModel LeftTable { get; }
		public TableViewModel RightTable { get; }

		private bool _needToUpdateLeftTable;

		private bool _isDcTestChecked;
		private bool _isTrTestChecked;
		private bool _isLeakTestChecked;
		private bool _isXxTestChecked;
		private bool _isInertionTestChecked;

		public EngineAutoSetupViewModel(TableViewModel leftTable, TableViewModel rightTable, INotifySendingEnabled notifySendingEnabled, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsWriter ainSettingsWriter, IUserInterfaceRoot uiRoot) {
			LeftTable = leftTable;
			RightTable = rightTable;
			_notifySendingEnabled = notifySendingEnabled;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_ainSettingsWriter = ainSettingsWriter;
			_uiRoot = uiRoot;

			_needToUpdateLeftTable = true; // on app start we have no settings:

			_isDcTestChecked = false;
			_isTrTestChecked = false;
			_isLeakTestChecked = false;
			_isXxTestChecked = false;
			_isInertionTestChecked = false;

			_launchAutoSetupCmd = new RelayCommand(LaunchAutoSetup, CheckLaunchAutoSetupPossible);

			_notifySendingEnabled.SendingEnabledChanged += NotifySendingEnabledOnSendingEnabledChanged;
			_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete; //.AinSettingsUpdated += AinSettingsStorageUpdatedNotifyOnAinSettingsUpdated;
		}

		private bool CheckLaunchAutoSetupPossible() {
			// TODO: check tests are selected


			return _notifySendingEnabled.IsSendingEnabled;
		}

		private void LaunchAutoSetup() {
			throw new NotImplementedException();
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
						RaisePropertyChanged(()=>IsTrTestChecked);
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
	}
}
