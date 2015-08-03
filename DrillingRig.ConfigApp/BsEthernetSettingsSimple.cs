namespace DrillingRig.ConfigApp {
	class BsEthernetSettingsSimple : IBsEthernetSettings {
		private readonly string _ipAddress;
		private readonly string _mask;
		private readonly string _gateway;
		private readonly string _dnsServer;
		private readonly string _macAddress;
		private readonly byte _modbusAddress;
		private readonly ushort _driveNumber;

		public BsEthernetSettingsSimple(string ipAddress, string mask, string gateway, string dnsServer, string macAddress, byte modbusAddress, ushort driveNumber) {
			_ipAddress = ipAddress;
			_mask = mask;
			_gateway = gateway;
			_dnsServer = dnsServer;
			_macAddress = macAddress;
			_modbusAddress = modbusAddress;
			_driveNumber = driveNumber;
		}

		public string IpAddress {
			get { return _ipAddress; }
		}

		public string Mask {
			get { return _mask; }
		}

		public string Gateway {
			get { return _gateway; }
		}

		public string DnsServer {
			get { return _dnsServer; }
		}

		public string MacAddress {
			get { return _macAddress; }
		}

		public byte ModbusAddress {
			get { return _modbusAddress; }
		}

		public ushort DriveNumber {
			get { return _driveNumber; }
		}
	}
}