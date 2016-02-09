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
		UdcLow, // 7
		AinLinkError, 		//8 Потеря связи с АИН.
		EthernetLinkError,//9 Потеря связи с Ethernet.
		CanLinkError,			//10 Потеря связи по линии CAN.
		ChangedAinMode, 	//11 Изменился режим работы (Одиночный/ведущий/ведомый).
		SlaveNotReady,		//12 В режиме Ведущий  не готов Ведомый.
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
				case FaultState.UdcLow:
					return 7;

				case FaultState.AinLinkError:
					return 8;
				case FaultState.EthernetLinkError:
					return 9;
				case FaultState.CanLinkError:
					return 10;

				case FaultState.ChangedAinMode:
					return 11;
				case FaultState.SlaveNotReady:
					return 12;
				default:
					throw new Exception("Cannot convert such state to ushort");
			}
		}

		public static string ToText(this FaultState state) {
			switch (state)
			{
				case FaultState.NoError:
					return "NO_ERROR";
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
				case FaultState.UdcLow:
					return "UDC_LOW";

				case FaultState.AinLinkError:
					return "AIN_LINK_ERROR";
				case FaultState.EthernetLinkError:
					return "ETHERNET_LINK_ERROR";
				case FaultState.CanLinkError:
					return "CAN_LINK_ERROR";

				case FaultState.ChangedAinMode:
					return "CHANGED_AIN_MODE";
				case FaultState.SlaveNotReady:
					return "SLAVE_NOT_READY";
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
				case 7:
					return FaultState.UdcLow;

				case 8:
					return FaultState.AinLinkError;
				case 9:
					return FaultState.EthernetLinkError;
				case 10:
					return FaultState.CanLinkError;

				case 11:
					return FaultState.ChangedAinMode;
				case 12:
					return FaultState.SlaveNotReady;
				default:
					throw new Exception("Cannot get ushort " + value + " as " + typeof (FaultState).Name);
			}
		}
	}
}