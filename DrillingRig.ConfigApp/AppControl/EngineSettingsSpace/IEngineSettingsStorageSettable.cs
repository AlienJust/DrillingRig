using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace
{
	/// <summary>
	/// ��������� �������� ��������� � ������������ ������ �������� � ���������
	/// </summary>
	interface IEngineSettingsStorageSettable : IEngineSettingsStorage
	{
		void SetSettings(IEngineSettings settings);
	}
}