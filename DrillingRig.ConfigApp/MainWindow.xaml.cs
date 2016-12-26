using System;
using System.Collections.Generic;
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
		private readonly Action _closingAction;

		public MainWindow(IThreadNotifier appMainThreadNotifier, Action closingAction) {
			_appMainThreadNotifier = appMainThreadNotifier;
			_closingAction = closingAction;
			InitializeComponent();
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			_closingAction.Invoke();
		}

		private void MetroWindow_Closed(object sender, EventArgs e) {
			_appMainThreadNotifier.Notify(() => {
				Application.Current.Shutdown();
			});
		}
	}
}
