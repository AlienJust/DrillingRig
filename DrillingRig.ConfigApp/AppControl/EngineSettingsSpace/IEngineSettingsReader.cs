using System;
using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace
{
	/// <summary>
	/// ��������� ������ � �������� ���
	/// </summary>
	internal interface IEngineSettingsReader
	{
		/// <summary>
		/// ����������� ������ ��������
		/// </summary>
		/// <param name="callback">����� ��������� ������, � ������� ��������� ���������� ���������� (���� ��������) � �������������� ���������</param>
		/// <param name="forceRead">����, ����������� �� ������������ ��������� ��������� �����</param>
		void ReadSettingsAsync(bool forceRead, Action<Exception, IEngineSettings> callback);
	}

	/// <summary>
	/// �������� � ���, ��� ��������� ���� ���������
	/// </summary>
	interface IEngineSettingsReadNotify
	{
		event EngineSettingsReadStartedDelegate EngineSettingsReadStarted;
		event EngineSettingsReadCompleteDelegate EngineSettingsReadComplete;
	}

	internal interface IAinSettingsReadNotifyRaisable : IEngineSettingsReadNotify
	{
		void RaiseEngineSettingsReadStarted();
		void RaiseEngineSettingsReadComplete(Exception innerException, IEngineSettings settings);
	}

	delegate void EngineSettingsReadStartedDelegate();
	delegate void EngineSettingsReadCompleteDelegate(Exception readInnerException, IEngineSettings settings);
}