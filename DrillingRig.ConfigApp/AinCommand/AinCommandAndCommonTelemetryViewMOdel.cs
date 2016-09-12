using System;
using System.Threading;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.CommonTelemetry;
using DrillingRig.Commands.RtuModbus.Telemetry01;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AvaDock;

namespace DrillingRig.ConfigApp.AinCommand {
	internal class AinCommandAndCommonTelemetryViewModel : DockWindowViewModel, ICyclePart {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly INotifySendingEnabled _notifySendingEnabled;

		private readonly object _syncCancel;
		private bool _cancel;
		public AinCommandAndCommonTelemetryViewModel(AinCommandOnlyViewModel ainCommandOnlyViewModel, TelemetryCommonViewModel commonTelemetryVm, ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, INotifySendingEnabled notifySendingEnabled) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;
			_notifySendingEnabled = notifySendingEnabled;
			AinCommandOnlyVm = ainCommandOnlyViewModel;
			CommonTelemetryVm = commonTelemetryVm;

			_syncCancel = new object();

			_cancel = !_notifySendingEnabled.IsSendingEnabled; // TODO: possible state loss between lines
			_notifySendingEnabled.SendingEnabledChanged += isSendingEnabled => {
				Cancel = !isSendingEnabled;
			};
		}
		
		public TelemetryCommonViewModel CommonTelemetryVm { get; }

		public AinCommandOnlyViewModel AinCommandOnlyVm { get; }
		public void InCycleAction() {
			var waiter = new ManualResetEvent(false);
			var cmd = new ReadCommonTelemetryCommand();
			_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress,
				cmd, TimeSpan.FromSeconds(0.1),
				(exception, bytes) => {
					
					try {
						if (exception != null) {
							throw new Exception("Произошла ошибка во время обмена", exception);
						}
						var commonTelemetry = cmd.GetResult(bytes);
						_uiRoot.Notifier.Notify(() => {
							CommonTelemetryVm.UpdateCommonEngineState(commonTelemetry.CommonEngineState);
							CommonTelemetryVm.UpdateCommonFaultState(commonTelemetry.CommonFaultState);
							CommonTelemetryVm.UpdateAinsLinkState(commonTelemetry.Ain1LinkFault, commonTelemetry.Ain2LinkFault, commonTelemetry.Ain3LinkFault);
							CommonTelemetryVm.UpdateAin1Status(commonTelemetry.Ain1Status);
						});
					}
					catch (Exception ex) {
						_uiRoot.Notifier.Notify(() => {
							CommonTelemetryVm.UpdateCommonEngineState(null);
							CommonTelemetryVm.UpdateCommonFaultState(null);
							CommonTelemetryVm.UpdateAinsLinkState(null, null, null);
							CommonTelemetryVm.UpdateAin1Status(null);
							_logger.Log("Ошибка: " + ex.Message);
						});
						Console.WriteLine(ex);
					}
					finally {
						waiter.Set(); // set async action complete
					}
				});
			waiter.WaitOne(); // syncing...
			waiter.Reset();
		}

		public bool Cancel {
			get {
				lock (_syncCancel) {
					return _cancel;
				}
			}
			set {
				lock (_syncCancel) {
					_cancel = value;
				}
			}
		}
	}
}
