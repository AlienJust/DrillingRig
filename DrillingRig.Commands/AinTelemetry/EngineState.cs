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
		Drive0,									//9 Двигатель запущен с нулевой скоростью.
		Drive,									//10 Двигатель запущен с ненулевой скоростью.
		DriveSlave,						//11 Двигатель запущен с управлением от мастера по CAN.
		Off2,										//12 Останов выбегом (высший приоритет).
		Off3,										//13 Аварийный Останов линейным замедлением (средний приоритет).
		Off1,										//14 Останов линейным замедлением (низший приоритет).
		Inching1,								//15 Толчок1.
		Inching2,								//16 Толчок2.
		PostInching,						//17 После толчка поддержание ключей включенными
		FaultState,						//18 Авария.
		ChopperMode,						//19 Режим Чоппер.
		ReadKIs,								//20 Опрос КИ.
		ReadMOs,								//21 Опрос МО.
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
				case EngineState.Drive0:
					return 9;
				case EngineState.Drive:
					return 10;
				case EngineState.DriveSlave:
					return 11;
				case EngineState.Off2:
					return 12;
				case EngineState.Off3:
					return 13;
				case EngineState.Off1:
					return 14;
				case EngineState.Inching1:
					return 15;
				case EngineState.Inching2:
					return 16;
				case EngineState.PostInching:
					return 17;
				case EngineState.FaultState:
					return 18;
				case EngineState.ChopperMode:
					return 19;
				case EngineState.ReadKIs:
					return 20;
				case EngineState.ReadMOs:
					return 21;

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
				case EngineState.Drive0:
					return "Drive0";
				case EngineState.Drive:
					return "Drive";
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
					return EngineState.Drive0; //9 Двигатель запущен с нулевой скоростью.
				case 10:
					return EngineState.Drive; //10 Двигатель запущен с ненулевой скоростью.
				case 11:
					return EngineState.DriveSlave; //11 Двигатель запущен с управлением от мастера по CAN.
				case 12:
					return EngineState.Off2; //12 Останов выбегом (высший приоритет).
				case 13:
					return EngineState.Off3; //13 Аварийный Останов линейным замедлением (средний приоритет).
				case 14:
					return EngineState.Off1; //14 Останов линейным замедлением (низший приоритет).
				case 15:
					return EngineState.Inching1; //15 Толчок1.
				case 16:
					return EngineState.Inching2; //16 Толчок2.
				case 17:
					return EngineState.PostInching; //17 После толчка поддержание ключей включенными
				case 18:
					return EngineState.FaultState; //18 Авария.
				case 19:
					return EngineState.ChopperMode; //19 Режим Чоппер.
				case 20:
					return EngineState.ReadKIs; //20 Опрос КИ.
				case 21:
					return EngineState.ReadMOs; //21 Опрос МО.
				default:
					throw new Exception("Cannot get ushort " + value + " as " + typeof (EngineState).Name);
			}
		}
	}
}