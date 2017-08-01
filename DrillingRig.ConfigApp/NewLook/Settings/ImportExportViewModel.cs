using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using AlienJust.Support.Collections;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.Commands.EngineSettings;
using DrillingRig.ConfigApp.AppControl.AinSettingsStorage;
using DrillingRig.ConfigApp.AppControl.EngineSettingsSpace;
using IAinSettingsReadNotifyRaisable = DrillingRig.ConfigApp.AppControl.AinSettingsRead.IAinSettingsReadNotifyRaisable;

namespace DrillingRig.ConfigApp.NewLook.Settings {
	class ImportExportViewModel : ViewModelBase {
		private readonly IAinSettingsStorageSettable _ainSettingsStorageSettable;
		private readonly IAinSettingsReadNotifyRaisable _ainSettingsReadNotifyRaisable;
		private readonly IEngineSettingsStorageSettable _engineSettingsStorageSettable;
		private readonly IEngineSettingsReadNotifyRaisable _engineSettingsReadNotifyRaisable;
		private readonly IMultiLoggerWithStackTrace<int> _debugLogger;
		private readonly RelayCommand _loadCmd;
		private readonly RelayCommand _saveCmd;

		public ImportExportViewModel(IAinSettingsStorageSettable ainSettingsStorageSettable, IAinSettingsReadNotifyRaisable ainSettingsReadNotifyRaisable, IEngineSettingsStorageSettable engineSettingsStorageSettable, IEngineSettingsReadNotifyRaisable engineSettingsReadNotifyRaisable, IMultiLoggerWithStackTrace<int> debugLogger) {
			_ainSettingsStorageSettable = ainSettingsStorageSettable;
			_ainSettingsReadNotifyRaisable = ainSettingsReadNotifyRaisable;
			_engineSettingsStorageSettable = engineSettingsStorageSettable;
			_engineSettingsReadNotifyRaisable = engineSettingsReadNotifyRaisable;
			_debugLogger = debugLogger;

			_loadCmd = new RelayCommand(Load, () => true);
			_saveCmd = new RelayCommand(Save, () => true);
		}

		private XElement TryGetChildElement(XElement parent, string childName) {
			try {
				var element = parent.Element(childName);
				if (element == null) throw new Exception("XML element " + parent.Name + "." + childName + " is null");
				return element;
			}
			catch (Exception ex) {
				_debugLogger.GetLogger(1).Log("Не удалось найти XML элемент " + parent.Name + "." + childName, new StackTrace(ex, true));
				throw;
			}
		}

