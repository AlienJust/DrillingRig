using DrillingRig.Commands.EngineSettings;

namespace DrillingRig.ConfigApp.AppControl.EngineSettingsStorage {
	class EngineSettingsStorageThreadSafe : IEngineSettingsStorageSettable, IEngineSettingsStorageUpdatedNotify
	{
		private readonly object _engineSettingsSync;
		private IEngineSettings _engineSettings;
		public EngineSettingsStorageThreadSafe() {
			_engineSettingsSync = new object();
			_engineSettings = null;
		}

		public IEngineSettings EngineSettings {
			get
			{
				lock (_engineSettingsSync)
				{
					return _engineSettings;
				}
			}
		}
		public void SetSettings(IEngineSettings settings) {
			lock (_engineSettingsSync) {
				_engineSettings = settings;
			}
			RaiseAinSettingsUpdated(settings);
		}

		private void RaiseAinSettingsUpdated(IEngineSettings settings) {
			var eve = EngineSettingsUpdated;
			eve?.Invoke(settings);
		}

		public event StoredEngineSettingsUpdatedDelegate EngineSettingsUpdated;
	}
}
