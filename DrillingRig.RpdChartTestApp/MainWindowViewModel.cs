using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp;
using DrillingRig.ConfigApp.LookedLikeAbb;
using DrillingRig.ConfigApp.LookedLikeAbb.Chart;

namespace DrillingRig.RpdChartTestApp {
	class MainWindowViewModel : ViewModelBase, IUserInterfaceRoot {
		private readonly ParameterLogger _paramLogger;

		public ICommand AddChartStatic { get; }
		public ICommand AddChartBits { get; }
		public ICommand AddChartDynamic { get; }
		public ICommand AddChartDynamicReally { get; }

		public ChartViewModel ChartControlVm { get; }

		private readonly Thread _backThread;
		public ParameterDoubleReadonlyViewModel ParamVm { get; }

		public MainWindowViewModel(IThreadNotifier notifier) {
			Notifier = notifier;

			AddChartStatic = new RelayCommand(DoAddChartStatic);
			AddChartBits = new RelayCommand(DoAddChartBits);
			AddChartDynamic = new RelayCommand(DoAddChartDynamic);
			AddChartDynamicReally = new RelayCommand(DoAddChartDynamicReally);

			ChartControlVm = new ChartViewModel();
			
			_paramLogger = new ParameterLogger(ChartControlVm);
			ParamVm = new ParameterDoubleReadonlyViewModel("Динамически добавляемый параметр!", "f2", null, _paramLogger);

			_backThread = new Thread(AddDynamicReallyFunc) {IsBackground = true};
			
		}

		private void AddDynamicReallyFunc() {
			while (true) {
				//_paramLogger.LogParameter("Really dynamic param", DateTime.Now.Millisecond * 1.0);
				Notifier.Notify(() => {
					ParamVm.CurrentValue = DateTime.Now.Millisecond;
				});
				Thread.Sleep(100);
			}
		}

		private void DoAddChartStatic() {
			_paramLogger.AddDataCommandExecute(DateTime.Now, 1.0, DateTime.Now.Ticks.ToString());
		}

		private void DoAddChartBits() {
			_paramLogger.AddDiscreeteCommandExecute(DateTime.Now, 1.0, DateTime.Now.Ticks.ToString() );
		}

		private void DoAddChartDynamic() {
			_paramLogger.LogParameter("Dynamic param", DateTime.Now.Second * 1.0);
		}

		private void DoAddChartDynamicReally() {
			_backThread.Start();
		}


		public IThreadNotifier Notifier { get; }
	}
}
