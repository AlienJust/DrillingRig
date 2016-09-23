using System.Collections.Generic;

namespace DrillingRig.ConfigApp.AppControl.ParamLogger {
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


		public void LogAnalogueParameter(string parameterName, double? value) {
			lock (_syncLoggers) {
				foreach (var logger in _registredLoggers) {
					logger.LogAnalogueParameter(parameterName, value);
				}
			}
		}

		public void LogDiscreteParameter(string parameterName, bool? value) {
			lock (_syncLoggers) {
				foreach (var logger in _registredLoggers) {
					logger.LogDiscreteParameter(parameterName, value);
				}
			}
		}

		public void RemoveSeries(string parameterName) {
			lock (_syncLoggers) {
				foreach (var logger in _registredLoggers) {
					logger.RemoveSeries(parameterName);
				}
			}
		}
	}
}