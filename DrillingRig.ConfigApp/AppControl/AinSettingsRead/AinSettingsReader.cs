using System;
using AlienJust.Support.Loggers.Contracts;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsRead {
	class AinSettingsReader : IAinSettingsReader, IAinSettingsReadNotify {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly ILogger _logger;
		private readonly TimeSpan _readSettingsTimeout;

		public event AinSettingsReadCompleteDelegate AinSettingsReadComplete;
		public event AinSettingsReadStartedDelegate AinSettingsReadStarted;

		public AinSettingsReader(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, ILogger logger) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_logger = logger;
			_readSettingsTimeout = TimeSpan.FromMilliseconds(200.0);
		}

		public void ReadSettingsAsync(byte zeroBasedAinNumber, Action<Exception, IAinSettings> callback) {
			// чтение настроек производится только для первого АИН
			var sender = _commandSenderHost.Sender;
			if (sender == null) throw new NullReferenceException("Порт передачи данных не открыт");

			var readSettingsCmd = new ReadAinSettingsCommand(zeroBasedAinNumber);

			FireEventAinSettingsReadStarted(zeroBasedAinNumber);
			sender.SendCommandAsync(_targerAddressHost.TargetAddress, readSettingsCmd, _readSettingsTimeout,
				(sendException, replyBytes) => {
					if (sendException != null) {
						var errorMessage = "Произошла ошибка во время чтения настрок АИН" + (zeroBasedAinNumber + 1).ToString();
						_logger.Log(errorMessage);
						try {
							var ex = new Exception(errorMessage, sendException);
							callback.Invoke(ex, null);
							FireEventAinSettingsReadComplete(zeroBasedAinNumber, ex, null);
						}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после неудачного чтения настроек АИН" + (zeroBasedAinNumber + 1).ToString());
							// TODO: log exception
						}
						return;
					}

					try {
						var result = readSettingsCmd.GetResult(replyBytes);

						try {
							callback.Invoke(null, result);
							FireEventAinSettingsReadComplete(zeroBasedAinNumber, null, result);
						}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после успешного чтения настроек АИН" + (zeroBasedAinNumber + 1).ToString());
							// TODO: log exception
						}

					}
					catch (Exception resultGetException) {
						var errorMessage = "Ошибка во время разбора ответа на команду чтения настроек АИН" + (zeroBasedAinNumber + 1).ToString();

						try {
							var ex = new Exception(errorMessage, resultGetException);
							callback.Invoke(ex, null);
							FireEventAinSettingsReadComplete(zeroBasedAinNumber, ex, null);
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