using System;
using System.Collections.Generic;
using DrillingRid.Commands.Contracts;
using DrillingRig.Commands.BsEthernetSettings;

namespace DrillingRig.Commands.EngineTests {
	public class EngineTestLaunchCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IWriteBsEthernetSettingsResult>, IRrModbusCommandWithTestReply {
		private readonly EngineTestId _testId;
		private readonly IEngineTestParams _testParams;

		public EngineTestLaunchCommand(EngineTestId testId, IEngineTestParams testParams) {
			_testId = testId;
			_testParams = testParams;
		}

		public byte CommandCode => 0x93;

		public string Name => "Запуск теста двигателя";

		public byte[] Serialize() {
			var result = new List<byte> {(byte) _testId};

			result.AddRange(BitConverter.GetBytes(_testParams.Te1));
			result.AddRange(BitConverter.GetBytes(_testParams.T1C));
			result.AddRange(BitConverter.GetBytes(_testParams.T2C));
			result.AddRange(BitConverter.GetBytes(_testParams.K21));
			result.AddRange(BitConverter.GetBytes(_testParams.Te6));
			result.AddRange(BitConverter.GetBytes(_testParams.F1));
			result.AddRange(BitConverter.GetBytes(_testParams.F2));
			result.AddRange(BitConverter.GetBytes(_testParams.Acc8));
			result.AddRange(BitConverter.GetBytes(_testParams.Dir10));

			result.AddRange(BitConverter.GetBytes(_testParams.Tj1));
			result.AddRange(BitConverter.GetBytes(_testParams.Tj2));
			result.AddRange(BitConverter.GetBytes(_testParams.Tj3));
			result.AddRange(BitConverter.GetBytes(_testParams.Tj4));
			
			result.AddRange(BitConverter.GetBytes(_testParams.Kp1));
			result.AddRange(BitConverter.GetBytes(_testParams.Ki1));
			result.AddRange(BitConverter.GetBytes(_testParams.Kp6));
			result.AddRange(BitConverter.GetBytes(_testParams.Ki6));

			result.AddRange(BitConverter.GetBytes(_testParams.TauI));//= 7e-3 
			result.AddRange(BitConverter.GetBytes(_testParams.TauFi));//= 50e-3;
			result.AddRange(BitConverter.GetBytes(_testParams.TauSpd));
			
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