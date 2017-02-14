using System;
using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace
{
	/// <summary>
	/// Интерфейс чтения и настроек АИН
	/// </summary>
	internal interface IEngineSettingsReader
	{
		/// <summary>
		/// Асинхронное чтение настроек
		/// </summary>
		/// <param name="callback">Метод обратного вызова, в который передаётся внутреннее исключение (если возникло) и результирующие настройки</param>
		/// <param name="forceRead">Флаг, указывающий не использовать настройки считанные ранее</param>
		void ReadSettingsAsync(bool forceRead, Action<Exception, IEngineSettings> callback);
	}

	/// <summary>
	/// Сообщает о том, что настройки были прочитаны
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