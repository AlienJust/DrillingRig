﻿using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.ConfigApp.AinCommand;
using DrillingRig.ConfigApp.AinsSettings;
using DrillingRig.ConfigApp.AinTelemetry;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.BsEthernetNominals;
using DrillingRig.ConfigApp.BsEthernetSettings;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.CoolerTelemetry;
using DrillingRig.ConfigApp.EngineSettings;
using DrillingRig.ConfigApp.RectifierTelemetry;
using DrillingRig.ConfigApp.SystemControl;

namespace DrillingRig.ConfigApp.NewLook.OldLook {
	class OldLookViewModel {
		private readonly IAinSettingsStorage _ainSettingsStorage;
		private readonly IAinSettingsStorageUpdatedNotify _storageUpdatedNotify;
		public BsEthernetSettingsViewModel BsEthernetSettingsVm { get; }

		public BsEthernetNominalsViewModel BsEthernetNominalsVm { get; }

		public AinTelemetriesViewModel AinTelemetriesVm { get; }

		public AinCommandViewModel Ain1CommandVm { get; }

		public AinCommandViewModel Ain2CommandVm { get; }

		public AinCommandViewModel Ain3CommandVm { get; }

		public SystemControlViewModel SystemControlVm { get; }

		public RectifierTelemetriesViewModel RectifierTelemetriesVm { get; }

		public CoolerTelemetriesViewModel CoolerTelemetriesVm { get; }

		public AinSettingsViewModel Ain1SettingsVm { get; }

		public AinSettingsViewModel Ain2SettingsVm { get; }

		public AinSettingsViewModel Ain3SettingsVm { get; }

		public EngineSettingsViewModel EngineSettingsVm { get; }

		public OldLookViewModel(IUserInterfaceRoot userInterfaceRoot, IWindowSystem windowSystem, ICommandSenderHost commanSenderHost, ITargetAddressHost targetAddressHost, INotifySendingEnabled notifySendingEnabled, ILinkContol linkContol, ILogger logger, IMultiLoggerWithStackTrace<int> debugLogger, ICycleThreadHolder cycleThreadHolder, IAinsCounter ainsCounter, IParameterLogger parameterLogger, IAinSettingsStorage ainSettingsStorage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify) {
			_ainSettingsStorage = ainSettingsStorage;
			_storageUpdatedNotify = storageUpdatedNotify;
			var commonTelemetryVm = new TelemetryCommonViewModel();

			BsEthernetSettingsVm = new BsEthernetSettingsViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem, notifySendingEnabled);
			BsEthernetNominalsVm = new BsEthernetNominalsViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem, notifySendingEnabled);

			SystemControlVm = new SystemControlViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem, notifySendingEnabled, linkContol, commonTelemetryVm);

			var ain1TelemetryVm = new AinTelemetryViewModel(commonTelemetryVm, 0, commanSenderHost, logger, userInterfaceRoot);
			var ain2TelemetryVm = new AinTelemetryViewModel(commonTelemetryVm, 1, commanSenderHost, logger, userInterfaceRoot);
			var ain3TelemetryVm = new AinTelemetryViewModel(commonTelemetryVm, 2, commanSenderHost, logger, userInterfaceRoot);

			AinTelemetriesVm = new AinTelemetriesViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem, SystemControlVm, commonTelemetryVm, ain1TelemetryVm, ain2TelemetryVm, ain3TelemetryVm); // TODO: sending enabled control?

			cycleThreadHolder.RegisterAsCyclePart(ain1TelemetryVm);
			cycleThreadHolder.RegisterAsCyclePart(ain2TelemetryVm);
			cycleThreadHolder.RegisterAsCyclePart(ain3TelemetryVm);
			cycleThreadHolder.RegisterAsCyclePart(AinTelemetriesVm);

			var ain1CommandOnlyVm = new AinCommandAndMinimalCommonTelemetryViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, notifySendingEnabled, 0, _ainSettingsStorage, _storageUpdatedNotify);
			var ain2CommandOnlyVm = new AinCommandAndMinimalCommonTelemetryViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, notifySendingEnabled, 1, _ainSettingsStorage, _storageUpdatedNotify);
			var ain3CommandOnlyVm = new AinCommandAndMinimalCommonTelemetryViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, notifySendingEnabled, 2, _ainSettingsStorage, _storageUpdatedNotify);

			Ain1CommandVm = new AinCommandViewModel(ain1CommandOnlyVm, commonTelemetryVm, ain1TelemetryVm, AinTelemetriesVm);
			Ain2CommandVm = new AinCommandViewModel(ain2CommandOnlyVm, commonTelemetryVm, ain2TelemetryVm, AinTelemetriesVm);
			Ain3CommandVm = new AinCommandViewModel(ain3CommandOnlyVm, commonTelemetryVm, ain3TelemetryVm, AinTelemetriesVm);

			Ain1SettingsVm = new AinSettingsViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem, notifySendingEnabled, 0);
			Ain2SettingsVm = new AinSettingsViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem, notifySendingEnabled, 1);
			Ain3SettingsVm = new AinSettingsViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem, notifySendingEnabled, 2);

			RectifierTelemetriesVm = new RectifierTelemetriesViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem); // TODO: sending enabled control?
			cycleThreadHolder.RegisterAsCyclePart(RectifierTelemetriesVm);

			CoolerTelemetriesVm = new CoolerTelemetriesViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, debugLogger, windowSystem); // TODO: sending enabled control?

			EngineSettingsVm = new EngineSettingsViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, windowSystem, notifySendingEnabled);
		}
	}
}
