using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using DrillingRig.Commands.BsEthernetSettings;

namespace DrillingRig.ConfigApp.BsEthernetSettings {
	class BsEthernetSettingsXmlWorker : IBsEthernetSettingsExporter, IBsEthernetSettingsImporter {
		public const string BsEthernetSettingsElementName = "BsEthernetSettings";
		public const string MacAddressElementName = "MacAddress";
		public const string IpAddressElementName = "IpAddress";
		public const string MaskElementName = "Mask";
		public const string GatewayElementName = "Gateway";
		public const string DnsServerElementName = "DnsServer";
		public const string ModbusAddressElementName = "ModbusAddress";
		public const string DriveNumberElementName = "DriveNumber";
		public const string AddressCanElementName = "AddressCan";
		public const string FriquencyTransformerRoleElementName = "FriquencyTransformerRole";

		private readonly string _filename;
		public BsEthernetSettingsXmlWorker(string filename) {
			_filename = filename;
		}

		public void ExportSettings(IBsEthernetSettings settings)
		{
			var mac = settings.MacAddress.GetAddressBytes().Aggregate(String.Empty, (current, b) => current + (b.ToString("X2") + "."));
			mac = mac.Substring(0, mac.Length - 1);

			var doc = new XDocument(
				new XDeclaration("1,0", "utf-8", "yes"),
				new XElement(BsEthernetSettingsElementName,
					new XElement(MacAddressElementName, mac),
					new XElement(IpAddressElementName, settings.IpAddress),
					new XElement(MaskElementName, settings.Mask),
					new XElement(GatewayElementName, settings.Gateway),
					new XElement(DnsServerElementName, settings.DnsServer),
					new XElement(ModbusAddressElementName, settings.ModbusAddress),
					new XElement(DriveNumberElementName, settings.DriveNumber),
					new XElement(AddressCanElementName, settings.AddressCan),
					new XElement(FriquencyTransformerRoleElementName, settings.FtRole.ToByte())
					));
			doc.Save(_filename);
		}

		public IBsEthernetSettings ImportSettings()
		{
			// TODO: move strings to constants (shared beetween import and export static class)
			try
			{
				var doc = XDocument.Load(_filename);
				var root = doc.Element(BsEthernetSettingsElementName);

				var mac = new PhysicalAddress(root.Element(MacAddressElementName).Value.Split('.').Select(s => Byte.Parse(s, NumberStyles.HexNumber)).ToArray());

				var ip = new IPAddress(root.Element(IpAddressElementName).Value.Split('.').Select(Byte.Parse).ToArray());
				var mask = new IPAddress(root.Element(MaskElementName).Value.Split('.').Select(Byte.Parse).ToArray());
				var gate = new IPAddress(root.Element(GatewayElementName).Value.Split('.').Select(Byte.Parse).ToArray());
				var dns = new IPAddress(root.Element(DnsServerElementName).Value.Split('.').Select(Byte.Parse).ToArray());

				var modbusAddress = Byte.Parse(root.Element(ModbusAddressElementName).Value);
				var driveNumber = Byte.Parse(root.Element(DriveNumberElementName).Value);

				var addressCan = Byte.Parse(root.Element(AddressCanElementName).Value);
				var ftRole = FriquencyTransformerRoleExtension.FromByte(Byte.Parse(root.Element(FriquencyTransformerRoleElementName).Value));

				return new BsEthernetSettingsSimple(mac, ip, mask, gate, dns, modbusAddress, driveNumber, addressCan, ftRole);
			}
			catch (Exception ex)
			{
				throw new Exception("Не удалось импортировать настройки", ex);
			}
		}
	}
}