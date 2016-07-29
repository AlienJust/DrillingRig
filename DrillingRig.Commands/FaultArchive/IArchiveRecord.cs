using System;
using AlienJust.Support.Numeric;

namespace DrillingRig.Commands.FaultArchive {
	public interface IArchiveRecord {
		byte Second { get; }
		byte Minute { get; }

		byte Hour { get; }

		byte Day { get; }

		byte Month { get; }

		byte Year { get; }
		DateTime Time { get; }
		ushort FaultState { get; }
		ushort Mcw { get; }
		ushort Msw { get; }

		Crc16 Crc { get; }

		bool IsCrcCorrect { get; }
	}
}