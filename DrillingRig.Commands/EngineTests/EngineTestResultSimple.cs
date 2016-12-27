namespace DrillingRig.Commands.EngineTests {
	class EngineTestResultSimple : IEngineTestResult {
		public EngineTestResultSimple(byte testResultByte, int rs, int rr, int lsI, int lrI, int lm, int flNom, int j, int tr, int roverL)
		{
			TestResultByte = testResultByte;

			Rs = rs;
			Rr = rr;
			LsI = lsI;
			LrI = lrI;
			Lm = lm;
			FlNom = flNom;
			J = j;
			Tr = tr;
			RoverL = roverL;
		}

		public byte TestResultByte { get; }

		public int Rs { get; }
		public int Rr { get; }
		public int LsI { get; }
		public int LrI { get; }
		public int Lm { get; }
		public int FlNom { get; }
		public int J { get; }
		public int Tr { get; }
		public int RoverL { get; }
	}
}