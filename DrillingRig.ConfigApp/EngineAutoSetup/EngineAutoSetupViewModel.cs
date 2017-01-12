using System;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;

namespace DrillingRig.ConfigApp.EngineAutoSetup {
	class EngineAutoSetupViewModel : ViewModelBase {
		private readonly INotifySendingEnabled _notifySendingEnabled;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;
		private readonly IAinSettingsWriter _ainSettingsWriter;
		private readonly IUserInterfaceRoot _uiRoot;
		public TableViewModel LeftTable { get; }
		public TableViewModel RightTable { get; }

		private bool _needToUpdateLeftTable;

		public EngineAutoSetupViewModel(TableViewModel leftTable, TableViewModel rightTable, INotifySendingEnabled notifySendingEnabled, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsWriter ainSettingsWriter, IUserInterfaceRoot uiRoot) {
			LeftTable = leftTable;
			RightTable = rightTable;
			_notifySendingEnabled = notifySendingEnabled;
			_ainSettingsReadNotify = ainSettingsReadNotify;
			_ainSettingsWriter = ainSettingsWriter;
			_uiRoot = uiRoot;

			_needToUpdateLeftTable = true; // on app start we have no settings:

			_notifySendingEnabled.SendingEnabledChanged += NotifySendingEnabledOnSendingEnabledChanged;
			_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete; //.AinSettingsUpdated += AinSettingsStorageUpdatedNotifyOnAinSettingsUpdated;
		}

		private void AinSettingsReadNotifyOnAinSettingsReadComplete(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings)
		{
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

		private void NotifySendingEnabledOnSendingEnabledChanged(bool isSendingEnabled)
		{
			_uiRoot.Notifier.Notify(() => {
				_needToUpdateLeftTable = true;
			});
		}
	}
}
