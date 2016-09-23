using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.SystemControl;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;

namespace DrillingRig.ConfigApp.AinTelemetry {
	internal class AinTelemetriesViewModel : ViewModelBase, ICommonAinTelemetryVm, IAinTelemetriesCycleControl , ICyclePart {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;

		private readonly RelayCommand _readCycleCommand;
		private readonly RelayCommand _stopReadingCommand;

		private readonly List<AinTelemetryExpandedViewModel> _ainTelemetryVms;

		private readonly object _syncCancel;
		private bool _cancel;

		private bool _readingInProgress;

		private readonly TelemetryCommonViewModel _commonTelemetryVm;

		private readonly IDebugInformationShower _debugInformationShower;

		public AinTelemetriesViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, IDebugInformationShower debugInformationShower, TelemetryCommonViewModel externalTelemetryVm, AinTelemetryViewModel ain1TelemetyVm, AinTelemetryViewModel ain2TelemetyVm,AinTelemetryViewModel ain3TelemetyVm)
		{
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_debugInformationShower = debugInformationShower;

			_commonTelemetryVm = externalTelemetryVm;

			_readCycleCommand = new RelayCommand(ReadCycle, () => !_readingInProgress); // TODO: check port opened
			_stopReadingCommand = new RelayCommand(StopReading, () => _readingInProgress); 

			_ainTelemetryVms = new List<AinTelemetryExpandedViewModel> {
				new AinTelemetryExpandedViewModel("АИН №1", ain1TelemetyVm),
				new AinTelemetryExpandedViewModel("АИН №2", ain2TelemetyVm),
				new AinTelemetryExpandedViewModel("АИН №3", ain3TelemetyVm)
			};


			//_backWorker = new SingleThreadedRelayQueueWorker<Action>("AinTelemetryBackWorker", a => a(), ThreadPriority.BelowNormal, true, null, new RelayActionLogger(Console.WriteLine, new ChainedFormatter(new List<ITextFormatter> {new PreffixTextFormatter("TelemetryBackWorker > "), new DateTimeFormatter(" > ")})));
			_syncCancel = new object();
			_cancel = true;
			_readingInProgress = false;
		}

		private void StopReading() {
			Cancel = true;
			foreach (var ainTelemetryExpandedViewModel in _ainTelemetryVms) {
				ainTelemetryExpandedViewModel.AinTelemetryVm.Cancel = true;
			}

			_readingInProgress = false;
			_logger.Log("Взведен внутренний флаг прерывания циклического опроса");
			_readCycleCommand.RaiseCanExecuteChanged();
			_stopReadingCommand.RaiseCanExecuteChanged();
		}

		private void ReadCycle() {
			_logger.Log("Запуск циклического опроса телеметрии");
			Cancel = false;
			foreach (var ainTelemetryExpandedViewModel in _ainTelemetryVms) {
				ainTelemetryExpandedViewModel.AinTelemetryVm.Cancel = false;
			}
			_readingInProgress = true;
			_readCycleCommand.RaiseCanExecuteChanged();
			_stopReadingCommand.RaiseCanExecuteChanged();
		}

		public IEnumerable<AinTelemetryExpandedViewModel> AinTelemetryVms => _ainTelemetryVms;

		public ICommand ReadCycleCommand => _readCycleCommand;

		public ICommand StopReadingCommand => _stopReadingCommand;


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


		public void InCycleAction() {
			var waiter = new ManualResetEvent(false);

			var cmdDebug = new ReadDebugInfoCommand();
			_commandSenderHost.Sender.SendCommandAsync(0x01, cmdDebug, TimeSpan.FromSeconds(0.1), (exception, bytes) => {
				try {
					if (exception != null) {
						throw new Exception("Произошла ошибка во время обмена", exception);
					}
					_userInterfaceRoot.Notifier.Notify(() => _debugInformationShower.ShowBytes(bytes));
				}
				catch (Exception ex) {
					// TODO: log exception, null values
					_logger.Log("Ошибка: " + ex.Message);
					Console.WriteLine(ex);
				}
				waiter.Set();
			});
			waiter.WaitOne();
			waiter.Reset();
		}
		
		public void UpdateCommonEngineState(ushort? value) {
			_commonTelemetryVm.UpdateCommonEngineState(value);
		}

		public void UpdateCommonFaultState(ushort? value)
		{
			_commonTelemetryVm.UpdateCommonFaultState(value);
		}

		public void UpdateAinsLinkState(bool? ain1LinkFault, bool? ain2LinkFault, bool? ain3LinkFault) {
			_commonTelemetryVm.UpdateAinsLinkState(ain1LinkFault, ain2LinkFault, ain3LinkFault);
		}

		public void UpdateAin1Status(ushort? value) {
			_commonTelemetryVm.UpdateAin1Status(value);
		}

		public TelemetryCommonViewModel CommonTelemetryVm => _commonTelemetryVm;
	}
}
