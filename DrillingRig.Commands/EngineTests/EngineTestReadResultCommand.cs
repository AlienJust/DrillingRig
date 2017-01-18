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
			// TODO: perfomance could be increased easily
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
				throw new Exception("Reply error, reply length must be " + ReplyLength);

			var testResult = reply[0];
			// TODO: perfomance could be increased easily
			var rs = (short)GetFloatLe(reply.Skip(1).Take(4));
			var rr = GetFloatLe(reply.Skip(5).Take(4));
			var lsi = GetFloatLe(reply.Skip(9).Take(4));
			var lri = GetFloatLe(reply.Skip(13).Take(4));
			var lm = GetFloatLe(reply.Skip(17).Take(4));
			var flnom = (short)GetFloatLe(reply.Skip(21).Take(4));
			var j = GetFloatLe(reply.Skip(25).Take(4));
			var tr = GetFloatLe(reply.Skip(29).Take(4));
			var roverl = GetFloatLe(reply.Skip(33).Take(4));
			

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