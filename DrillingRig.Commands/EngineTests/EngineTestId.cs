namespace DrillingRig.Commands.EngineTests
{
	/// <summary>
	/// Идентификатор теста.
	/// Наследуется от байта, что является конвертором в значение, передаваемое на нижний уровень БС-Ethernet.
	/// </summary>
	public enum EngineTestId : byte
	{
		DcTest = 0x01,
		RlTest = 0x21,
		LslLrlTest = 0x06,
		TestXx = 0x08,
		Inertion = 0x10,
		Tests1And6And8 = 0x0F,
		Tests1And6And8And10 = 0x1F,
		Tests1And21And6And8 = 0x2F,
		Tests1And21And6And8And10 = 0x3F,
		NoTestAutoSetupOnly = 0x00
	}
}