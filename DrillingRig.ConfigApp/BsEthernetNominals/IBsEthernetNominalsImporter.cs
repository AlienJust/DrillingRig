using DrillingRig.Commands.BsEthernetNominals;

namespace DrillingRig.ConfigApp.BsEthernetNominals {
	public interface IBsEthernetNominalsImporter
	{
		IBsEthernetNominals ImportSettings();
	}
}