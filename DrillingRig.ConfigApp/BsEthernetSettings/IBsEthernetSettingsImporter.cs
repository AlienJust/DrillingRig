using DrillingRig.Commands.BsEthernetSettings;

namespace DrillingRig.ConfigApp.BsEthernetSettings {
	public interface IBsEthernetSettingsImporter {
		IBsEthernetSettings ImportSettings();
	}
}