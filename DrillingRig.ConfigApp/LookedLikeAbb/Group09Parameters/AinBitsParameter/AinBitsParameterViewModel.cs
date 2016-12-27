using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanReadonly;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Group09Parameters.AinBitsParameter {
	class AinBitsParameterViewModel: ViewModelBase {
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
		public AinBitsParameterViewModel(ParameterStringReadonlyViewModel parameterLiteralVm, IParameterLogger parameterLogger) {
			ParameterLiteralVm = parameterLiteralVm;
			//ParameterLiteralVm = new ParameterStringReadonlyViewModel("09.01 СТАТУС АИН1", string.Empty);
			Parameter01Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.01 Ошибки драйвера 1", null, parameterLogger);
			Parameter02Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.02 Ошибки драйвера 2", null, parameterLogger);
			Parameter03Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.03 Ошибки драйвера 3", null, parameterLogger);
			Parameter04Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.04 Ошибки драйвера 4", null, parameterLogger);
			Parameter05Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.05 Ошибки драйвера 5", null, parameterLogger);
			Parameter06Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.06 Ошибки драйвера 6", null, parameterLogger);

			Parameter07Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.07 Превышение допустимого тока по любой из фаз", null, parameterLogger);
			Parameter08Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.08 Превышение температуры радиатора ключей +85 градусов", null, parameterLogger);
			Parameter09Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.09 Выход за пределы напряжения DC", null, parameterLogger);
			Parameter10Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.10 Ошибка I2C/EEPROM, загружены параметры по умолчанию", null, parameterLogger);
			Parameter11Vm = new ParameterBooleanReadonlyViewModel("СТАТУС.11 Ошибка CRC EEPROM, загружены параметры по умолчанию", null, parameterLogger);
		}

		


		public void UpdateTelemetry(ushort? msw) {

			if (msw.HasValue) {
				ParameterLiteralVm.CurrentValue = msw.Value.ToString("X4");
				Parameter01Vm.CurrentValue = (msw.Value & 0x0001) == 0x0001;
				Parameter02Vm.CurrentValue = (msw.Value & 0x0002) == 0x0002;
				Parameter03Vm.CurrentValue = (msw.Value & 0x0004) == 0x0004;
				Parameter04Vm.CurrentValue = (msw.Value & 0x0008) == 0x0008;
				Parameter05Vm.CurrentValue = (msw.Value & 0x0010) == 0x0010;
				Parameter06Vm.CurrentValue = (msw.Value & 0x0020) == 0x0020;
				Parameter07Vm.CurrentValue = (msw.Value & 0x0040) == 0x0040;
				Parameter08Vm.CurrentValue = (msw.Value & 0x0080) == 0x0080;
				Parameter09Vm.CurrentValue = (msw.Value & 0x0100) == 0x0100;
				Parameter10Vm.CurrentValue = (msw.Value & 0x0200) == 0x0200;
				Parameter11Vm.CurrentValue = (msw.Value & 0x0400) == 0x0400;
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
			}
		}
	}
}