		private void Load() {
			var winSystem = new AlienJust.Adaptation.WindowsPresentation.WpfWindowSystem();
			var filename = winSystem.ShowOpenFileDialog("Импорт настроек", "XML files|*.xml|All files|*.*");

			if (filename != null) {
				try {
					var doc = XDocument.Load(filename);
					var rootElement = doc.Element("Settings");
					try {
						var ainElements = rootElement.Elements("AinSettings");
						foreach (var ainElement in ainElements) {
							var zbAinNumber = byte.Parse(ainElement.Attribute("Number").Value);
							try {
								_ainSettingsReadNotifyRaisable.RaiseAinSettingsReadStarted(zbAinNumber);

								var reserved00 = BytesPair.Parse(TryGetChildElement(ainElement, "Reserved00").Value);
								var kpW = decimal.Parse(TryGetChildElement(ainElement, "KpW").Value, CultureInfo.InvariantCulture);
								var kiW = decimal.Parse(TryGetChildElement(ainElement, "KiW").Value, CultureInfo.InvariantCulture);

								var fiNom = decimal.Parse(TryGetChildElement(ainElement, "FiNom").Value, CultureInfo.InvariantCulture);
								var imax = short.Parse(TryGetChildElement(ainElement, "Imax").Value, CultureInfo.InvariantCulture);
								var udcMax = short.Parse(TryGetChildElement(ainElement, "UdcMax").Value, CultureInfo.InvariantCulture);
								var udcMin = short.Parse(TryGetChildElement(ainElement, "UdcMin").Value, CultureInfo.InvariantCulture);
								var fnom = decimal.Parse(TryGetChildElement(ainElement, "Fnom").Value, CultureInfo.InvariantCulture);
								var fmax = decimal.Parse(TryGetChildElement(ainElement, "Fmax").Value, CultureInfo.InvariantCulture);

								var dflLim = decimal.Parse(TryGetChildElement(ainElement, "DflLim").Value, CultureInfo.InvariantCulture);
								var flMinMin = decimal.Parse(TryGetChildElement(ainElement, "FlMinMin").Value, CultureInfo.InvariantCulture);

								var ioutMax = short.Parse(TryGetChildElement(ainElement, "IoutMax").Value, CultureInfo.InvariantCulture);
								var fiMin = decimal.Parse(TryGetChildElement(ainElement, "FiMin").Value, CultureInfo.InvariantCulture);
								var dacCh = ushort.Parse(TryGetChildElement(ainElement, "DacCh").Value, CultureInfo.InvariantCulture);
								var imcw = ushort.Parse(TryGetChildElement(ainElement, "Imcw").Value, CultureInfo.InvariantCulture);
								var ia0 = short.Parse(TryGetChildElement(ainElement, "Ia0").Value, CultureInfo.InvariantCulture);
								var ib0 = short.Parse(TryGetChildElement(ainElement, "Ib0").Value, CultureInfo.InvariantCulture);
								var ic0 = short.Parse(TryGetChildElement(ainElement, "Ic0").Value, CultureInfo.InvariantCulture);
								var udc0 = short.Parse(TryGetChildElement(ainElement, "Udc0").Value, CultureInfo.InvariantCulture);

								var tauR = decimal.Parse(TryGetChildElement(ainElement, "TauR").Value, CultureInfo.InvariantCulture);
								var lm = decimal.Parse(TryGetChildElement(ainElement, "Lm").Value, CultureInfo.InvariantCulture);
								var lsl = decimal.Parse(TryGetChildElement(ainElement, "Lsl").Value, CultureInfo.InvariantCulture);
								var lrl = decimal.Parse(TryGetChildElement(ainElement, "Lrl").Value, CultureInfo.InvariantCulture);
								var reserved24 = BytesPair.Parse(TryGetChildElement(ainElement, "Reserved24").Value);

								var kpFi = decimal.Parse(TryGetChildElement(ainElement, "KpFi").Value, CultureInfo.InvariantCulture);
								var kiFi = decimal.Parse(TryGetChildElement(ainElement, "KiFi").Value, CultureInfo.InvariantCulture);

								var reserved28 = BytesPair.Parse(TryGetChildElement(ainElement, "Reserved28").Value);

								var kpId = decimal.Parse(TryGetChildElement(ainElement, "KpId").Value, CultureInfo.InvariantCulture);
								var kiId = decimal.Parse(TryGetChildElement(ainElement, "KiId").Value, CultureInfo.InvariantCulture);

								var reserved32 = BytesPair.Parse(TryGetChildElement(ainElement, "Reserved32").Value);

								var kpIq = decimal.Parse(TryGetChildElement(ainElement, "KpIq").Value, CultureInfo.InvariantCulture);
								var kiIq = decimal.Parse(TryGetChildElement(ainElement, "KiIq").Value, CultureInfo.InvariantCulture);

								var accDfDt = decimal.Parse(TryGetChildElement(ainElement, "AccDfDt").Value, CultureInfo.InvariantCulture);
								var decDfDt = decimal.Parse(TryGetChildElement(ainElement, "DecDfDt").Value, CultureInfo.InvariantCulture);
								var unom = decimal.Parse(TryGetChildElement(ainElement, "Unom").Value, CultureInfo.InvariantCulture);

								var tauFlLim = decimal.Parse(TryGetChildElement(ainElement, "TauFlLim").Value, CultureInfo.InvariantCulture);

								var rs = decimal.Parse(TryGetChildElement(ainElement, "Rs").Value, CultureInfo.InvariantCulture);
								var fmin = decimal.Parse(TryGetChildElement(ainElement, "Fmin").Value, CultureInfo.InvariantCulture);
								var tauM = decimal.Parse(TryGetChildElement(ainElement, "TauM").Value, CultureInfo.InvariantCulture);
								var tauF = decimal.Parse(TryGetChildElement(ainElement, "TauF").Value, CultureInfo.InvariantCulture);
								var tauFSet = decimal.Parse(TryGetChildElement(ainElement, "TauFSet").Value, CultureInfo.InvariantCulture);
								var tauFi = decimal.Parse(TryGetChildElement(ainElement, "TauFi").Value, CultureInfo.InvariantCulture);
								var idSetMin = short.Parse(TryGetChildElement(ainElement, "IdSetMin").Value, CultureInfo.InvariantCulture);
								var idSetMax = short.Parse(TryGetChildElement(ainElement, "IdSetMax").Value, CultureInfo.InvariantCulture);
								var uchMin = BytesPair.Parse(TryGetChildElement(ainElement, "UchMin").Value);
								var uchMax = BytesPair.Parse(TryGetChildElement(ainElement, "UchMax").Value);

								var reserved50 = BytesPair.Parse(TryGetChildElement(ainElement, "Reserved50").Value);
								var reserved51 = BytesPair.Parse(TryGetChildElement(ainElement, "Reserved51").Value);

								var np = int.Parse(TryGetChildElement(ainElement, "Np").Value, CultureInfo.InvariantCulture);
								var nimpFloorCode = int.Parse(TryGetChildElement(ainElement, "NimpFloorCode").Value, CultureInfo.InvariantCulture);
								var fanMode = AinTelemetryFanWorkmodeExtensions.FromIoBits(int.Parse(TryGetChildElement(ainElement, "FanMode").Value, CultureInfo.InvariantCulture));
								var directCurrentMagnetization = bool.Parse(TryGetChildElement(ainElement, "DirectCurrentMagnetization").Value);

								var umodThr = decimal.Parse(TryGetChildElement(ainElement, "UmodThr").Value, CultureInfo.InvariantCulture);

								var emdecDfdt = decimal.Parse(TryGetChildElement(ainElement, "EmdecDfdt").Value, CultureInfo.InvariantCulture);

								var textMax = short.Parse(TryGetChildElement(ainElement, "TextMax").Value, CultureInfo.InvariantCulture);
								var toHl = short.Parse(TryGetChildElement(ainElement, "ToHl").Value, CultureInfo.InvariantCulture);
								var ain1LinkFault = bool.Parse(TryGetChildElement(ainElement, "Ain1LinkFault").Value);
								var ain2LinkFault = bool.Parse(TryGetChildElement(ainElement, "Ain2LinkFault").Value);
								var ain3LinkFault = bool.Parse(TryGetChildElement(ainElement, "Ain3LinkFault").Value);


								var setting = new AinSettingsSimple(reserved00, kpW, kiW, fiNom, imax, udcMax, udcMin, fnom, fmax, dflLim, flMinMin, ioutMax, fiMin, dacCh, imcw, ia0, ib0, ic0, udc0, tauR, lm, lsl, lrl, reserved24, kpFi, kiFi, reserved28, kpId, kiId, reserved32, kpIq, kiIq, accDfDt, decDfDt, unom, tauFlLim, rs, fmin, tauM, tauF, tauFSet, tauFi, idSetMin, idSetMax, uchMin, uchMax, reserved50, reserved51, np, nimpFloorCode, fanMode, directCurrentMagnetization, umodThr, emdecDfdt, textMax, toHl, ain1LinkFault, ain2LinkFault, ain3LinkFault);
								_ainSettingsStorageSettable.SetSettings(zbAinNumber, setting);
								_ainSettingsReadNotifyRaisable.RaiseAinSettingsReadComplete(zbAinNumber, null, setting); // TODO: try catch real exception
								_debugLogger.GetLogger(5).Log("Настройки №" + zbAinNumber + " успешно импортированы", new StackTrace(true));
							}
							catch (Exception e) {
								_debugLogger.GetLogger(1).Log("Не удалось импортировать настройки АИН№" + zbAinNumber + Environment.NewLine + e, new StackTrace(e, true));
							}
						}
					}
					catch (Exception exx) {
						_debugLogger.GetLogger(1).Log("Не удалось импортировать настройки АИН" + Environment.NewLine + exx, new StackTrace(exx, true));
					}
					var engineSettingsElement = rootElement.Element("EngineSettings");
					if (engineSettingsElement != null) {
						try {
							var settings = new EngineSettingsSimple {
								Inom = ushort.Parse(TryGetChildElement(engineSettingsElement, "Inom").Value, CultureInfo.InvariantCulture),
								Nnom = ushort.Parse(TryGetChildElement(engineSettingsElement, "Nnom").Value, CultureInfo.InvariantCulture),
								Nmax = ushort.Parse(TryGetChildElement(engineSettingsElement, "Nmax").Value, CultureInfo.InvariantCulture),
								Pnom = decimal.Parse(TryGetChildElement(engineSettingsElement, "Pnom").Value, CultureInfo.InvariantCulture),
								CosFi = decimal.Parse(TryGetChildElement(engineSettingsElement, "CosFi").Value, CultureInfo.InvariantCulture),
								Eff = decimal.Parse(TryGetChildElement(engineSettingsElement, "Eff").Value, CultureInfo.InvariantCulture),
								Mass = ushort.Parse(TryGetChildElement(engineSettingsElement, "Mass").Value, CultureInfo.InvariantCulture),
								MmM = ushort.Parse(TryGetChildElement(engineSettingsElement, "MmM").Value, CultureInfo.InvariantCulture),
								Height = ushort.Parse(TryGetChildElement(engineSettingsElement, "Height").Value, CultureInfo.InvariantCulture),
								I2Tmax = uint.Parse(TryGetChildElement(engineSettingsElement, "I2Tmax").Value, CultureInfo.InvariantCulture),
								Icontinious = ushort.Parse(TryGetChildElement(engineSettingsElement, "Icontinious").Value, CultureInfo.InvariantCulture),
								ZeroF = ushort.Parse(TryGetChildElement(engineSettingsElement, "ZeroF").Value, CultureInfo.InvariantCulture)
							};
							_engineSettingsStorageSettable.SetSettings(settings);
							_engineSettingsReadNotifyRaisable.RaiseEngineSettingsReadComplete(null, settings);
							_debugLogger.GetLogger(5).Log("Настройки двигателя успешно импортированы", new StackTrace(true));
						}
						catch (Exception ex) {
							_debugLogger.GetLogger(1).Log("Не удалось импортировать настройки двигателя" + Environment.NewLine + ex, new StackTrace(ex, true));
							throw;
						}
					}
				}
				catch (Exception ex) {
					_debugLogger.GetLogger(1).Log("Не удалось импортировать настройки из " + filename + Environment.NewLine + ex, new StackTrace(ex, true));
					throw;
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
						var element = new XElement("AinSettings",
							new XAttribute("Number", i),
							new XElement("Reserved00", curAinSettings.Reserved00),

							new XElement("AccDfDt", curAinSettings.AccDfDt.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ain1LinkFault", curAinSettings.Ain1LinkFault.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ain2LinkFault", curAinSettings.Ain2LinkFault.ToString(CultureInfo.InvariantCulture)),
							new XElement("Ain3LinkFault", curAinSettings.Ain3LinkFault.ToString(CultureInfo.InvariantCulture)),
							new XElement("DacCh", curAinSettings.DacCh.ToString(CultureInfo.InvariantCulture)),
							new XElement("DecDfDt", curAinSettings.DecDfDt.ToString(CultureInfo.InvariantCulture)),
							new XElement("DflLim", curAinSettings.DflLim.ToString(CultureInfo.InvariantCulture)),
							new XElement("EmdecDfdt", curAinSettings.EmdecDfdt.ToString(CultureInfo.InvariantCulture)),

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

							new XElement("Np", curAinSettings.Np.ToString(CultureInfo.InvariantCulture)),
							new XElement("NimpFloorCode", curAinSettings.NimpFloorCode.ToString(CultureInfo.InvariantCulture)),
							new XElement("FanMode", curAinSettings.FanMode.ToIoBits().ToString(CultureInfo.InvariantCulture)),
							new XElement("DirectCurrentMagnetization", curAinSettings.DirectCurrentMagnetization.ToString(CultureInfo.InvariantCulture)),


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
							new XElement("UchMax", curAinSettings.UchMax), new XElement("UchMin", curAinSettings.UchMin),
							new XElement("Udc0", curAinSettings.Udc0.ToString(CultureInfo.InvariantCulture)),
							new XElement("UdcMax", curAinSettings.UdcMax.ToString(CultureInfo.InvariantCulture)),
							new XElement("UdcMin", curAinSettings.UdcMin.ToString(CultureInfo.InvariantCulture)),
							new XElement("UmodThr", curAinSettings.UmodThr.ToString(CultureInfo.InvariantCulture)),
							new XElement("Unom", curAinSettings.Unom.ToString(CultureInfo.InvariantCulture)));
						rootElement.Add(element);
					}
				}
				var engineSettings = _engineSettingsStorageSettable.EngineSettings;
				if (engineSettings != null) {
					var engineSettingsElement = new XElement("EngineSettings", new XElement("Inom", engineSettings.Inom.ToString(CultureInfo.InvariantCulture)),
						new XElement("Nnom", engineSettings.Nnom.ToString(CultureInfo.InvariantCulture)), new XElement("Nmax", engineSettings.Nmax.ToString(CultureInfo.InvariantCulture)),
						new XElement("Pnom", engineSettings.Pnom.ToString(CultureInfo.InvariantCulture)), new XElement("CosFi", engineSettings.CosFi.ToString(CultureInfo.InvariantCulture)),
						new XElement("Eff", engineSettings.Eff.ToString(CultureInfo.InvariantCulture)), new XElement("Mass", engineSettings.Mass.ToString(CultureInfo.InvariantCulture)),
						new XElement("MmM", engineSettings.MmM.ToString(CultureInfo.InvariantCulture)), new XElement("Height", engineSettings.Height.ToString(CultureInfo.InvariantCulture)),
						new XElement("I2Tmax", engineSettings.I2Tmax.ToString(CultureInfo.InvariantCulture)), new XElement("Icontinious", engineSettings.Icontinious.ToString(CultureInfo.InvariantCulture)),
						new XElement("ZeroF", engineSettings.ZeroF.ToString(CultureInfo.InvariantCulture)));
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