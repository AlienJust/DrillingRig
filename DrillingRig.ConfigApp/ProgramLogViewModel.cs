using System.Collections.ObjectModel;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp
{
	class ProgramLogViewModel : ViewModelBase, ILogger {
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ObservableCollection<ILogLine> _logLines;

		public ProgramLogViewModel(IUserInterfaceRoot userInterfaceRoot) {
			_userInterfaceRoot = userInterfaceRoot;
			_logLines = new ObservableCollection<ILogLine>();
		}

		public ObservableCollection<ILogLine> LogLines {
			get { return _logLines; }
		}


		public void Log(string text) {
			_userInterfaceRoot.Notifier.Notify(() => LogLines.Add(new LogLineSimple(text)));
		}

		public void Log(object obj) {
			Log(obj.ToString());
		}
	}

	internal interface ILogLine {
		string MessageText { get; }
	}

	class LogLineSimple : ILogLine {
		private readonly string _messageText;
		public LogLineSimple(string messageText) {
			_messageText = messageText;
		}

		public string MessageText {
			get {
				return _messageText;
			}
		}
	}
}
