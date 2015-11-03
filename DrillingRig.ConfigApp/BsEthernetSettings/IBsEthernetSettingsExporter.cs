using DrillingRig.Commands.BsEthernetSettings;

namespace DrillingRig.ConfigApp.BsEthernetSettings {
	public interface IBsEthernetSettingsExporter {
		void ExportSettings(IBsEthernetSettings settings);
	}
}