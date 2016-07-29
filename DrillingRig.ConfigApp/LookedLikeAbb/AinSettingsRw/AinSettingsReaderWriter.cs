using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw {
	class AinSettingsReaderWriter : IAinSettingsReaderWriter {
		private readonly IAinSettingsReader _reader;
		private readonly IAinSettingsWriter _writer;

		public AinSettingsReaderWriter(IAinSettingsReader reader, IAinSettingsWriter writer) {
			_reader = reader;
			_writer = writer;
		}

		public void ReadSettingsAsync(byte zeroBasedAinNumber, Action<Exception, IAinSettings> callback) {
			_reader.ReadSettingsAsync(zeroBasedAinNumber, callback);
		}

		public void WriteSettingsAsync(IAinSettingsPart settings, Action<Exception> callback) {
			_writer.WriteSettingsAsync(settings, callback);
		}
	}
}