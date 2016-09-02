using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw {
	internal class AinSettingsWriter : IAinSettingsWriter {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IAinsCounter _ainsCounter;
		private readonly IAinSettingsReader _ainSettingsReader;

		private readonly object _ainsCountSyncObject;
		private readonly TimeSpan _writeSettingsTimeout;

		public AinSettingsWriter(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IAinsCounter ainsCounter, IAinSettingsReader ainSettingsReader) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_ainsCounter = ainsCounter;
			_ainSettingsReader = ainSettingsReader;

			_ainsCountSyncObject = new object();

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

		public void WriteSettingsAsync(IAinSettingsPart settingsPart, Action<Exception> callback) {
			var sender = _commandSenderHost.Sender;
			if (sender == null) throw new NullReferenceException("���� �������� ������ �� ������");

			int ainsCountToWriteSettings = AinsCountThreadSafe;

			// ������ ������ ��������� ����� �������
			_ainSettingsReader.ReadSettingsAsync(0, (readSettingsException, readedAin1Settings) => {
				if (readSettingsException != null) {
					callback(new Exception("�� ������� �������� ���������, �������� ������ ��� ��������������� �� ������ �� ����� ���1", readSettingsException));
					return;
				}

				var settingsForAin1 = new AinSettingsWritable(readedAin1Settings);
				settingsForAin1.ModifyFromPart(settingsPart);
				settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xFCFF); // ���� 8 � 9 ��������, �������



				if (ainsCountToWriteSettings == 1) {
					// ����� � ������� ���� ���� ���
					if (readedAin1Settings.Ain1LinkFault) {
						callback(new Exception("�� ������� �������� ���������, ��� ��������������� �� ������ �� ����� ���1 ���� ������� ������ ����� � ���1 �������"));
						return;
					}

					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xF0FF); // ���� 8,9,10 � 11 ��������, �������� ������
					var writeAin1SettingsCmd = new WriteAinSettingsCommand(0, settingsForAin1);
					sender.SendCommandAsync(
						_targerAddressHost.TargetAddress,
						writeAin1SettingsCmd,
						_writeSettingsTimeout,
						(sendException, replyBytes) => {
							if (sendException != null) {
								callback(new Exception("������ �������� ������� ������ �������� ���1", sendException));
								return;
							}

                            // ����� 300 �� ��� ����, ����� ��� ����� �������� ����� ������ � EEPROM,
                            // � ����� ��-Ethernet ����� �� �������� �� ���.
                            System.Threading.Thread.Sleep(300);

                            // �������� ������ �������� ���1 ����� �� ���������� ������
                            _ainSettingsReader.ReadSettingsAsync(0, (exceptionReRead1, settings1ReReaded) => {
								if (exceptionReRead1 != null) {
									callback(new Exception("�� ������� ����������������� ������������ ������ �������� ���1 ���� �� ����������� �����������"));
									return;
								}
								try {
									settingsForAin1.CompareSettingsAfterReReading(settings1ReReaded, 0);
								}
								catch (Exception compareEx1) {
									callback(new Exception("������ ��� ��������� ������ �������� ���1", compareEx1));
									return;
								}
								callback(null);
							});
						});
				}



				else if (ainsCountToWriteSettings == 2) {
					// ����� � ������� ��� ����� ���
					if (readedAin1Settings.Ain1LinkFault) {
						callback(new Exception("�� ������� �������� ���������, ��� ��������������� �� ������ �� ����� ���1 ���� ������� ������ ����� � ���1 �������"));
						return;
					}
					if (readedAin1Settings.Ain2LinkFault) {
						callback(new Exception("�� ������� �������� ���������, ��� ��������������� �� ������ �� ����� ���1 ���� ������� ������ ����� � ���2 �������"));
						return;
					}

					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xF0FF); // ���� 8,9,11 ��������, ��� ���� � �������
					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw | 0x0400); // ��� 10 �������, ��� ���� � �������

					var writeAin1SettingsCmd = new WriteAinSettingsCommand(0, settingsForAin1);
					sender.SendCommandAsync(
						_targerAddressHost.TargetAddress,
						writeAin1SettingsCmd,
						_writeSettingsTimeout,
						(sendException, replyBytes) => {
							if (sendException != null) {
								callback(new Exception("������ �������� ������� ������ �������� ���1", sendException));
								return;
							}

                            // ����� 300 �� ��� ����, ����� ��� ����� �������� ����� ������ � EEPROM,
                            // � ����� ��-Ethernet ����� �� �������� �� ���.
                            System.Threading.Thread.Sleep(300);

                            // �������� ������ �������� ���1 ����� �� ���������� ������
                            _ainSettingsReader.ReadSettingsAsync(0, (exceptionReRead1, settings1ReReaded) => {
								if (exceptionReRead1 != null) {
									callback(new Exception("�� ������� ����������������� ������������ ������ �������� ���1 ���� �� ����������� �����������"));
									return;
								}
								try {
									settingsForAin1.CompareSettingsAfterReReading(settings1ReReaded, 0);
								}
								catch (Exception compareEx1) {
									callback(new Exception("������ ��� ��������� ������ �������� ���1", compareEx1));
									return;
								}

								var settingsForAin2 = new AinSettingsWritable(readedAin1Settings);
								settingsForAin2.Imcw = (short) (settingsForAin2.Imcw & 0xF0FF); // ���� 9,11 ��������, ������� 1
                                settingsForAin2.Imcw = (short) (settingsForAin2.Imcw | 0x0500); // ���� 8,10 ��������,, ������� 1

								var writeAin2SettingsCmd = new WriteAinSettingsCommand(1, settingsForAin2);
								sender.SendCommandAsync(
									_targerAddressHost.TargetAddress,
									writeAin2SettingsCmd,
									_writeSettingsTimeout,
									(sendException2, replyBytes2) => {
										if (sendException2 != null) {
											callback(new Exception("������ �������� ������� ������ �������� ���2", sendException2));
											return;
										}

                                        // ����� 300 �� ��� ����, ����� ��� ����� �������� ����� ������ � EEPROM,
                                        // � ����� ��-Ethernet ����� �� �������� �� ���.
                                        System.Threading.Thread.Sleep(300);

                                        // �������� ������ �������� ���2 ����� �� ���������� ������
                                        _ainSettingsReader.ReadSettingsAsync(1, (exceptionReRead2, settings2ReReaded) => {
											if (exceptionReRead2 != null) {
												callback(new Exception("�� ������� ����������������� ������������ ������ �������� ���2 ���� �� ����������� �����������"));
												return;
											}
											try {
												settingsForAin2.CompareSettingsAfterReReading(settings2ReReaded, 1);
											}
											catch (Exception compareEx2) {
												callback(new Exception("������ ��� ��������� ������ �������� ���2", compareEx2));
												return;
											}
											callback(null);
										});
									});
							});
						});
				}



				else if (ainsCountToWriteSettings == 3) {
					// ����� � ������� ��� ����� ���
					if (readedAin1Settings.Ain1LinkFault) {
						callback(new Exception("�� ������� �������� ���������, ��� ��������������� �� ������ �� ����� ���1 ���� ������� ������ ����� � ���1 �������"));
						return;
					}
					if (readedAin1Settings.Ain2LinkFault) {
						callback(new Exception("�� ������� �������� ���������, ��� ��������������� �� ������ �� ����� ���1 ���� ������� ������ ����� � ���2 �������"));
						return;
					}
					if (readedAin1Settings.Ain3LinkFault) {
						callback(new Exception("�� ������� �������� ���������, ��� ��������������� �� ������ �� ����� ���1 ���� ������� ������ ����� � ���2 �������"));
						return;
					}

					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw & 0xF0FF); // ���� 8.9.10 ��������, ��� ���� � �������
					settingsForAin1.Imcw = (short) (settingsForAin1.Imcw | 0x0800); // ��� 11 �������, ��� ���� � �������

					var writeAin1SettingsCmd = new WriteAinSettingsCommand(0, settingsForAin1);
					sender.SendCommandAsync(
						_targerAddressHost.TargetAddress,
						writeAin1SettingsCmd,
						_writeSettingsTimeout,
						(sendException, replyBytes) => {
							if (sendException != null) {
								callback(new Exception("������ �������� ������� ������ �������� ���1", sendException));
								return;
							}

                            // ����� 300 �� ��� ����, ����� ��� ����� �������� ����� ������ � EEPROM,
                            // � ����� ��-Ethernet ����� �� �������� �� ���.
                            System.Threading.Thread.Sleep(300);

                            // �������� ������ �������� ���1 ����� �� ���������� ������
                            _ainSettingsReader.ReadSettingsAsync(0, (exceptionReRead1, settings1ReReaded) => {
								if (exceptionReRead1 != null) {
									callback(new Exception("�� ������� ����������������� ������������ ������ �������� ���1 ���� �� ����������� �����������"));
									return;
								}
								try {
									settingsForAin1.CompareSettingsAfterReReading(settings1ReReaded, 0);
								}
								catch (Exception compareEx1) {
									callback(new Exception("������ ��� ��������� ������ �������� ���1", compareEx1));
									return;
								}

								var settingsForAin2 = new AinSettingsWritable(readedAin1Settings);
								settingsForAin2.Imcw = (short) (settingsForAin2.Imcw & 0xF0FF); // ���� 9,10 ��������, ������� 1
                                settingsForAin2.Imcw = (short) (settingsForAin2.Imcw | 0x0900); // ���� 8,11 ��������, ������� 1

                                var writeAin2SettingsCmd = new WriteAinSettingsCommand(1, settingsForAin2);
								sender.SendCommandAsync(
									_targerAddressHost.TargetAddress,
									writeAin2SettingsCmd,
									_writeSettingsTimeout,
									(sendException2, replyBytes2) => {
										if (sendException2 != null) {
											callback(new Exception("������ �������� ������� ������ �������� ���2", sendException2));
											return;
										}

                                        // ����� 300 �� ��� ����, ����� ��� ����� �������� ����� ������ � EEPROM,
                                        // � ����� ��-Ethernet ����� �� �������� �� ���.
                                        System.Threading.Thread.Sleep(300);

                                        // �������� ������ �������� ���2 ����� �� ���������� ������
                                        _ainSettingsReader.ReadSettingsAsync(1, (exceptionReRead2, settings2ReReaded) => {
											if (exceptionReRead2 != null) {
												callback(new Exception("�� ������� ����������������� ������������ ������ �������� ���2 ���� �� ����������� �����������"));
												return;
											}
											try {
												settingsForAin2.CompareSettingsAfterReReading(settings2ReReaded, 1);
											}
											catch (Exception compareEx2) {
												callback(new Exception("������ ��� ��������� ������ �������� ���2", compareEx2));
												return;
											}

											var settingsForAin3 = new AinSettingsWritable(readedAin1Settings);
											settingsForAin3.Imcw = (short) (settingsForAin3.Imcw & 0xF0FF); // ���� 8,10 ��������, ������� 2
                                            settingsForAin3.Imcw = (short) (settingsForAin3.Imcw | 0x0A00); // ���� 9,11 ��������, ������� 2

											var writeAin3SettingsCmd = new WriteAinSettingsCommand(2, settingsForAin3);
											sender.SendCommandAsync(
												_targerAddressHost.TargetAddress,
												writeAin3SettingsCmd,
												_writeSettingsTimeout,
												(sendException3, replyBytes3) => {

                                                    // ����� 300 �� ��� ����, ����� ��� ����� �������� ����� ������ � EEPROM,
                                                    // � ����� ��-Ethernet ����� �� �������� �� ���.
                                                    System.Threading.Thread.Sleep(300);

                                                    // �������� ������ �������� ���3 ����� �� ���������� ������
                                                    _ainSettingsReader.ReadSettingsAsync(2, (exceptionReRead3, settings3ReReaded) => {
														if (exceptionReRead3 != null) {
															callback(new Exception("�� ������� ����������������� ������������ ������ �������� ���3 ���� �� ����������� �����������"));
															return;
														}
														try {
															settingsForAin3.CompareSettingsAfterReReading(settings3ReReaded, 2);
														}
														catch (Exception compareEx3) {
															callback(new Exception("������ ��� ��������� ������ �������� ���3", compareEx3));
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
					callback.Invoke(new Exception("����������������� ����� ������ ���: " + ainsCountToWriteSettings + ", �������������� 1, 2 � 3 ����� ���"));
				}
			});
		}
	}
}