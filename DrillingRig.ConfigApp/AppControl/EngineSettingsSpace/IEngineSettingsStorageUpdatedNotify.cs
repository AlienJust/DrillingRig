using System;
using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace
{
	/// <summary>
	/// Сообщает о том, что настройки были обновлены
	/// </summary>
	interface IEngineSettingsStorageUpdatedNotify
	{
		/// <summary>
		/// Возникает при обновлении настроек в хранилище
		/// </summary>
		event StoredEngineSettingsUpdatedDelegate EngineSettingsUpdated;
	}

	internal interface IEngineSettingsReadNotifyRaisable : IEngineSettingsReadNotify
	{
		void RaiseEngineSettingsReadStarted();
		void RaiseEngineSettingsReadComplete(Exception innerException, IEngineSettings settings);
	}
}