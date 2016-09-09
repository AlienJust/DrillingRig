using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp.AinCommand {
	/// <summary>
	/// Interaction logic for CommandWindow.xaml
	/// </summary>
	public partial class CommandWindow : MetroWindow {
		public CommandWindow() {
			InitializeComponent();
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			e.Cancel = true;
		}
	}
}
