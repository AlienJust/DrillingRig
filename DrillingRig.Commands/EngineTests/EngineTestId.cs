using System;

namespace DrillingRig.Commands.EngineTests
{
	/// <summary>
	/// Идентификатор теста.
	/// Наследуется от байта, что является конвертором в значение, передаваемое на нижний уровень БС-Ethernet.
	/// </summary>
	[Flags]
	public enum EngineTestId
	{
		AutoSetupOnly = 0x00,

		DcTest = 0x01,
		LslTest = 0x02,
		LrlTest = 0x04,
		XxTest = 0x08,
		InertionTest = 0x10,
		RlTest = 0x20 // Cannot be run without DC test
	}
}