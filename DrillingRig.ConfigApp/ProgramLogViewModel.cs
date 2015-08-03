using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp
{
	class ProgramLogViewModel : ViewModelBase, ILogger {
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ObservableCollection<string> _logLines;

		public ProgramLogViewModel(IUserInterfaceRoot userInterfaceRoot) {
			_userInterfaceRoot = userInterfaceRoot;
			_logLines = new ObservableCollection<string>();
		}

		public ObservableCollection<string> LogLines {
			get { return _logLines; }
		}


		public void Log(string text) {
			_userInterfaceRoot.Notifier.Notify(() => LogLines.Add(text));
		}

		public void Log(object obj) {
			Log(obj.ToString());
		}
	}
}
