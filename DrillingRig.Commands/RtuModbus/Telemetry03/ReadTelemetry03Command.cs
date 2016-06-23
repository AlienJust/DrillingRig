using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus.Telemetry03 {
	public class ReadTelemetry03Command : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ITelemetry03>, IRrModbusCommandWithTestReply {
		private readonly RtuModbusReadHoldingRegistersCommand _rtuModbusCmd = new RtuModbusReadHoldingRegistersCommand(300, 11);

		public byte CommandCode => _rtuModbusCmd.CommandCode;
		public string Name => $"Чтение телеметрии, группа параметров 03 >> {_rtuModbusCmd.Name}";

		public byte[] Serialize() {
			return _rtuModbusCmd.Serialize();
		}

		public ITelemetry03 GetResult(byte[] reply) {
			var rtuModbusParams = _rtuModbusCmd.GetResult(reply);
			return new Telemetry03Simple {
				Kpwm = rtuModbusParams[0].HighFirstSignedValue,
				Ud = rtuModbusParams[1].HighFirstSignedValue,
				Uq = rtuModbusParams[2].HighFirstSignedValue,
				Id = rtuModbusParams[3].HighFirstSignedValue,
				Iq = rtuModbusParams[4].HighFirstSignedValue,
				UCompQ = rtuModbusParams[5].HighFirstSignedValue,
				UcompD = rtuModbusParams[6].HighFirstSignedValue,
				Aux1 = rtuModbusParams[7].HighFirstSignedValue,
				Aux2 = rtuModbusParams[8].HighFirstSignedValue,
				I2t = rtuModbusParams[9].HighFirstSignedValue,
				FollowMout = rtuModbusParams[10].HighFirstSignedValue
			};
		}

		public int ReplyLength => _rtuModbusCmd.ReplyLength;

		public byte[] GetTestReply() {
			return _rtuModbusCmd.GetTestReply();
		}
	}
}