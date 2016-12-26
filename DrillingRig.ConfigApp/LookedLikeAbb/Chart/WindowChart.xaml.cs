using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Abt.Controls.SciChart.Visuals;
using AlienJust.Adaptation.WindowsPresentation;
using AlienJust.Support.Concurrent.Contracts;
using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	public partial class WindowChart : MetroWindow, IUpdatable {
		private readonly IThreadNotifier _uiNotifier;
		private SciChartSurface _sciChartSurface;
		public WindowChart() {
			InitializeComponent();
			_uiNotifier = new WpfUiNotifierAsync(Dispatcher);
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
			cvm?.ChartVm.SetUpdatable(this);

			foreach (var child in FindVisualChildren<SciChartSurface>(ChartView1)) {
				_sciChartSurface = child;
				break;
			}
		}

		public void Update() {
			_uiNotifier.Notify(() => {
				if (CheckBox1.IsChecked.HasValue && CheckBox1.IsChecked.Value) {
					foreach (var child in FindVisualChildren<SciChartSurface>(ChartView1)) {
						child.ZoomExtents();
					}
					_sciChartSurface.ZoomExtents();
				}
			});
		}
	}
}
