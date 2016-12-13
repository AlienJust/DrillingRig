using System;
using System.Threading;
using DrillingRig.Commands.BsEthernetLogs;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.BsEthernetLogs {
	class ReadCycleModel {
		private readonly Action<IBsEthernetLogLine> _onReadCallback;
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targetAddressHost;
		private readonly Thread _backgroundThread;
		private readonly ManualResetEventSlim _backThreadWaiter;

		private readonly object _syncStop;
		private readonly object _syncEnabled;

		private bool _isStopFlagRaised;

		public ReadCycleModel(Action<IBsEthernetLogLine> onReadCallback, ICommandSenderHost commandSenderHost, ITargetAddressHost targetAddressHost) {
			_onReadCallback = onReadCallback;
			_commandSenderHost = commandSenderHost;
			_targetAddressHost = targetAddressHost;
			_syncStop = new object();
			_syncEnabled = new object();

			_isStopFlagRaised = false;
			_backThreadWaiter = new ManualResetEventSlim(false);
			_backgroundThread = new Thread(ReadLogsCycle);
		}

		private void ReadLogsCycle() {
			while (IsStopFlagRaised) {
				_backThreadWaiter.Wait();
				// TODO: read logs
				Thread.Sleep(1000);
			}
		}

		public void StopBackgroundThreadAndWaitForIt() {
			IsStopFlagRaised = true;
			_backgroundThread.Join();
		}

		private bool IsStopFlagRaised {
			get {
				lock (_syncStop) {
					return _isStopFlagRaised;
				}
			}
			set {
				lock (_syncStop) _isStopFlagRaised = value;
			}
		}

		public bool IsReadCycleEnabled {
			get {
				lock (_syncEnabled) return _backThreadWaiter.IsSet;
			}
			set {
				lock (_syncEnabled) // TODO: do I need sync?
					if (value) _backThreadWaiter.Set();
					else _backThreadWaiter.Reset();
			}
		}
	}
}
