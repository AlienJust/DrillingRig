using System;
using System.Collections;
using System.Collections.Generic;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.FaultArchive {
	public class ReadArchiveCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IList<IArchiveRecord>>, IRrModbusCommandWithTestReply
	{
		private readonly byte _zeroBasedArchvieNumber;

		public ReadArchiveCommand(byte zeroBasedArchvieNumber) {
			_zeroBasedArchvieNumber = zeroBasedArchvieNumber;
		}

		public byte CommandCode => 0x87;

		public string Name => "Чтение аварийного архива #" + (_zeroBasedArchvieNumber + 1);

		public byte[] Serialize()
		{
			// first byte is low
			return new[] {
				(byte)(_zeroBasedArchvieNumber + 1)
			};
		}

		public IList<IArchiveRecord> GetResult(byte[] reply) {
			if (reply.Length != ReplyLength) throw new Exception("Неверная длина ответа от АИН");
			var result = new List<IArchiveRecord>();
			const int recordLen = 14;
			var recordBytes = new byte[recordLen];
			for (int zbArchNumber = 0; zbArchNumber < 10; zbArchNumber++) {
				int recordStartAbsPosition = zbArchNumber*recordLen;

				for (int inRecordPosition = 0; inRecordPosition < recordLen; ++inRecordPosition) {
					int curAbsPosition = recordStartAbsPosition + inRecordPosition;
					recordBytes[inRecordPosition] = reply[curAbsPosition];
				}
				result.Add(new ArchiveRecordFromReplyBuilder(recordBytes).Build());
			}
			return result;
		}

		public int ReplyLength => 140;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}