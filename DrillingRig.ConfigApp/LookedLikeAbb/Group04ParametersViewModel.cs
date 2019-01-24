using System;
using AlienJust.Support.Collections;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.RtuModbus.Telemetry04;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class Group04ParametersViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly INotifySendingEnabled _sendingEnabledNotifier;
		public ParameterStringReadonlyViewModel Parameter01Vm { get; }
		public ParameterStringReadonlyViewModel Parameter02Vm { get; }
		public ParameterStringReadonlyViewModel Parameter03Vm { get; }

		public RelayCommand ReadCycleCmd { get; }

		public Group04ParametersViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot uiRoot, ILogger logger, INotifySendingEnabled sendingEnabledNotifier) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_uiRoot = uiRoot;
			_logger = logger;
			_sendingEnabledNotifier = sendingEnabledNotifier;

			Parameter01Vm = new ParameterStringReadonlyViewModel("Версия прошивки АИН", string.Empty);
			Parameter02Vm = new ParameterStringReadonlyViewModel("Дата сборки прошивки АИН", string.Empty);
			// TODO: change to display datetime
			Parameter03Vm = new ParameterStringReadonlyViewModel("Версия прошивки КТ", string.Empty);

			ReadCycleCmd = new RelayCommand(ReadFunc, () => _sendingEnabledNotifier.IsSendingEnabled); // TODO: check port opened

			_sendingEnabledNotifier.SendingEnabledChanged += SendingEnabledNotifierOnSendingEnabledChanged;
		}

		private void SendingEnabledNotifierOnSendingEnabledChanged(bool isSendingEnabled) {
			_uiRoot.Notifier.Notify(() => {
				if (isSendingEnabled) {
					ReadFunc();
				}

				ReadCycleCmd.RaiseCanExecuteChanged();
			});
		}

		private void ReadFunc() {
			_logger.Log("Опрос телеметрии (версии ПО)");

			var cmd = new ReadTelemetry04Command();
			try {
				_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress, cmd, TimeSpan.FromSeconds(0.1), 2, (exception, bytes) => {
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
						//Console.WriteLine(ex);
					}
					finally {
						_uiRoot.Notifier.Notify(() => { UpdateTelemetry(telemetry); });
					}
				});
			}
			catch {
				_logger.Log("Не удалось отправить команду чтения телеметрии (версии ПО), убедитесь, что COM-порт открыт");
			}
		}

		private void UpdateTelemetry(ITelemetry04 telemetry) {
			if (telemetry == null) {
				Parameter01Vm.CurrentValue = "-";
				Parameter02Vm.CurrentValue = "-";
				Parameter03Vm.CurrentValue = "-";
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
	}
}