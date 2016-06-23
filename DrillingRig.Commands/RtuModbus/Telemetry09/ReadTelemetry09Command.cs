using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus.Telemetry09 {
	public class ReadTelemetry09Command : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ITelemetry09>, IRrModbusCommandWithTestReply {
		private readonly RtuModbusReadHoldingRegistersCommand _rtuModbusCmd = new RtuModbusReadHoldingRegistersCommand(900, 7);

		public byte CommandCode => _rtuModbusCmd.CommandCode;
		public string Name => $"Чтение телеметрии, группа параметров 09 >> {_rtuModbusCmd.Name}";

		public byte[] Serialize() {
			return _rtuModbusCmd.Serialize();
		}

		public ITelemetry09 GetResult(byte[] reply) {
			var rtuModbusParams = _rtuModbusCmd.GetResult(reply);
			return new Telemetry09Simple {
				Status1 = rtuModbusParams[0].HighFirstUnsignedValue,
				Status2 = rtuModbusParams[1].HighFirstUnsignedValue,
				Status3 = rtuModbusParams[2].HighFirstUnsignedValue,
				FaultState = rtuModbusParams[3].HighFirstUnsignedValue,
				Warning = rtuModbusParams[4].HighFirstUnsignedValue,
				ErrLinkAin = rtuModbusParams[5].HighFirstUnsignedValue,
				FollowStatus = rtuModbusParams[6].HighFirstUnsignedValue,
			};
		}

		public int ReplyLength => _rtuModbusCmd.ReplyLength;

		public byte[] GetTestReply() {
			return _rtuModbusCmd.GetTestReply();
		}
	}
}