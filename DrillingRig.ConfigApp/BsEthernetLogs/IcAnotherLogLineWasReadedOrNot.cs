using DrillingRig.Commands.BsEthernetLogs;

namespace DrillingRig.ConfigApp.BsEthernetLogs
{
	/// <summary>
	/// Очередная строка логов была считана или нет
	/// </summary>
	/// <param name="logLine">Удачно считанная строка логов или null</param>
	delegate void IcAnotherLogLineWasReadedOrNot(IBsEthernetLogLine logLine);
}