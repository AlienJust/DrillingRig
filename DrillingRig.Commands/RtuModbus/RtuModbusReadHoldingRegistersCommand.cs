using System;
using System.Collections.Generic;
using AlienJust.Support.Collections;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus {
	public class RtuModbusReadHoldingRegistersCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IList<BytesPair>>, IRrModbusCommandWithTestReply {
		private readonly ushort _firstRegisterAddress;
		private readonly ushort _registersCountToRead;

		public RtuModbusReadHoldingRegistersCommand(ushort firstRegisterAddress, ushort registersCountToRead) {
			_firstRegisterAddress = firstRegisterAddress;
			_registersCountToRead = registersCountToRead;
		}

		public byte CommandCode => 0x04;

		public string Name => "Read holding registers Modbus RTU command, first address: " + _firstRegisterAddress + ", count: " + _registersCountToRead;

		public byte[] Serialize() {
			var result = new byte[4];
			result[0] = (byte)((_firstRegisterAddress & 0xFF00) >> 8);
			result[1] = (byte) (_firstRegisterAddress & 0xFF);

			result[2] = (byte)((_registersCountToRead & 0xFF00) >> 8);
			result[3] = (byte)(_registersCountToRead & 0xFF);
			return result;
		}

		public IList<BytesPair> GetResult(byte[] reply) {
			if (reply.Length != ReplyLength) throw new Exception("wrong reply length");
			if (reply[0] != ReplyLength - 1) throw new Exception("first byte in reply must have value readed register values bytes count: " + (ReplyLength - 1));

			List<BytesPair> registers = new List<BytesPair>();
			for (int i = 0; i < _registersCountToRead; ++i) {
				registers.Add(new BytesPair(reply[1 + i*2], reply[2 + i*2]));
			}
			return registers;
		}

		public int ReplyLength => 1 + _registersCountToRead*2;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var testResult = new byte[ReplyLength];
			rnd.NextBytes(testResult);
			testResult[0] = (byte) (ReplyLength - 1);
			return testResult;
		}
	}
}