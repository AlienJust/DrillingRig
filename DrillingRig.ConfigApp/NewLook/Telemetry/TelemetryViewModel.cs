using AlienJust.Support.Loggers.Contracts;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.Cycle;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;
using DrillingRig.ConfigApp.CommandSenderHost;
using DrillingRig.ConfigApp.LookedLikeAbb;
using DrillingRig.ConfigApp.LookedLikeAbb.Group08Parameters;

namespace DrillingRig.ConfigApp.NewLook.Telemetry {
	class TelemetryViewModel {
		public Group01ParametersViewModel Group01ParametersVm { get; }
		public Group02ParametersViewModel Group02ParametersVm { get; }
		public Group03ParametersViewModel Group03ParametersVm { get; }
		public Group04ParametersViewModel Group04ParametersVm { get; }
		public Group07ParametersViewModel Group07ParametersVm { get; }
		public Group08ParametersViewModel Group08ParametersVm { get; }
		public Group09ParametersViewModel Group09ParametersVm { get; }

		public TelemetryViewModel(IUserInterfaceRoot userInterfaceRoot, ICommandSenderHost commanSenderHost, ITargetAddressHost targetAddressHost, ILogger logger, ICycleThreadHolder cycleThreadHolder, IAinsCounter ainsCounter, IParameterLogger parameterLogger) {
			Group01ParametersVm = new Group01ParametersViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, ainsCounter, parameterLogger);
			cycleThreadHolder.RegisterAsCyclePart(Group01ParametersVm);

			Group02ParametersVm = new Group02ParametersViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, parameterLogger);
			cycleThreadHolder.RegisterAsCyclePart(Group02ParametersVm);

			Group03ParametersVm = new Group03ParametersViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, parameterLogger);
			cycleThreadHolder.RegisterAsCyclePart(Group03ParametersVm);

			Group04ParametersVm = new Group04ParametersViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, parameterLogger);
			cycleThreadHolder.RegisterAsCyclePart(Group04ParametersVm);

			Group07ParametersVm = new Group07ParametersViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, parameterLogger);
			cycleThreadHolder.RegisterAsCyclePart(Group07ParametersVm);

			Group08ParametersVm = new Group08ParametersViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, parameterLogger);
			cycleThreadHolder.RegisterAsCyclePart(Group08ParametersVm);

			Group09ParametersVm = new Group09ParametersViewModel(commanSenderHost, targetAddressHost, userInterfaceRoot, logger, ainsCounter, parameterLogger);
			cycleThreadHolder.RegisterAsCyclePart(Group09ParametersVm);
		}
	}
}
