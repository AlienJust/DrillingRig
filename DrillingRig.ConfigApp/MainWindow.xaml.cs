using System.Windows;
using AlienJust.Support.Concurrent.Contracts;
using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		private readonly IThreadNotifier _appMainThreadNotifier;

		public MainWindow(IThreadNotifier appMainThreadNotifier) {
			_appMainThreadNotifier = appMainThreadNotifier;
			InitializeComponent();
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			_appMainThreadNotifier.Notify(() => {
				Application.Current.Shutdown();
			});
		}
	}
}
