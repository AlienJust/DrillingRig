using System;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.BsEthernetNominals;

namespace DrillingRig.ConfigApp.BsEthernetNominals {
	internal class BsEthernetNominalsViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;
		private readonly RelayCommand _readNominalsCommand;
		private readonly RelayCommand _writeNominalsCommand;
		private readonly RelayCommand _importNominalsCommand;
		private readonly RelayCommand _exportNominalsCommand;
		private short _ratedRotationFriquencyCalculated;
		private short _ratedPwmModulationCoefficient;
		private short _ratedMomentumCurrentSetting;
		private short _ratedRadiatorTemperature;
		private short _ratedDcBusVoltage;
		private short _ratedAllPhasesCurrentAmplitudeEnvelopeCurve;
		private short _ratedRegulatorCurrentDoutput;
		private short _ratedRegulatorCurrentQoutput;
		private short _ratedFriquencyIntensitySetpointOutput;
		private short _ratedFlowSetting;
		private short _ratedMeasuredMoment;
		private short _ratedSpeedRegulatorOutputOrMomentSetting;
		private short _ratedMeasuredFlow;
		private short _ratedSettingExcitationCurrent;

		public BsEthernetNominalsViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;

			_readNominalsCommand = new RelayCommand(ReadNominals, () => _sendingEnabledControl.IsSendingEnabled);
			_writeNominalsCommand = new RelayCommand(WriteNominals, () => _sendingEnabledControl.IsSendingEnabled);

			_importNominalsCommand = new RelayCommand(ImportNominals);
			_exportNominalsCommand = new RelayCommand(ExportNominals);

