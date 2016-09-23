using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.AppControl.ParamLogger;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanReadonly;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Group08Parameters.AswParameter {
	class AswParameterViewModel : ViewModelBase {
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

		public AswParameterViewModel(IParameterLogger parameterLogger) {
			ParameterLiteralVm = new ParameterStringReadonlyViewModel("08.02 ASW", string.Empty);
			Parameter01Vm = new ParameterBooleanReadonlyViewModel("ASW.01 LOGG_DATA_READY (1=Регистратор данных СРАБАТЫВАЕТ (0=работает))", null, parameterLogger);
			Parameter02Vm = new ParameterBooleanReadonlyViewModel("ASW.02 OUT_OF_WINDOW (1=Разность скоростей вышла за пределы окна)", null, parameterLogger);
			Parameter03Vm = new ParameterBooleanReadonlyViewModel("ASW.03 EMERG_STOP_COAST (1=Функция аврийного останова НЕ СРАБОТАЛА)", null, parameterLogger);

			Parameter04Vm = new ParameterBooleanReadonlyViewModel("ASW.04 MAGNETIZED (1=В двигателе сформирован магнитный поток)", null, parameterLogger);
			Parameter05Vm = new ParameterBooleanReadonlyViewModel("ASW.05 RUN_DISABLE (1=Внешний сигнал РАЗРЕШЕНИЕ_РАБОТЫ НЕ АКТИВЕН)", null, parameterLogger);
			Parameter06Vm = new ParameterBooleanReadonlyViewModel("ASW.06 SYNC_RDY (1=Счетчик положения синхронизирован)", null, parameterLogger);

			Parameter07Vm = new ParameterBooleanReadonlyViewModel("ASW.07 1START_NOT_DONE", null, parameterLogger);
			Parameter08Vm = new ParameterBooleanReadonlyViewModel("ASW.08 IDENTIF_RUN_DONE", null, parameterLogger);
			Parameter09Vm = new ParameterBooleanReadonlyViewModel("ASW.09 START_INHIBITION (1=функция безопасного отключения крутящего момента активна)", null, parameterLogger);
			Parameter10Vm = new ParameterBooleanReadonlyViewModel("ASW.10 LIMITING (1=Управление находится на пределе)", null, parameterLogger);
			Parameter11Vm = new ParameterBooleanReadonlyViewModel("ASW.11 TORQ_CONTROL (1=Отслеживается задание крутящего момента)", null, parameterLogger);
			Parameter12Vm = new ParameterBooleanReadonlyViewModel("ASW.12 ZERO_SPEED (1=Скорость вращения ниже нулевого предела)", null, parameterLogger);
			Parameter13Vm = new ParameterBooleanReadonlyViewModel("ASW.13 INTERNAL_SPEED_FB", null, parameterLogger);
			Parameter14Vm = new ParameterBooleanReadonlyViewModel("ASW.14 CH2_COMM_LOSS (1=Ошибка связи в канале CH2 (линия ведущий/ведомый))", null, parameterLogger);
			Parameter15Vm = new ParameterBooleanReadonlyViewModel("ASW.15 USER_MACROS1", null, parameterLogger);
			Parameter16Vm = new ParameterBooleanReadonlyViewModel("ASW.16 USER_MACROS2", null, parameterLogger);
		}

		public void UpdateTelemetry(ushort? asw) {

			if (asw.HasValue) {
				ParameterLiteralVm.CurrentValue = asw.Value.ToString("X4");
				Parameter01Vm.CurrentValue = (asw.Value & 0x0001) == 0x0001;
				Parameter02Vm.CurrentValue = (asw.Value & 0x0002) == 0x0002;
				Parameter03Vm.CurrentValue = (asw.Value & 0x0004) == 0x0004;
				Parameter04Vm.CurrentValue = (asw.Value & 0x0008) == 0x0008;
				Parameter05Vm.CurrentValue = (asw.Value & 0x0010) == 0x0010;
				Parameter06Vm.CurrentValue = (asw.Value & 0x0020) == 0x0020;
				Parameter07Vm.CurrentValue = (asw.Value & 0x0040) == 0x0040;
				Parameter08Vm.CurrentValue = (asw.Value & 0x0080) == 0x0080;
				Parameter09Vm.CurrentValue = (asw.Value & 0x0100) == 0x0100;
				Parameter10Vm.CurrentValue = (asw.Value & 0x0200) == 0x0200;
				Parameter11Vm.CurrentValue = (asw.Value & 0x0400) == 0x0400;
				Parameter12Vm.CurrentValue = (asw.Value & 0x0800) == 0x0800;
				Parameter13Vm.CurrentValue = (asw.Value & 0x1000) == 0x1000;
				Parameter14Vm.CurrentValue = (asw.Value & 0x2000) == 0x2000;
				Parameter15Vm.CurrentValue = (asw.Value & 0x4000) == 0x4000;
				Parameter16Vm.CurrentValue = (asw.Value & 0x8000) == 0x8000;
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
