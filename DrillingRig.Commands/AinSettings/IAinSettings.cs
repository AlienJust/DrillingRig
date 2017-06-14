
using AlienJust.Support.Collections;

namespace DrillingRig.Commands.AinSettings {
	public interface IAinSettings {
		BytesPair Reserved00 { get; }

		double KpW { get; }
		double KiW { get; }

		double FiNom { get; }
		short Imax { get; }
		short UdcMax { get; }
		short UdcMin { get; }
		double Fnom { get; }
		double Fmax { get; }

		double DflLim { get; }
		double FlMinMin { get; }
		
		short IoutMax { get; }
		double FiMin { get; }

		ushort DacCh { get; }
		ushort Imcw { get; }
		
		short Ia0 { get; }
		short Ib0 { get; }
		short Ic0 { get; }

		short Udc0 { get; }

		double TauR { get; }
		double Lm { get; }
		double Lsl { get; }
		double Lrl { get; }

		BytesPair Reserved24 { get; }
		double KpFi { get; }
		double KiFi { get; }

		BytesPair Reserved28 { get; }
		double KpId { get; }
		double KiId { get; }

		BytesPair Reserved32 { get; }
		double KpIq { get; }
		double KiIq { get; }

		double AccDfDt { get; }
		double DecDfDt { get; }

		//short Unom { get; }
		double Unom { get; }

		/// <summary>
		/// Постоянная времени регулятора компенсации потока
		/// </summary>
		double TauFlLim { get; }

		double Rs { get; }

		double Fmin { get; }

		double TauM { get; }
		double TauF { get; }
		double TauFSet { get; }
		double TauFi { get; }

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
		double UmodThr { get; }

		double EmdecDfdt { get; }
		short TextMax { get; }
		short ToHl { get; }


		bool Ain1LinkFault { get; }
		bool Ain2LinkFault { get; }
		bool Ain3LinkFault { get; }
	}
}