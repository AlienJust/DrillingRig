using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanReadonly;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Group09Parameters.AinBitsParameter {
	class AinBitsParameterViewModel : ViewModelBase {
		public ParameterStringReadonlyViewModel ParameterLiteralVm { get; }
		public ParameterBooleanReadonlyViewModel Parameter01Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter02Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter03Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter04Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter05Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter06Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter07Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter08Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter09Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter10Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter11Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter12Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter13Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter14Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter15Vm { get; }
		public ParameterBooleanReadonlyViewModel Parameter16Vm { get; }
		public AinBitsParameterViewModel(ParameterStringReadonlyViewModel parameterLiteralVm, IParameterLogger parameterLogger) {
			ParameterLiteralVm = parameterLiteralVm;
			
			Parameter01Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.01 Ошибки драйвера 1", null, parameterLogger);
			Parameter02Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.02 Ошибки драйвера 2", null, parameterLogger);
			Parameter03Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.03 Ошибки драйвера 3", null, parameterLogger);
			Parameter04Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.04 Ошибки драйвера 4", null, parameterLogger);
			Parameter05Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.05 Ошибки драйвера 5", null, parameterLogger);
			Parameter06Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.06 Ошибки драйвера 6", null, parameterLogger);

			Parameter07Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.07 Превышение допустимого тока по любой из фаз", null, parameterLogger);
			Parameter08Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.08 Превышение температуры радиатора ключей +85 градусов", null, parameterLogger);
			Parameter09Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.09 Выход за пределы напряжения DC", null, parameterLogger);

			Parameter10Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.10 Нет связи по линии синхронизации", null, parameterLogger);
			Parameter11Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.11 Превышение порога внешней температуры", null, parameterLogger);
			Parameter12Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.12 Отказ датчика частоты вращения", null, parameterLogger);

			Parameter13Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.13 Ошибка I2C/EEPROM, загружены параметры по умолчанию", null, parameterLogger);
			Parameter14Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.14 Ошибка CRC EEPROM, загружены параметры по умолчанию", null, parameterLogger);

			Parameter15Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.15 Отказ одного из ведомых приборов при параллельной работе (только для ведущего)", null, parameterLogger);
			Parameter16Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.16 Смена конфигурации при параллельной работе, требование подтвердить", null, parameterLogger);
		}




		public void UpdateTelemetry(ushort? status) {

			if (status.HasValue) {
				ParameterLiteralVm.CurrentValue = status.Value.ToString("X4");
				Parameter01Vm.CurrentValue = (status.Value & 0x0001) != 0;
				Parameter02Vm.CurrentValue = (status.Value & 0x0002) != 0;
				Parameter03Vm.CurrentValue = (status.Value & 0x0004) != 0;
				Parameter04Vm.CurrentValue = (status.Value & 0x0008) != 0;
				Parameter05Vm.CurrentValue = (status.Value & 0x0010) != 0;
				Parameter06Vm.CurrentValue = (status.Value & 0x0020) != 0;
				Parameter07Vm.CurrentValue = (status.Value & 0x0040) != 0;
				Parameter08Vm.CurrentValue = (status.Value & 0x0080) != 0;
				Parameter09Vm.CurrentValue = (status.Value & 0x0100) != 0;
				Parameter10Vm.CurrentValue = (status.Value & 0x0200) != 0;
				Parameter11Vm.CurrentValue = (status.Value & 0x0400) != 0;
				Parameter12Vm.CurrentValue = (status.Value & 0x0800) != 0;
				Parameter13Vm.CurrentValue = (status.Value & 0x1000) != 0;
				Parameter14Vm.CurrentValue = (status.Value & 0x2000) != 0;
				Parameter15Vm.CurrentValue = (status.Value & 0x4000) != 0;
				Parameter16Vm.CurrentValue = (status.Value & 0x8000) != 0;
			}
			else {
				ParameterLiteralVm.CurrentValue = string.Empty;
				Parameter01Vm.CurrentValue = null;
				Parameter02Vm.CurrentValue = null;
				Parameter03Vm.CurrentValue = null;
				Parameter04Vm.CurrentValue = null;
				Parameter05Vm.CurrentValue = null;
				Parameter06Vm.CurrentValue = null;
				Parameter07Vm.CurrentValue = null;
				Parameter08Vm.CurrentValue = null;
				Parameter09Vm.CurrentValue = null;
				Parameter10Vm.CurrentValue = null;
				Parameter11Vm.CurrentValue = null;
				Parameter12Vm.CurrentValue = null;
				Parameter13Vm.CurrentValue = null;
				Parameter14Vm.CurrentValue = null;
				Parameter15Vm.CurrentValue = null;
				Parameter16Vm.CurrentValue = null;
			}
		}
	}
}
