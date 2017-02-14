using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace
{
	/// <summary>
	/// ��������� �������� ���������
	/// </summary>
	interface IEngineSettingsStorage
	{
		/// <summary>
		/// ���������� ����� ��������� ���������
		/// </summary>
		IEngineSettings EngineSettings { get; }
	}
}