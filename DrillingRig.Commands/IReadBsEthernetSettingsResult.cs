using System.Net;
using System.Net.NetworkInformation;

namespace DrillingRig.Commands {
	public interface IReadBsEthernetSettingsResult {
		IPAddress IpAddress { get; }
		IPAddress Mask { get; }
		IPAddress Gateway { get; }
		IPAddress DnsServer { get; }
		PhysicalAddress MacAddress { get; }
		byte ModbusAddress { get; }
		ushort DriveNumber { get; }
	}
}