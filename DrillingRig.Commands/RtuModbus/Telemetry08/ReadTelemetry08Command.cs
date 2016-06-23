using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus.Telemetry08 {
	public class ReadTelemetry08Command : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ITelemetry08>, IRrModbusCommandWithTestReply {
		private readonly RtuModbusReadHoldingRegistersCommand _rtuModbusCmd = new RtuModbusReadHoldingRegistersCommand(800, 6);

		public byte CommandCode => _rtuModbusCmd.CommandCode;
		public string Name => $"Чтение телеметрии, группа параметров 08 >> {_rtuModbusCmd.Name}";

		public byte[] Serialize() {
			return _rtuModbusCmd.Serialize();
		}

		public ITelemetry08 GetResult(byte[] reply) {
			var rtuModbusParams = _rtuModbusCmd.GetResult(reply);
			return new Telemetry08Simple {
				Msw = rtuModbusParams[0].HighFirstUnsignedValue,
				Asw = rtuModbusParams[1].HighFirstUnsignedValue,
				EngineState = rtuModbusParams[2].HighFirstUnsignedValue,
				FollowMsw = rtuModbusParams[3].HighFirstUnsignedValue,
				FollowAsw = rtuModbusParams[4].HighFirstUnsignedValue,
				FollowEngineState = rtuModbusParams[5].HighFirstUnsignedValue,
			};
		}

		public int ReplyLength => _rtuModbusCmd.ReplyLength;

		public byte[] GetTestReply() {
			return _rtuModbusCmd.GetTestReply();
		}
	}
}