using System;
using System.Collections.Generic;
using System.Linq;
using DrillingRid.Commands.Contracts;
using DrillingRig.Commands.BsEthernetSettings;

namespace DrillingRig.Commands.EngineTests {
	public class EngineTestLaunchCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<bool>, IRrModbusCommandWithTestReply {
		private readonly EngineTestId _testId;
		private readonly IEngineTestParams _testParams;

		public EngineTestLaunchCommand(EngineTestId testId, IEngineTestParams testParams) {
			_testId = testId;
			_testParams = testParams;
		}

		public byte CommandCode => 0x93;

		public string Name => "Запуск теста двигателя";

		public byte[] Serialize() {
			var result = new List<byte> { (byte)_testId };

			result.AddRange(GetBytesLe(_testParams.Te1));
			result.AddRange(GetBytesLe(_testParams.T1C));
			result.AddRange(GetBytesLe(_testParams.T2C));
			result.AddRange(GetBytesLe(_testParams.K21));
			result.AddRange(GetBytesLe(_testParams.Te6));
			result.AddRange(GetBytesLe(_testParams.F1));
			result.AddRange(GetBytesLe(_testParams.F2));
			result.AddRange(GetBytesLe(_testParams.Acc8));
			result.AddRange(GetBytesLe(_testParams.Dir10));

			result.AddRange(GetBytesLe(_testParams.Tj1));
			result.AddRange(GetBytesLe(_testParams.Tj2));
			result.AddRange(GetBytesLe(_testParams.Tj3));
			result.AddRange(GetBytesLe(_testParams.Tj4));

			result.AddRange(GetBytesLe(_testParams.Kp1));
			result.AddRange(GetBytesLe(_testParams.Ki1));
			result.AddRange(GetBytesLe(_testParams.Kp6));
			result.AddRange(GetBytesLe(_testParams.Ki6));

			result.AddRange(GetBytesLe(_testParams.TauI));//= 7e-3 
			result.AddRange(GetBytesLe(_testParams.TauFi));//= 50e-3;
			result.AddRange(GetBytesLe(_testParams.TauSpd));

			return result.ToArray();
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
		public bool GetResult(byte[] reply) {
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be 1");
			return reply[0] == 0;
		}

		public int ReplyLength => 1;

		public byte[] GetTestReply() {
			return new byte[] {0};
		}
	}
}