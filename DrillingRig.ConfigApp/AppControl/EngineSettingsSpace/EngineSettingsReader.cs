using System;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers.Contracts;
using DrillingRig.Commands.EngineSettings;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace {
	class EngineSettingsReader : IEngineSettingsReader, IEngineSettingsReadNotifyRaisable {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly ILogger _logger;
		private readonly IEngineSettingsStorageSettable _settingsStorageSettable;
		private readonly TimeSpan _readSettingsTimeout;
		private readonly IWorker<Action> _notifyWorker;

		public event EngineSettingsReadCompleteDelegate EngineSettingsReadComplete;
		public event EngineSettingsReadStartedDelegate EngineSettingsReadStarted;

		public EngineSettingsReader(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, ILogger logger,
			IEngineSettingsStorageSettable settingsStorageSettable, IMultiLoggerWithStackTrace<int> debugLogger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_logger = logger;
			_settingsStorageSettable = settingsStorageSettable;
			_readSettingsTimeout = TimeSpan.FromMilliseconds(200.0);
			_notifyWorker = new SingleThreadedRelayQueueWorker<Action>("EngineSettingsReaderNotify", a => a(),
				ThreadPriority.BelowNormal, true, null, debugLogger.GetLogger(0));
		}

		public void ReadSettingsAsync(bool forceRead, Action<Exception, IEngineSettings> callback) {
			// чтение настроек производится только для первого АИН

			if (forceRead == false) {
				var settings = _settingsStorageSettable.EngineSettings;
				if (settings != null) {
					_notifyWorker.AddWork(() => callback.Invoke(null, settings));
					return;
				}
			}
			var sender = _commandSenderHost.Sender;
			if (sender == null) throw new NullReferenceException("Порт передачи данных не открыт");

			var readSettingsCmd = new ReadEngineSettingsCommand();

			_notifyWorker.AddWork(FireEventEngineSettingsReadStarted);
			_logger.Log("Чтение настроек двигателя...");
			sender.SendCommandAsync(_targerAddressHost.TargetAddress, readSettingsCmd, _readSettingsTimeout, 2,
				(sendException, replyBytes) => {
					if (sendException != null) {
						var errorMessage = "Произошла ошибка во время чтения настрок двигателя";
						_logger.Log(errorMessage);
						try {
							var ex = new Exception(errorMessage, sendException);

							_notifyWorker.AddWork(() => callback.Invoke(ex, null));
							_notifyWorker.AddWork(() => FireEventEngineSettingsReadComplete(ex, null));
							_notifyWorker.AddWork(() => _settingsStorageSettable.SetSettings(null));
						}
						catch (Exception) {
							_logger.Log("Не удалось совершить обратный вызов после неудачного чтения настроек двигателя (либо не удалось обnullить в хранилище)");
						}
						return;
					}

					try {
						var result = readSettingsCmd.GetResult(replyBytes);
						try {
							_notifyWorker.AddWork(() => callback.Invoke(null, result));
							_notifyWorker.AddWork(() => FireEventEngineSettingsReadComplete(null, result));
							_notifyWorker.AddWork(() => _settingsStorageSettable.SetSettings(result));
							_logger.Log("Настройки двигателя успешно прочитаны");
						}
						catch {
							_logger.Log(
								"Не удалось совершить обратный вызов после успешного чтения настроек двигателя (либо не удалось сохранить настройки в хранилище)");
						}
					}
					catch (Exception resultGetException) {
						var errorMessage = "Ошибка во время разбора ответа на команду чтения настроек двигателя: " + resultGetException.Message;
						_logger.Log(errorMessage);
						try {
							var ex = new Exception(errorMessage, resultGetException);
							_notifyWorker.AddWork(() => callback.Invoke(ex, null));
							_notifyWorker.AddWork(() => FireEventEngineSettingsReadComplete(ex, null));
							_notifyWorker.AddWork(() => _settingsStorageSettable.SetSettings(null));
						}
						catch {
							_logger.Log("Не удалось совершить обратный вызов после неудачного парсинга настроек двигателя (либо не удалось обnullить в хранилище)");
						}
					}
				});
		}

		private void FireEventEngineSettingsReadComplete(Exception innerException, IEngineSettings settings) {
			var eve = EngineSettingsReadComplete;
			eve?.Invoke(innerException, settings);
		}

		private void FireEventEngineSettingsReadStarted() {
			var eve = EngineSettingsReadStarted;
			eve?.Invoke();
		}

		public void RaiseEngineSettingsReadStarted() {
			_notifyWorker.AddWork(FireEventEngineSettingsReadStarted);
		}

		public void RaiseEngineSettingsReadComplete(Exception innerException, IEngineSettings settings) {
			_notifyWorker.AddWork(() => FireEventEngineSettingsReadComplete(innerException, settings));
		}
	}
}