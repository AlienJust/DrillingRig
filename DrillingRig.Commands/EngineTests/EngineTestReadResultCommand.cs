using System;
using System.Collections.Generic;
using System.Linq;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.EngineTests {
	public class EngineTestReadResultCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IEngineTestResult>, IRrModbusCommandWithTestReply {

		public EngineTestReadResultCommand() {

		}

		public byte CommandCode => 0x93;

		public string Name => "Запуск теста двигателя";

		public byte[] Serialize()
		{
			return new byte[0];
		}

		byte[] GetBytesLe(int value) {
			var bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian) return bytes;
			return bytes.Reverse().ToArray(); // TODO: improve perfomance
		}
		byte[] GetBytesLe(float value) {
			var bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian) return bytes;
			return bytes.Reverse().ToArray();
		}

		/// <summary>
		/// Получает результат выполнения команды
		/// </summary>
		/// <param name="reply">Байты ответа</param>
		/// <returns>Успешность запуска тестирования</returns>
		public IEngineTestResult GetResult(byte[] reply) {
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be 1");
			throw new NotImplementedException("TODO");
		}

		public int ReplyLength => 1;

		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}