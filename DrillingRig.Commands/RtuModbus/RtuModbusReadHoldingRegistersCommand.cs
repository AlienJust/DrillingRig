using System;
using System.Collections.Generic;
using System.Linq;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus
{
	public class RtuModbusReadHoldingRegistersCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IList<byte>>, IRrModbusCommandWithTestReply
	{
		private readonly ushort _firstRegisterAddress;
		private readonly ushort _registersCountToRead;

		public RtuModbusReadHoldingRegistersCommand(ushort firstRegisterAddress, ushort registersCountToRead) {
			_firstRegisterAddress = firstRegisterAddress;
			_registersCountToRead = registersCountToRead;
		}

		public byte CommandCode
		{
			get { return 0x03; }
		}

		public string Name
		{
			get { return "Read holding registers Modbus RTU command, first address: " + _firstRegisterAddress + ", count: " + _registersCountToRead; }
		}

		public byte[] Serialize() {
			var result = new byte[4];
			result[0] = (byte)(_firstRegisterAddress & 0xFF);
			result[1] = (byte) ((_firstRegisterAddress & 0xFF00) >> 8);
			return result;
		}

		public IList<byte> GetResult(byte[] reply) {
			if (reply.Length != ReplyLength) throw new Exception("wrong reply length");
			if (reply[0] != ReplyLength - 1) throw new Exception("first byte in reply must have value readed register values bytes count: " + (ReplyLength - 1));
			return reply.Skip(1).ToList();
		}

		public int ReplyLength
		{
			get {
				return 1 + _registersCountToRead * 2;
			}
		}

		public byte[] GetTestReply() {
			var rnd = new Random();
			var testResult = new byte[ReplyLength];
			rnd.NextBytes(testResult);

			return testResult;
		}
	}
}