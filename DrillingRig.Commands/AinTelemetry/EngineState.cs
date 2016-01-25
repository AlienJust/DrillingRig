using System;

namespace DrillingRig.Commands.AinTelemetry {
	public enum EngineState
	{
		PowerOn, 					//0 Неопределенное состояние АИН (после включения/рестарта)
		WaitStart,							//1 (3 сек) Завершение переходных процессов в АИН, Ожидание пульта.
		TestImcw,							//2 Проверка ролей АИН.
		TestMasterSlave,			//3 Проверка роли ПЧ (ведущий/ведомый).
		SetTorqueSelector,		//4 Запись селектора задания момента (по скорости / внешнему моменту)
		ResetErrors,						//5 Сброс аварий АИН2,3,1 (ведущий - последний!).
		NotReady,							//6 НЕ Готов к запуску двигателя.
		ReadyToSwitchOn,			//7
		Ready,									//8 Готов к запуску двигателя.
		
		EnableOperation, // 9 Запущен ШИМ, RAMP_OUT_ZERO.
		EnableOutput, // 10 Запущен ШИМ, RAMP_HOLD
		AcceleratorEnable, // 11 Запущен ШИМ, RAMP_IN_ZERO
		OperatingState, // 12 Запущен двигатель с ненулевой скоростью

		DriveSlave,						//13 Двигатель запущен с управлением от мастера по CAN.
		Off2,										//14 Останов выбегом (высший приоритет).
		Off3,										//15 Аварийный Останов линейным замедлением (средний приоритет).
		Off1,										//16 Останов линейным замедлением (низший приоритет).
		Inching1,								//17 Толчок1.
		Inching2,								//18 Толчок2.
		PostInching,						//19 После толчка поддержание ключей включенными
		FaultState, //20 Авария.
		SwitchOnInhibit, // 21 Ожидание штатного отключения после аварийного.
		InhibitOperationActiv, // 22 Ожидание штатного отключения после пропадания сигнала RUN.
		ChopperMode, //22 Режим Чоппер.
		ReadKIs, //23 Опрос КИ.
		ReadMOs, //24 Опрос МО.
		
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
				case EngineState.TestMasterSlave:
					return 3;
				case EngineState.SetTorqueSelector:
					return 4;
				case EngineState.ResetErrors:
					return 5;
				case EngineState.NotReady:
					return 6;
				case EngineState.ReadyToSwitchOn:
					return 7;
				case EngineState.Ready:
					return 8;

				case EngineState.EnableOperation:
					return 9;
				case EngineState.EnableOutput:
					return 10;
				case EngineState.AcceleratorEnable:
					return 11;
				case EngineState.OperatingState:
					return 12;

				case EngineState.DriveSlave:
					return 13;
				case EngineState.Off2:
					return 14;
				case EngineState.Off3:
					return 15;
				case EngineState.Off1:
					return 16;
				case EngineState.Inching1:
					return 17;
				case EngineState.Inching2:
					return 18;
				case EngineState.PostInching:
					return 19;
				case EngineState.FaultState:
					return 20;
				case EngineState.SwitchOnInhibit:
					return 21;
				case EngineState.InhibitOperationActiv:
					return 22;
				case EngineState.ChopperMode:
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
					return "PowerOn";
				case EngineState.WaitStart:
					return "WaitStart";
				case EngineState.TestImcw:
					return "TestImcw";
				case EngineState.TestMasterSlave:
					return "TestMasterSlave";
				case EngineState.SetTorqueSelector:
					return "SetTorqueSelector";
				case EngineState.ResetErrors:
					return "ResetErrors";
				case EngineState.NotReady:
					return "NotReady";
				case EngineState.ReadyToSwitchOn:
					return "ReadyToSwitchOn";
				case EngineState.Ready:
					return "Ready";

				case EngineState.EnableOperation:
					return "EnableOperation";
				case EngineState.EnableOutput:
					return "EnableOutput";
					case EngineState.AcceleratorEnable:
					return "AcceleratorEnable";
					case EngineState.OperatingState:
					return "OperatingState";

				case EngineState.DriveSlave:
					return "DriveSlave";
				case EngineState.Off2:
					return "Off2";
				case EngineState.Off3:
					return "Off3";
				case EngineState.Off1:
					return "Off1";
				case EngineState.Inching1:
					return "Inching1";
				case EngineState.Inching2:
					return "Inching2";
				case EngineState.PostInching:
					return "PostInching";
				case EngineState.FaultState:
					return "FaultState";
				case EngineState.SwitchOnInhibit:
					return "SwitchOnInhibit";
				case EngineState.InhibitOperationActiv:
					return "INHIBIT_OPERATION_ACTIV";
				case EngineState.ChopperMode:
					return "ChopperMode";
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
					return EngineState.TestMasterSlave; //3 Проверка роли ПЧ (ведущий/ведомый).
				case 4:
					return EngineState.SetTorqueSelector; //4 Запись селектора задания момента (по скорости / внешнему моменту)
				case 5:
					return EngineState.ResetErrors; //5 Сброс аварий АИН2,3,1 (ведущий - последний!).
				case 6:
					return EngineState.NotReady; //6 НЕ Готов к запуску двигателя.
				case 7:
					return EngineState.ReadyToSwitchOn; //7
				case 8:
					return EngineState.Ready; //8 Готов к запуску двигателя.

				case 9:
					return EngineState.EnableOperation;
				case 10:
					return EngineState.EnableOutput;
				case 11:
					return EngineState.AcceleratorEnable;
				case 12:
					return EngineState.OperatingState;

				case 13:
					return EngineState.DriveSlave;
				case 14:
					return EngineState.Off2;
				case 15:
					return EngineState.Off3;
				case 16:
					return EngineState.Off1;
				case 17:
					return EngineState.Inching1;
				case 18:
					return EngineState.Inching2;
				case 19:
					return EngineState.PostInching;
				case 20:
					return EngineState.FaultState;
				case 21:
					return EngineState.SwitchOnInhibit;
				case 22:
					return EngineState.InhibitOperationActiv;
				case 23:
					return EngineState.ChopperMode;
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