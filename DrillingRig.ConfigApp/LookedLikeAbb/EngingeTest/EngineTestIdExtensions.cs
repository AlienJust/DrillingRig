using System;

namespace DrillingRig.ConfigApp.LookedLikeAbb.EngingeTest
{
	static class EngineTestIdExtensions {
		public static string ToText(this EngineTestId testId) {
			switch (testId) {
				case EngineTestId.Test1:
					return "Тест 1";
				case EngineTestId.Test2:
					return "Тест 2";
				default:
					throw new ArgumentOutOfRangeException(nameof(testId), testId, null);
			}
		}
	}
}