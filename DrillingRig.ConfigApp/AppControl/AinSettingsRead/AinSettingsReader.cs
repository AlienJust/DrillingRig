using System;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Loggers.Contracts;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsRead {
	class AinSettingsReader : IAinSettingsReader, IAinSettingsReadNotify {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly ILogger _logger;
		private readonly IAinSettingsStorageSettable _ainSettingsStorageSettable;
		private readonly TimeSpan _readSettingsTimeout;
		private readonly IWorker<Action> _notifyWorker;

		public event AinSettingsReadCompleteDelegate AinSettingsReadComplete;
		public event AinSettingsReadStartedDelegate AinSettingsReadStarted;

		public AinSettingsReader(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, ILogger logger, IAinSettingsStorageSettable ainSettingsStorageSettable, IMultiLoggerWithStackTrace debugLogger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_logger = logger;
			_ainSettingsStorageSettable = ainSettingsStorageSettable;
			_readSettingsTimeout = TimeSpan.FromMilliseconds(200.0);
			_notifyWorker = new SingleThreadedRelayQueueWorker<Action>("AinSettingsReaderNotify", a=>a(), ThreadPriority.BelowNormal, true, null, debugLogger.GetLogger(0));
		}

		public void ReadSettingsAsync(byte zeroBasedAinNumber, bool forceRead, Action<Exception, IAinSettings> callback) {
			// чтение настроек производится только для первого АИН

			if (forceRead == false) {
				var settings = _ainSettingsStorageSettable.GetSettings(zeroBasedAinNumber);
				if (settings != null) {
					_notifyWorker.AddWork(() => callback.Invoke(null, settings));
					return;
				}
			}
			var sender = _commandSenderHost.Sender;
			if (sender == null) throw new NullReferenceException("Порт передачи данных не открыт");

			var readSettingsCmd = new ReadAinSettingsCommand(zeroBasedAinNumber);

			_notifyWorker.AddWork(() => FireEventAinSettingsReadStarted(zeroBasedAinNumber));
			sender.SendCommandAsync(_targerAddressHost.TargetAddress, readSettingsCmd, _readSettingsTimeout,
				(sendException, replyBytes) => {
					if (sendException != null) {
						var errorMessage = "Произошла ошибка во время чтения настрок АИН" + (zeroBasedAinNumber + 1).ToString();
						_logger.Log(errorMessage);
						try {
							var ex = new Exception(errorMessage, sendException);

							_notifyWorker.AddWork(() => callback.Invoke(ex, null));
							_notifyWorker.AddWork(() => FireEventAinSettingsReadComplete(zeroBasedAinNumber, ex, null));
							_notifyWorker.AddWork(() => _ainSettingsStorageSettable.SetSettings(zeroBasedAinNumber, null));
						}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после неудачного чтения настроек АИН" + (zeroBasedAinNumber + 1).ToString());
							// TODO: log exception
						}
						return;
					}

					try {
						var result = readSettingsCmd.GetResult(replyBytes);
						if (zeroBasedAinNumber == 0 && result.Ain1LinkFault) throw new Exception("Настройки АИН1 были считаны, однако флаг ошибки связи с АИН1 взведен");
						if (zeroBasedAinNumber == 1 && result.Ain2LinkFault) throw new Exception("Настройки АИН2 были считаны, однако флаг ошибки связи с АИН2 взведен");
						if (zeroBasedAinNumber == 2 && result.Ain3LinkFault) throw new Exception("Настройки АИН3 были считаны, однако флаг ошибки связи с АИН3 взведен");

						try {
							_notifyWorker.AddWork(() => callback.Invoke(null, result));
							_notifyWorker.AddWork(() => FireEventAinSettingsReadComplete(zeroBasedAinNumber, null, result));
							_notifyWorker.AddWork(() => _ainSettingsStorageSettable.SetSettings(zeroBasedAinNumber, result));
						}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после успешного чтения настроек АИН" + (zeroBasedAinNumber + 1).ToString());
							// TODO: log exception
						}

					}
					catch (Exception resultGetException) {
						var errorMessage = "Ошибка во время разбора ответа на команду чтения настроек АИН" + (zeroBasedAinNumber + 1).ToString() + ": " + resultGetException.Message;

						try {
							var ex = new Exception(errorMessage, resultGetException);
							_notifyWorker.AddWork(() => callback.Invoke(ex, null));
							_notifyWorker.AddWork(() => FireEventAinSettingsReadComplete(zeroBasedAinNumber, ex, null));
							_notifyWorker.AddWork(() => _ainSettingsStorageSettable.SetSettings(zeroBasedAinNumber, null));
						}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после неудачного чтения настроек АИН" + (zeroBasedAinNumber + 1).ToString());
							// TODO: log exception
						}
					}
				});
		}
		
		private void FireEventAinSettingsReadComplete(byte zbAinNumber, Exception innerException, IAinSettings settings) {
			var eve = AinSettingsReadComplete;
			eve?.Invoke(zbAinNumber, innerException, settings);
		}

		private void FireEventAinSettingsReadStarted(byte zbAinNumber) {
			var eve = AinSettingsReadStarted;
			eve?.Invoke(zbAinNumber);
		}
	}
}