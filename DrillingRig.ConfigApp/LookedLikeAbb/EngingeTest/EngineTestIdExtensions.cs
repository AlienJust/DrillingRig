using System;
using DrillingRig.Commands.EngineTests;

namespace DrillingRig.ConfigApp.LookedLikeAbb.EngingeTest
{
	static class EngineTestIdExtensions {
		public static string ToText(this EngineTestId testId) {
			switch (testId)
			{
				case EngineTestId.DcTest:
					return "1 - DC тест";
				case EngineTestId.RlTest:
					return "21 - R/L тест";
				case EngineTestId.LslLrlTest:
					return "6 - LsI/LrI";
				case EngineTestId.TestXx:
					return "8 - Тест ХХ";
				case EngineTestId.Inertion:
					return "10 - Определение инерции";
				case EngineTestId.Tests1And6And8:
					return "F - Тесты 1,6,8";
				case EngineTestId.Tests1And6And8And10:
					return "1F - Тесты 1,6,8,10";
				case EngineTestId.Tests1And21And6And8:
					return "2F - Тесты 1,21,6,8";
				case EngineTestId.Tests1And21And6And8And10:
					return "3F - Тесты 1,21,6,8,10";
				case EngineTestId.NoTestAutoSetupOnly:
					return "0 - Автонастройка";
				default:
					throw new ArgumentOutOfRangeException(nameof(testId), testId, null);
			}
		}
	}
}