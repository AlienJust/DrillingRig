using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	/// <summary>
	/// Interaction logic for WindowChart.xaml
	/// </summary>
	public partial class WindowChart : MetroWindow {
		public WindowChart() {
			InitializeComponent();
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			e.Cancel = true;
		}
	}
}
