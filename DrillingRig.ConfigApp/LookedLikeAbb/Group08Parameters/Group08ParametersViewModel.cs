using System;
using System.Threading;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.Telemetry08;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.LookedLikeAbb.Group08Parameters.AswParameter;
using DrillingRig.ConfigApp.LookedLikeAbb.Group08Parameters.MswParameter;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Group08Parameters {
	class Group08ParametersViewModel : ViewModelBase, ICyclePart {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		public MswParameterViewModel Parameter01Vm { get; }
		public AswParameterViewModel Parameter02Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter03Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter04Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter05Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter06Vm { get; }

		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		private readonly object _syncCancel;
		private bool _cancel;
		private bool _readingInProgress;

		public Group08ParametersViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, IParameterLogger parameterLogger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;

			Parameter01Vm = new MswParameterViewModel(parameterLogger);
			Parameter02Vm = new AswParameterViewModel(parameterLogger);
			Parameter03Vm = new ParameterDoubleReadonlyViewModel("08.03 Этап работы с частотным приводом.", "f0", null, parameterLogger);
			Parameter04Vm = new ParameterDoubleReadonlyViewModel("08.04 MSW Ведомого привода.", "f0", null, parameterLogger);
			Parameter05Vm = new ParameterDoubleReadonlyViewModel("08.05 ASW Ведомого привода.", "f0", null, parameterLogger);
			Parameter06Vm = new ParameterDoubleReadonlyViewModel("08.06 (Ведомый привод) Этап работы с частотным приводом.", "f0", null, parameterLogger);


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
			var cmd = new ReadTelemetry08Command();
			_commandSenderHost.SilentSender.SendCommandAsync(_targerAddressHost.TargetAddress,
				cmd, TimeSpan.FromSeconds(0.1), 2,
				(exception, bytes) => {
					ITelemetry08 telemetry = null;
					try {
						if (exception != null) {
							throw new Exception("Произошла ошибка во время обмена", exception);
						}
						var result = cmd.GetResult(bytes);
						telemetry = result;
					}
					catch (Exception ex) {
						telemetry = null;
						//_logger.Log("Ошибка: " + ex.Message);
						//Console.WriteLine(ex);
					}
					finally {
						_uiRoot.Notifier.Notify(() => {
							//Console.WriteLine("UserInterface thread begin action =============================");
							//Console.WriteLine("Now update telemetry Group03...");
							// TODO: result update telemetry
							UpdateTelemetry(telemetry);
							//Console.WriteLine("Done");
							//Console.WriteLine("UserInterface thread end action ===============================");
						});
						waiter.Set();
					}
				});
			waiter.WaitOne();
			waiter.Reset();
		}

		private void UpdateTelemetry(ITelemetry08 telemetry) {
			Parameter01Vm.UpdateTelemetry(telemetry?.Msw);
			Parameter02Vm.UpdateTelemetry(telemetry?.Asw);

			Parameter03Vm.CurrentValue = telemetry?.EngineState;
			Parameter04Vm.CurrentValue = telemetry?.FollowMsw;
			Parameter05Vm.CurrentValue = telemetry?.FollowAsw;

			Parameter06Vm.CurrentValue = telemetry?.FollowEngineState;
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
