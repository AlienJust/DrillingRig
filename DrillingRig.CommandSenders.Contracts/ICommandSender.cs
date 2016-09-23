namespace DrillingRig.CommandSenders.Contracts {
	public interface ICommandSender : IRrModbusCommandSender
	{
		void EndWork();
	}
}