using System;
using System.Collections;
using System.ComponentModel;

namespace DrillingRig.Commands.AinTelemetry {
	public enum FaultState {
		NoError = 0, //0 Нет ошибок.
		RuleImcwConflict, //1 Конфликт ролей каналов (по IMCW).
		RuleAinConflict, //2 Конфликт ролей АИН при работе двух ПЧ на одну лебедку.
		NoAinLink, //3 Нет связи с АИН.
		NotMagnetized, //4 Двигатель не намагнитился за 5 сек.
		SpeedLimit, //5 Превышение максимальной скорости длительное время.
		StatusError, //6 Появление ошибок STATUS АИН.
	}

	public static class FaultStateExtensions {
		public static ushort ToUshort(this FaultState state) {
			switch (state) {
				case FaultState.NoError:
					return 0;
				case FaultState.RuleImcwConflict:
					return 1;
				case FaultState.RuleAinConflict:
					return 2;
				case FaultState.NoAinLink:
					return 3;
				case FaultState.NotMagnetized:
					return 4;
				case FaultState.SpeedLimit:
					return 5;
				case FaultState.StatusError:
					return 6;
				default:
					throw new Exception("Cannot convert such state to ushort");
			}
		}

		public static string ToText(this FaultState state) {
			switch (state)
			{
				case FaultState.NoError:
					return "NoError";
				case FaultState.RuleImcwConflict:
					return "RuleImcwConflict";
				case FaultState.RuleAinConflict:
					return "RuleAinConflict";
				case FaultState.NoAinLink:
					return "NoAinLink";
				case FaultState.NotMagnetized:
					return "NotMagnetized";
				case FaultState.SpeedLimit:
					return "SpeedLimit";
				case FaultState.StatusError:
					return "StatusError";
				default:
					throw new Exception("Cannot convert such state to string");
			}
		}

		public static FaultState GetStateFromUshort(ushort value) {
			switch (value) {
				case 0:
					return FaultState.NoError;
				case 1:
					return FaultState.RuleImcwConflict;
				case 2:
					return FaultState.RuleAinConflict;
				case 3:
					return FaultState.NoAinLink;
				case 4:
					return FaultState.NotMagnetized;
				case 5:
					return FaultState.SpeedLimit;
				case 6:
					return FaultState.StatusError;
				default:
					throw new Exception("Cannot get ushort " + value + " as " + typeof (FaultState).Name);
			}
		}
	}
}