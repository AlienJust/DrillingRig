using System;
using System.Xml.Linq;

namespace DrillingRig.ConfigApp {
	public interface IBsEthernetSettingsImporter {
		IBsEthernetSettings ImportSettings();
	}

	class BsEthernetSettingsImporter : IBsEthernetSettingsImporter {
		private readonly string _filename;
		public BsEthernetSettingsImporter(string filename) {
			_filename = filename;
		}

		public IBsEthernetSettings ImportSettings() {
			// TODO: move strings to constants (shared beetween import and export static class)
			try {
				var doc = XDocument.Load(_filename);
				var root = doc.Element("BsEthernetSettings");
				var ip = root.Element("IpAddress").Value;
				var mask = root.Element("Mask").Value;
				var gate = root.Element("Gateway").Value;
				var dns = root.Element("DnsServer").Value;
				var mac = root.Element("MacAddress").Value;
				var modbusAddress = byte.Parse(root.Element("ModbusAddress").Value);
				var driveNumber = ushort.Parse(root.Element("DriveNumber").Value);

				return new BsEthernetSettingsSimple(ip, mask, gate, dns, mac, modbusAddress, driveNumber);
			}
			catch (Exception ex) {
				throw new Exception("Cannot import settings", ex);
			}
		}
	}
}