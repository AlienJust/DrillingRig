using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Xml.Linq;
using AlienJust.Support.Collections;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.AinSettingsRead;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;

namespace DrillingRig.ConfigApp.NewLook.Settings {
	class ImportExportViewModel : ViewModelBase {
		private readonly IAinSettingsStorageSettable _ainSettingsStorageSettable;
		private readonly IAinSettingsReadNotifyRaisable _ainSettingsReadNotifyRaisable;
		private readonly RelayCommand _loadCmd;
		private readonly RelayCommand _saveCmd;

		public ImportExportViewModel(IAinSettingsStorageSettable ainSettingsStorageSettable, IAinSettingsReadNotifyRaisable ainSettingsReadNotifyRaisable) {
			_ainSettingsStorageSettable = ainSettingsStorageSettable;
			_ainSettingsReadNotifyRaisable = ainSettingsReadNotifyRaisable;
			_loadCmd = new RelayCommand(Load, () => true);
			_saveCmd = new RelayCommand(Save, () => true);
		}

		private void Load()
		{
			var winSystem = new AlienJust.Adaptation.WindowsPresentation.WpfWindowSystem();
			var filename = winSystem.ShowOpenFileDialog("Импорт настроек", "XML files|*.xml|All files|*.*");
			if (filename != null)
			{
				var doc = XDocument.Load(filename);
				var rootElement = doc.Element("Settings");
				var ainElements = rootElement.Elements("AinSettings");
				foreach (var ainElement in ainElements)
				{
					var zbAinNumber = byte.Parse(ainElement.Attribute("Number").Value);
					_ainSettingsReadNotifyRaisable.RaiseAinSettingsReadStarted(zbAinNumber);

					var curAinSettings = new AinSettingsPartWritable();

					var accDfDt = short.Parse(ainElement.Element("AccDfDt").Value, CultureInfo.InvariantCulture);
					var ain1LinkFault = bool.Parse(ainElement.Element("Ain1LinkFault").Value);
					var ain2LinkFault = bool.Parse(ainElement.Element("Ain2LinkFault").Value);
					var ain3LinkFault = bool.Parse(ainElement.Element("Ain3LinkFault").Value);
					var dacCh = short.Parse(ainElement.Element("DacCh").Value, CultureInfo.InvariantCulture);
					var decDfDt = short.Parse(ainElement.Element("DecDfDt").Value, CultureInfo.InvariantCulture);
					var dflLim = short.Parse(ainElement.Element("DflLim").Value, CultureInfo.InvariantCulture);
					var emdecDfdt = short.Parse(ainElement.Element("EmdecDfdt").Value, CultureInfo.InvariantCulture);
					var fanMode = AinTelemetryFanWorkmodeExtensions.FromIoBits(int.Parse(ainElement.Element("FanMode").Value, CultureInfo.InvariantCulture));
					var fiMin = short.Parse(ainElement.Element("FiMin").Value, CultureInfo.InvariantCulture);
					var fiNom = short.Parse(ainElement.Element("FiNom").Value, CultureInfo.InvariantCulture);
					var flMinMin = short.Parse(ainElement.Element("FlMinMin").Value, CultureInfo.InvariantCulture);
					var fmax = short.Parse(ainElement.Element("Fmax").Value, CultureInfo.InvariantCulture);
					var fmin = short.Parse(ainElement.Element("Fmin").Value, CultureInfo.InvariantCulture);
					var fnom = short.Parse(ainElement.Element("Fnom").Value, CultureInfo.InvariantCulture);
					var ia0 = short.Parse(ainElement.Element("Ia0").Value, CultureInfo.InvariantCulture);
					var ib0 = short.Parse(ainElement.Element("Ib0").Value, CultureInfo.InvariantCulture);
					var ic0 = short.Parse(ainElement.Element("Ic0").Value, CultureInfo.InvariantCulture);
					var idSetMax = short.Parse(ainElement.Element("IdSetMax").Value, CultureInfo.InvariantCulture);
					var idSetMin = short.Parse(ainElement.Element("IdSetMin").Value, CultureInfo.InvariantCulture);
					var imax = short.Parse(ainElement.Element("Imax").Value, CultureInfo.InvariantCulture);
					var imcw = short.Parse(ainElement.Element("Imcw").Value, CultureInfo.InvariantCulture);
					var ioutMax = short.Parse(ainElement.Element("IoutMax").Value, CultureInfo.InvariantCulture);
					var kiFi = int.Parse(ainElement.Element("KiFi").Value, CultureInfo.InvariantCulture);
					var kiId = int.Parse(ainElement.Element("KiId").Value, CultureInfo.InvariantCulture);
					var kiIq = int.Parse(ainElement.Element("KiIq").Value, CultureInfo.InvariantCulture);
					var kiW = int.Parse(ainElement.Element("KiW").Value, CultureInfo.InvariantCulture);
					var kpFi = double.Parse(ainElement.Element("KpFi").Value, CultureInfo.InvariantCulture);
					var kpId = double.Parse(ainElement.Element("KpId").Value, CultureInfo.InvariantCulture);
					var kpIq = double.Parse(ainElement.Element("KpIq").Value, CultureInfo.InvariantCulture);
					var kpW = double.Parse(ainElement.Element("KpW").Value, CultureInfo.InvariantCulture);
					var lm = short.Parse(ainElement.Element("Lm").Value, CultureInfo.InvariantCulture);
					var lrl = short.Parse(ainElement.Element("Lrl").Value, CultureInfo.InvariantCulture);
					var lsl = short.Parse(ainElement.Element("Lsl").Value, CultureInfo.InvariantCulture);
					var nimpFloorCode = int.Parse(ainElement.Element("NimpFloorCode").Value, CultureInfo.InvariantCulture);
					var np = int.Parse(ainElement.Element("Np").Value, CultureInfo.InvariantCulture);
					var reserved00 = BytesPair.Parse(ainElement.Element("Reserved00").Value);
					var reserved24 = BytesPair.Parse(ainElement.Element("Reserved24").Value);
					var reserved28 = BytesPair.Parse(ainElement.Element("Reserved28").Value);
					var reserved32 = BytesPair.Parse(ainElement.Element("Reserved32").Value);
					var reserved50 = BytesPair.Parse(ainElement.Element("Reserved50").Value);
					var reserved51 = BytesPair.Parse(ainElement.Element("Reserved51").Value);
					var rs = short.Parse(ainElement.Element("Rs").Value, CultureInfo.InvariantCulture);
					var tauF = short.Parse(ainElement.Element("TauF").Value, CultureInfo.InvariantCulture);
					var tauFSet = short.Parse(ainElement.Element("TauFSet").Value, CultureInfo.InvariantCulture);
					var tauFi = short.Parse(ainElement.Element("TauFi").Value, CultureInfo.InvariantCulture);
					var tauFlLim = short.Parse(ainElement.Element("TauFlLim").Value, CultureInfo.InvariantCulture);
					var tauM = short.Parse(ainElement.Element("TauM").Value, CultureInfo.InvariantCulture);
					var tauR = short.Parse(ainElement.Element("TauR").Value, CultureInfo.InvariantCulture);
					var textMax = short.Parse(ainElement.Element("TextMax").Value, CultureInfo.InvariantCulture);
					var toHl = short.Parse(ainElement.Element("ToHl").Value, CultureInfo.InvariantCulture);
					var uchMax = BytesPair.Parse(ainElement.Element("UchMax").Value);
					var uchMin = BytesPair.Parse(ainElement.Element("UchMin").Value);
					var udc0 = short.Parse(ainElement.Element("Udc0").Value, CultureInfo.InvariantCulture);
					var udcMax = short.Parse(ainElement.Element("UdcMax").Value, CultureInfo.InvariantCulture);
					var udcMin = short.Parse(ainElement.Element("UdcMin").Value, CultureInfo.InvariantCulture);
					var umodThr = short.Parse(ainElement.Element("UmodThr").Value, CultureInfo.InvariantCulture);
					var unom = short.Parse(ainElement.Element("Unom").Value, CultureInfo.InvariantCulture);

					var setting = new AinSettingsSimple(reserved00, kpW, kiW, fiNom, imax, udcMax, udcMin, fnom, fmax, dflLim,
						flMinMin, ioutMax, fiMin, dacCh, imcw, ia0, ib0, ic0, udc0, tauR, lm, lsl, lrl, reserved24, kpFi, kiFi, reserved28,
						kpId, kiId, reserved32, kpIq, kiIq, accDfDt, decDfDt, unom, tauFlLim, rs, fmin, tauM, tauF, tauFSet, tauFi,
						idSetMin, idSetMax, uchMin, uchMax, reserved50, reserved51, np, nimpFloorCode, fanMode, umodThr, emdecDfdt,
						textMax, toHl, ain1LinkFault, ain2LinkFault, ain3LinkFault);
					_ainSettingsStorageSettable.SetSettings(zbAinNumber, setting);
					_ainSettingsReadNotifyRaisable.RaiseAinSettingsReadComplete(zbAinNumber, null, setting); // TODO: try catch real exception
				}
			}
		}

