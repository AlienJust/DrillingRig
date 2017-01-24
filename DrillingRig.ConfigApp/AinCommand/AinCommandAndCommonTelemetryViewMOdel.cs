using System;
using System.Threading;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.CommonTelemetry;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.AinCommand {
	internal class AinCommandAndCommonTelemetryViewModel : ViewModelBase, ICyclePart, IAinsLinkControl {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly INotifySendingEnabled _notifySendingEnabled;

		private readonly object _syncCancel;
		private bool _cancel;
		private int _errorCounts;
		public AinCommandAndCommonTelemetryViewModel(AinCommandAndMinimalCommonTelemetryViewModel ainCommandAndMinimalCommonTelemetryViewModel, TelemetryCommonViewModel commonTelemetryVm, ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, INotifySendingEnabled notifySendingEnabled) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
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
			_errorCounts = 0;
		}

		public TelemetryCommonViewModel CommonTelemetryVm { get; }

		public AinCommandAndMinimalCommonTelemetryViewModel AinCommandAndMinimalCommonTelemetryVm { get; }
		public void InCycleAction() {
			var waiter = new ManualResetEvent(false);
			var cmd = new ReadCommonTelemetryCommand();
			_commandSenderHost.SilentSender.SendCommandAsync(_targerAddressHost.TargetAddress,
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
						_errorCounts = 0;
					}
					catch (Exception ex) {
						const int maxErrors = 3;
						_errorCounts++;
						if (_errorCounts < maxErrors) return;

						_uiRoot.Notifier.Notify(() => {
							AinCommandAndMinimalCommonTelemetryVm.UpdateCommonTelemetry(null);
							CommonTelemetryVm.UpdateCommonEngineState(null);
							CommonTelemetryVm.UpdateCommonFaultState(null);
							CommonTelemetryVm.UpdateAinsLinkState(null, null, null);
							CommonTelemetryVm.UpdateAinStatuses(null, null, null);

							Ain1LinkError = null;
							Ain2LinkError = null;
							Ain3LinkError = null;
							RaiseAinsLinkInformationHasBeenUpdated();
						});
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

					// TODO: should I reset errors count on set cancel true?
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
