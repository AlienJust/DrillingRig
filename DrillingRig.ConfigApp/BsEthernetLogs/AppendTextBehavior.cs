using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DrillingRig.ConfigApp.BsEthernetLogs
{
	public class AppendTextBehavior : Behavior<TextBox> {
		public Action<string> AppendTextAction {
			get { return (Action<string>)GetValue(AppendTextActionProperty); }
			set { SetValue(AppendTextActionProperty, value); }
		}

		// Using a DependencyProperty as the backing store for AppendTextAction.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AppendTextActionProperty =
			DependencyProperty.Register("AppendTextAction", typeof(Action<string>), typeof(AppendTextBehavior), new PropertyMetadata(null));

		protected override void OnAttached() {
			SetCurrentValue(AppendTextActionProperty, (Action<string>)AssociatedObject.AppendText);
			base.OnAttached();
		}
	}
}