namespace DrillingRig.Commands.EngineTests
{
	public interface IEngineTestParams {
		int Te1 { get; }
		float T1C { get; }
		float T2C { get; }
		int K21 { get; }
		int Te6 { get; }
		float F1 { get; }//= 100.0;
		float F2 { get; }//= 150.0;
		int Acc8 { get; }
		int Dir10 { get; }
		int Tj1 { get; }
		int Tj2 { get; }
		int Tj3 { get; }
		int Tj4 { get; }

		float Kp1 { get; }
		float Ki1 { get; }
		float Kp6 { get; }
		float Ki6 { get; }

		float TauI { get; }//= 7e-3 
		float TauFi { get; }//= 50e-3;
		float TauSpd { get; }
	}
}