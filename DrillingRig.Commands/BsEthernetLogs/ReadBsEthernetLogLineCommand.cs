using System;
using System.Text;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.BsEthernetLogs {
	/// <summary>
	/// БС-Ethernet умеет вести логи,
	/// записывает построчно,
	/// каждая строка лога имеет номер.
	/// </summary>
	public class ReadBsEthernetLogLineCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IBsEthernetLogLine>, IRrModbusCommandWithTestReply {

		public byte CommandCode => 0x84;

		public string Name => "Чтение номинальных значений БС-Ethernet";

		public byte[] Serialize() {
			return new byte[0];
		}

		public IBsEthernetLogLine GetResult(byte[] reply) {
			//reply length = first 2 bytes are log line number + 200 bytes of log line content
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be " + ReplyLength);
			var logLineNumber = reply[0] + reply[1] * 256;
			var encoding = Encoding.GetEncoding(1251); // ASCII windows cyrillic
			var logLineContent = encoding.GetString(reply, 2, reply.Length - 2);
			return new BsEthernetLogLineSimple(logLineNumber, logLineContent);
		}

		public int ReplyLength => 202;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var reply = new byte[ReplyLength];
			rnd.NextBytes(reply);
			return reply;
		}
	}
}