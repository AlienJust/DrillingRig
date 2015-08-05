using System.Xml.Linq;

namespace DrillingRig.ConfigApp.BsEthernetSettings {
	class BsEthernetSettingsExporterXml : IBsEthernetSettingsExporter {
		private readonly string _filename;
		public BsEthernetSettingsExporterXml(string filename) {
			_filename = filename;
		}

		public void ExportSettings(IBsEthernetSettings settings) {
			var doc = new XDocument(
				new XDeclaration("1,0", "utf-8", "yes"),
				new XElement("BsEthernetSettings",
					new XElement("IpAddress", settings.IpAddress),
					new XElement("Mask", settings.Mask),
					new XElement("Gateway", settings.Gateway),
					new XElement("DnsServer", settings.DnsServer),
					new XElement("MacAddress", settings.MacAddress),
					new XElement("ModbusAddress", settings.ModbusAddress),
					new XElement("DriveNumber", settings.DriveNumber)
					));
			doc.Save(_filename);
		}
	}
}