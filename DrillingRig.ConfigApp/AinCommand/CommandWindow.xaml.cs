using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp.AinCommand {
	/// <summary>
	/// Interaction logic for CommandWindow.xaml
	/// </summary>
	public partial class CommandWindow : MetroWindow {
		private readonly MainWindow _mainWindow;

		public CommandWindow(MainWindow mainWindow) {
			_mainWindow = mainWindow;
			InitializeComponent();
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			_mainWindow.Close();
		}
	}
}
