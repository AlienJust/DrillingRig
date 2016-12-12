using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp.BsEthernetLogs {
	/// <summary>
	/// Логика взаимодействия для WindowView.xaml
	/// </summary>
	public partial class WindowView : MetroWindow {
		private readonly MainWindow _mainWindow;

		public WindowView(MainWindow mainWindow)
		{
			_mainWindow = mainWindow;
			InitializeComponent();
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			_mainWindow.Close();
		}
	}
}
