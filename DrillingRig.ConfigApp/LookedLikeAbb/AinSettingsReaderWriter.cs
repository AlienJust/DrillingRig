using System;
using AlienJust.Support.Loggers.Contracts;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	internal class AinSettingsReaderWriter : IAinSettingsReaderWriter {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly ILogger _logger;
		private readonly IAinsCounter _ainsCounter;

		private readonly object _ainsCountSyncObject;
		private readonly TimeSpan _readSettingsTimeout;
		private readonly TimeSpan _writeSettingsTimeout;

		public AinSettingsReaderWriter(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IAinsCounter ainsCounter) {
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
			ReadSettingsAsync(0, callback);
		}

		private void ReadSettingsAsync(byte zeroBasedAinNumber, Action<Exception, IAinSettings> callback) {
			// чтение настроек производится только для первого АИН
			var sender = _commandSenderHost.Sender;
			if (sender == null) throw new NullReferenceException("Порт передачи данных не открыт");

			var readSettingsCmd = new ReadAinSettingsCommand(zeroBasedAinNumber);
			sender.SendCommandAsync(_targerAddressHost.TargetAddress, readSettingsCmd, _readSettingsTimeout,
				(sendException, replyBytes) => {
					if (sendException != null) {
						var errorMessage = "Произошла ошибка во время чтения настрок АИН1";
						_logger.Log(errorMessage);
						try {
							callback.Invoke(new Exception(errorMessage, sendException), null);
						}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после неудачного чтения настроек АИН1");
							// TODO: log exception
						}
						return;
					}

					try {
						var result = readSettingsCmd.GetResult(replyBytes);
						try {
							callback.Invoke(null, result);
						}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после успешного чтения настроек АИН1");
							// TODO: log exception
						}

					}
					catch (Exception resultGetException) {
						var errorMessage = "Ошибка во время разбора ответа на команду чтения настроек АИН1";

						try {
							callback.Invoke(new Exception(errorMessage, resultGetException), null);
						}
						catch (Exception ex) {
							_logger.Log("Не удалось совершить обратный вызов после неудачного чтения настроек АИН1");
							// TODO: log exception
						}
					}
				});
		}

		public void WriteSettingsAsync(IAinSettingsPart settingsPart, Action<Exception> callback) {
			var sender = _commandSenderHost.Sender;
			if (sender == null) throw new NullReferenceException("Порт передачи данных не открыт");

			int ainsCountToWriteSettings = AinsCountThreadSafe;

			ReadSettingsAsync((readSettingsException, readedAin1Settings) => {
				if (readSettingsException != null) {
					callback(new Exception("Не удалось записать настройки, возникла ошибка при предварительном их чтении из блока АИН1", readSettingsException));
					return;
				}

				var settingsForAin1 = new AinSettingsWritable(readedAin1Settings);
				settingsForAin1.ModifyFromPart(settingsPart);
				settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xFCFF); // биты 8 и 9 занулены, ведущий



				if (ainsCountToWriteSettings == 1) {
					// Когда в системе один блок АИН
					if (readedAin1Settings.Ain1LinkFault) {
						callback(new Exception("Не удалось записать настройки, при предварительном их чтении из блока АИН1 флаг наличия ошибки связи с АИН1 взведен"));
						return;
					}

					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xF3FF); // биты 10 и 11 занулены, одиночая работа
					var writeAin1SettingsCmd = new WriteAinSettingsCommand(0, settingsForAin1);
					sender.SendCommandAsync(
						_targerAddressHost.TargetAddress,
						writeAin1SettingsCmd,
						_writeSettingsTimeout,
						(sendException, replyBytes) => {
							if (sendException != null) {
								callback(new Exception("Ошибка отправки команды записи настроек АИН1", sendException));
								return;
							}

							// Проверка записи настроек АИН1 путем их повторного чтения
							ReadSettingsAsync(0, (exceptionReRead1, settings1ReReaded) => {
								if (exceptionReRead1 != null) {
									callback(new Exception("Не удалось проконтролировать корректность записи настроек АИН1 путём их повтороного вычитывания"));
									return;
								}
								try {
									settingsForAin1.CompareSettingsAfterReReading(settings1ReReaded, 0);
								}
								catch (Exception compareEx1) {
									callback(new Exception("Ошибка при повторном чтении настроек АИН1", compareEx1));
									return;
								}
								callback(null);
							});
						});
				}



				else if (ainsCountToWriteSettings == 2) {
					// Когда в системе два блока АИН
					if (readedAin1Settings.Ain1LinkFault) {
						callback(new Exception("Не удалось записать настройки, при предварительном их чтении из блока АИН1 флаг наличия ошибки связи с АИН1 взведен"));
						return;
					}
					if (readedAin1Settings.Ain2LinkFault) {
						callback(new Exception("Не удалось записать настройки, при предварительном их чтении из блока АИН1 флаг наличия ошибки связи с АИН2 взведен"));
						return;
					}

					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xF3FF); // бит 10 взведен, бит 11 занулен, два АИНа в системе
					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw | 0x0400); // бит 10 взведен, бит 11 занулен, два АИНа в системе

					var writeAin1SettingsCmd = new WriteAinSettingsCommand(0, settingsForAin1);
					sender.SendCommandAsync(
						_targerAddressHost.TargetAddress,
						writeAin1SettingsCmd,
						_writeSettingsTimeout,
						(sendException, replyBytes) => {
							if (sendException != null) {
								callback(new Exception("Ошибка отправки команды записи настроек АИН1", sendException));
								return;
							}

							// Проверка записи настроек АИН1 путем их повторного чтения
							ReadSettingsAsync(0, (exceptionReRead1, settings1ReReaded) => {
								if (exceptionReRead1 != null) {
									callback(new Exception("Не удалось проконтролировать корректность записи настроек АИН1 путём их повтороного вычитывания"));
									return;
								}
								try {
									settingsForAin1.CompareSettingsAfterReReading(settings1ReReaded, 0);
								}
								catch (Exception compareEx1) {
									callback(new Exception("Ошибка при повторном чтении настроек АИН1", compareEx1));
									return;
								}

								var settingsForAin2 = new AinSettingsWritable(readedAin1Settings);
								settingsForAin2.Imcw = (short) (settingsForAin2.Imcw & 0xFCFF); // бит 8 взведен, бит 9 занулен, ведомый 1
								settingsForAin2.Imcw = (short) (settingsForAin2.Imcw | 0x0100); // бит 8 взведен, бит 9 занулен, ведомый 1

								var writeAin2SettingsCmd = new WriteAinSettingsCommand(1, settingsForAin2);
								sender.SendCommandAsync(
									_targerAddressHost.TargetAddress,
									writeAin2SettingsCmd,
									_writeSettingsTimeout,
									(sendException2, replyBytes2) => {
										if (sendException2 != null) {
											callback(new Exception("Ошибка отправки команды записи настроек АИН2", sendException2));
											return;
										}
										// Проверка записи настроек АИН2 путем их повторного чтения
										ReadSettingsAsync(1, (exceptionReRead2, settings2ReReaded) => {
											if (exceptionReRead2 != null) {
												callback(new Exception("Не удалось проконтролировать корректность записи настроек АИН2 путём их повтороного вычитывания"));
												return;
											}
											try {
												settingsForAin2.CompareSettingsAfterReReading(settings2ReReaded, 1);
											}
											catch (Exception compareEx2) {
												callback(new Exception("Ошибка при повторном чтении настроек АИН2", compareEx2));
												return;
											}
											callback(null);
										});
									});
							});
						});
				}



				else if (ainsCountToWriteSettings == 3) {
					// Когда в системе три блока АИН
					if (readedAin1Settings.Ain1LinkFault) {
						callback(new Exception("Не удалось записать настройки, при предварительном их чтении из блока АИН1 флаг наличия ошибки связи с АИН1 взведен"));
						return;
					}
					if (readedAin1Settings.Ain2LinkFault) {
						callback(new Exception("Не удалось записать настройки, при предварительном их чтении из блока АИН1 флаг наличия ошибки связи с АИН2 взведен"));
						return;
					}
					if (readedAin1Settings.Ain3LinkFault) {
						callback(new Exception("Не удалось записать настройки, при предварительном их чтении из блока АИН1 флаг наличия ошибки связи с АИН2 взведен"));
						return;
					}

					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xF3FF); // бит 10 занулен, бит 11 взведен, три АИНа в системе
					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw | 0x0800); // бит 10 занулен, бит 11 взведен, три АИНа в системе

					var writeAin1SettingsCmd = new WriteAinSettingsCommand(0, settingsForAin1);
					sender.SendCommandAsync(
						_targerAddressHost.TargetAddress,
						writeAin1SettingsCmd,
						_writeSettingsTimeout,
						(sendException, replyBytes) => {
							if (sendException != null) {
								callback(new Exception("Ошибка отправки команды записи настроек АИН1", sendException));
								return;
							}

							// Проверка записи настроек АИН1 путем их повторного чтения
							ReadSettingsAsync(0, (exceptionReRead1, settings1ReReaded) => {
								if (exceptionReRead1 != null) {
									callback(new Exception("Не удалось проконтролировать корректность записи настроек АИН1 путём их повтороного вычитывания"));
									return;
								}
								try {
									settingsForAin1.CompareSettingsAfterReReading(settings1ReReaded, 0);
								}
								catch (Exception compareEx1) {
									callback(new Exception("Ошибка при повторном чтении настроек АИН1", compareEx1));
									return;
								}

								var settingsForAin2 = new AinSettingsWritable(readedAin1Settings);
								settingsForAin2.Imcw = (short) (settingsForAin2.Imcw & 0xFCFF); // бит 8 взведен, бит 9 занулен, ведомый 1
								settingsForAin2.Imcw = (short) (settingsForAin2.Imcw | 0x0100); // бит 8 взведен, бит 9 занулен, ведомый 1

								var writeAin2SettingsCmd = new WriteAinSettingsCommand(1, settingsForAin2);
								sender.SendCommandAsync(
									_targerAddressHost.TargetAddress,
									writeAin2SettingsCmd,
									_writeSettingsTimeout,
									(sendException2, replyBytes2) => {
										if (sendException2 != null) {
											callback(new Exception("Ошибка отправки команды записи настроек АИН2", sendException2));
											return;
										}

										// Проверка записи настроек АИН2 путем их повторного чтения
										ReadSettingsAsync(1, (exceptionReRead2, settings2ReReaded) => {
											if (exceptionReRead2 != null) {
												callback(new Exception("Не удалось проконтролировать корректность записи настроек АИН2 путём их повтороного вычитывания"));
												return;
											}
											try {
												settingsForAin2.CompareSettingsAfterReReading(settings2ReReaded, 1);
											}
											catch (Exception compareEx2) {
												callback(new Exception("Ошибка при повторном чтении настроек АИН2", compareEx2));
												return;
											}

											var settingsForAin3 = new AinSettingsWritable(readedAin1Settings);
											settingsForAin3.Imcw = (short) (settingsForAin3.Imcw & 0xFCFF); // бит 8 занулен, бит 9 взведен, ведомый 2
											settingsForAin3.Imcw = (short) (settingsForAin3.Imcw | 0x0200); // бит 8 занулен, бит 9 взведен, ведомый 2

											var writeAin3SettingsCmd = new WriteAinSettingsCommand(2, settingsForAin2);
											sender.SendCommandAsync(
												_targerAddressHost.TargetAddress,
												writeAin3SettingsCmd,
												_writeSettingsTimeout,
												(sendException3, replyBytes3) => {
													// Проверка записи настроек АИН3 путем их повторного чтения
													ReadSettingsAsync(2, (exceptionReRead3, settings3ReReaded) => {
														if (exceptionReRead3 != null) {
															callback(new Exception("Не удалось проконтролировать корректность записи настроек АИН3 путём их повтороного вычитывания"));
															return;
														}
														try {
															settingsForAin3.CompareSettingsAfterReReading(settings3ReReaded, 2);
														}
														catch (Exception compareEx3) {
															callback(new Exception("Ошибка при повторном чтении настроек АИН3", compareEx3));
															return;
														}
														callback(null);
													});
												});
										});
									});
							});
						});
				}
				else {
					callback.Invoke(new Exception("Неподдердживаемое число блоков АИН: " + ainsCountToWriteSettings + ", поддерживается 1, 2 и 3 блока АИН"));
				}
			});
		}
	}
}