using System.Windows;
using AlienJust.Adaptation.WindowsPresentation;
using MahApps.Metro.Controls;

namespace DrillingRig.RpdChartTestApp {
	public partial class MainWindow : MetroWindow {
		public MainWindow() {
			InitializeComponent();
			DataContext = new MainWindowViewModel(new WpfUiNotifier(Dispatcher));
		}
	}
}
