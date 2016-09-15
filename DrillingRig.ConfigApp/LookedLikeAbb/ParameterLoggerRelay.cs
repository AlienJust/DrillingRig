using System.Collections.Generic;

namespace DrillingRig.ConfigApp.LookedLikeAbb {
	class ParameterLoggerRelay : IParameterLogger, IParameterLoggerContainer {
		private readonly List<IParameterLogger> _relayOnLoggers;
		public ParameterLoggerRelay(List<IParameterLogger> relayOnLoggers) {
			_relayOnLoggers = relayOnLoggers;
		}

		public void LogAnalogueParameter(string parameterName, double? value) {
			foreach (var parameterLogger in _relayOnLoggers) {
				parameterLogger.LogAnalogueParameter(parameterName, value);
			}
		}

		public void LogDiscreteParameter(string parameterName, bool? value) {
			foreach (var parameterLogger in _relayOnLoggers) {
				parameterLogger.LogDiscreteParameter(parameterName, value);
			}
		}

		public void RemoveSeries(string parameterName) {
			foreach (var parameterLogger in _relayOnLoggers) {
				parameterLogger.RemoveSeries(parameterName);
			}
		}

		public void AddParamLogger(IParameterLogger logger) {
			_relayOnLoggers.Add(logger);
		}
	}
}