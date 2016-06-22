using System;
using System.Linq;
using DrillingRid.Commands.Contracts;
using DrillingRig.Commands.RtuModbus.Telemetry01;

namespace DrillingRig.Commands.AinTelemetry {
	public class ReadTelemetry01Command : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<ITelemetry01>, IRrModbusCommandWithTestReply
	{

		public byte CommandCode => 0x04;

		public string Name => "Чтение телеметрии, группа параметров 01";

		public byte[] Serialize() {
			return new byte[] {
				0,
				100,
				0,
				19
			};
		}

		public ITelemetry01 GetResult(byte[] reply) {
			// TODO: check if reply[0] is equal _zbAinNumber
			if (reply[0] != 38) throw new Exception("В заголовке указано (в нулевом байте ответа) количество байт, отличающееся от ожидаемого (ожидалось 38 байт данных в заголовке)");
			//var oldReply = reply.Skip(4).ToList();



			return new Telemetry01Simple {
				We = (short) (reply[2] + (reply[1] << 8)),
				Wm = (short) (reply[4] + (reply[3] << 8)),
				WfbF = (short) (reply[6] + (reply[5] << 8)),
				Isum = (short) (reply[8] + (reply[7] << 8)),
				Uout = (short) (reply[10] + (reply[9] << 8)),
				Udc = (short) (reply[12] + (reply[11] << 8)),
				T1 = (short) (reply[14] + (reply[13] << 8)),
				T2 = (short) (reply[16] + (reply[15] << 8)),
				T3 = (short) (reply[18] + (reply[17] << 8)),
				Text1 = (short) (reply[20] + (reply[19] << 8)),
				Text2 = (short) (reply[22] + (reply[21] << 8)),
				Text3 = (short) (reply[24] + (reply[23] << 8)),
				Torq = (short) (reply[26] + (reply[25] << 8)),
				TorqF = (short) (reply[28] + (reply[27] << 8)),
				Mout = (short) (reply[30] + (reply[29] << 8)),
				P = (short) (reply[32] + (reply[31] << 8)),
				Din = (ushort) (reply[34] + (reply[33] << 8)),
				Dout = (ushort) (reply[36] + (reply[35] << 8)),
				SelTorq = (ushort) (reply[38] + (reply[37] << 8))
			};
		}

		public int ReplyLength => 39;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			result[0] = 38;
			
			result[1] = (byte) rnd.Next(0, 21);
			result[2] = 0;

			result[3] = (byte)rnd.Next(0, 6);
			result[4] = 0;

			result[36] = 0x85;
			result[38] = 0x1F;
			return result;
		}
	}
}