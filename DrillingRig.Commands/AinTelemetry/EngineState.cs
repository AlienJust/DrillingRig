using System;

namespace DrillingRig.Commands.AinTelemetry {
	public enum EngineState
	{
		PowerOn, // 0 Неопределенное состояние АИН (после включения/рестарта)
		WaitStart, // 1 (3 сек) Завершение переходных процессов в АИН, Ожидание пульта.
		WaitLinkAin1, //2 Ожидание связи с АИН1 (с ведущим). 

		/// <summary>
		/// 3 Проверка ролей АИН.
		/// </summary>
		TestImcw,
		PreResetErrors, //4 Подготовка к Сбросу аварий АИН3,2,1.

		ResetErrors, // 5 Сброс аварий АИН2,3,1 (ведущий - последний!).
		NotReady, // 6 НЕ Готов к запуску двигателя.
		ReadyToSwitchOn, // 7 Готов к работе.
		ReadyRun, // 8 Готов к запуску двигателя.
		
		EnableOperation, // 9 Запущен ШИМ, RAMP_OUT_ZERO.
		EnableOutput, // 10 Запущен ШИМ, RAMP_HOLD
		AcceleratorEnable, // 11 Запущен ШИМ, RAMP_IN_ZERO
		OperatingState, // 12 Запущен двигатель с ненулевой скоростью

		ReadySlave, // 13 Готов к запуску двигателя с управлением от мастера по CAN
		DriveSlave, //14 Двигатель запущен с управлением от мастера по CAN.
		Off2, //15 Останов выбегом (высший приоритет).
		Off3, //16 Аварийный Останов линейным замедлением (средний приоритет).
		Off1, //17 Останов линейным замедлением (низший приоритет).

		Inching1, //18 Толчок1.
		Inching2, //19 Толчок2.
		PostInching, //20 После толчка поддержание ключей включенными

		FaultState, //21 Авария.
		FaultStateWaitReset, // 22 Авария ожидаем рестарт
		SwitchOnInhibit, // 23 Ожидание штатного отключения после аварийного
		InhibitOperationActive, // 24 Ожидание штатного отключения после пропадания сигнала RUN.
		ChopperNotReady, // 25 Чоппер не готов
		ChopperRun, //26 Чоппер запущен.
		ReadKIs, //27 Опрос КИ.
		ReadMOs, //28 Опрос МО.

		/// <summary>
		/// 29 Начало процедуры определения параметров двигателя
		/// </summary>
		AciIdentifyStart,

		/// <summary>
		/// 30 Проведение процедуры определения параметров двигателя
		/// </summary>
		AciIdentifyExecution,

		/// <summary>
		/// 31 РЕЗЕРВ Завершение процедуры определения параметров двигателя
		/// </summary>
		AciIdentifyEnd,

		/// <summary>
		/// 32 ТРАМВАЙ. Отключен. На тормозе.
		/// </summary>
		TramStop,

		/// <summary>
		/// 33 ТРАМВАЙ. Двигатель намагничивается. На тормозе.
		/// </summary>
		TramMagnetizing,

		/// <summary>
		/// 34 ТРАМВАЙ. Разгон. Движение ВПЕРЕД.
		/// </summary>
		TramAccelerationForward,

		/// <summary>
		/// 35 ТРАМВАЙ. Движение без тяги. Движение ВПЕРЕД.
		/// </summary>
		TramMovingForward,

		/// <summary>
		/// 36 ТРАМВАЙ. Торможение. Движение ВПЕРЕД.
		/// </summary>
		TramDecelerationForward,

		/// <summary>
		/// 37 ТРАМВАЙ. Разгон. Движение НАЗАД.
		/// </summary>
		TramAccelerationBackward,

		/// <summary>
		/// 38 ТРАМВАЙ. Движение без тяги. Движение НАЗАД.
		/// </summary>
		TramMovingBackward,

		/// <summary>
		/// 39 ТРАМВАЙ. Торможение. Движение НАЗАД.
		/// </summary>
		TramDecelerationBackward
	}

