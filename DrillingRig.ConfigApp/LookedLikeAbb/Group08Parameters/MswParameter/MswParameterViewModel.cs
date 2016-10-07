using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanReadonly;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Group08Parameters.MswParameter {
	class MswParameterViewModel: ViewModelBase {
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
		public MswParameterViewModel(IParameterLogger parameterLogger) {
			ParameterLiteralVm = new ParameterStringReadonlyViewModel("08.01 MSW", string.Empty);
			Parameter01Vm = new ParameterBooleanReadonlyViewModel("MSW.01 RDY_ON (1=готов к включению)", null, parameterLogger);
			Parameter02Vm = new ParameterBooleanReadonlyViewModel("MSW.02 RDY_RUN (1=готов к работе)", null, parameterLogger);
			Parameter03Vm = new ParameterBooleanReadonlyViewModel("MSW.03 RDY_REF (1=работа разрешена:RUNNING)", null, parameterLogger);

			Parameter04Vm = new ParameterBooleanReadonlyViewModel("MSW.04 TRIPPED (1=отказ)", null, parameterLogger);
			Parameter05Vm = new ParameterBooleanReadonlyViewModel("MSW.05 OFF_2STA (0=OFF2 активно)", null, parameterLogger);
			Parameter06Vm = new ParameterBooleanReadonlyViewModel("MSW.06 OFF_3STA (0=OFF3 активно)", null, parameterLogger);

			Parameter07Vm = new ParameterBooleanReadonlyViewModel("MSW.07 ON_INHIBITED (1=включение запрещено)", null, parameterLogger);
			Parameter08Vm = new ParameterBooleanReadonlyViewModel("MSW.08 ALARM (1=предупреждение)", null, parameterLogger);
			Parameter09Vm = new ParameterBooleanReadonlyViewModel("MSW.09 AT_SETPOINT (1=скорость достигла заданной)", null, parameterLogger);
			Parameter10Vm = new ParameterBooleanReadonlyViewModel("MSW.10 REMOTE (1=дистанционное управление)", null, parameterLogger);
			Parameter11Vm = new ParameterBooleanReadonlyViewModel("MSW.11 ABOVE_LIMIT (1=скорость превысила предел)", null, parameterLogger);
			Parameter12Vm = new ParameterBooleanReadonlyViewModel("MSW.12 SELECTABLE", null, parameterLogger);
			Parameter13Vm = new ParameterBooleanReadonlyViewModel("MSW.13 INTERNAL_INTERLOCK", null, parameterLogger);
			Parameter14Vm = new ParameterBooleanReadonlyViewModel("MSW.14 RUN_INTERLOCK", null, parameterLogger);
			Parameter15Vm = new ParameterBooleanReadonlyViewModel("MSW.15 MODULATING (1=Модуляция: транзисторы IGBT управляются)", null, parameterLogger);
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
				Parameter12Vm.CurrentValue = (msw.Value & 0x0800) == 0x0800;
				Parameter13Vm.CurrentValue = (msw.Value & 0x1000) == 0x1000;
				Parameter14Vm.CurrentValue = (msw.Value & 0x2000) == 0x2000;
				Parameter15Vm.CurrentValue = (msw.Value & 0x4000) == 0x4000;
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
			}
		}
	}
}
