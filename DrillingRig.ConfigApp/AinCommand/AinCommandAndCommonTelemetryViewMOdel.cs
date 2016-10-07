using System;
using System.Diagnostics;
using System.Threading;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.CommonTelemetry;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;

namespace DrillingRig.ConfigApp.AinCommand {
	internal class AinCommandAndCommonTelemetryViewModel : ViewModelBase, ICyclePart, IAinsLinkControl {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IMultiLoggerWithStackTrace _debugLogger;
		private readonly INotifySendingEnabled _notifySendingEnabled;

		private readonly object _syncCancel;
		private bool _cancel;
		public AinCommandAndCommonTelemetryViewModel(AinCommandAndMinimalCommonTelemetryViewModel ainCommandAndMinimalCommonTelemetryViewModel, TelemetryCommonViewModel commonTelemetryVm, ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, IMultiLoggerWithStackTrace debugLogger, INotifySendingEnabled notifySendingEnabled) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;
			_debugLogger = debugLogger;
			_notifySendingEnabled = notifySendingEnabled;
			AinCommandAndMinimalCommonTelemetryVm = ainCommandAndMinimalCommonTelemetryViewModel;
			CommonTelemetryVm = commonTelemetryVm;

			_syncCancel = new object();

			_cancel = !_notifySendingEnabled.IsSendingEnabled; // TODO: possible state loss between lines
			_notifySendingEnabled.SendingEnabledChanged += isSendingEnabled => {
				Cancel = !isSendingEnabled;
			}; // TODO: unsubscribe on exit

			Ain1LinkError = null;
			Ain2LinkError = null;
			Ain3LinkError = null;
		}

		public TelemetryCommonViewModel CommonTelemetryVm { get; }

		public AinCommandAndMinimalCommonTelemetryViewModel AinCommandAndMinimalCommonTelemetryVm { get; }
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
							AinCommandAndMinimalCommonTelemetryVm.UpdateCommonTelemetry(commonTelemetry);
							CommonTelemetryVm.UpdateCommonEngineState(commonTelemetry.CommonEngineState);
							CommonTelemetryVm.UpdateCommonFaultState(commonTelemetry.CommonFaultState);
							CommonTelemetryVm.UpdateAinsLinkState(commonTelemetry.Ain1LinkFault, commonTelemetry.Ain2LinkFault, commonTelemetry.Ain3LinkFault);
							CommonTelemetryVm.UpdateAinStatuses(commonTelemetry.Ain1Status, commonTelemetry.Ain2Status, commonTelemetry.Ain3Status);
							Ain1LinkError = commonTelemetry.Ain1LinkFault;
							Ain2LinkError = commonTelemetry.Ain2LinkFault;
							Ain3LinkError = commonTelemetry.Ain3LinkFault;
							RaiseAinsLinkInformationHasBeenUpdated();
						});
					}
					catch (Exception ex) {
						_uiRoot.Notifier.Notify(() => {
							CommonTelemetryVm.UpdateCommonEngineState(null);
							CommonTelemetryVm.UpdateCommonFaultState(null);
							CommonTelemetryVm.UpdateAinsLinkState(null, null, null);
							CommonTelemetryVm.UpdateAinStatuses(null, null, null);

							Ain1LinkError = null;
							Ain2LinkError = null;
							Ain3LinkError = null;
							RaiseAinsLinkInformationHasBeenUpdated();

							_debugLogger.GetLogger(4).Log("Ошибка: " + ex.Message, new StackTrace(Thread.CurrentThread, true));
						});
						_debugLogger.GetLogger(4).Log(ex, new StackTrace(Thread.CurrentThread, true));
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

		public bool? Ain1LinkError { get; private set; }
		public bool? Ain2LinkError { get; private set; }
		public bool? Ain3LinkError { get; private set; }

		public event AinsLinkInformationHasBeenUpdatedDelegate AinsLinkInformationHasBeenUpdated;

		private void RaiseAinsLinkInformationHasBeenUpdated() {
			_uiRoot.Notifier.Notify(() => {
				AinsLinkInformationHasBeenUpdated?.Invoke(Ain1LinkError, Ain2LinkError, Ain3LinkError);
			});
		}
	}
}