	public static class EngineStateExtensions {
		public static ushort ToUshort(this EngineState state) {
			switch (state) {
				case EngineState.PowerOn:
					return 0;
				case EngineState.WaitStart:
					return 1;
				case EngineState.WaitLinkAin1:
					return 2;
				case EngineState.TestImcw:
					return 3;
				case EngineState.PreResetErrors:
					return 4;
				case EngineState.ResetErrors:
					return 5;
				case EngineState.NotReady:
					return 6;
				case EngineState.ReadyToSwitchOn:
					return 7;
				case EngineState.ReadyRun:
					return 8;
				case EngineState.EnableOperation:
					return 9;
				case EngineState.EnableOutput:
					return 10;
				case EngineState.AcceleratorEnable:
					return 11;
				case EngineState.OperatingState:
					return 12;

				case EngineState.ReadySlave:
					return 13;
				case EngineState.DriveSlave:
					return 14;

				case EngineState.Off2:
					return 15;
				case EngineState.Off3:
					return 16;
				case EngineState.Off1:
					return 17;

				case EngineState.Inching1:
					return 18;
				case EngineState.Inching2:
					return 19;
				case EngineState.PostInching:
					return 20;

				case EngineState.FaultState:
					return 21;
				case EngineState.FaultStateWaitReset:
					return 22;
				case EngineState.SwitchOnInhibit:
					return 23;
				case EngineState.InhibitOperationActive:
					return 24;
				case EngineState.ChopperNotReady:
					return 25;
				case EngineState.ChopperRun:
					return 26;
				case EngineState.ReadKIs:
					return 27;
				case EngineState.ReadMOs:
					return 28;
				case EngineState.AciIdentifyStart:
					return 29;
				case EngineState.AciIdentifyExecution:
					return 30;
				case EngineState.AciIdentifyEnd:
					return 31;
				case EngineState.TramStop:
					return 32;
				case EngineState.TramMagnetizing:
					return 33;
				case EngineState.TramAccelerationForward:
					return 34;
				case EngineState.TramMovingForward:
					return 35;
				case EngineState.TramDecelerationForward:
					return 36;
				case EngineState.TramAccelerationBackward:
					return 37;
				case EngineState.TramMovingBackward:
					return 38;
				case EngineState.TramDecelerationBackward:
					return 39;
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
				case EngineState.WaitLinkAin1:
					return "WAIT_LINK_AIN1";
				case EngineState.TestImcw:
					return "TestImcw";
				case EngineState.PreResetErrors:
					return "PRE_RESET_ERRORS";
				case EngineState.ResetErrors:
					return "RESET_ERRORS";
				case EngineState.NotReady:
					return "NOT_READY";
				case EngineState.ReadyToSwitchOn:
					return "ReadyToSwitchOn";
				case EngineState.ReadyRun:
					return "READY_RUN";

				case EngineState.EnableOperation:
					return "ENABLE_OPERATION";
				case EngineState.EnableOutput:
					return "ENABLE_OUTPUT";
					case EngineState.AcceleratorEnable:
					return "ACCELERATOR_ENABLE";
					case EngineState.OperatingState:
					return "OPERATING_STATE";

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
					return "INCHING1";
				case EngineState.Inching2:
					return "INCHING2";
				case EngineState.PostInching:
					return "POST_INCHING";
				case EngineState.FaultState:
					return "FAULT_STATE";
				case EngineState.FaultStateWaitReset:
					return "FAULT_STATE_WAIT_RESET";
				case EngineState.SwitchOnInhibit:
					return "SWITCH_ON_INHIBIT";
				case EngineState.InhibitOperationActive:
					return "INHIBIT_OPERATION_ACTIVE";
				case EngineState.ChopperNotReady:
					return "CHOPPER_NOT_READY";
				case EngineState.ChopperRun:
					return "CHOPPER_RUN";
				case EngineState.ReadKIs:
					return "READ_KIs";
				case EngineState.ReadMOs:
					return "READ_MOs";
				case EngineState.AciIdentifyStart:
					return "ACI_IDENTIFY_START";
				case EngineState.AciIdentifyExecution:
					return "ACI_IDENTIFY_EXECUTION";

				case EngineState.AciIdentifyEnd:
					return "ACI_IDENTIFY_END";
				case EngineState.TramStop:
					return "TRAM_STOP";
				case EngineState.TramMagnetizing:
					return "TRAM_MAGNETIZING";
				case EngineState.TramAccelerationForward:
					return "TRAM_ACCELERATION_FORWARD";
				case EngineState.TramMovingForward:
					return "TRAM_MOVING_FORWARD";
				case EngineState.TramDecelerationForward:
					return "TRAM_DECELERATION_FORWARD";
				case EngineState.TramAccelerationBackward:
					return "TRAM_ACCELERATION_BACKWARD";
				case EngineState.TramMovingBackward:
					return "TRAM_MOVING_BACKWARD";
				case EngineState.TramDecelerationBackward:
					return "TRAM_DECELERATION_BACKWARD";
				default:
					throw new Exception("Cannot convert such state to ushort");
			}
		}

