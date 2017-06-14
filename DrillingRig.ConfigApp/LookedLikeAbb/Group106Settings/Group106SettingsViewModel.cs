using System;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinsCounter;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw;
using DrillingRig.ConfigApp.LookedLikeAbb.Group106Settings.ImvcParameter;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterDoubleEditCheck;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterHexEditable;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Group106Settings {
	class Group106SettingsViewModel : ViewModelBase {
		private readonly IUserInterfaceRoot _uiRoot;
		private readonly ILogger _logger;
		private readonly IAinSettingsReaderWriter _readerWriter;
		private readonly IAinSettingsReadNotify _ainSettingsReadNotify;

		public ParameterHexEditableViewModel Parameter01Vm { get; }
		public ImcwParameterViewModel Parameter02Vm { get; }
		public ParameterDoubleEditCheckViewModel Parameter03Vm { get; }

		public RelayCommand ReadSettingsCmd { get; }
		public RelayCommand WriteSettingsCmd { get; }

		public Group106SettingsViewModel(IUserInterfaceRoot uiRoot, ILogger logger, 
			IAinSettingsReaderWriter readerWriter, IAinSettingsReadNotify ainSettingsReadNotify, IAinSettingsStorage storage, IAinSettingsStorageUpdatedNotify storageUpdatedNotify, IAinsCounter ainsCounter,
			ImcwParameterViewModel imcwParameterVm) {
			_uiRoot = uiRoot;
			_logger = logger;
			_readerWriter = readerWriter;
			_ainSettingsReadNotify = ainSettingsReadNotify;

			Parameter01Vm = new ParameterHexEditableViewModel("106.01. Каналы ЦАП", "X4", 0, 65535, null);
			Parameter02Vm = imcwParameterVm;
			Parameter03Vm = new ParameterDoubleEditCheckViewModel("106.03. Таймаут по системной линии связи", "f0", -10000, 10000, null);

			ReadSettingsCmd = new RelayCommand(ReadSettings, () => true); // TODO: read only when connected to COM
			WriteSettingsCmd = new RelayCommand(WriteSettings, () => true); // TODO: read only when connected to COM

			_ainSettingsReadNotify.AinSettingsReadComplete += AinSettingsReadNotifyOnAinSettingsReadComplete;
		}

		private void AinSettingsReadNotifyOnAinSettingsReadComplete(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings) {
			if (zeroBasedAinNumber == 0) {
				UpdateSettingsInUiThread(readInnerException, settings);
			}
		}

		private void WriteSettings() {
			try {
				var settingsPart = new AinSettingsPartWritable {
					DacCh = (ushort)Parameter01Vm.CurrentValue.Value,
					Imcw = (ushort)Parameter02Vm.FullValue.Value,
					ToHl = ConvertDoubleToShort(Parameter03Vm.CurrentValue)
				};
				_readerWriter.WriteSettingsAsync(settingsPart, exception => {
					_uiRoot.Notifier.Notify(() => {
						if (exception != null) {
							_logger.Log("Ошибка при записи настроек. " + exception.Message);
						}
						else _logger.Log("Группа настроек была успешно записана");
					});
				}
					);
			}
			catch (Exception ex) {
				_logger.Log("Не удалось записать группу настроек. " + ex.Message);
			}
		}

		private void ReadSettings() {
			try {
				_readerWriter.ReadSettingsAsync(0, true, (exception, settings) => { });
			}
			catch (Exception ex) {
				_logger.Log("Не удалось прочитать группу настроек. " + ex.Message);
			}
		}

		private short? ConvertDoubleToShort(double? value) {
			if (!value.HasValue) return null;
			return (short)value.Value;
		}

		private void UpdateSettingsInUiThread(Exception exception, IAinSettings settings) {
			_uiRoot.Notifier.Notify(() => {
				if (exception != null) {
					Parameter01Vm.CurrentValue = null;
					Parameter02Vm.FullValue = null;
					Parameter03Vm.CurrentValue = null;
					return;
				}

				Parameter01Vm.CurrentValue = settings.DacCh;
				Parameter02Vm.FullValue = (ushort)settings.Imcw;
				Parameter03Vm.CurrentValue = settings.ToHl;
			});
		}
	}
}
