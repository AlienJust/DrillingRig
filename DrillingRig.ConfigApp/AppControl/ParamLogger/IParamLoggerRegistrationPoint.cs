using AlienJust.Support.Loggers.Contracts;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.LookedLikeAbb;

namespace DrillingRig.ConfigApp.AppControl.ParamLogger {
	internal interface IParamLoggerRegistrationPoint {
		void RegisterLoggegr(IParameterLogger logger);
	}
}