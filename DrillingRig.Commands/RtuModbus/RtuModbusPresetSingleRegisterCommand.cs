using System;
using System.Collections.Generic;
using System.Linq;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus
{
	public class RtuModbusPresetSingleRegisterCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IList<byte>>, IRrModbusCommandWithTestReply
	{
		private readonly ushort _registerAddress;
		private readonly ushort _valueToSet;

		public RtuModbusPresetSingleRegisterCommand(ushort registerAddress, ushort valueToSet) {
			_registerAddress = registerAddress;
			_valueToSet = valueToSet;
		}

		public byte CommandCode
		{
			get { return 0x06; }
		}

		public string Name
		{
			get { return "Preset Single Register at address: " + _registerAddress + ", value to set: " + _valueToSet; }
		}

		public byte[] Serialize() {
			var result = new byte[4];
			// TODO:
			//result[0] = (byte)(_firstRegisterAddress & 0xFF);
			//result[1] = (byte) ((_firstRegisterAddress & 0xFF00) >> 8);
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
				//return 1 + _registersCountToRead * 2;
				throw new NotImplementedException("TODO");
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