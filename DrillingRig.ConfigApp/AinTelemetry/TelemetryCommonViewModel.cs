using System;
using System.Diagnostics;
using System.Globalization;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinTelemetry;

namespace DrillingRig.ConfigApp.AinTelemetry {
	class TelemetryCommonViewModel : ViewModelBase, ICommonAinTelemetryVm {
		private string _engineState;
		private string _faultState;
		private string _ainsLinkState;
		private readonly ILogger _logger;
		private readonly IMultiLoggerWithStackTrace<int> _debugLogger;
		private string _ainStatuses;

		public TelemetryCommonViewModel(ILogger logger, IMultiLoggerWithStackTrace<int> debugLogger) {
			_logger = logger;
			_debugLogger = debugLogger;
		}

		private const string UnknownValueText = "Неизвестно";

		public void UpdateCommonEngineState(ushort? value) {
			if (!value.HasValue) CommonEngineState = UnknownValueText;
			else {
				string commonEngineState = value.Value.ToString(CultureInfo.InvariantCulture);
				try {
					commonEngineState += " - " + EngineStateExtensions.GetStateFromUshort(value.Value).ToText();
				}
				catch (Exception ex) {
					_debugLogger.GetLogger(2).Log(ex, new StackTrace());
				}
				CommonEngineState = commonEngineState;
			}
		}

		public void UpdateCommonFaultState(ushort? value) {
			if (!value.HasValue) CommonFaultState = UnknownValueText;
			else {
				string commonFaultState = value.Value.ToString(CultureInfo.InvariantCulture);
				try {
					commonFaultState += " - " + FaultStateExtensions.GetStateFromUshort(value.Value).ToText();
				}
				catch (Exception ex) {
					_debugLogger.GetLogger(2).Log(ex, new StackTrace());
				}
				CommonFaultState = commonFaultState;
			}
		}

		public void UpdateAinsLinkState(bool? ain1LinkFault, bool? ain2LinkFault, bool? ain3LinkFault) {
			string ain1LinkInfo = ain1LinkFault.HasValue ? (ain1LinkFault.Value ? "ER" : "OK") : "X3";
			string ain2LinkInfo = ain2LinkFault.HasValue ? (ain2LinkFault.Value ? "ER" : "OK") : "X3";
			string ain3LinkInfo = ain3LinkFault.HasValue ? (ain3LinkFault.Value ? "ER" : "OK") : "X3";

			AinsLinkState = ain1LinkInfo + " | " + ain2LinkInfo + " | " + ain3LinkInfo;
		}

		public void UpdateAinStatuses(ushort? status1, ushort? status2, ushort? status3)
		{
			string ainStatuses;
			if (!status1.HasValue)
				ainStatuses = UnknownValueText;
			else {
				ainStatuses = "0x" + status1.Value.ToString("X4");
			}
			ainStatuses += " | ";
			if (!status2.HasValue)
				ainStatuses += UnknownValueText;
			else {
				ainStatuses += "0x" + status2.Value.ToString("X4");
			}
			ainStatuses += " | ";
			if (!status3.HasValue)
				ainStatuses += UnknownValueText;
			else {
				ainStatuses += "0x" + status3.Value.ToString("X4");
			}
			AinStatuses = ainStatuses;
		}

		public string AinStatuses {
			get { return _ainStatuses; }
			set {
				if (_ainStatuses != value) {
					_ainStatuses = value;
					RaisePropertyChanged(() => AinStatuses);
				}
			}
		}

		public string CommonEngineState {
			get { return _engineState; }
			set {
				if (_engineState != value) {
					_engineState = value;
					RaisePropertyChanged(() => CommonEngineState);
				}
			}
		}

		public string CommonFaultState {
			get { return _faultState; }
			set {
				if (_faultState != value) {
					_faultState = value;
					RaisePropertyChanged(() => CommonFaultState);
				}
			}
		}

		public string AinsLinkState {
			get { return _ainsLinkState; }
			set {
				if (_ainsLinkState != value) {
					_ainsLinkState = value;
					RaisePropertyChanged(() => AinsLinkState);
				}
			}
		}
	}
}
