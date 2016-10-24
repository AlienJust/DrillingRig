using System;
using System.Threading;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.Telemetry09;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.LookedLikeAbb.Group09Parameters.AinBitsParameter;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleReadonly;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group09ParametersViewModel : ViewModelBase, ICyclePart {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinsCounter _ainsCounter;
		public AinBitsParameterViewModel Parameter01Vm { get; }
		public AinBitsParameterViewModel Parameter02Vm { get; }
		public AinBitsParameterViewModel Parameter03Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter04Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter05Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter06Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter07Vm { get; }

		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		private readonly object _syncCancel;
		private bool _cancel;
		private bool _readingInProgress;
		private int _errorCounts;

		public Group09ParametersViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, IAinsCounter ainsCounter, IParameterLogger parameterLogger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new AinBitsParameterViewModel(new ParameterStringReadonlyViewModel("09.01 СТАТУС АИН1", string.Empty), parameterLogger);
			Parameter02Vm = new AinBitsParameterViewModel(new ParameterStringReadonlyViewModel("09.02 СТАТУС АИН2", string.Empty), parameterLogger);
			Parameter03Vm = new AinBitsParameterViewModel(new ParameterStringReadonlyViewModel("09.03 СТАТУС АИН3", string.Empty), parameterLogger);
			Parameter04Vm = new ParameterDoubleReadonlyViewModel("09.04 Текущий код аварии", "f0", null, parameterLogger);
			Parameter05Vm = new ParameterDoubleReadonlyViewModel("09.05 Код последнего сигнала предупреждения.", "f0", null, parameterLogger);
			Parameter06Vm = new ParameterDoubleReadonlyViewModel("09.06 Ошибки связи с блоками АИН.", "f0", null, parameterLogger);
			Parameter07Vm = new ParameterDoubleReadonlyViewModel("09.07 (Ведомый привод) Биты ошибок АИН", "f0", null, parameterLogger);


			ReadCycleCmd = new RelayCommand(ReadCycleFunc, () => !_readingInProgress); // TODO: check port opened
			StopReadCycleCmd = new RelayCommand(StopReadingFunc, () => _readingInProgress);

			_syncCancel = new object();
			_cancel = true;
			_readingInProgress = false;
			_errorCounts = 0;
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
			var cmd = new ReadTelemetry09Command();
			_commandSenderHost.Sender.SendCommandAsyncNoLog(_targerAddressHost.TargetAddress,
				cmd, TimeSpan.FromSeconds(0.1),
				(exception, bytes) => {
					ITelemetry09 telemetry = null;
					try {
						if (exception != null) {
							throw new Exception("Произошла ошибка во время обмена", exception);
						}
						var result = cmd.GetResult(bytes);
						_errorCounts = 0;
						telemetry = result;
					}
					catch (Exception ex) {
						_errorCounts++; // TODO: потенциально опасная ситуация (переполнение инта (примерно через 233 часа при опросе телеметрии раз в 50 милисекунд)
						telemetry = null;
						//_logger.Log("Ошибка: " + ex.Message);
						//Console.WriteLine(ex);
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

		private void UpdateTelemetry(ITelemetry09 telemetry) {
			const int maxErrors = 3;
			if (telemetry == null && _errorCounts < maxErrors) return;

			Parameter01Vm.UpdateTelemetry(telemetry?.Status1);
			Parameter02Vm.UpdateTelemetry(_ainsCounter.SelectedAinsCount >= 2 ? telemetry?.Status2 : null);
			Parameter03Vm.UpdateTelemetry(_ainsCounter.SelectedAinsCount >= 3 ? telemetry?.Status3 : null);

			Parameter04Vm.CurrentValue = telemetry?.FaultState;
			Parameter05Vm.CurrentValue = telemetry?.Warning;
			Parameter06Vm.CurrentValue = telemetry?.ErrLinkAin;
			Parameter06Vm.CurrentValue = telemetry?.FollowStatus;
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
