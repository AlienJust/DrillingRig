using System;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using AlienJust.Support.Collections;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.Commands.EngineSettings;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.AinSettingsWrite;
using DrillingRig.ConfigApp.AppControl.EngineSettingsSpace;
using IAinSettingsReadNotifyRaisable = DrillingRig.ConfigApp.AppControl.AinSettingsRead.IAinSettingsReadNotifyRaisable;

namespace DrillingRig.ConfigApp.NewLook.Settings {
	class ImportExportViewModel : ViewModelBase {
		private readonly IAinSettingsStorageSettable _ainSettingsStorageSettable;
		private readonly IAinSettingsReadNotifyRaisable _ainSettingsReadNotifyRaisable;
		private readonly IEngineSettingsStorageSettable _engineSettingsStorageSettable;
		private readonly IEngineSettingsReadNotifyRaisable _engineSettingsReadNotifyRaisable;
		private readonly RelayCommand _loadCmd;
		private readonly RelayCommand _saveCmd;

		public ImportExportViewModel(IAinSettingsStorageSettable ainSettingsStorageSettable, IAinSettingsReadNotifyRaisable ainSettingsReadNotifyRaisable, IEngineSettingsStorageSettable engineSettingsStorageSettable, IEngineSettingsReadNotifyRaisable engineSettingsReadNotifyRaisable) {
			_ainSettingsStorageSettable = ainSettingsStorageSettable;
			_ainSettingsReadNotifyRaisable = ainSettingsReadNotifyRaisable;
			_engineSettingsStorageSettable = engineSettingsStorageSettable;
			_engineSettingsReadNotifyRaisable = engineSettingsReadNotifyRaisable;

			_loadCmd = new RelayCommand(Load, () => true);
			_saveCmd = new RelayCommand(Save, () => true);
		}

