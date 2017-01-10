using System;
using System.Threading;
using DrillingRig.Commands.BsEthernetLogs;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.BsEthernetLogs {
	class ReadCycleModel : IReadCycleModel {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targetAddressHost;
		private readonly INotifySendingEnabled _notifySendingEnabled;
		private readonly Thread _backgroundThread;
		private readonly ManualResetEventSlim _conditionsChangedWaiter;

		private readonly object _syncStop;
		private readonly object _syncEnabled;

		private bool _isStopFlagRaised;
		private bool _isReadCycleEnabled;

		public ReadCycleModel(ICommandSenderHost commandSenderHost,
			ITargetAddressHost targetAddressHost, INotifySendingEnabled notifySendingEnabled) {
			_commandSenderHost = commandSenderHost;
			_targetAddressHost = targetAddressHost;
			_notifySendingEnabled = notifySendingEnabled;
			_notifySendingEnabled.SendingEnabledChanged += NotifySendingEnabledOnSendingEnabledChanged;

			_syncStop = new object();
			_syncEnabled = new object();

			_isReadCycleEnabled = true;
			_isStopFlagRaised = false;
			_conditionsChangedWaiter = new ManualResetEventSlim(false);

			_backgroundThread = new Thread(ReadLogsCycle) { Priority = ThreadPriority.BelowNormal, IsBackground = true};
			_backgroundThread.Start();
		}

		private void NotifySendingEnabledOnSendingEnabledChanged(bool isSendingEnabled) {
			if (isSendingEnabled) _conditionsChangedWaiter.Set();
			else _conditionsChangedWaiter.Reset();
		}

		private void ReadLogsCycle() {
			var cmd = new ReadBsEthernetLogLineCommand();
			var timeout = TimeSpan.FromMilliseconds(100);
			while (!IsStopFlagRaised) {
				_conditionsChangedWaiter.Wait();
				if (!IsStopFlagRaised && IsReadCycleEnabled && _notifySendingEnabled.IsSendingEnabled) {
					_commandSenderHost.SilentSender.SendCommandAsync(_targetAddressHost.TargetAddress, cmd,
						timeout,
						(exception, bytes) => {
							try {
								if (exception != null) throw exception;
								try {
									RaiseAnotherLogLineWasReaded(cmd.GetResult(bytes));
								}
								catch {
									// TODO: may be log? It is program error, user callback is broken!
								}
							}
							catch {
								RaiseAnotherLogLineWasReaded(null);
							}
						});
					Thread.Sleep(500);
				}
			}
		}

		private void RaiseAnotherLogLineWasReaded(IBsEthernetLogLine logLine) {
			try {
				AnotherLogLineWasReaded?.Invoke(logLine);
			}
			catch {
				// TODO: may be log? It is program error, user callback is broken!
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
				lock (_syncStop) {
					if (value) {
						_isStopFlagRaised = true;
						_conditionsChangedWaiter.Set();
					}
				}
			}
		}

		public bool IsReadCycleEnabled {
			get {
				lock (_syncEnabled) return _isReadCycleEnabled;
			}
			set {
				lock (_syncEnabled) {
					_isReadCycleEnabled = value;

					if (value) _conditionsChangedWaiter.Set();
					else _conditionsChangedWaiter.Reset();
				}
			}
		}

		public event IcAnotherLogLineWasReadedOrNot AnotherLogLineWasReaded;
	}
}
