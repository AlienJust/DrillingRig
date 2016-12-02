using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.EngineTests;

namespace DrillingRig.ConfigApp.LookedLikeAbb.EngingeTest
{
	internal class TestViewModel : ViewModelBase {
		public TestViewModel(EngineTestId testId) {
			TestId = testId;
		}

		public EngineTestId TestId { get; }
		public string TestIdText => TestId.ToText();
	}
}