using System.Collections.Generic;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsStorage {
	class AinSettingsStorageThreadSafe : IAinSettingsStorageSettable, IAinSettingsStorageUpdatedNotify {
		private readonly object _ainSettingsSync;
		private readonly List<IAinSettings> _ainsSettings;
		public AinSettingsStorageThreadSafe() {
			_ainSettingsSync = new object();
			_ainsSettings = new List<IAinSettings> {null, null, null};
		}

		public IAinSettings GetSettings(byte zeroBasedAinNumber) {
			lock (_ainSettingsSync) {
				return _ainsSettings[zeroBasedAinNumber];
			}
		}
		public void SetSettings(byte zeroBasedAinNumber, IAinSettings settings) {
			lock (_ainSettingsSync) {
				_ainsSettings[zeroBasedAinNumber] = settings;
			}
			RaiseAinSettingsUpdated(zeroBasedAinNumber, settings);
		}

		private void RaiseAinSettingsUpdated(byte zeroBasedAinNumber, IAinSettings settings) {
			var eve = AinSettingsUpdated;
			eve?.Invoke(zeroBasedAinNumber, settings);
		}

		public event StoredAinSettingsUpdatedDelegate AinSettingsUpdated;
	}
}
