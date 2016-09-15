using System;
using System.Threading;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.Telemetry01;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group01ParametersViewModel : ViewModelBase, ICyclePart {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly IAinsCounter _ainsCounter;
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
		public ParameterDoubleReadonlyViewModel Parameter12Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter13Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter14Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter15Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter16Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter17Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter18Vm { get; }
		public ParameterDoubleReadonlyViewModel Parameter19Vm { get; }

		public RelayCommand ReadCycleCmd { get; }
		public RelayCommand StopReadCycleCmd { get; }

		private readonly object _syncCancel;
		private bool _cancel;
		private bool _readingInProgress;

		public Group01ParametersViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, IAinsCounter ainsCounter, IParameterLogger parameterLogger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;
			_ainsCounter = ainsCounter;

			Parameter01Vm = new ParameterDoubleReadonlyViewModel("01.01 Вычисленная частота вращения [об/мин]", "f0", null, parameterLogger);
			Parameter02Vm = new ParameterDoubleReadonlyViewModel("01.02 Частота вращения, измеренная ДЧВ [об/мин]", "f0", null, parameterLogger);
			Parameter03Vm = new ParameterDoubleReadonlyViewModel("01.03 Частота на ОС регулятора скорости после фильтра [об/мин]", "f0", null, parameterLogger);

			Parameter04Vm = new ParameterDoubleReadonlyViewModel("01.04 Измеренный ток двигателя [А]", "f0", null, parameterLogger);

			Parameter05Vm = new ParameterDoubleReadonlyViewModel("01.05 Вычисленное выходное напряжение на двигателе [В]", "f0", null, parameterLogger); // TODO: спросить Марата, в процентах или как задаётся момент.
			Parameter06Vm = new ParameterDoubleReadonlyViewModel("01.06 Напряжение шины DC [В]", "f0", null, parameterLogger);

			Parameter07Vm = new ParameterDoubleReadonlyViewModel("01.07 Температура радиатора АИН1 [град С]", "f0", null, parameterLogger);
			Parameter08Vm = new ParameterDoubleReadonlyViewModel("01.08 Температура радиатора АИН2 [град С]", "f0", null, parameterLogger);
			Parameter09Vm = new ParameterDoubleReadonlyViewModel("01.09 Температура радиатора АИН3 [град С]", "f0", null, parameterLogger);

			Parameter10Vm = new ParameterDoubleReadonlyViewModel("01.10 Температура внешняя АИН1 [град С]", "f0", null, parameterLogger);
			Parameter11Vm = new ParameterDoubleReadonlyViewModel("01.11 Температура внешняя АИН2 [град С]", "f0", null, parameterLogger);
			Parameter12Vm = new ParameterDoubleReadonlyViewModel("01.12 Температура внешняя АИН3 [град С]", "f0", null, parameterLogger);

			Parameter13Vm = new ParameterDoubleReadonlyViewModel("01.13 Измеренный момент [Нм]", "f0", null, parameterLogger);
			Parameter14Vm = new ParameterDoubleReadonlyViewModel("01.14 Измеренный момент после фильтра [Нм]", "f0", null, parameterLogger);

			Parameter15Vm = new ParameterDoubleReadonlyViewModel("01.15 Уставка моментного тока (Выход регулятора скорости) [%]", "f0", null, parameterLogger);
			Parameter16Vm = new ParameterDoubleReadonlyViewModel("01.16 Мощность, подаваемая на двигатель", "f0", null, parameterLogger);

			Parameter17Vm = new ParameterDoubleReadonlyViewModel("01.17 Состояние цифровых входов", "f0", null, parameterLogger);
			Parameter18Vm = new ParameterDoubleReadonlyViewModel("01.18 Состояние релейных выходов", "f0", null, parameterLogger);

			Parameter19Vm = new ParameterDoubleReadonlyViewModel("01.19 Активный режим регулирования (Управление по скорости/Управление крутящим моментом)", "f0", null, parameterLogger); // (0 – регулятор скорости, 1 – внешний момент, 2 – их сумма, 3 - 0 )

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
			var cmd = new ReadTelemetry01Command();
			_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress,
				cmd, TimeSpan.FromSeconds(0.1),
				(exception, bytes) => {
					ITelemetry01 telemetry = null;
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
							Console.WriteLine("Now update telemetry Group01...");
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

		private void UpdateTelemetry(ITelemetry01 telemetry) {
			Parameter01Vm.CurrentValue = telemetry?.We;
			Parameter02Vm.CurrentValue = telemetry?.Wm;
			Parameter03Vm.CurrentValue = telemetry?.WfbF;
			Parameter04Vm.CurrentValue = telemetry?.Isum;
			Parameter05Vm.CurrentValue = telemetry?.Uout;
			Parameter06Vm.CurrentValue = telemetry?.Udc;

			Parameter07Vm.CurrentValue = telemetry?.T1;
			Parameter08Vm.CurrentValue = _ainsCounter.SelectedAinsCount >= 2 ? telemetry?.T2 : null;
			Parameter09Vm.CurrentValue = _ainsCounter.SelectedAinsCount >= 3 ? telemetry?.T3 : null;
			
			Parameter10Vm.CurrentValue = telemetry?.Text1;
			Parameter11Vm.CurrentValue = _ainsCounter.SelectedAinsCount >= 2 ? telemetry?.Text2 : null;
			Parameter12Vm.CurrentValue = _ainsCounter.SelectedAinsCount >= 3 ? telemetry?.Text3 : null;

			Parameter13Vm.CurrentValue = telemetry?.Torq;
			Parameter14Vm.CurrentValue = telemetry?.TorqF;
			Parameter15Vm.CurrentValue = telemetry?.Mout;

			Parameter16Vm.CurrentValue = telemetry?.P;
			Parameter17Vm.CurrentValue = telemetry?.Din;
			Parameter18Vm.CurrentValue = telemetry?.Dout;
			Parameter19Vm.CurrentValue = telemetry?.SelTorq;
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
