using System;
using System.Globalization;
using System.IO;
using AlienJust.Support.Conversion.Contracts;
using DrillingRig.Commands.EngineTests;

namespace DrillingRig.ConfigApp.EngineAutoSetup {
	internal class EngineTestParamsBuilderAciIdentifyIni : IBuilderManyToOne<IEngineTestParams> {
		private readonly string _aciIdentifyIniPath;

		public EngineTestParamsBuilderAciIdentifyIni(string aciIdentifyIniPath) {
			_aciIdentifyIniPath = aciIdentifyIniPath;
		}

		public IEngineTestParams Build() {
			string content;
			using (var sr = new StreamReader(_aciIdentifyIniPath)) {
				content = sr.ReadToEnd();
				sr.Close();
			}
			var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			var settings = new EngineTestParamsSetAllFirst();

			foreach (var line in lines) {
				var lineParts = line.Split('\t');
				if (lineParts.Length == 2) {
					switch (lineParts[1]) {
						case "TE1":
							settings.Te1 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "t1c":
							settings.T1C = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "t2c":
							settings.T2C = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "K21":
							settings.K21 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "TE6":
							settings.Te6 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "F1":
							settings.F1 = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "F2":
							settings.F2 = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "acc8":
							settings.Acc8 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "dir10":
							settings.Dir10 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "TJ1":
							settings.Tj1 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "TJ2":
							settings.Tj2 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "TJ3":
							settings.Tj3 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "TJ4":
							settings.Tj4 = int.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "Kp1":
							settings.Kp1 = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "Ki1":
							settings.Ki1 = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "Kp6":
							settings.Kp6 = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "Ki6":
							settings.Ki6 = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "TauI":
							settings.TauI = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "TauFI":
							settings.TauFi = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "TauSpd":
							settings.TauSpd = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						case "F0":
							settings.F0 = float.Parse(lineParts[0], CultureInfo.InvariantCulture);
							break;
						default:
							continue;
					}
				}
			}
			return settings;
		}
	}
}