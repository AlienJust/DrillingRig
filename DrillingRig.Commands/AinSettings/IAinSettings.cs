
using AlienJust.Support.Collections;

namespace DrillingRig.Commands.AinSettings {
	public interface IAinSettings {
		BytesPair Reserved00 { get; }

		decimal KpW { get; }
		decimal KiW { get; }

		decimal FiNom { get; }
		short Imax { get; }
		short UdcMax { get; }
		short UdcMin { get; }
		decimal Fnom { get; }
		decimal Fmax { get; }

		decimal DflLim { get; }
		decimal FlMinMin { get; }
		
		short IoutMax { get; }
		decimal FiMin { get; }

		ushort DacCh { get; }
		ushort Imcw { get; }
		
		short Ia0 { get; }
		short Ib0 { get; }
		short Ic0 { get; }

		short Udc0 { get; }

		decimal TauR { get; }
		decimal Lm { get; }
		decimal Lsl { get; }
		decimal Lrl { get; }

		BytesPair Reserved24 { get; }
		decimal KpFi { get; }
		decimal KiFi { get; }

		BytesPair Reserved28 { get; }
		decimal KpId { get; }
		decimal KiId { get; }

		BytesPair Reserved32 { get; }
		decimal KpIq { get; }
		decimal KiIq { get; }

		decimal AccDfDt { get; }
		decimal DecDfDt { get; }

		//short Unom { get; }
		decimal Unom { get; }

		/// <summary>
		/// Постоянная времени регулятора компенсации потока
		/// </summary>
		decimal TauFlLim { get; }

		decimal Rs { get; }

		decimal Fmin { get; }

		decimal TauM { get; }
		decimal TauF { get; }
		decimal TauFSet { get; }
		decimal TauFi { get; }

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

		int Np { get; }
		//
		int NimpFloorCode { get; }
		AinTelemetryFanWorkmode FanMode { get; }


		/// <summary>
		/// Порог компенсации напряжения DC за счет потока
		/// </summary>
		decimal UmodThr { get; }

		decimal EmdecDfdt { get; }
		short TextMax { get; }
		short ToHl { get; }


		bool Ain1LinkFault { get; }
		bool Ain2LinkFault { get; }
		bool Ain3LinkFault { get; }
	}
}