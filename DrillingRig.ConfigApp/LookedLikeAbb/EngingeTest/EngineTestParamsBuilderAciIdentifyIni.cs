using System;
using System.IO;
using AlienJust.Support.Conversion.Contracts;
using DrillingRig.Commands.EngineTests;

namespace DrillingRig.ConfigApp.LookedLikeAbb.EngingeTest {
	internal class EngineTestParamsBuilderAciIdentifyIni : IBuilderManyToOne<IEngineTestParams>
	{
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
						case "F1":
							settings.F1 = float.Parse(lineParts[0]);
							break;
						case "F2":
							settings.F2 = float.Parse(lineParts[0]);
							break;
						case "TE":
							settings.Te = int.Parse(lineParts[0]); // TODO: NOT IN FILE aci_identify.ini???
							break;
						case "TE1":
							settings.Te1 = int.Parse(lineParts[0]);
							break;
						case "TE6":
							settings.Te6 = int.Parse(lineParts[0]);
							break;
						case "TE8":
							settings.Te8 = int.Parse(lineParts[0]); // TODO: NOT IN FILE aci_identify.ini???
							break;
						case "K21":
							settings.K21 = int.Parse(lineParts[0]);
							break;
						case "TJ1":
							settings.Tj1 = int.Parse(lineParts[0]);
							break;
						case "TJ2":
							settings.Tj2 = int.Parse(lineParts[0]);
							break;
						case "TJ3":
							settings.Tj3 = int.Parse(lineParts[0]);
							break;
						case "TJ4":
							settings.Tj4 = int.Parse(lineParts[0]);
							break;
						case "acc8":
							settings.Acc8 = int.Parse(lineParts[0]);
							break;
						case "dir10":
							settings.Dir10 = int.Parse(lineParts[0]);
							break;
						case "t1c":
							settings.T1C = float.Parse(lineParts[0]);
							break;
						case "t2c":
							settings.T2C = float.Parse(lineParts[0]);
							break;
						case "Kp1":
							settings.Kp1 = float.Parse(lineParts[0]);
							break;
						case "Ki1":
							settings.Ki1 = float.Parse(lineParts[0]);
							break;
						case "Kp6":
							settings.Kp6 = float.Parse(lineParts[0]);
							break;
						case "Ki6":
							settings.Ki6 = float.Parse(lineParts[0]);
							break;
						case "TauI":
							settings.TauI = float.Parse(lineParts[0]);
							break;
						case "TauSpd":
							settings.TauSpd = float.Parse(lineParts[0]);
							break;
						case "TauFI":
							settings.TauFi = float.Parse(lineParts[0]);
							break;
						default:
							continue;
					}
				}
			}
			return settings;
		}
	}

	/// <summary>
	/// Выдаст исключение при доступе к свойству, если оно не было сперва инициализировано
	/// </summary>
	class EngineTestParamsSetAllFirst : IEngineTestParams {
		private float? _f1;
		private float? _f2;
		private int? _te;
		private int? _te1;
		private int? _te6;
		private int? _te8;
		private int? _k21;
		private int? _tj1;
		private int? _tj2;
		private int? _tj3;
		private int? _tj4;
		private int? _acc8;
		private int? _dir10;
		private float? _t1C;
		private float? _t2C;
		private float? _kp1;
		private float? _ki1;
		private float? _kp6;
		private float? _ki6;
		private float? _tauI;
		private float? _tauSpd;
		private float? _tauFi;

		public float F1
		{
			get { return _f1.Value; }
			set { _f1 = value; }
		}

		public float F2
		{
			get { return _f2.Value; }
			set { _f2 = value; }
		}

		public int Te
		{
			get { return _te.Value; }
			set { _te = value; }
		}

		public int Te1
		{
			get { return _te1.Value; }
			set { _te1 = value; }
		}

		public int Te6
		{
			get { return _te6.Value; }
			set { _te6 = value; }
		}

		public int Te8
		{
			get { return _te8.Value; }
			set { _te8 = value; }
		}

		public int K21
		{
			get { return _k21.Value; }
			set { _k21 = value; }
		}

		public int Tj1
		{
			get { return _tj1.Value; }
			set { _tj1 = value; }
		}

		public int Tj2
		{
			get { return _tj2.Value; }
			set { _tj2 = value; }
		}

		public int Tj3
		{
			get { return _tj3.Value; }
			set { _tj3 = value; }
		}

		public int Tj4
		{
			get { return _tj4.Value; }
			set { _tj4 = value; }
		}

		public int Acc8
		{
			get { return _acc8.Value; }
			set { _acc8 = value; }
		}

		public int Dir10
		{
			get { return _dir10.Value; }
			set { _dir10 = value; }
		}

		public float T1C
		{
			get { return _t1C.Value; }
			set { _t1C = value; }
		}

		public float T2C
		{
			get { return _t2C.Value; }
			set { _t2C = value; }
		}

		public float Kp1
		{
			get { return _kp1.Value; }
			set { _kp1 = value; }
		}

		public float Ki1
		{
			get { return _ki1.Value; }
			set { _ki1 = value; }
		}

		public float Kp6
		{
			get { return _kp6.Value; }
			set { _kp6 = value; }
		}

		public float Ki6
		{
			get { return _ki6.Value; }
			set { _ki6 = value; }
		}

		public float TauI
		{
			get { return _tauI.Value; }
			set { _tauI = value; }
		}

		public float TauSpd
		{
			get { return _tauSpd.Value; }
			set { _tauSpd = value; }
		}

		public float TauFi
		{
			get { return _tauFi.Value; }
			set { _tauFi = value; }
		}
	}
}