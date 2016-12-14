using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DrillingRig.ConfigApp.BsEthernetLogs
{
	public class AutoScrollingBehavior : Behavior<ScrollViewer> {
		public object UpdateTrigger {
			get { return (object)GetValue(UpdateTriggerProperty); }
			set { SetValue(UpdateTriggerProperty, value); }
		}

		public static readonly DependencyProperty UpdateTriggerProperty =
			DependencyProperty.Register("UpdateTrigger", typeof(object), typeof(AutoScrollingBehavior), new UIPropertyMetadata(Update));

		private bool IsScrolledDown {
			get { return (bool)GetValue(IsScrolledDownProperty); }
			set { SetValue(IsScrolledDownProperty, value); }
		}

		public static readonly DependencyProperty IsScrolledDownProperty =
			DependencyProperty.Register("IsScrolledDown", typeof(bool), typeof(AutoScrollingBehavior), new UIPropertyMetadata(false));

		private static void Update(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			//if ((bool)d.GetValue(IsScrolledDownProperty)) {
			if ((bool)d.GetValue(IsScrolledDownProperty)) {
				var scroll = ((AutoScrollingBehavior)d).AssociatedObject;
				scroll.ScrollToBottom();
			}
		}

		protected override void OnAttached() {
			AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
			AssociatedObject.ScrollChanged += new ScrollChangedEventHandler(AssociatedObject_ScrollChanged);
			AssociatedObject.IsVisibleChanged += AssociatedObjectOnIsVisibleChanged;
		}
		private const double Tolerance = 50.0;
		private void AssociatedObjectOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			IsScrolledDown = Math.Abs(AssociatedObject.VerticalOffset - AssociatedObject.ScrollableHeight) < Tolerance;
		}

		private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e) {
			//var isScrollDown
			IsScrolledDown = Math.Abs(AssociatedObject.VerticalOffset - AssociatedObject.ScrollableHeight) < Tolerance;
			//if AssociatedObject.ScrollToBottom();
		}

		private void AssociatedObject_Loaded(object sender, RoutedEventArgs e) {
			IsScrolledDown = Math.Abs(AssociatedObject.VerticalOffset - AssociatedObject.ScrollableHeight) < Tolerance;
		}
	}
}