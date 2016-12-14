using System;
using System.Linq;
using System.Text;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.BsEthernetLogs {
	/// <summary>
	/// БС-Ethernet умеет вести логи,
	/// записывает построчно,
	/// каждая строка лога имеет номер.
	/// </summary>
	public class ReadBsEthernetLogLineCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IBsEthernetLogLine>, IRrModbusCommandWithTestReply {

		public byte CommandCode => 0x8D;

		public string Name => "Чтение сроки логов БС-Ethernet";

		public byte[] Serialize() {
			return new byte[0];
		}

		public IBsEthernetLogLine GetResult(byte[] reply) {
			//reply length = first 2 bytes are log line number + 200 bytes of log line content
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be " + ReplyLength);
			var logLineNumber = reply[0] + reply[1] * 256;
			var encoding = Encoding.GetEncoding(1251); // ASCII windows cyrillic
			var contentBytes = reply.Skip(2).TakeWhile(b => b != 0).ToArray();
			var logLineContent = encoding.GetString(contentBytes);
			return new BsEthernetLogLineSimple(logLineNumber, logLineContent);
		}

		public int ReplyLength => 202;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var reply = new byte[ReplyLength];
			var number = rnd.Next(0, 65536);
			reply[0] = (byte)(number & 0xFF);
			reply[1] = (byte)((number & 0xFF00)>> 8);
			var content = "This is test log line #" + number + ", случайное значение строки: " + rnd.Next(0, 256);
			var encoding = Encoding.GetEncoding(1251); // ASCII windows cyrillic
			encoding.GetBytes(content).CopyTo(reply, 2);
			return reply;
		}
	}
}