		private void Load() {
			var winSystem = new AlienJust.Adaptation.WindowsPresentation.WpfWindowSystem();
			var filename = winSystem.ShowOpenFileDialog("Импорт настроек", "XML files|*.xml|All files|*.*");
			if (filename != null) {
				var doc = XDocument.Load(filename);
				var rootElement = doc.Element("Settings");
				var ainElements = rootElement.Elements("AinSettings");
				foreach (var ainElement in ainElements) {
					var zbAinNumber = byte.Parse(ainElement.Attribute("Number").Value);
					_ainSettingsReadNotifyRaisable.RaiseAinSettingsReadStarted(zbAinNumber);

					var curAinSettings = new AinSettingsPartWritable();

					var reserved00 = BytesPair.Parse(ainElement.Element("Reserved00").Value);
					var kpW = decimal.Parse(ainElement.Element("KpW").Value, CultureInfo.InvariantCulture);
					var kiW = double.Parse(ainElement.Element("KiW").Value, CultureInfo.InvariantCulture);

					var fiNom = double.Parse(ainElement.Element("FiNom").Value, CultureInfo.InvariantCulture);
					var imax = short.Parse(ainElement.Element("Imax").Value, CultureInfo.InvariantCulture);
					var udcMax = short.Parse(ainElement.Element("UdcMax").Value, CultureInfo.InvariantCulture);
					var udcMin = short.Parse(ainElement.Element("UdcMin").Value, CultureInfo.InvariantCulture);
					var fnom = double.Parse(ainElement.Element("Fnom").Value, CultureInfo.InvariantCulture);
					var fmax = double.Parse(ainElement.Element("Fmax").Value, CultureInfo.InvariantCulture);

					var dflLim = double.Parse(ainElement.Element("DflLim").Value, CultureInfo.InvariantCulture);
					var flMinMin = double.Parse(ainElement.Element("FlMinMin").Value, CultureInfo.InvariantCulture);

					var ioutMax = short.Parse(ainElement.Element("IoutMax").Value, CultureInfo.InvariantCulture);
					var fiMin = double.Parse(ainElement.Element("FiMin").Value, CultureInfo.InvariantCulture);
					var dacCh = ushort.Parse(ainElement.Element("DacCh").Value, CultureInfo.InvariantCulture);
					var imcw = ushort.Parse(ainElement.Element("Imcw").Value, CultureInfo.InvariantCulture);
					var ia0 = short.Parse(ainElement.Element("Ia0").Value, CultureInfo.InvariantCulture);
					var ib0 = short.Parse(ainElement.Element("Ib0").Value, CultureInfo.InvariantCulture);
					var ic0 = short.Parse(ainElement.Element("Ic0").Value, CultureInfo.InvariantCulture);
					var udc0 = short.Parse(ainElement.Element("Udc0").Value, CultureInfo.InvariantCulture);

					var tauR = double.Parse(ainElement.Element("TauR").Value, CultureInfo.InvariantCulture);
					var lm = double.Parse(ainElement.Element("Lm").Value, CultureInfo.InvariantCulture);
					var lsl = double.Parse(ainElement.Element("Lsl").Value, CultureInfo.InvariantCulture);
					var lrl = double.Parse(ainElement.Element("Lrl").Value, CultureInfo.InvariantCulture);
					var reserved24 = BytesPair.Parse(ainElement.Element("Reserved24").Value);

					var kpFi = double.Parse(ainElement.Element("KpFi").Value, CultureInfo.InvariantCulture);
					var kiFi = double.Parse(ainElement.Element("KiFi").Value, CultureInfo.InvariantCulture);

					var reserved28 = BytesPair.Parse(ainElement.Element("Reserved28").Value);

					var kpId = double.Parse(ainElement.Element("KpId").Value, CultureInfo.InvariantCulture);
					var kiId = double.Parse(ainElement.Element("KiId").Value, CultureInfo.InvariantCulture);

					var reserved32 = BytesPair.Parse(ainElement.Element("Reserved32").Value);

					var kpIq = double.Parse(ainElement.Element("KpIq").Value, CultureInfo.InvariantCulture);
					var kiIq = double.Parse(ainElement.Element("KiIq").Value, CultureInfo.InvariantCulture);

					var accDfDt = double.Parse(ainElement.Element("AccDfDt").Value, CultureInfo.InvariantCulture);
					var decDfDt = double.Parse(ainElement.Element("DecDfDt").Value, CultureInfo.InvariantCulture);
					var unom = double.Parse(ainElement.Element("Unom").Value, CultureInfo.InvariantCulture);

					var tauFlLim = double.Parse(ainElement.Element("TauFlLim").Value, CultureInfo.InvariantCulture);

					var rs = double.Parse(ainElement.Element("Rs").Value, CultureInfo.InvariantCulture);
					var fmin = double.Parse(ainElement.Element("Fmin").Value, CultureInfo.InvariantCulture);
					var tauM = double.Parse(ainElement.Element("TauM").Value, CultureInfo.InvariantCulture);
					var tauF = double.Parse(ainElement.Element("TauF").Value, CultureInfo.InvariantCulture);
					var tauFSet = double.Parse(ainElement.Element("TauFSet").Value, CultureInfo.InvariantCulture);
					var tauFi = double.Parse(ainElement.Element("TauFi").Value, CultureInfo.InvariantCulture);
					var idSetMin = short.Parse(ainElement.Element("IdSetMin").Value, CultureInfo.InvariantCulture);
					var idSetMax = short.Parse(ainElement.Element("IdSetMax").Value, CultureInfo.InvariantCulture);
					var uchMin = BytesPair.Parse(ainElement.Element("UchMin").Value);
					var uchMax = BytesPair.Parse(ainElement.Element("UchMax").Value);

					var reserved50 = BytesPair.Parse(ainElement.Element("Reserved50").Value);
					var reserved51 = BytesPair.Parse(ainElement.Element("Reserved51").Value);

					var np = int.Parse(ainElement.Element("Np").Value, CultureInfo.InvariantCulture);
					var nimpFloorCode = int.Parse(ainElement.Element("NimpFloorCode").Value, CultureInfo.InvariantCulture);
					var fanMode = AinTelemetryFanWorkmodeExtensions.FromIoBits(int.Parse(ainElement.Element("FanMode").Value, CultureInfo.InvariantCulture));

					var umodThr = double.Parse(ainElement.Element("UmodThr").Value, CultureInfo.InvariantCulture);

					var emdecDfdt = double.Parse(ainElement.Element("EmdecDfdt").Value, CultureInfo.InvariantCulture);

					var textMax = short.Parse(ainElement.Element("TextMax").Value, CultureInfo.InvariantCulture);
					var toHl = short.Parse(ainElement.Element("ToHl").Value, CultureInfo.InvariantCulture);
					var ain1LinkFault = bool.Parse(ainElement.Element("Ain1LinkFault").Value);
					var ain2LinkFault = bool.Parse(ainElement.Element("Ain2LinkFault").Value);
					var ain3LinkFault = bool.Parse(ainElement.Element("Ain3LinkFault").Value);


					var setting = new AinSettingsSimple(reserved00, kpW, kiW, fiNom, imax, udcMax, udcMin, fnom, fmax, dflLim, flMinMin, ioutMax, fiMin, dacCh, imcw, ia0, ib0, ic0, udc0, tauR, lm, lsl, lrl, reserved24, kpFi, kiFi, reserved28, kpId, kiId, reserved32, kpIq, kiIq, accDfDt, decDfDt, unom, tauFlLim, rs, fmin, tauM, tauF, tauFSet, tauFi, idSetMin, idSetMax, uchMin, uchMax, reserved50, reserved51, np, nimpFloorCode, fanMode, umodThr, emdecDfdt, textMax, toHl, ain1LinkFault, ain2LinkFault, ain3LinkFault);
					_ainSettingsStorageSettable.SetSettings(zbAinNumber, setting);
					_ainSettingsReadNotifyRaisable.RaiseAinSettingsReadComplete(zbAinNumber, null, setting); // TODO: try catch real exception
				}
				var engineSettingsElement = rootElement.Element("EngineSettings");
				if (engineSettingsElement != null) {
					try {
						var settings = new EngineSettingsSimple
						{
							Inom = ushort.Parse(engineSettingsElement.Element("Inom").Value, CultureInfo.InvariantCulture),
							Nnom = ushort.Parse(engineSettingsElement.Element("Nnom").Value, CultureInfo.InvariantCulture),
							Nmax = ushort.Parse(engineSettingsElement.Element("Nmax").Value, CultureInfo.InvariantCulture),
							Pnom = double.Parse(engineSettingsElement.Element("Pnom").Value, CultureInfo.InvariantCulture),
							CosFi = double.Parse(engineSettingsElement.Element("CosFi").Value, CultureInfo.InvariantCulture),
							Eff = double.Parse(engineSettingsElement.Element("Eff").Value, CultureInfo.InvariantCulture),
							Mass = ushort.Parse(engineSettingsElement.Element("Mass").Value, CultureInfo.InvariantCulture),
							MmM = ushort.Parse(engineSettingsElement.Element("MmM").Value, CultureInfo.InvariantCulture),
							Height = ushort.Parse(engineSettingsElement.Element("Height").Value, CultureInfo.InvariantCulture),
							I2Tmax = uint.Parse(engineSettingsElement.Element("I2Tmax").Value, CultureInfo.InvariantCulture),
							Icontinious = ushort.Parse(engineSettingsElement.Element("Icontinious").Value, CultureInfo.InvariantCulture),
							ZeroF = ushort.Parse(engineSettingsElement.Element("ZeroF").Value, CultureInfo.InvariantCulture)
						};
						_engineSettingsStorageSettable.SetSettings(settings);
						_engineSettingsReadNotifyRaisable.RaiseEngineSettingsReadComplete(null,settings);
					}
					catch (Exception ex) {
						Console.WriteLine(ex);
					}
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
						var element = new XElement("AinSettings", new XAttribute("Number", i), new XElement("AccDfDt", curAinSettings.AccDfDt.ToString(CultureInfo.InvariantCulture)), new XElement("Ain1LinkFault", curAinSettings.Ain1LinkFault.ToString(CultureInfo.InvariantCulture)), new XElement("Ain2LinkFault", curAinSettings.Ain2LinkFault.ToString(CultureInfo.InvariantCulture)), new XElement("Ain3LinkFault", curAinSettings.Ain3LinkFault.ToString(CultureInfo.InvariantCulture)), new XElement("DacCh", curAinSettings.DacCh.ToString(CultureInfo.InvariantCulture)), new XElement("DecDfDt", curAinSettings.DecDfDt.ToString(CultureInfo.InvariantCulture)), new XElement("DflLim", curAinSettings.DflLim.ToString(CultureInfo.InvariantCulture)), new XElement("EmdecDfdt", curAinSettings.EmdecDfdt.ToString(CultureInfo.InvariantCulture)), new XElement("FanMode", curAinSettings.FanMode.ToIoBits().ToString(CultureInfo.InvariantCulture)), new XElement("FiMin", curAinSettings.FiMin.ToString(CultureInfo.InvariantCulture)), new XElement("FiNom", curAinSettings.FiNom.ToString(CultureInfo.InvariantCulture)), new XElement("FlMinMin", curAinSettings.FlMinMin.ToString(CultureInfo.InvariantCulture)), new XElement("Fmax", curAinSettings.Fmax.ToString(CultureInfo.InvariantCulture)), new XElement("Fmin", curAinSettings.Fmin.ToString(CultureInfo.InvariantCulture)), new XElement("Fnom", curAinSettings.Fnom.ToString(CultureInfo.InvariantCulture)), new XElement("Ia0", curAinSettings.Ia0.ToString(CultureInfo.InvariantCulture)), new XElement("Ib0", curAinSettings.Ib0.ToString(CultureInfo.InvariantCulture)), new XElement("Ic0", curAinSettings.Ic0.ToString(CultureInfo.InvariantCulture)), new XElement("IdSetMax", curAinSettings.IdSetMax.ToString(CultureInfo.InvariantCulture)), new XElement("IdSetMin", curAinSettings.IdSetMin.ToString(CultureInfo.InvariantCulture)), new XElement("Imax", curAinSettings.Imax.ToString(CultureInfo.InvariantCulture)), new XElement("Imcw", curAinSettings.Imcw.ToString(CultureInfo.InvariantCulture)), new XElement("IoutMax", curAinSettings.IoutMax.ToString(CultureInfo.InvariantCulture)), new XElement("KiFi", curAinSettings.KiFi.ToString(CultureInfo.InvariantCulture)), new XElement("KiId", curAinSettings.KiId.ToString(CultureInfo.InvariantCulture)), new XElement("KiIq", curAinSettings.KiIq.ToString(CultureInfo.InvariantCulture)), new XElement("KiW", curAinSettings.KiW.ToString(CultureInfo.InvariantCulture)), new XElement("KpFi", curAinSettings.KpFi.ToString(CultureInfo.InvariantCulture)), new XElement("KpId", curAinSettings.KpId.ToString(CultureInfo.InvariantCulture)), new XElement("KpIq", curAinSettings.KpIq.ToString(CultureInfo.InvariantCulture)), new XElement("KpW", curAinSettings.KpW.ToString(CultureInfo.InvariantCulture)), new XElement("Lm", curAinSettings.Lm.ToString(CultureInfo.InvariantCulture)), new XElement("Lrl", curAinSettings.Lrl.ToString(CultureInfo.InvariantCulture)), new XElement("Lsl", curAinSettings.Lsl.ToString(CultureInfo.InvariantCulture)), new XElement("NimpFloorCode", curAinSettings.NimpFloorCode.ToString(CultureInfo.InvariantCulture)), new XElement("Np", curAinSettings.Np.ToString(CultureInfo.InvariantCulture)), new XElement("Reserved00", curAinSettings.Reserved00), new XElement("Reserved24", curAinSettings.Reserved24), new XElement("Reserved28", curAinSettings.Reserved28), new XElement("Reserved32", curAinSettings.Reserved32), new XElement("Reserved50", curAinSettings.Reserved50), new XElement("Reserved51", curAinSettings.Reserved51), new XElement("Rs", curAinSettings.Rs.ToString(CultureInfo.InvariantCulture)), new XElement("TauF", curAinSettings.TauF.ToString(CultureInfo.InvariantCulture)), new XElement("TauFSet", curAinSettings.TauFSet.ToString(CultureInfo.InvariantCulture)), new XElement("TauFi", curAinSettings.TauFi.ToString(CultureInfo.InvariantCulture)), new XElement("TauFlLim", curAinSettings.TauFlLim.ToString(CultureInfo.InvariantCulture)), new XElement("TauM", curAinSettings.TauM.ToString(CultureInfo.InvariantCulture)), new XElement("TauR", curAinSettings.TauR.ToString(CultureInfo.InvariantCulture)), new XElement("TextMax", curAinSettings.TextMax.ToString(CultureInfo.InvariantCulture)), new XElement("ToHl", curAinSettings.ToHl.ToString(CultureInfo.InvariantCulture)), new XElement("UchMax", curAinSettings.UchMax), new XElement("UchMin", curAinSettings.UchMin), new XElement("Udc0", curAinSettings.Udc0.ToString(CultureInfo.InvariantCulture)), new XElement("UdcMax", curAinSettings.UdcMax.ToString(CultureInfo.InvariantCulture)), new XElement("UdcMin", curAinSettings.UdcMin.ToString(CultureInfo.InvariantCulture)), new XElement("UmodThr", curAinSettings.UmodThr.ToString(CultureInfo.InvariantCulture)), new XElement("Unom", curAinSettings.Unom.ToString(CultureInfo.InvariantCulture)));
						rootElement.Add(element);
					}
				}
				var engineSettings = _engineSettingsStorageSettable.EngineSettings;
				if (engineSettings != null) {
					var engineSettingsElement = new XElement("EngineSettings", new XElement("Inom", engineSettings.Inom.ToString(CultureInfo.InvariantCulture)), new XElement("Nnom", engineSettings.Nnom.ToString(CultureInfo.InvariantCulture)), new XElement("Nmax", engineSettings.Nmax.ToString(CultureInfo.InvariantCulture)), new XElement("Pnom", engineSettings.Pnom.ToString(CultureInfo.InvariantCulture)), new XElement("CosFi", engineSettings.CosFi.ToString(CultureInfo.InvariantCulture)), new XElement("Eff", engineSettings.Eff.ToString(CultureInfo.InvariantCulture)), new XElement("Mass", engineSettings.Mass.ToString(CultureInfo.InvariantCulture)), new XElement("MmM", engineSettings.MmM.ToString(CultureInfo.InvariantCulture)), new XElement("Height", engineSettings.Height.ToString(CultureInfo.InvariantCulture)), new XElement("I2Tmax", engineSettings.I2Tmax.ToString(CultureInfo.InvariantCulture)), new XElement("Icontinious", engineSettings.Icontinious.ToString(CultureInfo.InvariantCulture)), new XElement("ZeroF", engineSettings.ZeroF.ToString(CultureInfo.InvariantCulture)));
					rootElement.Add(engineSettingsElement);
				}
				doc.Add(rootElement);
				doc.Save(File.Open(filename, FileMode.Create, FileAccess.ReadWrite));
			}
		}

		public ICommand LoadCmd => _loadCmd;
		public ICommand SaveCmd => _saveCmd;
	}
}