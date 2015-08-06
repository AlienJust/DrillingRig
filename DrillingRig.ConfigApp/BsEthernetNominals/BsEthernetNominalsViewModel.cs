using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;

namespace DrillingRig.ConfigApp.BsEthernetNominals
{
	class BsEthernetNominalsViewModel :ViewModelBase
	{
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly RelayCommand _readSettingCommand;
		private readonly RelayCommand _writeSettingsCommand;
		private readonly RelayCommand _importSettingCommand;
		private readonly RelayCommand _exportSettingsCommand;
		private ushort _ratedRotationFriquencyCalculated;
		private ushort _ratedPwmModulationCoefficient;
		private ushort _ratedMomentumCurrentSetting;
		private ushort _ratedRadiatorTemperature;
		private ushort _ratedDcBusVoltage;
		private ushort _ratedAllPhasesCurrentAmplitudeEnvelopeCurve;
		private ushort _ratedRegulatorCurrentDoutput;
		private ushort _ratedRegulatorCurrentQoutput;
		private ushort _ratedFriquencyIntensitySetpointOutput;
		private ushort _ratedFlowSetting;
		private ushort _ratedMeasuredMoment;
		private ushort _ratedSpeedRegulatorOutputOrMomentSetting;
		private ushort _ratedMeasuredFlow;
		private ushort _ratedSettingExcitationCurrent;

		public BsEthernetNominalsViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;

			_readSettingCommand = new RelayCommand(ReadNominals);
			_writeSettingsCommand = new RelayCommand(WriteNominals);

			_importSettingCommand = new RelayCommand(ImportNominals);
			_exportSettingsCommand = new RelayCommand(ExportNominals);

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
		}

		private void ExportNominals() {
			throw new NotImplementedException();
		}

		private void ImportNominals() {
			throw new NotImplementedException();
		}

		private void WriteNominals() {
			throw new NotImplementedException();
		}

		private void ReadNominals() {
			throw new NotImplementedException();
		}

		public ushort RatedRotationFriquencyCalculated {
			get { return _ratedRotationFriquencyCalculated; }
			set {
				if (_ratedRotationFriquencyCalculated != value) {
					_ratedRotationFriquencyCalculated = value;
					RaisePropertyChanged(() => RatedRotationFriquencyCalculated);
				}
			}
		}

		public ushort RatedPwmModulationCoefficient {
			get { return _ratedPwmModulationCoefficient; }
			set
			{
				if (_ratedPwmModulationCoefficient != value)
				{
					_ratedPwmModulationCoefficient = value;
					RaisePropertyChanged(() => RatedPwmModulationCoefficient);
				}
			}
		}

		public ushort RatedMomentumCurrentSetting {
			get { return _ratedMomentumCurrentSetting; }
			set
			{
				if (_ratedMomentumCurrentSetting != value)
				{
					_ratedMomentumCurrentSetting = value;
					RaisePropertyChanged(() => RatedMomentumCurrentSetting);
				}
			}
		}

		public ushort RatedRadiatorTemperature {
			get { return _ratedRadiatorTemperature; }
			set
			{
				if (_ratedRadiatorTemperature != value)
				{
					_ratedRadiatorTemperature = value;
					RaisePropertyChanged(() => RatedRadiatorTemperature);
				}
			}
		}

		public ushort RatedDcBusVoltage {
			get { return _ratedDcBusVoltage; }
			set
			{
				if (_ratedDcBusVoltage != value)
				{
					_ratedDcBusVoltage = value;
					RaisePropertyChanged(() => RatedDcBusVoltage);
				}
			}
		}

		public ushort RatedAllPhasesCurrentAmplitudeEnvelopeCurve {
			get { return _ratedAllPhasesCurrentAmplitudeEnvelopeCurve; }
			set
			{
				if (_ratedAllPhasesCurrentAmplitudeEnvelopeCurve != value)
				{
					_ratedAllPhasesCurrentAmplitudeEnvelopeCurve = value;
					RaisePropertyChanged(() => RatedAllPhasesCurrentAmplitudeEnvelopeCurve);
				}
			}
		}

		public ushort RatedRegulatorCurrentDoutput {
			get { return _ratedRegulatorCurrentDoutput; }
			set
			{
				if (_ratedRegulatorCurrentDoutput != value)
				{
					_ratedRegulatorCurrentDoutput = value;
					RaisePropertyChanged(() => RatedRegulatorCurrentDoutput);
				}
			}
		}

		public ushort RatedRegulatorCurrentQoutput {
			get { return _ratedRegulatorCurrentQoutput; }
			set
			{
				if (_ratedRegulatorCurrentQoutput != value)
				{
					_ratedRegulatorCurrentQoutput = value;
					RaisePropertyChanged(() => RatedRegulatorCurrentQoutput);
				}
			}
		}

		public ushort RatedFriquencyIntensitySetpointOutput {
			get { return _ratedFriquencyIntensitySetpointOutput; }
			set
			{
				if (_ratedFriquencyIntensitySetpointOutput != value)
				{
					_ratedFriquencyIntensitySetpointOutput = value;
					RaisePropertyChanged(() => RatedFriquencyIntensitySetpointOutput);
				}
			}
		}

		public ushort RatedFlowSetting {
			get { return _ratedFlowSetting; }
			set
			{
				if (_ratedFlowSetting != value)
				{
					_ratedFlowSetting = value;
					RaisePropertyChanged(() => RatedFlowSetting);
				}
			}
		}

		public ushort RatedMeasuredMoment {
			get { return _ratedMeasuredMoment; }
			set
			{
				if (_ratedMeasuredMoment != value)
				{
					_ratedMeasuredMoment = value;
					RaisePropertyChanged(() => RatedMeasuredMoment);
				}
			}
		}

		public ushort RatedSpeedRegulatorOutputOrMomentSetting {
			get { return _ratedSpeedRegulatorOutputOrMomentSetting; }
			set
			{
				if (_ratedSpeedRegulatorOutputOrMomentSetting != value)
				{
					_ratedSpeedRegulatorOutputOrMomentSetting = value;
					RaisePropertyChanged(() => RatedSpeedRegulatorOutputOrMomentSetting);
				}
			}
		}

		public ushort RatedMeasuredFlow {
			get { return _ratedMeasuredFlow; }
			set
			{
				if (_ratedMeasuredFlow != value)
				{
					_ratedMeasuredFlow = value;
					RaisePropertyChanged(() => RatedMeasuredFlow);
				}
			}
		}

		public ushort RatedSettingExcitationCurrent {
			get { return _ratedSettingExcitationCurrent; }
			set
			{
				if (_ratedSettingExcitationCurrent != value)
				{
					_ratedSettingExcitationCurrent = value;
					RaisePropertyChanged(() => RatedSettingExcitationCurrent);
				}
			}
		}
	}
}
