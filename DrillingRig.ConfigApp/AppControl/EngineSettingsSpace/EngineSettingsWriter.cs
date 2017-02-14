using System;
using DrillingRig.Commands.EngineSettings;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace {
	internal class EngineSettingsWriter : IEngineSettingsWriter {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IEngineSettingsReader _engineSettingsReader;

		private readonly TimeSpan _writeSettingsTimeout;

		public EngineSettingsWriter(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IEngineSettingsReader engineSettingsReader) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_engineSettingsReader = engineSettingsReader;
			_writeSettingsTimeout = TimeSpan.FromMilliseconds(200.0);
		}

		public void WriteSettingsAsync(IEngineSettingsPart settingsPart, Action<Exception> callback) {
			var sender = _commandSenderHost.Sender;
			if (sender == null) throw new NullReferenceException("Порт передачи данных не открыт");

			// Читаем настройки перед записью (из хранилища, или нет - неважно)
			_engineSettingsReader.ReadSettingsAsync(false, (readSettingsException, engineSettings) => {
				if (readSettingsException != null) {
					callback(new Exception("Не удалось записать настройки двигателя, возникла ошибка при предварительном их чтении из BsEthernet", readSettingsException));
					return;
				}

				var engineSettingsMoified = new EngineSettingsWritable(engineSettings);
				engineSettingsMoified.ModifyFromPart(settingsPart);
				var writeAin1SettingsCmd = new WriteEngineSettingsCommand(engineSettingsMoified);
				sender.SendCommandAsync(
					_targerAddressHost.TargetAddress,
					writeAin1SettingsCmd,
					_writeSettingsTimeout,
					(sendException, replyBytes) => {
						if (sendException != null) {
							callback(new Exception("Ошибка отправки команды записи настроек АИН1 - нет ответа от BsEthernet", sendException));
							return;
						}

						// Пауза 300 мс для того, чтобы АИН успел записать новые данные в EEPROM,
						// а затем БС-Ethernet успел их вычитать из АИН.
						System.Threading.Thread.Sleep(300);

						// Проверка записи настроек АИН1 путем их повторного чтения
						_engineSettingsReader.ReadSettingsAsync(true, (exceptionOnReReading, engineSettingsReReaded) => {
							if (exceptionOnReReading != null) {
								callback(new Exception("Не удалось проконтролировать корректность записи настроек двигателя путём их повтороного вычитывания - нет ответа от BsEthernet"));
								return;
							}
							try {
								engineSettingsMoified.CompareSettingsAfterReReading(engineSettingsReReaded, 0);
							}
							catch (Exception compareEx1) {
								callback(new Exception("Ошибка при повторном чтении настроек двигателя: " + compareEx1.Message, compareEx1));
								return;
							}
							callback(null);
						});
					});
			});
		}
	}
}