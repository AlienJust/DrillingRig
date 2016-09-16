using DrillingRig.ConfigApp.AppControl.LoggerHost;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	internal interface IParameterLoggerContainer {
		void AddParamLogger(IParameterLogger logger);
	}
}