using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsStorage
{
	/// <summary>
	/// ��������� �������� ���������
	/// </summary>
	interface IEngineSettingsStorage
	{
		IEngineSettings EngineSettings { get; }
	}
}