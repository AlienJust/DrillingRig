namespace DrillingRig.Commands.EngineTests {
	/// <summary>
	/// ��������� ������������ ���������
	/// </summary>
	public interface IEngineTestResult {
		byte TestResultByte { get; }

		int Rs { get; }
		int Rr { get; }
		int LsI { get; }
		int LrI { get; }
		int Lm { get; }
		int FlNom { get; }
		int J { get; }
		int Tr { get; }
		int RoverL { get; }
	}
}