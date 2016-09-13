using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.Logs
{
	internal class ProgramLogViewModel : ViewModelBase, ILogger {
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly IMultiLoggerWithStackTrace _debugLogger;
		private readonly ObservableCollection<ILogLine> _logLines;

		private bool _scrollAutomaticly;
		private ILogLine _selectedLine;

		public ProgramLogViewModel(IUserInterfaceRoot userInterfaceRoot, IMultiLoggerWithStackTrace debugLogger) {
			_userInterfaceRoot = userInterfaceRoot;
			_debugLogger = debugLogger;
			_logLines = new ObservableCollection<ILogLine>();

			ClearLogCmd = new RelayCommand(ClearLog);
			ScrollAutomaticly = true;
		}

		private void ClearLog() {
			_logLines.Clear();
		}

		public ObservableCollection<ILogLine> LogLines => _logLines;

		public ICommand ClearLogCmd { get; }

		public bool ScrollAutomaticly {
			get { return _scrollAutomaticly; }
			set {
				if (_scrollAutomaticly != value) {
					_scrollAutomaticly = value;
					RaisePropertyChanged(() => ScrollAutomaticly);
				}
			}
		}


		public void Log(string text) {
			_userInterfaceRoot.Notifier.Notify(() => {
				var logLine = new LogLineSimple(text);
				LogLines.Add(logLine);
				if (ScrollAutomaticly) SelectedLine = logLine;
			});
			_debugLogger.GetLogger(5).Log(text, new StackTrace());
		}

		public void Log(object obj) {
			Log(obj.ToString());
		}

		public ILogLine SelectedLine {
			get { return _selectedLine; }
			set {
				if (_selectedLine != value) {
					_selectedLine = value;
					RaisePropertyChanged(() => SelectedLine);
				}
			}
		}
	}

	internal interface ILogLine {
		string MessageText { get; }
	}

	class LogLineSimple : ILogLine {
		public LogLineSimple(string messageText) {
			MessageText = messageText;
		}

		public string MessageText { get; }
	}
}
