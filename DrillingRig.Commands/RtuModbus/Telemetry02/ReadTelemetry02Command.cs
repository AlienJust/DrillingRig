using System;
using DrillingRid.Commands.Contracts;
using DrillingRig.Commands.RtuModbus.Telemetry01;

namespace DrillingRig.Commands.RtuModbus.Telemetry02 {
	public class ReadTelemetry02Command : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ITelemetry02>, IRrModbusCommandWithTestReply {
		private readonly RtuModbusReadHoldingRegistersCommand _rtuModbusCmd = new RtuModbusReadHoldingRegistersCommand(200, 11);

		public byte CommandCode => _rtuModbusCmd.CommandCode;
		public string Name => $"Чтение телеметрии, группа параметров 02 >> {_rtuModbusCmd.Name}";

		public byte[] Serialize() {
			return _rtuModbusCmd.Serialize();
		}

		public ITelemetry02 GetResult(byte[] reply) {
			var rtuModbusParams = _rtuModbusCmd.GetResult(reply);
			return new Telemetry02Simple {
				Wout = rtuModbusParams[0].HighFirstSignedValue,
				WsetF = rtuModbusParams[1].HighFirstSignedValue,
				FIset = rtuModbusParams[2].HighFirstSignedValue,
				FImag = rtuModbusParams[3].HighFirstSignedValue,
				FImagF = rtuModbusParams[4].HighFirstSignedValue,
				IqSet = rtuModbusParams[5].HighFirstSignedValue,
				IdSet = rtuModbusParams[6].HighFirstSignedValue,
				Ed = rtuModbusParams[7].HighFirstSignedValue,
				Eq = rtuModbusParams[8].HighFirstSignedValue,
				Ef = rtuModbusParams[9].HighFirstSignedValue,
				Efi = rtuModbusParams[10].HighFirstSignedValue
			};
		}

		public int ReplyLength => _rtuModbusCmd.ReplyLength;

		public byte[] GetTestReply() {
			return _rtuModbusCmd.GetTestReply();
		}
	}
}