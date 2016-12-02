using System;
using System.Collections.Generic;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.BsEthernetSettings {
	public class WriteBsEthernetSettingsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IWriteBsEthernetSettingsResult>, IRrModbusCommandWithTestReply
	{
		private readonly IBsEthernetSettings _bsEthernetSettings;

		public WriteBsEthernetSettingsCommand(IBsEthernetSettings bsEthernetSettings) {
			_bsEthernetSettings = bsEthernetSettings;
		}

		public byte CommandCode => 0x81;

		public string Name => "Запись настроек БС-Ethernet";

		public byte[] Serialize() {
			var result = new List<byte>();
			result.AddRange(_bsEthernetSettings.MacAddress.GetAddressBytes());
			
			result.AddRange(_bsEthernetSettings.IpAddress.GetAddressBytes());
			result.AddRange(_bsEthernetSettings.Mask.GetAddressBytes());
			result.AddRange(_bsEthernetSettings.Gateway.GetAddressBytes());
			result.AddRange(_bsEthernetSettings.DnsServer.GetAddressBytes());

			result.Add(_bsEthernetSettings.ModbusAddress);
			result.Add(_bsEthernetSettings.DriveNumber);
			result.Add(_bsEthernetSettings.AddressCan);
			result.Add(_bsEthernetSettings.FtRole.ToByte());
			return result.ToArray();
		}

		public IWriteBsEthernetSettingsResult GetResult(byte[] reply) {
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be 0");
			return new WriteBsEthernetSettingsResultSimple();
		}

		public int ReplyLength => 0;

		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}