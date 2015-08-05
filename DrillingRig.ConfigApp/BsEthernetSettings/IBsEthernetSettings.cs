namespace DrillingRig.ConfigApp.BsEthernetSettings {
	public interface IBsEthernetSettings {
		string IpAddress { get; }
		string Mask { get; }
		string Gateway { get; }
		string DnsServer { get; }
		string MacAddress { get; }
		byte ModbusAddress { get; }
		ushort DriveNumber { get; }
	}
}