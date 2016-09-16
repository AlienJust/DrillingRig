using System.Collections.Generic;
using AlienJust.Support.Loggers.Contracts;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.LookedLikeAbb;

namespace DrillingRig.ConfigApp.AppControl.ParamLogger {
	internal interface IParamLoggerRegistrationPoint {
		void RegisterParamLoggegr(IParameterLogger logger);
	}

	class ParamLoggerRegistrationPointThreadSafe : IParamLoggerRegistrationPoint, IParameterLogger {
		private readonly List<IParameterLogger> _registredLoggers;
		private readonly object _syncLoggers;

		public ParamLoggerRegistrationPointThreadSafe() {
			_registredLoggers = new List<IParameterLogger>();
			_syncLoggers = new object();
		}

		public void RegisterLoggegr(IParameterLogger logger) {
			lock (_syncLoggers) {
				_registredLoggers.Add(logger);
			}
		}

		
	}
}