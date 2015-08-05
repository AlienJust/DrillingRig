namespace DrillingRid.Commands.Contracts {
	public interface IRrModbusCommandWithTestReply {
		byte[] GetTestReply();
	}
}