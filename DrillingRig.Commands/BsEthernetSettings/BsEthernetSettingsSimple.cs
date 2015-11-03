using System.Net;
using System.Net.NetworkInformation;

namespace DrillingRig.Commands.BsEthernetSettings {
	public class BsEthernetSettingsSimple : IBsEthernetSettings
	{
		private readonly PhysicalAddress _macAddress;
		private readonly IPAddress _ipAddress;
		private readonly IPAddress _mask;
		private readonly IPAddress _gateway;
		private readonly IPAddress _dnsServer;
		
		private readonly byte _modbusAddress;
		private readonly byte _driveNumber;
		private readonly byte _addressCan;
		private readonly FriquencyTransformerRole _ftRole;

		public BsEthernetSettingsSimple(
			PhysicalAddress macAddress, 
			IPAddress ipAddress, 
			IPAddress mask, 
			IPAddress gateway, 
			IPAddress dnsServer, 
			byte modbusAddress, 
			byte driveNumber,
			byte addressCan,
			FriquencyTransformerRole ftRole)
		{
			_ipAddress = ipAddress;
			_mask = mask;
			_gateway = gateway;
			_dnsServer = dnsServer;
			_macAddress = macAddress;
			_modbusAddress = modbusAddress;
			_driveNumber = driveNumber;
			_addressCan = addressCan;
			_ftRole = ftRole;
		}

		public PhysicalAddress MacAddress
		{
			get { return _macAddress; }
		}

		public IPAddress IpAddress
		{
			get { return _ipAddress; }
		}

		public IPAddress Mask
		{
			get { return _mask; }
		}

		public IPAddress Gateway
		{
			get { return _gateway; }
		}

		public IPAddress DnsServer
		{
			get { return _dnsServer; }
		}

		public byte ModbusAddress
		{
			get { return _modbusAddress; }
		}

		public byte DriveNumber
		{
			get { return _driveNumber; }
		}

		public byte AddressCan {
			get { return _addressCan; }
		}

		public FriquencyTransformerRole FtRole {
			get { return _ftRole; }
		}
	}
}