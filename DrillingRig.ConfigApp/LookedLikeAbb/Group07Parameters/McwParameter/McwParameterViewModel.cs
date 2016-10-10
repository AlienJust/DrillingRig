using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanReadonly;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Group07Parameters.McwParameter {
	class McwParameterViewModel: ViewModelBase {
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

		public McwParameterViewModel(IParameterLogger parameterLogger) {
			ParameterLiteralVm = new ParameterStringReadonlyViewModel("07.01 MCW", string.Empty);
			Parameter01Vm = new ParameterBooleanReadonlyViewModel("MCW.01 OFF1", null, parameterLogger);
			Parameter02Vm = new ParameterBooleanReadonlyViewModel("MCW.02 OFF2", null, parameterLogger);
			Parameter03Vm = new ParameterBooleanReadonlyViewModel("MCW.03 OFF3", null, parameterLogger);

			Parameter04Vm = new ParameterBooleanReadonlyViewModel("MCW.04 RUN", null, parameterLogger);
			Parameter05Vm = new ParameterBooleanReadonlyViewModel("MCW.05 RAMP_OUT_ZERO", null, parameterLogger);
			Parameter06Vm = new ParameterBooleanReadonlyViewModel("MCW.06 RAMP_HOLD", null, parameterLogger);

			Parameter07Vm = new ParameterBooleanReadonlyViewModel("MCW.07 RAMP_IN_ZERO", null, parameterLogger);
			Parameter08Vm = new ParameterBooleanReadonlyViewModel("MCW.08 RESET", null, parameterLogger);
			Parameter09Vm = new ParameterBooleanReadonlyViewModel("MCW.09 INCHING1", null, parameterLogger);
			Parameter10Vm = new ParameterBooleanReadonlyViewModel("MCW.10 INCHING2", null, parameterLogger);
			Parameter11Vm = new ParameterBooleanReadonlyViewModel("MCW.11 REMOTE", null, parameterLogger);
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
