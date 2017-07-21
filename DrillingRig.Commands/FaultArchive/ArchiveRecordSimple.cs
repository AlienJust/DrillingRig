using System;
using System.Collections;
using System.Collections.Generic;
using AlienJust.Support.Collections;
using AlienJust.Support.Numeric;

namespace DrillingRig.Commands.FaultArchive {
	class ArchiveRecordSimple : IArchiveRecord {
		public byte Second { get; set; }
		public byte Minute { get; set; }
		public byte Hour { get; set; }
		public byte Day { get; set; }
		public byte Month { get; set; }
		public byte Year { get; set; }

		public DateTime Time => new DateTime(Year, Month, Day, Hour, Minute, Second);

		public ushort FaultState { get; set; }
		public ushort Mcw { get; set; }
		public ushort Msw { get; set; }
		public Crc16 Crc { get; set; }

		public bool IsCrcCorrect { get; set; }
	}

	class ArchiveRecordFromReplyBuilder /*TODO: inherit from and implement IBuilder<IArchiveRecord>*/ {
		private readonly IList<byte> _bytesToBuild;
		public ArchiveRecordFromReplyBuilder(IList<byte> bytesToBuild) {
			_bytesToBuild = bytesToBuild;
		}

		public IArchiveRecord Build() {
			var receivedCrc = new Crc16(_bytesToBuild[12], _bytesToBuild[13]);
			var crcMustBe = MathExtensions.GetCrc16FromIlist(_bytesToBuild, 0, _bytesToBuild.Count - 2);
			bool isCrcCorrect = receivedCrc.High == crcMustBe.High && receivedCrc.Low == crcMustBe.Low;
			return new ArchiveRecordSimple {
				Second = _bytesToBuild[0],
				Minute = _bytesToBuild[1],
				Hour = _bytesToBuild[2],
				Day = _bytesToBuild[3],
				Month = _bytesToBuild[4],
				Year = _bytesToBuild[5],

				FaultState = new BytesPair(_bytesToBuild[6], _bytesToBuild[7]).LowFirstUnsignedValue,
				Mcw = new BytesPair(_bytesToBuild[8], _bytesToBuild[9]).LowFirstUnsignedValue,
				Msw = new BytesPair(_bytesToBuild[10], _bytesToBuild[11]).LowFirstUnsignedValue,
				Crc = receivedCrc,
				IsCrcCorrect = false
			};
		}
	}
}