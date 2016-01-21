using System.Collections.Generic;

namespace DrillingRig.ConfigApp {
	internal interface IDebugInformationShower {
		void ShowBytes(IList<byte> bytes);
	}
}