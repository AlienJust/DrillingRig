using DrillingRig.Commands.BsEthernetLogs;

namespace DrillingRig.ConfigApp.BsEthernetLogs
{
	/// <summary>
	/// ��������� ������ ����� ���� ������� ��� ���
	/// </summary>
	/// <param name="logLine">������ ��������� ������ ����� ��� null</param>
	delegate void IcAnotherLogLineWasReadedOrNot(IBsEthernetLogLine logLine);
}