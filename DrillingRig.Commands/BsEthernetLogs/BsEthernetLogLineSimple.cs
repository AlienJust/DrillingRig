namespace DrillingRig.Commands.BsEthernetLogs
{
	public class BsEthernetLogLineSimple : IBsEthernetLogLine {
		public BsEthernetLogLineSimple(int number, string content) {
			Number = number;
			Content = content;
		}

		public int Number { get; }
		public string Content { get; }
	}
}