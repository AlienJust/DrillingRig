using AlienJust.Support.Loggers.Contracts;
using DrillingRig.ConfigApp.LookedLikeAbb;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;

namespace DrillingRig.ConfigApp {
	class OnConnectSettingsReader {
		private readonly IAinsCounter _ainsCounter;
		private readonly IAinSettingsReader _ainSettingsReader;
		private ILogger _logger;

		public OnConnectSettingsReader(INotifySendingEnabled sendingEnabledNotifier, IAinsCounter ainsCounter, IAinSettingsReader ainSettingsReader, ILogger logger) {
			_ainsCounter = ainsCounter;
			_ainSettingsReader = ainSettingsReader;
			_logger = logger;
			sendingEnabledNotifier.SendingEnabledChanged += SendingEnabledNotifierOnSendingEnabledChanged; // TODO: unsubscribe on app quit
		}

		

		private void SendingEnabledNotifierOnSendingEnabledChanged(bool isSendingEnabled) {
			if (isSendingEnabled) {
				for (byte i = 0; i < _ainsCounter.SelectedAinsCount; i++) {
					_logger.Log("Автоматическое чтение настроек АИН №" + (i + 1) + " при подключении к COM-порту");
					_ainSettingsReader.ReadSettingsAsync(i, (ex, settings) => { }); // i dont need to know whether settings were readed or exception occured, just need to initiate read process
				}
			}
		}
	}
}
