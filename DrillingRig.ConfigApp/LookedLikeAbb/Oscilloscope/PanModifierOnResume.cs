using System.Collections.Generic;
using System.Windows;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.Axes;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Oscilloscope
{
	public class PanModifierOnResume : ChartModifierBase {
		private class PanningState {
			public Point LastPoint;
			public IEnumerable<IAxis> XAxesToScroll;
			public IEnumerable<IAxis> YAxesToScroll;
		}

		private PanningState _state;

		private PanningState CreateInitialPanningState(ModifierMouseArgs e) {
			foreach (var yAxis in YAxes) {
				if (HitTestableContainsMousePoint(yAxis, e)) {
					// Dragging started on an y-axis, only pan that y-axis.
					return new PanningState {
						LastPoint = e.MousePoint,
						XAxesToScroll = new IAxis[0],
						YAxesToScroll = new[] { yAxis }
					};
				}
			}
			// Dragging started on the main chart area, pan all the axes.
		
			return new PanningState {
				LastPoint = e.MousePoint,
				XAxesToScroll = XAxes,
				YAxesToScroll = YAxes
			};
		}

		private bool HitTestableContainsMousePoint(IHitTestable hitTestable, ModifierMouseArgs e) {
			Rect bounds = hitTestable.GetBoundsRelativeTo(RootGrid);
			return bounds.Contains(e.MousePoint);
		}
		
		public override void OnModifierMouseDown(ModifierMouseArgs e) {
			base.OnModifierMouseDown(e);
			ModifierSurface.CaptureMouse();
			_state = CreateInitialPanningState(e);
			e.Handled = true;
		}
		

		
		public override void OnModifierMouseMove(ModifierMouseArgs e) {
			base.OnModifierMouseMove(e);
			if (_state == null)
				return;

			var currentPoint = e.MousePoint;
			var xDelta = currentPoint.X - _state.LastPoint.X;
			var yDelta = _state.LastPoint.Y - currentPoint.Y;

			using (ParentSurface.SuspendUpdates()) {
				foreach (var yAxis in _state.YAxesToScroll) {
					yAxis.Scroll(YAxis.IsHorizontalAxis ? -xDelta : yDelta, ClipMode.None);
				}
			}
			_state.LastPoint = currentPoint;
		}
		

		
		public override void OnModifierMouseUp(ModifierMouseArgs e) {
			base.OnModifierMouseUp(e);
			ModifierSurface.ReleaseMouseCapture();
			_state = null;
		}

			
		public override void OnModifierMouseWheel(ModifierMouseArgs e) {
			// Get region of axis
			Rect yAxisBounds = YAxis.GetBoundsRelativeTo(RootGrid);
			if (yAxisBounds.Contains(e.MousePoint)) {
				if (e.Delta > 0)
					YAxis.ZoomBy(-0.1, -0.1);
				else if (e.Delta < 0)
					YAxis.ZoomBy(0.1, 0.1);

				e.Handled = true;
				return;
			}

			if (e.Delta > 0) {
				YAxis.ZoomBy(-0.1, -0.1);
			}
			else if (e.Delta < 0) {
				YAxis.ZoomBy(0.1, 0.1);
			}
			e.Handled = true;
		}
	}
}