namespace DrillingRig.Commands.EngineTests {
	/// <summary>
	/// Результат тестирования двигателя
	/// </summary>
	public interface IEngineTestResult {
		byte TestResultByte { get; }

		int Rs { get; }
		int Rr { get; }
		int Lsl { get; }
		int Lrl { get; }
		int Lm { get; }
		int FlNom { get; }
		int J { get; }
		int Tr { get; }
		int RoverL { get; }
	}
}