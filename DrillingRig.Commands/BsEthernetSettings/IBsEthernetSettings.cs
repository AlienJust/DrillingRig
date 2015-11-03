using System;
using System.Net;
using System.Net.NetworkInformation;

namespace DrillingRig.Commands.BsEthernetSettings {
	public interface IBsEthernetSettings {
		PhysicalAddress MacAddress { get; }
		IPAddress IpAddress { get; }
		IPAddress Mask { get; }
		IPAddress Gateway { get; }
		IPAddress DnsServer { get; }
		byte ModbusAddress { get; }
		byte DriveNumber { get; }
		byte AddressCan { get; }
		FriquencyTransformerRole FtRole { get; }
	}

	public enum FriquencyTransformerRole {
		Single,
		Master,
		Slave
	}

	public static class FriquencyTransformerRoleExtension {
		public static FriquencyTransformerRole FromByte(byte value) {
			switch (value) {
				case 0:
					return FriquencyTransformerRole.Single;
				case 1:
					return FriquencyTransformerRole.Master;
				case 2:
					return FriquencyTransformerRole.Slave;
				default:
					throw new Exception("Недопустимое значение байта: " + value);
			}
		}

		public static byte ToByte(this FriquencyTransformerRole value) {
			switch (value) {
				case FriquencyTransformerRole.Single:
					return 0;
				case FriquencyTransformerRole.Master:
					return 1;
				case FriquencyTransformerRole.Slave:
					return 2;
				default:
					throw new Exception("Невозможно представить данную роль ПЧ как байт");
			}
		}

		public static string ToText(this FriquencyTransformerRole value) {
			switch (value)
			{
				case FriquencyTransformerRole.Single:
					return "Одиночный прибор";
				case FriquencyTransformerRole.Master:
					return "Ведущий прибор";
				case FriquencyTransformerRole.Slave:
					return "Ведомый прибор";
				default:
					throw new Exception("Невозможно представить данную роль ПЧ в виде текста");
			}
		}
	}
}