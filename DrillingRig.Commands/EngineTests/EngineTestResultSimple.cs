namespace DrillingRig.Commands.EngineTests {
	class EngineTestResultSimple : IEngineTestResult {
		public EngineTestResultSimple(byte testResultByte, short rs, double rr, double lsI, double lrI, double lm, short flNom, double j, double tr, double roverL)
		{
			TestResultByte = testResultByte;

			Rs = rs;
			Rr = rr;
			Lsl = lsI;
			Lrl = lrI;
			Lm = lm;
			FlNom = flNom;
			J = j;
			TauR = tr;
			RoverL = roverL;
		}

		public byte TestResultByte { get; }

		public short Rs { get; } // short, type is synced IAinSettings
		public double Rr { get; }
		public double Lsl { get; }
		public double Lrl { get; }
		public double Lm { get; }
		public short FlNom { get; } // short, type is synced IAinSettings
		public double J { get; }
		public double TauR { get; }
		public double RoverL { get; }
	}
}