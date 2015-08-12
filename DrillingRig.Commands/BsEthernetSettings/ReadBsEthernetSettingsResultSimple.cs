using System.Net;
using System.Net.NetworkInformation;

namespace DrillingRig.Commands {
	class ReadBsEthernetSettingsResultSimple : IReadBsEthernetSettingsResult
	{
		private readonly IPAddress _ipAddress;
		private readonly IPAddress _mask;
		private readonly IPAddress _gateway;
		private readonly IPAddress _dnsServer;
		private readonly PhysicalAddress _macAddress;
		private readonly byte _modbusAddress;
		private readonly ushort _driveNumber;

		public ReadBsEthernetSettingsResultSimple(IPAddress ipAddress, IPAddress mask, IPAddress gateway, IPAddress dnsServer, PhysicalAddress macAddress, byte modbusAddress, ushort driveNumber)
		{
			_ipAddress = ipAddress;
			_mask = mask;
			_gateway = gateway;
			_dnsServer = dnsServer;
			_macAddress = macAddress;
			_modbusAddress = modbusAddress;
			_driveNumber = driveNumber;
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

		public PhysicalAddress MacAddress
		{
			get { return _macAddress; }
		}

		public byte ModbusAddress
		{
			get { return _modbusAddress; }
		}

		public ushort DriveNumber
		{
			get { return _driveNumber; }
		}
	}
}