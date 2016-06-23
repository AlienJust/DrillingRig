using System;
using System.Threading;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.Telemetry09;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group09ParametersViewModel : ViewModelBase, ICyclePart {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinsCounter _ainsCounter;
		public ParameterDoubleReadonlyViewModel Parameter01Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter02Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter03Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter04Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter05Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter06Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter07Vm { get; }

		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		private readonly object _syncCancel;
		private bool _cancel;
		private bool _readingInProgress;

		public Group09ParametersViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, IAinsCounter ainsCounter, IParameterLogger parameterLogger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleReadonlyViewModel("09.01 Биты ошибок АИН1", "f0", null, parameterLogger);
			Parameter02Vm = new ParameterDoubleReadonlyViewModel("09.02 Биты ошибок АИН2", "f0", null, parameterLogger);
			Parameter03Vm = new ParameterDoubleReadonlyViewModel("09.03 Биты ошибок АИН3", "f0", null, parameterLogger);
			Parameter04Vm = new ParameterDoubleReadonlyViewModel("09.04 Текущий код аварии", "f0", null, parameterLogger);
			Parameter05Vm = new ParameterDoubleReadonlyViewModel("09.05 Код последнего сигнала предупреждения.", "f0", null, parameterLogger);
			Parameter06Vm = new ParameterDoubleReadonlyViewModel("09.06 Ошибки связи с блоками АИН.", "f0", null, parameterLogger);
			Parameter07Vm = new ParameterDoubleReadonlyViewModel("09.07 (Ведомый привод) Биты ошибок АИН", "f0", null, parameterLogger);


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
			var cmd = new ReadTelemetry09Command();
			_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress,
				cmd, TimeSpan.FromSeconds(0.1),
				(exception, bytes) => {
					ITelemetry09 telemetry = null;
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
							Console.WriteLine("UserInterface thread begin action =============================");
							Console.WriteLine("Now update telemetry Group03...");
							// TODO: result update telemetry
							UpdateTelemetry(telemetry);
							Console.WriteLine("Done");
							Console.WriteLine("UserInterface thread end action ===============================");
						});
						waiter.Set();
					}
				});
			waiter.WaitOne();
			waiter.Reset();
		}

		private void UpdateTelemetry(ITelemetry09 telemetry) {
			Parameter01Vm.CurrentValue = telemetry?.Status1;
			Parameter02Vm.CurrentValue = _ainsCounter.SelectedAinsCount >= 2 ? telemetry?.Status2 : null;
			Parameter03Vm.CurrentValue = _ainsCounter.SelectedAinsCount >= 3 ? telemetry?.Status3 : null;
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
