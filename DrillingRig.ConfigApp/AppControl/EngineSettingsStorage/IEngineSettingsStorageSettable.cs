using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsStorage
{
	/// <summary>
	/// ��������� �������� ��������� � ������������ ������ �������� � ���������
	/// </summary>
	interface IEngineSettingsStorageSettable : IEngineSettingsStorage
	{
		void SetSettings(IEngineSettings settings);
	}
}