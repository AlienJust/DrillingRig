using System;
using AlienJust.Support.Loggers.Contracts;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	internal class AinSettingsReader : IAinSettingsReader {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly ILogger _logger;
		private readonly IAinsCounter _ainsCounter;

		private readonly object _ainsCountSyncObject;
		private readonly TimeSpan _readSettingsTimeout;
		private readonly TimeSpan _writeSettingsTimeout;

		public AinSettingsReader(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IAinsCounter ainsCounter) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_logger = logger;
			_ainsCounter = ainsCounter;

			_ainsCountSyncObject = new object();
			_readSettingsTimeout = TimeSpan.FromMilliseconds(200.0);
			_writeSettingsTimeout = TimeSpan.FromMilliseconds(200.0);
		}

		private int AinsCountThreadSafe
		{
			get
			{
				lock (_ainsCountSyncObject) {
					return _ainsCounter.SelectedAinsCount;
				}
			}
		}

		public void ReadSettingsAsync(Action<Exception, IAinSettings> callback) {
			// чтение настроек производится только для первого АИН
			var readSettingsCmd = new ReadAinSettingsCommand(0);
			_commandSenderHost.Sender.SendCommandAsync(_targerAddressHost.TargetAddress, readSettingsCmd, _readSettingsTimeout,
				(sendException, replyBytes) => {
					if (sendException != null) {
						var errorMessage = "Произошла ошибка во время чтения настрок АИН1";
						_logger.Log(errorMessage);
						try { callback.Invoke(new Exception(errorMessage, sendException), null);}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после неудачного чтения настроек АИН1");
							// TODO: log exception
						}
						return;
					}

					try {
						var result = readSettingsCmd.GetResult(replyBytes);
						try { callback.Invoke(null, result); }
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после успешного чтения настроек АИН1");
							// TODO: log exception
						}
						
					}
					catch (Exception resultGetException) {
						var errorMessage = "Ошибка во время разбора ответа на команду чтения настроек АИН1";

						try { callback.Invoke(new Exception(errorMessage, resultGetException), null);}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после неудачного чтения настроек АИН1");
							// TODO: log exception
						}
					}
				});
		}

		public void WriteSettingsAsync(IAinSettings settings, Action<Exception> callback) {
			throw new NotImplementedException();
		}
	}
}