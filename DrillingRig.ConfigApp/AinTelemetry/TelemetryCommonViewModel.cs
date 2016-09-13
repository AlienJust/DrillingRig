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
		private readonly IMultiLoggerWithStackTrace _debugLogger;
		private string _ain1Status;

		public TelemetryCommonViewModel(ILogger logger, IMultiLoggerWithStackTrace debugLogger) {
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

		public void UpdateAin1Status(ushort? value) {
			if (!value.HasValue) Ain1Status = UnknownValueText;
			else {
				string ain1Status = "0x" + value.Value.ToString("X4");

				Ain1Status = ain1Status;
			}
		}

		public string Ain1Status {
			get { return _ain1Status; }
			set {
				if (_ain1Status != value) {
					_ain1Status = value;
					RaisePropertyChanged(() => Ain1Status);
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
