using System;
using System.Collections.Generic;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.Rectifier
{
	/// <summary>
	/// Команда чтения телеметрии выпрямителей
	/// </summary>
	public class ReadRectifierTelemetriesCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IList<IRectifierTelemetry>>, IRrModbusCommandWithTestReply
	{
		const int RectifiersCount = 6;

		public byte CommandCode
		{
			get { return 0x90; }
		}

		public string Name
		{
			get { return "Чтение телеметрии блока выпрямителей"; }
		}

		public byte[] Serialize()
		{
			return new byte[0];
		}

		public IList<IRectifierTelemetry> GetResult(byte[] reply)
		{
			if (reply.Length != ReplyLength) throw new Exception("неверная длина ответа");
			var result = new List<IRectifierTelemetry>();
			for (int i = 0; i < RectifiersCount; ++i) {
				result.Add(
					new RectifierTelemetrySimple(
						(short) (reply[0] + (reply[1] << 8)),
						(short) (reply[2] + (reply[3] << 8)),
						(short) (reply[4] + (reply[5] << 8)),
						(short) (reply[6] + (reply[7] << 8)),
						(short) (reply[8] + (reply[9] << 8)),
						(short) (reply[10] + (reply[11] << 8)),
						(short) (reply[12] + (reply[13] << 8)),
						(short) (reply[14] + (reply[15] << 8))
						));
			}
			return result;
		}

		public int ReplyLength
		{
			get {
				return RectifiersCount * 16; // three Aiks each: 1 byte - ainNumber + 32 * 2 bytes + 1 byte of Marat's status (flags)
			}
		}

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}