using System.Collections.Generic;

namespace DrillingRid.Commands.Contracts.RtuModbus {
	public interface ICommandReadHoldingRegisters {
		string Name { get; }

		byte CommandCode { get; }
		TwoBytes FirstRegisterAddress { get; }
		IList<TwoBytes> RegisterValuesToWrite { get; } // Registers count is known by RegisterValuesToWrite.Count field
	}

	public interface ICommand {
		string Name { get; }
	}

	public interface ICommandModbus : ICommand {
		byte Function
	}
}