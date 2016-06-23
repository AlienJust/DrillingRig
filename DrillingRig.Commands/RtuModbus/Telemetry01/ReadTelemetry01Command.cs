using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.RtuModbus.Telemetry01 {
	public class ReadTelemetry01Command : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ITelemetry01>, IRrModbusCommandWithTestReply {
		private readonly RtuModbusReadHoldingRegistersCommand _rtuModbusCmd = new RtuModbusReadHoldingRegistersCommand(100, 19);

		public byte CommandCode => _rtuModbusCmd.CommandCode;
		public string Name => $"Чтение телеметрии, группа параметров 01 >> {_rtuModbusCmd.Name}";

		public byte[] Serialize() {
			return _rtuModbusCmd.Serialize();
		}

		public ITelemetry01 GetResult(byte[] reply) {
			var rtuModbusParams = _rtuModbusCmd.GetResult(reply);

			return new Telemetry01Simple {
				We = rtuModbusParams[0].HighFirstSignedValue,
				Wm = rtuModbusParams[1].HighFirstSignedValue,
				WfbF = rtuModbusParams[2].HighFirstSignedValue,
				Isum = rtuModbusParams[3].HighFirstSignedValue,
				Uout = rtuModbusParams[4].HighFirstSignedValue,
				Udc = rtuModbusParams[5].HighFirstSignedValue,
				T1 = rtuModbusParams[6].HighFirstSignedValue,
				T2 = rtuModbusParams[7].HighFirstSignedValue,
				T3 = rtuModbusParams[8].HighFirstSignedValue,
				Text1 = rtuModbusParams[9].HighFirstSignedValue,
				Text2 = rtuModbusParams[10].HighFirstSignedValue,
				Text3 = rtuModbusParams[11].HighFirstSignedValue,
				Torq = rtuModbusParams[12].HighFirstSignedValue,
				TorqF = rtuModbusParams[13].HighFirstSignedValue,
				Mout = rtuModbusParams[14].HighFirstSignedValue,
				P = rtuModbusParams[15].HighFirstSignedValue,
				Din = rtuModbusParams[16].HighFirstUnsignedValue,
				Dout = rtuModbusParams[17].HighFirstUnsignedValue,
				SelTorq = rtuModbusParams[18].HighFirstUnsignedValue
			};
		}

		public int ReplyLength => _rtuModbusCmd.ReplyLength;

		public byte[] GetTestReply() {
			return _rtuModbusCmd.GetTestReply();
		}
	}
}