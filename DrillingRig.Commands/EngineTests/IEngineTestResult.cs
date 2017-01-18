namespace DrillingRig.Commands.EngineTests {
	/// <summary>
	/// ��������� ������������ ���������
	/// </summary>
	public interface IEngineTestResult {
		byte TestResultByte { get; }

		short Rs { get; } // short, because of must be synced with IAinSettings
		double Rr { get; }
		double Lsl { get; }
		double Lrl { get; }
		double Lm { get; }
		short FlNom { get; } // short, because of must be synced with IAinSettings
		double J { get; }
		double TauR { get; }
		double RoverL { get; }
	}
}