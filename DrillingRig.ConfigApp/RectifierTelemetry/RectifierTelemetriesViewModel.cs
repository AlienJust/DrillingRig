using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.AinTelemetry;
using DrillingRig.Commands.Rectifier;

namespace DrillingRig.ConfigApp.RectifierTelemetry
{
	internal class RectifierTelemetriesViewModel : ViewModelBase
	{
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;

		private readonly RelayCommand _readCycleCommand;
		private readonly RelayCommand _stopReadingCommand;

		private readonly List<RectifierTelemetryViewModel> _rectifierTelemetryVms;

		private readonly IWorker<Action> _backWorker;

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

			_backWorker = new SingleThreadedRelayQueueWorker<Action>("RectifierTelemetryWorker", a => a(), ThreadPriority.BelowNormal, true, null, new RelayLogger(null));
			_syncCancel = new object();
			_cancel = false;
			_readingInProgress = false;
		}

		private void StopReading()
		{
			Cancel = true;
			_logger.Log("Взведен внутренний флаг прерывания циклического опроса");
		}

		private void ReadCycle()
		{
			_logger.Log("Запуск циклического опроса телеметрии");
			Cancel = false;
			_readingInProgress = true;
			_readCycleCommand.RaiseCanExecuteChanged();
			_stopReadingCommand.RaiseCanExecuteChanged();


			_backWorker.AddWork(() =>
			{
				try
				{
					var w8er = new ManualResetEvent(false);
					while (!Cancel)
					{
						var cmd = new ReadRectifierTelemetriesCommand();
						_commandSenderHost.Sender.SendCommandAsync(
							0x01,
							cmd,
							TimeSpan.FromSeconds(1.0),
							(exception, bytes) =>
							{
								IList<IRectifierTelemetry> rectifierTelemetries = null;
								try
								{
									if (exception != null)
									{
										throw new Exception("Произошла ошибка во время обмена", exception);
									}
									var result = cmd.GetResult(bytes);
									rectifierTelemetries = result;
								}
								catch (Exception ex)
								{
									// TODO: log exception, null values
									_logger.Log("Ошибка: " + ex.Message);
									Console.WriteLine(ex);
								}
								finally
								{

									_userInterfaceRoot.Notifier.Notify(() =>
									{
										for (int i = 0; i < _rectifierTelemetryVms.Count; ++i)
										{
											var telemetryToUpdate = rectifierTelemetries == null ? null : rectifierTelemetries[i];
											_rectifierTelemetryVms[i].UpdateTelemetry(telemetryToUpdate);
										}
									});
									w8er.Set();
								}
							});
						w8er.WaitOne();
						w8er.Reset();
						Thread.Sleep(300); // TODO: interval must be setted by user
					}
				}
				catch (Exception ex)
				{
					_logger.Log("Ошибка фонового потока очереди отправки: " + ex.Message);
				}
				finally
				{
					_logger.Log("Циклический опрос окончен");
					_userInterfaceRoot.Notifier.Notify(() =>
					{
						_readingInProgress = false;
						_readCycleCommand.RaiseCanExecuteChanged();
						_stopReadingCommand.RaiseCanExecuteChanged();
					});
				}
			});
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

		private bool Cancel
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
