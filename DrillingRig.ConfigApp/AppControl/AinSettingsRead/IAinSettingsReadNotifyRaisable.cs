using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsRead
{
	internal interface IAinSettingsReadNotifyRaisable : IAinSettingsReadNotify
	{
		void RaiseAinSettingsReadStarted(byte zbAinNumber);
		void RaiseAinSettingsReadComplete(byte zbAinNumber, Exception innerException, IAinSettings settings);
	}
}