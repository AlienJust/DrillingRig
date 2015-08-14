using DrillingRig.Commands.BsEthernetNominals;

namespace DrillingRig.ConfigApp.BsEthernetNominals {
	public interface IBsEthernetNominalsExporter
	{
		void ExportSettings(IBsEthernetNominals nominals);
	}
}