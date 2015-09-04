using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.BsEthernetSettings {
	public class WriteBsEthernetSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IWriteBsEthernetSettingsResult>, IRrModbusCommandWithTestReply
	{
		private readonly IPAddress _ip;
		private readonly IPAddress _mask;
		private readonly IPAddress _gateway;
		private readonly IPAddress _dns;
		private readonly PhysicalAddress _mac;
		private readonly byte _modbusAddress;
		private readonly ushort _driveNumber;

		public WriteBsEthernetSettingsCommand(IPAddress ip, IPAddress mask, IPAddress gateway, IPAddress dns, PhysicalAddress mac, byte modbusAddress, ushort driveNumber)
		{
			_ip = ip;
			_mask = mask;
			_gateway = gateway;
			_dns = dns;
			_mac = mac;
			_modbusAddress = modbusAddress;
			_driveNumber = driveNumber;
		}

		public byte CommandCode {
			get { return 0x81; }
		}

		public string Name {
			get { return "Запись настроек БС-Ethernet"; }
		}

		public byte[] Serialize() {
			var result = new List<byte>();
			result.AddRange(_ip.GetAddressBytes());
			result.AddRange(_mask.GetAddressBytes());
			result.AddRange(_gateway.GetAddressBytes());
			result.AddRange(_dns.GetAddressBytes());
			result.AddRange(_mac.GetAddressBytes());
			result.Add(_modbusAddress);
			result.Add((byte)(_driveNumber & 0x00FF));
			result.Add((byte) ((_driveNumber & 0xFF00) >> 8));
			return result.ToArray();
		}

		public IWriteBsEthernetSettingsResult GetResult(byte[] reply) {
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be 0");
			return new WriteBsEthernetSettingsResultSimple();
		}

		public int ReplyLength {
			get { return 0; }
		}

		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}