using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.Rectifier;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;

namespace DrillingRig.ConfigApp.RectifierTelemetry
{
	internal class RectifierTelemetriesViewModel : ViewModelBase, ICyclePart
	{
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;

		private readonly RelayCommand _readCycleCommand;
		private readonly RelayCommand _stopReadingCommand;

		private readonly List<RectifierTelemetryViewModel> _rectifierTelemetryVms;

		private readonly object _syncCancel;
		private bool _cancel;

		private bool _readingInProgress;


		public RectifierTelemetriesViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem)
		{
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;

			_readCycleCommand = new RelayCommand(ReadCycle, () => !_readingInProgress);
			_stopReadingCommand = new RelayCommand(StopReading, () => _readingInProgress);

			_rectifierTelemetryVms = new List<RectifierTelemetryViewModel> {
				new RectifierTelemetryViewModel("Выпрямитель 1"),
				new RectifierTelemetryViewModel("Выпрямитель 2"),
				new RectifierTelemetryViewModel("Выпрямитель 3"),
				new RectifierTelemetryViewModel("Выпрямитель 4"),
				new RectifierTelemetryViewModel("Выпрямитель 5"),
				new RectifierTelemetryViewModel("Выпрямитель 6"),
			};

			_syncCancel = new object();
			_cancel = true;
			_readingInProgress = false;
		}

		private void StopReading()
		{
			Cancel = true;
			_readingInProgress = false;
			_logger.Log("Взведен внутренний флаг прерывания циклического опроса");
			_readCycleCommand.RaiseCanExecuteChanged();
			_stopReadingCommand.RaiseCanExecuteChanged();
		}

		private void ReadCycle()
		{
			_logger.Log("Запуск циклического опроса телеметрии");
			Cancel = false;
			_readingInProgress = true;
			_readCycleCommand.RaiseCanExecuteChanged();
			_stopReadingCommand.RaiseCanExecuteChanged();
		}

		public IEnumerable<RectifierTelemetryViewModel> RectifierTelemetryVms
		{
			get { return _rectifierTelemetryVms; }
		}

		public ICommand ReadCycleCommand
		{
			get { return _readCycleCommand; }
		}

		public ICommand StopReadingCommand
		{
			get { return _stopReadingCommand; }
		}

		public void InCycleAction() {
			try {
				var w8er = new ManualResetEvent(false);
					var cmd = new ReadRectifierTelemetriesCommand();
					_commandSenderHost.Sender.SendCommandAsync(
						0x01,
						cmd,
						TimeSpan.FromSeconds(0.1),
						(exception, bytes) => {
							IList<IRectifierTelemetry> rectifierTelemetries = null;
							try {
								if (exception != null) {
									throw new Exception("Произошла ошибка во время обмена", exception);
								}
								var result = cmd.GetResult(bytes);
								rectifierTelemetries = result;
							}
							catch (Exception ex) {
									// TODO: log exception, null values
									_logger.Log("Ошибка: " + ex.Message);
								Console.WriteLine(ex);
							}
							finally {

								_userInterfaceRoot.Notifier.Notify(() => {
									for (int i = 0; i < _rectifierTelemetryVms.Count; ++i) {
										var telemetryToUpdate = rectifierTelemetries == null ? null : rectifierTelemetries[i];
										_rectifierTelemetryVms[i].UpdateTelemetry(telemetryToUpdate);
									}
								});
								w8er.Set();
							}
						});
					w8er.WaitOne();
					w8er.Reset();
			}
			catch (Exception ex) {
				_logger.Log("Ошибка фонового потока очереди отправки: " + ex.Message);
			}
		}

		public bool Cancel
		{
			get
			{
				lock (_syncCancel)
				{
					return _cancel;
				}
			}
			set
			{
				lock (_syncCancel)
				{
					_cancel = value;
				}
			}
		}
	}
}
