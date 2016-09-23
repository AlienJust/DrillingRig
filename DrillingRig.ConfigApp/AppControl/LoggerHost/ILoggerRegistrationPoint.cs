using AlienJust.Support.Loggers.Contracts;

namespace DrillingRig.ConfigApp.AppControl.LoggerHost {
	internal interface ILoggerRegistrationPoint {
		void RegisterLoggegr(ILogger logger);
	}
}