		public static EngineState GetStateFromUshort(ushort value) {
			switch (value) {
				case 0:
					return EngineState.PowerOn; // 0 Неопределенное состояние АИН (после включения/рестарта)
				case 1:
					return EngineState.WaitStart; // 1 (3 сек) Завершение переходных процессов в АИН, Ожидание пульта.
				case 2:
					return EngineState.WaitLinkAin1; // 2 Ожидание связи с АИН1 (с ведущим)
				case 3:
					return EngineState.TestImcw; // 3 Проверка ролей АИН.
				case 4:
					return EngineState.PreResetErrors; // 4 Подготовка к Сбросу аварий АИН3,2,1
				case 5:
					return EngineState.ResetErrors; // 5 Сброс аварий АИН2,3,1 (ведущий - последний!).
				case 6:
					return EngineState.NotReady; // 6 НЕ Готов к запуску двигателя.
				case 7:
					return EngineState.ReadyToSwitchOn; //7
				case 8:
					return EngineState.ReadyRun; //8 Готов к запуску двигателя.

				case 9:
					return EngineState.EnableOperation;
				case 10:
					return EngineState.EnableOutput;
				case 11:
					return EngineState.AcceleratorEnable;
				case 12:
					return EngineState.OperatingState;

				case 13:
					return EngineState.ReadySlave;
				case 14:
					return EngineState.DriveSlave;

				case 15:
					return EngineState.Off2;
				case 16:
					return EngineState.Off3;
				case 17:
					return EngineState.Off1;
				
				case 18:
					return EngineState.Inching1;
				case 19:
					return EngineState.Inching2;
				case 20:
					return EngineState.PostInching;
				case 21:
					return EngineState.FaultState;
				case 22:
					return EngineState.FaultStateWaitReset;
				case 23:
					return EngineState.SwitchOnInhibit;
				case 24:
					return EngineState.InhibitOperationActive;
				case 25:
					return EngineState.ChopperNotReady;
				case 26:
					return EngineState.ChopperRun;
				case 27:
					return EngineState.ReadKIs;
				case 28:
					return EngineState.ReadMOs;
				case 29:
					return EngineState.AciIdentifyStart;
				case 30:
					return EngineState.AciIdentifyExecution;
				case 31:
					return EngineState.AciIdentifyEnd;
					
				case 32:
					return EngineState.TramStop;
				case 33:
					return EngineState.TramMagnetizing;
				case 34:
					return EngineState.TramAccelerationForward;
				case 35:
					return EngineState.TramMovingForward;
				case 36:
					return EngineState.TramDecelerationForward;
				case 37:
					return EngineState.TramAccelerationBackward;
				case 38:
					return EngineState.TramMovingBackward;
				case 39:
					return EngineState.TramDecelerationBackward;

				default:
					throw new Exception("Cannot get ushort " + value + " as " + typeof (EngineState).Name);
			}
		}
	}
}