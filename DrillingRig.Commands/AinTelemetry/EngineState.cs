using System;

namespace DrillingRig.Commands.AinTelemetry {
	public enum EngineState
	{
		PowerOn, 					// 0 Неопределенное состояние АИН (после включения/рестарта)
		WaitStart,							// 1 (3 сек) Завершение переходных процессов в АИН, Ожидание пульта.
		TestImcw,							// 2 Проверка ролей АИН.
		
		ResetErrors,						// 3 Сброс аварий АИН2,3,1 (ведущий - последний!).
		NotReady,							// 4 НЕ Готов к запуску двигателя.
		ReadyToSwitchOn,			// 5 Готов к работе.
		ReadyRun,									// 6 Готов к запуску двигателя.
		
		EnableOperation, // 7 Запущен ШИМ, RAMP_OUT_ZERO.
		EnableOutput, // 8 Запущен ШИМ, RAMP_HOLD
		AcceleratorEnable, // 9 Запущен ШИМ, RAMP_IN_ZERO
		OperatingState, // 10 Запущен двигатель с ненулевой скоростью

		ReadySlave, // 11 Готов к запуску двигателя с управлением от мастера по CAN
		DriveSlave, //12 Двигатель запущен с управлением от мастера по CAN.
		Off2,										//13 Останов выбегом (высший приоритет).
		Off3,										//14 Аварийный Останов линейным замедлением (средний приоритет).
		Off1,										//15 Останов линейным замедлением (низший приоритет).

		Inching1,								//16 Толчок1.
		Inching2,								//17 Толчок2.
		PostInching,						//18 После толчка поддержание ключей включенными

		FaultState, //19 Авария.
		SwitchOnInhibit, // 20 Ожидание штатного отключения после аварийного.
		InhibitOperationActive, // 21 Ожидание штатного отключения после пропадания сигнала RUN.
		ChopperNotReady, // 22 Чоппер не готов
		ChopperRun, //23 Чоппер запущен.
		ReadKIs, //24 Опрос КИ.
		ReadMOs, //25 Опрос МО.
		
	}

	public static class EngineStateExtensions {
		public static ushort ToUshort(this EngineState state) {
			switch (state) {
				case EngineState.PowerOn:
					return 0;
				case EngineState.WaitStart:
					return 1;
				case EngineState.TestImcw:
					return 2;

				case EngineState.ResetErrors:
					return 3;
				case EngineState.NotReady:
					return 4;
				case EngineState.ReadyToSwitchOn:
					return 5;
				case EngineState.ReadyRun:
					return 6;

				case EngineState.EnableOperation:
					return 7;
				case EngineState.EnableOutput:
					return 8;
				case EngineState.AcceleratorEnable:
					return 9;
				case EngineState.OperatingState:
					return 10;

				case EngineState.ReadySlave:
					return 11;
				case EngineState.DriveSlave:
					return 12;

				case EngineState.Off2:
					return 13;
				case EngineState.Off3:
					return 14;
				case EngineState.Off1:
					return 15;

				case EngineState.Inching1:
					return 16;
				case EngineState.Inching2:
					return 17;
				case EngineState.PostInching:
					return 18;

				case EngineState.FaultState:
					return 19;
				case EngineState.SwitchOnInhibit:
					return 20;
				case EngineState.InhibitOperationActive:
					return 21;
				case EngineState.ChopperNotReady:
					return 22;
				case EngineState.ChopperRun:
					return 23;
				case EngineState.ReadKIs:
					return 24;
				case EngineState.ReadMOs:
					return 25;

				default:
					throw new Exception("Cannot convert such state to ushort");
			}
		}

		public static string ToText(this EngineState state) {
			switch (state) {
				case EngineState.PowerOn:
					return "POWER_ON";
				case EngineState.WaitStart:
					return "WAIT_START";
				case EngineState.TestImcw:
					return "TestImcw";
				case EngineState.ResetErrors:
					return "RESET_ERRORS";
				case EngineState.NotReady:
					return "NOT_READY";
				case EngineState.ReadyToSwitchOn:
					return "ReadyToSwitchOn";
				case EngineState.ReadyRun:
					return "READY_RUN";

				case EngineState.EnableOperation:
					return "EnableOperation";
				case EngineState.EnableOutput:
					return "EnableOutput";
					case EngineState.AcceleratorEnable:
					return "AcceleratorEnable";
					case EngineState.OperatingState:
					return "OperatingState";

					case EngineState.ReadySlave:
					return "READY_SLAVE";
				case EngineState.DriveSlave:
					return "DRIVE_SLAVE";

				case EngineState.Off2:
					return "OFF_2";
				case EngineState.Off3:
					return "OFF_3";
				case EngineState.Off1:
					return "OFF_1";
				case EngineState.Inching1:
					return "Inching1";
				case EngineState.Inching2:
					return "Inching2";
				case EngineState.PostInching:
					return "PostInching";
				case EngineState.FaultState:
					return "FAULT_STATE";
				case EngineState.SwitchOnInhibit:
					return "SwitchOnInhibit";
				case EngineState.InhibitOperationActive:
					return "INHIBIT_OPERATION_ACTIVE";
				case EngineState.ChopperNotReady:
					return "CHOPPER_NOT_READY";
				case EngineState.ChopperRun:
					return "CHOPPER_RUN";
				case EngineState.ReadKIs:
					return "ReadKIs";
				case EngineState.ReadMOs:
					return "ReadMOs";

				default:
					throw new Exception("Cannot convert such state to ushort");
			}
		}

		public static EngineState GetStateFromUshort(ushort value) {
			switch (value) {
				case 0:
					return EngineState.PowerOn; //0 Неопределенное состояние АИН (после включения/рестарта)
				case 1:
					return EngineState.WaitStart; //1 (3 сек) Завершение переходных процессов в АИН, Ожидание пульта.
				case 2:
					return EngineState.TestImcw; //2 Проверка ролей АИН.
				
				case 3:
					return EngineState.ResetErrors; //5 Сброс аварий АИН2,3,1 (ведущий - последний!).
				case 4:
					return EngineState.NotReady; //6 НЕ Готов к запуску двигателя.
				case 5:
					return EngineState.ReadyToSwitchOn; //7
				case 6:
					return EngineState.ReadyRun; //8 Готов к запуску двигателя.

				case 7:
					return EngineState.EnableOperation;
				case 8:
					return EngineState.EnableOutput;
				case 9:
					return EngineState.AcceleratorEnable;
				case 10:
					return EngineState.OperatingState;

				case 11:
					return EngineState.ReadySlave;
				case 12:
					return EngineState.DriveSlave;

				case 13:
					return EngineState.Off2;
				case 14:
					return EngineState.Off3;
				case 15:
					return EngineState.Off1;
				
				case 16:
					return EngineState.Inching1;
				case 17:
					return EngineState.Inching2;
				case 18:
					return EngineState.PostInching;
				case 19:
					return EngineState.FaultState;
				case 20:
					return EngineState.SwitchOnInhibit;
				case 21:
					return EngineState.InhibitOperationActive;
				case 22:
					return EngineState.ChopperNotReady;
				case 23:
					return EngineState.ChopperRun;
				case 24:
					return EngineState.ReadKIs;
				case 25:
					return EngineState.ReadMOs;

				default:
					throw new Exception("Cannot get ushort " + value + " as " + typeof (EngineState).Name);
			}
		}
	}
}