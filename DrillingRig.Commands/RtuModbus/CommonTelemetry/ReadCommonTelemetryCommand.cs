using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus.CommonTelemetry {
	public class ReadCommonTelemetryCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ICommonTelemetry>, IRrModbusCommandWithTestReply {
		private readonly RtuModbusReadHoldingRegistersCommand _rtuModbusCmd = new RtuModbusReadHoldingRegistersCommand(9000, 12);

		public byte CommandCode => _rtuModbusCmd.CommandCode;
		public string Name => $"Чтение общей телеметрии >> {_rtuModbusCmd.Name}";

		public byte[] Serialize() {
			return _rtuModbusCmd.Serialize();
		}

		public ICommonTelemetry GetResult(byte[] reply) {
			var rtuModbusParams = _rtuModbusCmd.GetResult(reply);
			var ainsLinkFault = rtuModbusParams[2].HighFirstUnsignedValue;
			return new CommonTelemetrySimple (
				rtuModbusParams[0].HighFirstUnsignedValue,
				rtuModbusParams[1].HighFirstUnsignedValue,
				(ainsLinkFault & 0x01) == 0x01,
				(ainsLinkFault & 0x02) == 0x02,
				(ainsLinkFault & 0x04) == 0x04,
				rtuModbusParams[3].HighFirstUnsignedValue,
				rtuModbusParams[4],
				rtuModbusParams[5],
				rtuModbusParams[6],
				rtuModbusParams[7],
				rtuModbusParams[8],
				rtuModbusParams[9],
				rtuModbusParams[10],
				rtuModbusParams[11]
			);
		}

		public int ReplyLength => _rtuModbusCmd.ReplyLength;

		public byte[] GetTestReply() {
			return _rtuModbusCmd.GetTestReply();
		}
	}
}