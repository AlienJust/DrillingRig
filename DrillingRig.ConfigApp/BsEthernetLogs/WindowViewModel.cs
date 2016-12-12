using System.Threading;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.BsEthernetLogs {
	class WindowViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ICommandSenderHost _cmdSenderHost;
		private readonly INotifySendingEnabled _notifySendingEnabled;
		private readonly ITargetAddressHost _targetAddressHost;

		private readonly Thread _sendingCycleThread;

		private bool _isActive;
		private readonly ManualResetEvent _signal;
		
		public WindowViewModel(IUserInterfaceRoot uiRoot, ICommandSenderHost cmdSenderHost, ITargetAddressHost targetAddressHost, INotifySendingEnabled notifySendingEnabled) {
			_uiRoot = uiRoot;
			_cmdSenderHost = cmdSenderHost;
			_targetAddressHost = targetAddressHost;
			_notifySendingEnabled = notifySendingEnabled;
			_sendingCycleThread = new Thread(GetLogsCycle);
			_signal = new ManualResetEvent(_isActive);
		}

		private void GetLogsCycle()
		{
			while (true)
			{
				_i
				
				Thread.Sleep(1);
			}
		}

		public bool IsActive {
			get { return _isActive; }
			set {
				if (_isActive != value) {
					_isActive = value;
					RaisePropertyChanged("IsActive");
				}
			}
		}


	}
}