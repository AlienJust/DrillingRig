using System;

namespace DrillingRig.Commands.AinTelemetry {
	public static class ModeSetMomentumSetterSelectorExtensions {
		public static ModeSetMomentumSetterSelector FromInt(int value) {
			switch (value) {
				case 0:
					return ModeSetMomentumSetterSelector.SpeedRegulator;
				case 1:
					return ModeSetMomentumSetterSelector.ExternalMoment;
				case 2:
					return ModeSetMomentumSetterSelector.Summary;
				case 3:
					return ModeSetMomentumSetterSelector.Zero;
				default:
					throw new Exception("Cannot cast integer value " + value + " to ModeSetMomentumSetterSelector enum");
			}
		}
	}
}