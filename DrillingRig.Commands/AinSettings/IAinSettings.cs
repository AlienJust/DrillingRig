
using AlienJust.Support.Collections;

namespace DrillingRig.Commands.AinSettings {
	public interface IAinSettings {
		BytesPair Reserved00 { get; }
		BytesPair KpW { get; }
		int KiW { get; }
		short FiNom { get; }
		short Imax { get; }
		short UdcMax { get; }
		short UdcMin { get; }
		short Fnom { get; }
		short Fmax { get; }

		short DflLim { get; }
		short FlMinMin { get; }
		
		short IoutMax { get; }
		short FiMin { get; }
		short DacCh { get; }

		short Imcw { get; }

		short Ia0 { get; }
		short Ib0 { get; }
		short Ic0 { get; }

		short Udc0 { get; }
		short TauR { get; }
		short Lm { get; }

		short Lsl { get; }
		short Lrl { get; }

		BytesPair Reserved24 { get; }
		BytesPair KpFi { get; }
		int KiFi { get; }

		BytesPair Reserved28 { get; }
		BytesPair KpId { get; }
		int KiId { get; }

		BytesPair Reserved32 { get; }
		BytesPair KpIq { get; }
		int KiIq { get; }

		short AccDfDt { get; }
		short DecDfDt { get; }

		short Unom { get; }

		/// <summary>
		/// Постоянная времени регулятора компенсации потока
		/// </summary>
		short TauFlLim { get; }

		short Rs { get; }
		short Fmin { get; }

		short TauM { get; }
		short TauF { get; }
		short TauFSet { get; }
		short TauFi { get; }

		short IdSetMin { get; }
		short IdSetMax { get; }

		/// <summary>
		/// В режиме чоппера нижний порог напряжения
		/// </summary>
		BytesPair UchMin { get; }

		/// <summary>
		/// В режиме чоппера верхний порог напряжения
		/// </summary>
		BytesPair UchMax { get; }

		BytesPair Reserved50 { get; }
		BytesPair Reserved51 { get; }
		short Np { get; }

		/// <summary>
		/// Порог компенсации напряжения DC за счет потока
		/// </summary>
		short UmodThr { get; }

		short EmdecDfdt { get; }
		short TextMax { get; }
		short ToHl { get; }


		bool Ain1LinkFault { get; }
		bool Ain2LinkFault { get; }
		bool Ain3LinkFault { get; }
	}
}