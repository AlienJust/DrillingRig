using System;
using System.Threading;
using AlienJust.Support.Collections;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.Telemetry04;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group04ParametersViewModel : ViewModelBase, ICyclePart {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		public ParameterStringReadonlyViewModel Parameter01Vm { get; }
		public ParameterStringReadonlyViewModel Parameter02Vm { get; }
		public ParameterStringReadonlyViewModel Parameter03Vm { get; }

		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		private readonly object _syncCancel;
		private bool _cancel;
		private bool _readingInProgress;

		public Group04ParametersViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, IParameterLogger parameterLogger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;

			Parameter01Vm = new ParameterStringReadonlyViewModel("04.01 Версия ПО (АИН)", string.Empty);
			Parameter02Vm = new ParameterStringReadonlyViewModel("04.02 Дата билда ПО (АИН)", string.Empty); // TODO: change to display datetime
			Parameter03Vm = new ParameterStringReadonlyViewModel("04.03 Версия ПО (БС-Ethernet)", string.Empty);

			

			ReadCycleCmd = new RelayCommand(ReadCycleFunc, () => !_readingInProgress); // TODO: check port opened
			StopReadCycleCmd = new RelayCommand(StopReadingFunc, () => _readingInProgress);

			_syncCancel = new object();
			_cancel = true;
			_readingInProgress = false;
		}


		private void StopReadingFunc() {
			Cancel = true;
			_readingInProgress = false;

			_logger.Log("Взведен внутренний флаг прерывания циклического опроса");
			ReadCycleCmd.RaiseCanExecuteChanged();
			StopReadCycleCmd.RaiseCanExecuteChanged();
		}

		private void ReadCycleFunc() {
			_logger.Log("Запуск циклического опроса телеметрии");
			Cancel = false;

			_readingInProgress = true;
			ReadCycleCmd.RaiseCanExecuteChanged();
			StopReadCycleCmd.RaiseCanExecuteChanged();
		}

		public void InCycleAction() {
			var waiter = new ManualResetEvent(false);
			var cmd = new ReadTelemetry04Command();
			_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress,
				cmd, TimeSpan.FromSeconds(0.1),
				(exception, bytes) => {
					ITelemetry04 telemetry = null;
					try {
						if (exception != null) {
							throw new Exception("Произошла ошибка во время обмена", exception);
						}
						var result = cmd.GetResult(bytes);
						telemetry = result;
					}
					catch (Exception ex) {
						telemetry = null;
						_logger.Log("Ошибка: " + ex.Message);
						Console.WriteLine(ex);
					}
					finally {
						_uiRoot.Notifier.Notify(() => {
							UpdateTelemetry(telemetry);
						});
						waiter.Set();
					}
				});
			waiter.WaitOne();
			waiter.Reset();
		}

		private void UpdateTelemetry(ITelemetry04 telemetry) {
			if (telemetry == null) {
				Parameter01Vm.CurrentValue = "--";
				Parameter02Vm.CurrentValue = "--";
				Parameter03Vm.CurrentValue = "--";
			}
			else {
				var bp = BytesPair.FromSignedShortHighFirst(telemetry.Pver);
				Parameter01Vm.CurrentValue = bp.First.ToString("d2") + "." + bp.Second.ToString("d2");

				var year = (telemetry.PvDate & 0xFE00) >> 9;
				var month = (telemetry.PvDate & 0x01E0) >> 5;
				var day = telemetry.PvDate & 0x001F;
				try {
					Parameter02Vm.CurrentValue = new DateTime(year + 2000, month, day).ToString("yyyy.MM.dd");
				}
				catch {
					// В приборах со старой версией прошивки (до 23.10.2015) значения версии и даты бессмысленны (c) Roma
					Parameter02Vm.CurrentValue = telemetry.PvDate.ToString();
				}
				Parameter03Vm.CurrentValue = telemetry.BsVer.ToString();
			}
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
