using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsStorage {
	/// <summary>
	/// ��������� �������� ������������ ������ ����� ��� ������ �������� ���. 
	/// ���� ��� �������� � ���������, ������ �������� ��������������� �� �����.
	/// </summary>
	interface IAinSettingsStorage {
		IAinSettings GetSettings(byte zeroBasedAinNumber);
	}
}