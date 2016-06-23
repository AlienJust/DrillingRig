using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus.Telemetry04 {
	public class ReadTelemetry04Command : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ITelemetry04>, IRrModbusCommandWithTestReply {
		private readonly RtuModbusReadHoldingRegistersCommand _rtuModbusCmd = new RtuModbusReadHoldingRegistersCommand(400, 3);

		public byte CommandCode => _rtuModbusCmd.CommandCode;
		public string Name => $"Чтение телеметрии, группа параметров 04 >> {_rtuModbusCmd.Name}";

		public byte[] Serialize() {
			return _rtuModbusCmd.Serialize();
		}

		public ITelemetry04 GetResult(byte[] reply) {
			var rtuModbusParams = _rtuModbusCmd.GetResult(reply);
			return new Telemetry04Simple {
				Pver = rtuModbusParams[0].HighFirstSignedValue,
				PvDate = rtuModbusParams[1].HighFirstUnsignedValue,
				BsVer = rtuModbusParams[2].HighFirstSignedValue
			};
		}

		public int ReplyLength => _rtuModbusCmd.ReplyLength;

		public byte[] GetTestReply() {
			return _rtuModbusCmd.GetTestReply();
		}
	}
}