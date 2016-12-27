using System;
using System.Collections.Generic;
using System.Linq;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.EngineTests {
	public class EngineTestReadResultCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IEngineTestResult>, IRrModbusCommandWithTestReply {
		public byte CommandCode => 0x94;
		public string Name => "Чтение результатов тестирования двигателя";

		public byte[] Serialize()
		{
			return new byte[0];
		}

		/// <summary>
		/// Gets int32 from bytes ordered little endian
		/// </summary>
		/// <param name="bytes">Little endian bytes of int32</param>
		/// <returns>Int32 from bytes</returns>
		int GetIntLe(IEnumerable<byte> bytes)
		{
			var preparedBytes = BitConverter.IsLittleEndian ? bytes.ToArray() : bytes.Reverse().ToArray();
			return BitConverter.ToInt32(preparedBytes, 0);
		}

		/// <summary>
		/// Gets float from bytes ordered little endian
		/// </summary>
		/// <param name="bytes">Little endian bytes of float</param>
		/// <returns>Float from bytes</returns>
		float GetFloatLe(IEnumerable<byte> bytes)
		{
			var preparedBytes = BitConverter.IsLittleEndian ? bytes.ToArray() : bytes.Reverse().ToArray();
			return BitConverter.ToSingle(preparedBytes, 0);
		}

		/// <summary>
		/// Получает результат выполнения команды
		/// </summary>
		/// <param name="reply">Байты ответа</param>
		/// <returns>Успешность запуска тестирования</returns>
		public IEngineTestResult GetResult(byte[] reply) {
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be 1");
			var testResult = reply[0];
			var rs = GetIntLe(reply.Skip(1).Take(4));
			var rr = GetIntLe(reply.Skip(5).Take(4));
			var lsi = GetIntLe(reply.Skip(9).Take(4));
			var lri = GetIntLe(reply.Skip(13).Take(4));
			var lm = GetIntLe(reply.Skip(17).Take(4));
			var flnom = GetIntLe(reply.Skip(21).Take(4));
			var j = GetIntLe(reply.Skip(25).Take(4));
			var tr = GetIntLe(reply.Skip(29).Take(4));
			var roverl = GetIntLe(reply.Skip(33).Take(4));
			

			return new EngineTestResultSimple(testResult, rs, rr, lsi, lri, lm, flnom, j, tr, roverl);
		}

		public int ReplyLength => 37; // 1 + 4*9

		public byte[] GetTestReply() {
			var rndBytes = new byte[ReplyLength];
			new Random().NextBytes(rndBytes);
			return rndBytes;
		}
	}
}