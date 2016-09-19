namespace DrillingRig.ConfigApp.AppControl.ParamLogger {
	public interface IParameterLogger {
		void LogAnalogueParameter(string parameterName, double? value);
		void LogDiscreteParameter(string parameterName, bool? value);
		void RemoveSeries(string parameterName);
	}
}