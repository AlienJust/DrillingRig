using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;

namespace DrillingRig.ConfigApp.NewLook.Settings {
	class ImportExportViewModel : ViewModelBase {
		private readonly IAinSettingsStorageSettable _ainSettingsStorageSettable;
		private readonly IAinSettingsStorageUpdatedNotify _ainSettingsStorageUpdatedNotify;
		private readonly RelayCommand _loadCmd;
		private readonly RelayCommand _saveCmd;

		public ImportExportViewModel(IAinSettingsStorageSettable ainSettingsStorageSettable, IAinSettingsStorageUpdatedNotify ainSettingsStorageUpdatedNotify) {
			_ainSettingsStorageSettable = ainSettingsStorageSettable;
			_ainSettingsStorageUpdatedNotify = ainSettingsStorageUpdatedNotify;

			_loadCmd = new RelayCommand(Load, () => true);
			_saveCmd = new RelayCommand(Save, () => true);
		}

		private void Load() {

		}

		private void Save() {
			// TODO: show save file dialog
			// TODO: save 
			var winSystem = new AlienJust.Adaptation.WindowsPresentation.WpfWindowSystem();
			var filename = winSystem.ShowSaveFileDialog("Сохранение настроек", "All files|*.*|XML files|*.xml");
			if (filename != null) {
				var doc = new XDocument(new XElement("Settings"));
				for (byte i = 0; i < 3; ++i) {
					var curAinSettings = _ainSettingsStorageSettable.GetSettings(i);
					if (curAinSettings != null)
					{
						var element = new XElement("AinSettings", new XAttribute("Number", i),
							new XElement("AccDfDt", curAinSettings.AccDfDt),
							new XElement("Ain1LinkFault", curAinSettings.Ain1LinkFault),
							new XElement("Ain2LinkFault", curAinSettings.Ain2LinkFault),
							new XElement("Ain3LinkFault", curAinSettings.Ain3LinkFault),
							new XElement("DacCh", curAinSettings.DacCh),
							new XElement("DecDfDt", curAinSettings.DecDfDt),
							new XElement("DflLim", curAinSettings.DflLim),
							new XElement("EmdecDfdt", curAinSettings.EmdecDfdt),
							new XElement("FanMode", curAinSettings.FanMode),
							new XElement("FiMin", curAinSettings.FiMin),
							new XElement("FiNom", curAinSettings.FiNom),
							new XElement("FlMinMin", curAinSettings.FlMinMin),
							new XElement("Fmax", curAinSettings.Fmax),
							new XElement("Fmin", curAinSettings.Fmin),
							new XElement("Fnom", curAinSettings.Fnom),
							new XElement("Ia0", curAinSettings.Ia0),
							new XElement("Ib0", curAinSettings.Ib0),
							new XElement("Ic0", curAinSettings.Ic0),
							new XElement("IdSetMax", curAinSettings.IdSetMax),
							new XElement("IdSetMin", curAinSettings.IdSetMin),
							new XElement("Imax", curAinSettings.Imax),
							new XElement("Imcw", curAinSettings.Imcw),
							new XElement("IoutMax", curAinSettings.IoutMax),
							new XElement("KiFi", curAinSettings.KiFi),
							new XElement("KiId", curAinSettings.KiId),
							new XElement("KiIq", curAinSettings.KiIq),
							new XElement("KiW", curAinSettings.KiW),
							new XElement("KpFi", curAinSettings.KpFi),
							new XElement("KpId", curAinSettings.KpId),
							new XElement("KpIq", curAinSettings.KpIq),
							new XElement("KpW", curAinSettings.KpW),
							new XElement("Lm", curAinSettings.Lm),
							new XElement("Lrl", curAinSettings.Lrl),
							new XElement("Lsl", curAinSettings.Lsl),
							new XElement("NimpFloorCode", curAinSettings.NimpFloorCode),
							new XElement("Np", curAinSettings.Np),
							new XElement("Reserved00", curAinSettings.Reserved00),
							new XElement("Reserved24", curAinSettings.Reserved24),
							new XElement("Reserved28", curAinSettings.Reserved28),
							new XElement("Reserved32", curAinSettings.Reserved32),
							new XElement("Reserved50", curAinSettings.Reserved50),
							new XElement("Reserved51", curAinSettings.Reserved51),
							new XElement("TauF", curAinSettings.TauF),
							new XElement("TauFSet", curAinSettings.TauFSet),
							new XElement("TauFi", curAinSettings.TauFi),
							new XElement("TauFlLim", curAinSettings.TauFlLim),
							new XElement("TauM", curAinSettings.TauM),
							new XElement("TauR", curAinSettings.TauR),
							new XElement("TextMax", curAinSettings.TextMax),
							new XElement("ToHl", curAinSettings.ToHl),
							new XElement("UchMax", curAinSettings.UchMax),
							new XElement("UchMin", curAinSettings.UchMin),
							new XElement("Udc0", curAinSettings.Udc0),
							new XElement("UdcMax", curAinSettings.UdcMax),
							new XElement("UdcMin", curAinSettings.UdcMin),
							new XElement("UmodThr", curAinSettings.UmodThr),
							new XElement("Unom", curAinSettings.Unom)
						);
					}
				}
				doc.Save(File.OpenWrite(filename));
			}
		}

		public ICommand LoadCmd => _loadCmd;
		public ICommand SaveCmd => _saveCmd;
	}
}
