namespace DrillingRid.Commands.Contracts
{
    public interface IRrModbusCommand
    {
		byte CommandCode { get; }
		string Name { get; }
	    byte[] Serialize();
    }
}