			_ratedRotationFriquencyCalculated = 0;
			_ratedPwmModulationCoefficient = 0;
			_ratedMomentumCurrentSetting = 0;
			_ratedRadiatorTemperature = 0;
			_ratedDcBusVoltage = 0;
			_ratedAllPhasesCurrentAmplitudeEnvelopeCurve = 0;
			_ratedRegulatorCurrentDoutput = 0;
			_ratedRegulatorCurrentQoutput = 0;
			_ratedFriquencyIntensitySetpointOutput = 0;
			_ratedFlowSetting = 0;
			_ratedMeasuredMoment = 0;
			_ratedSpeedRegulatorOutputOrMomentSetting = 0;
			_ratedMeasuredFlow = 0;
			_ratedSettingExcitationCurrent = 0;

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool isSendingEnabled) {
			_readNominalsCommand.RaiseCanExecuteChanged();
			_writeNominalsCommand.RaiseCanExecuteChanged();
		}

		private void ExportNominals() {
			_logger.Log("Начало экспорта номинальных значений БС-Ethernet");
			var dialogResult = _windowSystem.ShowSaveFileDialog("Выберите файл для сохранения номинальных значений БС-Ethernet", "XML files|*.xml|All files|*.*");
			if (!string.IsNullOrEmpty(dialogResult)) {
				try {
					var exporter = new BsEthernetNominalsExporterXml(dialogResult);
					exporter.ExportSettings(PropsToNominals());
					_logger.Log("Номинальные значения успешно экспортированы");
				}
				catch (Exception ex) {
					_logger.Log("Произошла ошибка во время экспорта номинальных значений. " + ex.Message);
				}
			}
			else {
				_logger.Log("Экспорт отменен пользователем");
			}
		}

		private void ImportNominals() {
			_logger.Log("Начало импорта номинальных значений БС-Ethernet");
			var dialogResult = _windowSystem.ShowOpenFileDialog("Выберите файл с номинальными значениями БС-Ethernet", "XML files|*.xml|All files|*.*");
			if (!string.IsNullOrEmpty(dialogResult)) {
				try {
					var importer = new BsEthernetNominalsImporterXml(dialogResult);
					var result = importer.ImportSettings();
					SetPropsFromNominals(result);
					_logger.Log("Номинальные значения успешно импортированы");
				}
				catch (Exception ex) {
					_logger.Log("Произошла ошибка во время импорта номинальных значений. " + ex.Message);
				}
			}
			else {
				_logger.Log("Импорт отменен пользователем");
			}
		}

		private void WriteNominals() {
			try {
				_logger.Log("Подготовка к записи номинальных значений БС-Ethernet");

				var cmd = new WriteBsEthernetNominalsCommand(PropsToNominals());
				_logger.Log("Команда записи номинальных значений БС-Ethernet поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(5)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes); // result is unused but GetResult can throw exception
								_logger.Log("Номинальные значения успешно записаны в БС-Ethernet");
							}
							catch (Exception exx) {
								// TODO: log exception about error on answer parsing
								throw new Exception("Ошибка при разборе ответа команды записи номинальных значений: " + exx.Message, exx);
							}
						}
						catch (Exception ex) {
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду записи номинальных значений БС-Ethernet в очередь: " + ex.Message);
			}
		}

		private void ReadNominals() {
			try {
				_logger.Log("Подготовка к чтению номинальных значений БС-Ethernet");

				var cmd = new ReadBsEthernetNominalsCommand();
				_logger.Log("Команда чтения номинальных значений БС-Ethernet поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(5)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								// TODO: do be done
								var result = cmd.GetResult(bytes); // result is unused but GetResult can throw exception
								_userInterfaceRoot.Notifier.Notify(() => SetPropsFromNominals(result));
								_logger.Log("Номинальные значения успешно прочитаны из БС-Ethernet");
							}
							catch (Exception exx) {
								// TODO: log exception about error on answer parsing
								throw new Exception("Ошибка при разборе ответа команды чтения номинальных значений: " + exx.Message, exx);
							}
						}
						catch (Exception ex) {
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду чтения номинальных значений БС-Ethernet в очередь: " + ex.Message);
			}
		}

		private void SetPropsFromNominals(IBsEthernetNominals result) {
			RatedRotationFriquencyCalculated = result.RatedRotationFriquencyCalculated;
			RatedPwmModulationCoefficient = result.RatedPwmModulationCoefficient;
			RatedMomentumCurrentSetting = result.RatedMomentumCurrentSetting;
			RatedRadiatorTemperature = result.RatedRadiatorTemperature;
			RatedDcBusVoltage = result.RatedDcBusVoltage;
			RatedAllPhasesCurrentAmplitudeEnvelopeCurve = result.RatedAllPhasesCurrentAmplitudeEnvelopeCurve;
			RatedRegulatorCurrentDoutput = result.RatedRegulatorCurrentDoutput;
			RatedRegulatorCurrentQoutput = result.RatedRegulatorCurrentQoutput;
			RatedFriquencyIntensitySetpointOutput = result.RatedFriquencyIntensitySetpointOutput;
			RatedFlowSetting = result.RatedFlowSetting;
			RatedMeasuredMoment = result.RatedMeasuredMoment;
			RatedSpeedRegulatorOutputOrMomentSetting = result.RatedSpeedRegulatorOutputOrMomentSetting;
			RatedMeasuredFlow = result.RatedMeasuredFlow;
			RatedSettingExcitationCurrent = result.RatedSettingExcitationCurrent;
		}

		private IBsEthernetNominals PropsToNominals() {
			return new BsEthernetNominalsSimple(
				_ratedRotationFriquencyCalculated,
				_ratedPwmModulationCoefficient,
				_ratedMomentumCurrentSetting,
				_ratedRadiatorTemperature,
				_ratedDcBusVoltage,
				_ratedAllPhasesCurrentAmplitudeEnvelopeCurve,
				_ratedRegulatorCurrentDoutput,
				_ratedRegulatorCurrentQoutput,
				_ratedFriquencyIntensitySetpointOutput,
				_ratedFlowSetting,
				_ratedMeasuredMoment,
				_ratedSpeedRegulatorOutputOrMomentSetting,
				_ratedMeasuredFlow,
				_ratedSettingExcitationCurrent);
		}

		public short RatedRotationFriquencyCalculated {
			get { return _ratedRotationFriquencyCalculated; }
			set {
				if (_ratedRotationFriquencyCalculated != value) {
					_ratedRotationFriquencyCalculated = value;
					RaisePropertyChanged(() => RatedRotationFriquencyCalculated);
				}
			}
		}

		public short RatedPwmModulationCoefficient {
			get { return _ratedPwmModulationCoefficient; }
			set {
				if (_ratedPwmModulationCoefficient != value) {
					_ratedPwmModulationCoefficient = value;
					RaisePropertyChanged(() => RatedPwmModulationCoefficient);
				}
			}
		}

		public short RatedMomentumCurrentSetting {
			get { return _ratedMomentumCurrentSetting; }
			set {
				if (_ratedMomentumCurrentSetting != value) {
					_ratedMomentumCurrentSetting = value;
					RaisePropertyChanged(() => RatedMomentumCurrentSetting);
				}
			}
		}

		public short RatedRadiatorTemperature {
			get { return _ratedRadiatorTemperature; }
			set {
				if (_ratedRadiatorTemperature != value) {
					_ratedRadiatorTemperature = value;
					RaisePropertyChanged(() => RatedRadiatorTemperature);
				}
			}
		}

		public short RatedDcBusVoltage {
			get { return _ratedDcBusVoltage; }
			set {
				if (_ratedDcBusVoltage != value) {
					_ratedDcBusVoltage = value;
					RaisePropertyChanged(() => RatedDcBusVoltage);
				}
			}
		}

		public short RatedAllPhasesCurrentAmplitudeEnvelopeCurve {
			get { return _ratedAllPhasesCurrentAmplitudeEnvelopeCurve; }
			set {
				if (_ratedAllPhasesCurrentAmplitudeEnvelopeCurve != value) {
					_ratedAllPhasesCurrentAmplitudeEnvelopeCurve = value;
					RaisePropertyChanged(() => RatedAllPhasesCurrentAmplitudeEnvelopeCurve);
				}
			}
		}

		public short RatedRegulatorCurrentDoutput {
			get { return _ratedRegulatorCurrentDoutput; }
			set {
				if (_ratedRegulatorCurrentDoutput != value) {
					_ratedRegulatorCurrentDoutput = value;
					RaisePropertyChanged(() => RatedRegulatorCurrentDoutput);
				}
			}
		}

		public short RatedRegulatorCurrentQoutput {
			get { return _ratedRegulatorCurrentQoutput; }
			set {
				if (_ratedRegulatorCurrentQoutput != value) {
					_ratedRegulatorCurrentQoutput = value;
					RaisePropertyChanged(() => RatedRegulatorCurrentQoutput);
				}
			}
		}

		public short RatedFriquencyIntensitySetpointOutput {
			get { return _ratedFriquencyIntensitySetpointOutput; }
			set {
				if (_ratedFriquencyIntensitySetpointOutput != value) {
					_ratedFriquencyIntensitySetpointOutput = value;
					RaisePropertyChanged(() => RatedFriquencyIntensitySetpointOutput);
				}
			}
		}

		public short RatedFlowSetting {
			get { return _ratedFlowSetting; }
			set {
				if (_ratedFlowSetting != value) {
					_ratedFlowSetting = value;
					RaisePropertyChanged(() => RatedFlowSetting);
				}
			}
		}

		public short RatedMeasuredMoment {
			get { return _ratedMeasuredMoment; }
			set {
				if (_ratedMeasuredMoment != value) {
					_ratedMeasuredMoment = value;
					RaisePropertyChanged(() => RatedMeasuredMoment);
				}
			}
		}

		public short RatedSpeedRegulatorOutputOrMomentSetting {
			get { return _ratedSpeedRegulatorOutputOrMomentSetting; }
			set {
				if (_ratedSpeedRegulatorOutputOrMomentSetting != value) {
					_ratedSpeedRegulatorOutputOrMomentSetting = value;
					RaisePropertyChanged(() => RatedSpeedRegulatorOutputOrMomentSetting);
				}
			}
		}

		public short RatedMeasuredFlow {
			get { return _ratedMeasuredFlow; }
			set {
				if (_ratedMeasuredFlow != value) {
					_ratedMeasuredFlow = value;
					RaisePropertyChanged(() => RatedMeasuredFlow);
				}
			}
		}

		public short RatedSettingExcitationCurrent {
			get { return _ratedSettingExcitationCurrent; }
			set {
				if (_ratedSettingExcitationCurrent != value) {
					_ratedSettingExcitationCurrent = value;
					RaisePropertyChanged(() => RatedSettingExcitationCurrent);
				}
			}
		}

		public ICommand ReadNominalsCommand {
			get { return _readNominalsCommand; }
		}

		public ICommand WriteNominalsCommand {
			get { return _writeNominalsCommand; }
		}

		public RelayCommand ImportNominalsCommand {
			get { return _importNominalsCommand; }
		}

		public RelayCommand ExportNominalsCommand {
			get { return _exportNominalsCommand; }
		}
	}
}
