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
		private const int BytesPerSingleRecifier = 16;

		public byte CommandCode => 0x90;

		public string Name => "Чтение телеметрии блока выпрямителей";

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
						(short)(reply[i * BytesPerSingleRecifier + 0] + (reply[i * BytesPerSingleRecifier + 1] << 8)),
						(short)(reply[i * BytesPerSingleRecifier + 2] + (reply[i * BytesPerSingleRecifier + 3] << 8)),
						(short)(reply[i * BytesPerSingleRecifier + 4] + (reply[i * BytesPerSingleRecifier + 5] << 8)),
						(short)(reply[i * BytesPerSingleRecifier + 6] + (reply[i * BytesPerSingleRecifier + 7] << 8)),
						(short)(reply[i * BytesPerSingleRecifier + 8] + (reply[i * BytesPerSingleRecifier + 9] << 8)),
						(short)(reply[i * BytesPerSingleRecifier + 10] + (reply[i * BytesPerSingleRecifier + 11] << 8)),
						(short)(reply[i * BytesPerSingleRecifier + 12] + (reply[i * BytesPerSingleRecifier + 13] << 8)),
						(short)(reply[i * BytesPerSingleRecifier + 14] + (reply[i * BytesPerSingleRecifier + 15] << 8))
						));
			}
			return result;
		}
		// three Ains each: 1 byte - ainNumber + 32 * 2 bytes + 1 byte of Marat's status (flags)
		public int ReplyLength => RectifiersCount * BytesPerSingleRecifier;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			return result;
		}
	}
}