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
	}
}
