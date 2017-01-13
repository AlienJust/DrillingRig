using System;
using DrillingRig.Commands.EngineTests;

namespace DrillingRig.ConfigApp.LookedLikeAbb.EngingeTest
{
	static class EngineTestIdExtensions {
		public static string ToText(this EngineTestId testId) {
			switch (testId)
			{
				case EngineTestId.DcTest:
					return "DC тест";
				case EngineTestId.RlTest:
					return "R/L тест";
				case EngineTestId.LslTest:
					return "Lsl";
				case EngineTestId.LrlTest:
					return "Lrl";
				case EngineTestId.XxTest:
					return "ХХ Тест";
				case EngineTestId.InertionTest:
					return "10 - Определение инерции";
				case EngineTestId.AutoSetupOnly:
					return "0 - Автонастройка";
				default:
					throw new ArgumentOutOfRangeException(nameof(testId), testId, null);
			}
		}
	}
}