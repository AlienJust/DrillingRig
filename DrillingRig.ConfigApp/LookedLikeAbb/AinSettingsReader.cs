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

		public void WriteSettingsAsync(IAinSettingsPart settingsPart, Action<Exception> callback) {
			int ainsCountToWriteSettings = AinsCountThreadSafe;

			ReadSettingsAsync((readSettingsException, readedAin1Settings) => {
				if (readSettingsException != null) {
					callback(new Exception("Не удалось записать настройки, возникла ошибка при предварительном их чтении", readSettingsException));
					return;
				}
				// TODO: build AIN settingsPart

				// TODO: нужно записать настройки АИН модифицировав Imcw для каждого прибора
				var settingsForAin1 = new AinSettingsWritable(readedAin1Settings);
				settingsForAin1.ModifyFromPart(settingsPart);

				settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xFCFF); // биты 8 и 9 занулены, ведущий

				if (ainsCountToWriteSettings == 1) {
					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xF3FF); // биты 10 и 11 занулены, одиночая работа
					var writeAin1SettingsCmd = new WriteAinSettingsCommand(0, settingsForAin1);
					_commandSenderHost.Sender.SendCommandAsync(
						_targerAddressHost.TargetAddress,
						writeAin1SettingsCmd,
						_writeSettingsTimeout,
						(sendException, replyBytes) => {
							callback(sendException != null ? new Exception("Ошибка отправки команды записи настроек АИН1", sendException) : null);
						});
				}

				else if (ainsCountToWriteSettings == 2) {
					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xF3FF); // бит 10 взведен, бит 11 занулен, два АИНа в системе
					settingsForAin1.Imcw = (short)(settingsForAin1.Imcw | 0xF7FF); // бит 10 взведен, бит 11 занулен, два АИНа в системе

					var writeAin1SettingsCmd = new WriteAinSettingsCommand(0, settingsForAin1);
					_commandSenderHost.Sender.SendCommandAsync(
						_targerAddressHost.TargetAddress,
						writeAin1SettingsCmd,
						_writeSettingsTimeout,
						(sendException, replyBytes) => {
							if (sendException != null) {
								callback(new Exception("Ошибка отправки команды записи настроек АИН1", sendException));
								return;
							}
							
							var settingsForAin2 = new AinSettingsWritable(readedAin1Settings);
							settingsForAin2.Imcw = (short)(settingsForAin1.Imcw & 0xFCFF); // бит 8 взведен, бит 9 занулен, ведомый 1
							settingsForAin2.Imcw = (short)(settingsForAin1.Imcw | 0xFDFF); // бит 8 взведен, бит 9 занулен, ведомый 1
							// TODO: write AIN2 settings
						});
				}
				else if (ainsCountToWriteSettings == 3) {
					// TODO: write AIN 1, 2 and 3 settings
				}
				else {
					callback.Invoke(new Exception("Неподдердживаемое число блоков АИН: " + ainsCountToWriteSettings + ", поддерживается 1, 2 и 3 блока АИН"));
				}
			});
		}
	}
}