		private void Save() {
			// TODO: show save file dialog
			// TODO: save 
			var winSystem = new AlienJust.Adaptation.WindowsPresentation.WpfWindowSystem();
			var filename = winSystem.ShowSaveFileDialog("Экспорт настроек", "XML files|*.xml|All files|*.*");
			if (filename != null) {
				var doc = new XDocument();
				var rootElement = new XElement("Settings");

				for (byte i = 0; i < 3; ++i) {
					var curAinSettings = _ainSettingsStorageSettable.GetSettings(i);
					if (curAinSettings != null) {
						var element = new XElement("AinSettings", new XAttribute("Number", i),
							new XElement("AccDfDt", curAinSettings.AccDfDt.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ain1LinkFault", curAinSettings.Ain1LinkFault.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ain2LinkFault", curAinSettings.Ain2LinkFault.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ain3LinkFault", curAinSettings.Ain3LinkFault.ToString(CultureInfo.InvariantCulture)),
							new XElement("DacCh", curAinSettings.DacCh.ToString(CultureInfo.InvariantCulture)),
							new XElement("DecDfDt", curAinSettings.DecDfDt.ToString(CultureInfo.InvariantCulture)),
							new XElement("DflLim", curAinSettings.DflLim.ToString(CultureInfo.InvariantCulture)),
							new XElement("EmdecDfdt", curAinSettings.EmdecDfdt.ToString(CultureInfo.InvariantCulture)),
							new XElement("FanMode", curAinSettings.FanMode.ToIoBits().ToString(CultureInfo.InvariantCulture)),
							new XElement("FiMin", curAinSettings.FiMin.ToString(CultureInfo.InvariantCulture)),
							new XElement("FiNom", curAinSettings.FiNom.ToString(CultureInfo.InvariantCulture)),
							new XElement("FlMinMin", curAinSettings.FlMinMin.ToString(CultureInfo.InvariantCulture)),
							new XElement("Fmax", curAinSettings.Fmax.ToString(CultureInfo.InvariantCulture)),
							new XElement("Fmin", curAinSettings.Fmin.ToString(CultureInfo.InvariantCulture)),
							new XElement("Fnom", curAinSettings.Fnom.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ia0", curAinSettings.Ia0.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ib0", curAinSettings.Ib0.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ic0", curAinSettings.Ic0.ToString(CultureInfo.InvariantCulture)),
							new XElement("IdSetMax", curAinSettings.IdSetMax.ToString(CultureInfo.InvariantCulture)),
							new XElement("IdSetMin", curAinSettings.IdSetMin.ToString(CultureInfo.InvariantCulture)),
							new XElement("Imax", curAinSettings.Imax.ToString(CultureInfo.InvariantCulture)),
							new XElement("Imcw", curAinSettings.Imcw.ToString(CultureInfo.InvariantCulture)),
							new XElement("IoutMax", curAinSettings.IoutMax.ToString(CultureInfo.InvariantCulture)),
							new XElement("KiFi", curAinSettings.KiFi.ToString(CultureInfo.InvariantCulture)),
							new XElement("KiId", curAinSettings.KiId.ToString(CultureInfo.InvariantCulture)),
							new XElement("KiIq", curAinSettings.KiIq.ToString(CultureInfo.InvariantCulture)),
							new XElement("KiW", curAinSettings.KiW.ToString(CultureInfo.InvariantCulture)),
							new XElement("KpFi", curAinSettings.KpFi.ToString(CultureInfo.InvariantCulture)),
							new XElement("KpId", curAinSettings.KpId.ToString(CultureInfo.InvariantCulture)),
							new XElement("KpIq", curAinSettings.KpIq.ToString(CultureInfo.InvariantCulture)),
							new XElement("KpW", curAinSettings.KpW.ToString(CultureInfo.InvariantCulture)),
							new XElement("Lm", curAinSettings.Lm.ToString(CultureInfo.InvariantCulture)),
							new XElement("Lrl", curAinSettings.Lrl.ToString(CultureInfo.InvariantCulture)),
							new XElement("Lsl", curAinSettings.Lsl.ToString(CultureInfo.InvariantCulture)),
							new XElement("NimpFloorCode", curAinSettings.NimpFloorCode.ToString(CultureInfo.InvariantCulture)),
							new XElement("Np", curAinSettings.Np.ToString(CultureInfo.InvariantCulture)),
							new XElement("Reserved00", curAinSettings.Reserved00),
							new XElement("Reserved24", curAinSettings.Reserved24),
							new XElement("Reserved28", curAinSettings.Reserved28),
							new XElement("Reserved32", curAinSettings.Reserved32),
							new XElement("Reserved50", curAinSettings.Reserved50),
							new XElement("Reserved51", curAinSettings.Reserved51),
							new XElement("Rs", curAinSettings.Rs.ToString(CultureInfo.InvariantCulture)),
							new XElement("TauF", curAinSettings.TauF.ToString(CultureInfo.InvariantCulture)),
							new XElement("TauFSet", curAinSettings.TauFSet.ToString(CultureInfo.InvariantCulture)),
							new XElement("TauFi", curAinSettings.TauFi.ToString(CultureInfo.InvariantCulture)),
							new XElement("TauFlLim", curAinSettings.TauFlLim.ToString(CultureInfo.InvariantCulture)),
							new XElement("TauM", curAinSettings.TauM.ToString(CultureInfo.InvariantCulture)),
							new XElement("TauR", curAinSettings.TauR.ToString(CultureInfo.InvariantCulture)),
							new XElement("TextMax", curAinSettings.TextMax.ToString(CultureInfo.InvariantCulture)),
							new XElement("ToHl", curAinSettings.ToHl.ToString(CultureInfo.InvariantCulture)),
							new XElement("UchMax", curAinSettings.UchMax),
							new XElement("UchMin", curAinSettings.UchMin),
							new XElement("Udc0", curAinSettings.Udc0.ToString(CultureInfo.InvariantCulture)),
							new XElement("UdcMax", curAinSettings.UdcMax.ToString(CultureInfo.InvariantCulture)),
							new XElement("UdcMin", curAinSettings.UdcMin.ToString(CultureInfo.InvariantCulture)),
							new XElement("UmodThr", curAinSettings.UmodThr.ToString(CultureInfo.InvariantCulture)),
							new XElement("Unom", curAinSettings.Unom.ToString(CultureInfo.InvariantCulture))
						);
						rootElement.Add(element);
					}
				}
				doc.Add(rootElement);
				doc.Save(File.Open(filename, FileMode.Create, FileAccess.ReadWrite));
			}
		}

		public ICommand LoadCmd => _loadCmd;
		public ICommand SaveCmd => _saveCmd;
	}
}
