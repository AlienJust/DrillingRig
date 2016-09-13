using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AlienJust.Adaptation.WindowsPresentation;
using AlienJust.Support.Concurrent.Contracts;
using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	/// <summary>
	/// Interaction logic for WindowChart.xaml
	/// </summary>
	public partial class WindowChart : MetroWindow, IUpdatable {
		private IThreadNotifier _uiNotifier;
		public WindowChart() {
			InitializeComponent();
			_uiNotifier = new WpfUiNotifier(Dispatcher);
		}

		private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			e.Cancel = true;
		}

		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject {
			if (depObj != null) {
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					var children = child as T;
					if (children != null) {
						yield return children;
					}

					foreach (T childOfChild in FindVisualChildren<T>(child)) {
						yield return childOfChild;
					}
				}
			}
		}

		private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
			var cvm = DataContext as WindowChartViewModel;
			cvm.ChartVm.SetUpdatable(this);
		}

		public void Update() {
			_uiNotifier.Notify(() => {
				if (CheckBox1.IsChecked.HasValue && CheckBox1.IsChecked.Value) {
					foreach (var child in FindVisualChildren<Abt.Controls.SciChart.Visuals.SciChartSurface>(ChartView1)) {
						child.ZoomExtents();
					}
				}
			});
		}
	}

	public interface IUpdatable {
		void Update();
	}
}
