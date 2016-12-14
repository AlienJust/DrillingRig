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

		private readonly string _logTextName;
		private readonly RelayCommand _closingWindowCommand;
		public string LogText { get; private set; }
		public Action<string> AppendTextAction { get; set; }

		public WindowViewModel(IUserInterfaceRoot uiRoot, ICommandSenderHost commandSenderHost, ITargetAddressHost targetAddressHost, INotifySendingEnabled notifySendingEnabled) {
			_uiRoot = uiRoot;

			_logTextName = ReflectedProperty.GetName(() => LogText);
			LogText = string.Empty;

			_closingWindowCommand = new RelayCommand(WindowClosing, ()=>true);

			_model = new ReadCycleModel(commandSenderHost, targetAddressHost, notifySendingEnabled);
			_model.AnotherLogLineWasReaded += ModelOnAnotherLogLineWasReaded; // TODO: unsubscribe on win close, also _destroy model
		}

		private void WindowClosing()
		{
			IsActive = false;
			RaisePropertyChanged(()=> IsActive);

			_model.StopBackgroundThreadAndWaitForIt();
		}

		private void ModelOnAnotherLogLineWasReaded(IBsEthernetLogLine logLine)
		{
			_uiRoot.Notifier.Notify(() =>
			{
				//LogText += Environment.NewLine + logLine.Number + " > " + logLine.Content;
				AppendTextAction.Invoke(Environment.NewLine + logLine.Number.ToString("d5") + " > " + logLine.Content);

				//RaisePropertyChanged(_logTextName);
			});
		}

		public bool IsActive {
			get { return _model.IsReadCycleEnabled; }
			set { _model.IsReadCycleEnabled = value; }
		}
	}
}