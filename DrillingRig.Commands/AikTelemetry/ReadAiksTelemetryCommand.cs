using System;
using System.Net;
using System.Net.NetworkInformation;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AikTelemetry {
	public class ReadAiksTelemetryCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IAiksTelemetry>, IRrModbusCommandWithTestReply
	{

		public byte CommandCode
		{
			get { return 0x85; } // TODO: 0x83 and 0x84 are for nominal setting values
		}

		public string Name
		{
			get { return "Чтение настроек БС-Ethernet"; }
		}

		public byte[] Serialize()
		{
			return new byte[0];
		}

		public IAiksTelemetry GetResult(byte[] reply)
		{
			throw new NotImplementedException("TODO: IMPLEMENT!");
		}

		public int ReplyLength
		{
			get {
				return 174; // three Aiks each 29 2 bytes params = 29 * 2 * 3 = 174 total
			}
		}

		public byte[] GetTestReply() {
			var result = new byte[174];
			for (int i = 0; i < result.Length; ++i) {
				result[i] = (byte)i; // TODO: fill random bytes
			}
			return result;
		}
	}
}