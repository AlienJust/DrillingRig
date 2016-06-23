using System;
using System.Threading;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.Telemetry02;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group02ParametersViewModel : ViewModelBase, ICyclePart {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		public ParameterDoubleReadonlyViewModel Parameter01Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter02Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter03Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter04Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter05Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter06Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter07Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter08Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter09Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter10Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter11Vm { get; }

		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		private readonly object _syncCancel;
		private bool _cancel;
		private bool _readingInProgress;

		public Group02ParametersViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, IParameterLogger parameterLogger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;

			Parameter01Vm = new ParameterDoubleReadonlyViewModel("02.01 Выход задатчика интенсивности частоты [об/мин]", "f0", null, parameterLogger);
			Parameter02Vm = new ParameterDoubleReadonlyViewModel("02.02 Выход задатчика интенсивности после фильтра [об/мин]", "f0", null, parameterLogger);
			Parameter03Vm = new ParameterDoubleReadonlyViewModel("02.03 Уставка потока [%]", "f0", null, parameterLogger);

			Parameter04Vm = new ParameterDoubleReadonlyViewModel("02.04 Измеренный поток [%]", "f0", null, parameterLogger);

			Parameter05Vm = new ParameterDoubleReadonlyViewModel("02.05 Измеренный поток после фильтра [%]", "f0", null, parameterLogger);
			Parameter06Vm = new ParameterDoubleReadonlyViewModel("02.06 Задание моментного тока [А]", "f0", null, parameterLogger);

			Parameter07Vm = new ParameterDoubleReadonlyViewModel("02.07 Задание тока возбуждения [А]", "f0", null, parameterLogger);
			Parameter08Vm = new ParameterDoubleReadonlyViewModel("02.08 Пропорциональная часть регулятора тока D [А]", "f0", null, parameterLogger);
			Parameter09Vm = new ParameterDoubleReadonlyViewModel("02.09 Пропорциональная часть регулятора тока Q [А]", "f0", null, parameterLogger);

			Parameter10Vm = new ParameterDoubleReadonlyViewModel("02.10 Пропорциональная часть регулятора скорости [об/мин]", "f0", null, parameterLogger);
			Parameter11Vm = new ParameterDoubleReadonlyViewModel("02.11 Пропорциональная часть регулятора потока [%]", "f0", null, parameterLogger);

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
			var cmd = new ReadTelemetry02Command();
			_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress,
				cmd, TimeSpan.FromSeconds(0.1),
				(exception, bytes) => {
					ITelemetry02 telemetry = null;
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
							Console.WriteLine("Now update telemetry Group02...");
							// TODO: result update telemetry
							UpdateTelemetry(telemetry);
							Console.WriteLine("Done");
							//if (_zeroBasedAinNumber == 0) _commonAinTelemetryVm.UpdateAin1Status(ainTelemetry?.Status);
							Console.WriteLine("UserInterface thread end action ===============================");
						});
						waiter.Set();
					}
				});
			waiter.WaitOne();
			waiter.Reset();
		}

		private void UpdateTelemetry(ITelemetry02 telemetry) {
			Parameter01Vm.CurrentValue = telemetry?.Wout;
			Parameter02Vm.CurrentValue = telemetry?.WsetF;

			Parameter03Vm.CurrentValue = telemetry?.FIset;
			Parameter04Vm.CurrentValue = telemetry?.FImag;
			Parameter05Vm.CurrentValue = telemetry?.FImagF;

			Parameter06Vm.CurrentValue = telemetry?.IqSet;
			Parameter07Vm.CurrentValue = telemetry?.IdSet;

			Parameter08Vm.CurrentValue = telemetry?.Ed;
			Parameter09Vm.CurrentValue = telemetry?.Eq;
			Parameter10Vm.CurrentValue = telemetry?.Ef;
			Parameter11Vm.CurrentValue = telemetry?.Efi;
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
