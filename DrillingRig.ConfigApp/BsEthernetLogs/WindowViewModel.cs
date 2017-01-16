using System;
using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.Reflection;
using DrillingRig.Commands.BsEthernetLogs;

namespace DrillingRig.ConfigApp.BsEthernetLogs {
	class WindowViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly IReadCycleModel _model;

		private readonly RelayCommand _closingWindowCommand;

		private string _logText;
		private readonly string _logTextName;

		public Action<string> AppendTextAction { get; set; }

		private IBsEthernetLogLine _lastLogLine;
		private int _errorsCount;

		public WindowViewModel(IUserInterfaceRoot uiRoot, ReadCycleModel model) {
			_uiRoot = uiRoot;

			_logTextName = ReflectedProperty.GetName(() => LogText);
			LogText = string.Empty;

			_closingWindowCommand = new RelayCommand(WindowClosing, () => true);
			_lastLogLine = null;

			_errorsCount = 0;

			//_model = new ReadCycleModel(commandSenderHost, targetAddressHost, notifySendingEnabled);
			_model = model;
			_model.AnotherLogLineWasReaded += ModelOnAnotherLogLineWasReaded; // TODO: unsubscribe on win close, also _destroy model
		}


		private void ModelOnAnotherLogLineWasReaded(IBsEthernetLogLine logLine) {
			_uiRoot.Notifier.Notify(() => {
				if (logLine == null)
				{
					if (_errorsCount <=5) _errorsCount++;
					if (_errorsCount == 5) AppendTextAction.Invoke(Environment.NewLine + "[ER]");
				}
				else
				{
					_errorsCount = 0;
					if (_lastLogLine == null || _lastLogLine.Number != logLine.Number)
					{
						AppendTextAction.Invoke(Environment.NewLine + "[OK] " + logLine.Number.ToString("d5") + " > " + logLine.Content);
						_lastLogLine = logLine;
					}
				}
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

		public ICommand ClosingWindowCommand => _closingWindowCommand;
	}
}