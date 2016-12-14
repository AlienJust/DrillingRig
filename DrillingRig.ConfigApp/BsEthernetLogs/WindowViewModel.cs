using System;
using System.Threading;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.Reflection;
using DrillingRig.Commands.BsEthernetLogs;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.BsEthernetLogs {
	class WindowViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly IReadCycleModel _model;

		private readonly RelayCommand _closingWindowCommand;

		private string _logText;
		private readonly string _logTextName;

		public Action<string> AppendTextAction { get; set; }

		public WindowViewModel(IUserInterfaceRoot uiRoot, ICommandSenderHost commandSenderHost, ITargetAddressHost targetAddressHost, INotifySendingEnabled notifySendingEnabled) {
			_uiRoot = uiRoot;

			_logTextName = ReflectedProperty.GetName(() => LogText);
			LogText = string.Empty;

			_closingWindowCommand = new RelayCommand(WindowClosing, () => true);

			_model = new ReadCycleModel(commandSenderHost, targetAddressHost, notifySendingEnabled);
			_model.AnotherLogLineWasReaded += ModelOnAnotherLogLineWasReaded; // TODO: unsubscribe on win close, also _destroy model
		}


		private void ModelOnAnotherLogLineWasReaded(IBsEthernetLogLine logLine) {
			_uiRoot.Notifier.Notify(() => {
				if (logLine == null) AppendTextAction.Invoke(Environment.NewLine + "[ER]");
				else AppendTextAction.Invoke(Environment.NewLine + "[OK] " + logLine.Number.ToString("d5") + " > " + logLine.Content);
				//RaisePropertyChanged(_logTextName);
			});
		}


		private void WindowClosing() {
			IsActive = false;
			RaisePropertyChanged(() => IsActive);

			_model.StopBackgroundThreadAndWaitForIt();
		}


		public bool IsActive {
			get { return _model.IsReadCycleEnabled; }
			set { _model.IsReadCycleEnabled = value; }
		}


		public string LogText {
			get { return _logText; }
			set {
				if (_logText != value) {
					_logText = value;
					RaisePropertyChanged(_logTextName);
				}
			}
		}
	}
}