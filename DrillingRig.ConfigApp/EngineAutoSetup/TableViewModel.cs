using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.Commands.EngineTests;

namespace DrillingRig.ConfigApp.EngineAutoSetup {
	class TableViewModel : ViewModelBase {
		public string Header { get; }

		private double? _rs;
		private double? _rr; // rs/2
		private double? _lslAndLrl;
		private double? _lm;
		private short? _flNom;
		private double? _j;
		private double? _tr;
		private double? _roverL;

		private double? _idIqKp;
		private double? _idIqTi;
		private double? _idIqKi;

		private double? _fluxKp;
		private double? _fluxTi;
		private double? _fluxKi;

		private double? _speedKp;
		private double? _speedTi;
		private double? _speedKi;

		public TableViewModel(string header) {
			Header = header;
		}

		/// <summary>
		/// Not thread safe!
		/// </summary>
		public void Update(IEngineTestResult testResult, IAinSettings settings, float f0) {
			// TODO: define what to do if some method param value is null
			Rs = settings?.Rs;
			
			LslAndLrl = settings?.Lrl;
			Lm = settings?.Lm;
			FlNom = settings?.FiNom;
			Tr = settings?.TauR;

			IdIqKp = settings?.KpId; // для асинхронника Kp ID = Kp IQ
			if (settings != null) {
				IdIqTi = 1.0 / (settings.KiId * f0);
			}
			IdIqKi = settings?.KiId;

			FluxKp = settings?.KpFi;
			if (settings != null) {
				FluxTi = 1.0 / (settings.KiFi * f0);
			}
			FluxKi = settings?.KiFi;

			SpeedKp = settings?.KpW;
			if (settings != null) {
				SpeedTi = 1.0 / (settings.KiW * f0);
			}
			SpeedKi = settings?.KiW;
			

			if (testResult != null) {
				Rr = (short)testResult.Rr;
				J = (short)testResult.J;
				RoverL = (short)testResult.RoverL;

				// Берём дублирующиеся параметры из результатов только, если настройки не вычитаны
				if (settings == null) {
					Rs = (short)testResult.Rs;
					LslAndLrl = (short)testResult.Lsl;
					Lm = (short)testResult.Lm;
					FlNom = (short)testResult.FlNom;
					Tr = (short)testResult.TauR;
				}
			}
			else {
				Rr = (short?)(settings?.Rs / 2);
				J = settings == null? 1 : (short?)null;
				RoverL = settings == null ? 0 : (short?)null;
			}
		}

		public double? Rs {
			get { return _rs; }
			set {
				if (_rs != value) {
					_rs = value;
					RaisePropertyChanged(() => Rs);
				}
			}
		}

		public double? Rr {
			get { return _rr; }
			set {
				if (_rr != value) {
					_rr = value;
					RaisePropertyChanged(() => Rr);
				}
			}
		}

		public double? LslAndLrl {
			get { return _lslAndLrl; }
			set {
				if (_lslAndLrl != value) {
					_lslAndLrl = value;
					RaisePropertyChanged(() => LslAndLrl);
				}
			}
		}

		public double? Lm {
			get { return _lm; }
			set {
				if (_lm != value) {
				}
				_lm = value;
				RaisePropertyChanged(() => Lm);
			}
		}

		public short? FlNom {
			get { return _flNom; }
			set {
				if (_flNom != value) {
					_flNom = value;
					RaisePropertyChanged(() => FlNom);
				}
			}
		}

		public double? J {
			get { return _j; }
			set {
				if (_j != value) {
					_j = value;
					RaisePropertyChanged(() => J);
				}
			}
		}

		public double? Tr {
			get { return _tr; }
			set {
				if (_tr != value) {
					_tr = value;
					RaisePropertyChanged(() => Tr);
				}
			}
		}

		public double? RoverL {
			get { return _roverL; }
			set {
				if (_roverL != value) {
					_roverL = value;
					RaisePropertyChanged(() => RoverL);
				}
			}
		}


		public double? IdIqKp {
			get { return _idIqKp; }
			set {
				if (_idIqKp != value) {
					_idIqKp = value;
					RaisePropertyChanged(() => IdIqKp);
				}
			}
		}
		public double? IdIqTi {
			get { return _idIqTi; }
			set {
				if (_idIqTi != value) {
					_idIqTi = value;
					RaisePropertyChanged(() => IdIqTi);
				}
			}
		}
		public double? IdIqKi {
			get { return _idIqKi; }
			set {
				if (_idIqKi != value) {
					_idIqKi = value;
					RaisePropertyChanged(() => IdIqKi);
				}
			}
		}


		public double? FluxKp {
			get { return _fluxKp; }
			set {
				if (_fluxKp != value) {
					_fluxKp = value;
					RaisePropertyChanged(() => FluxKp);
				}
			}
		}
		public double? FluxTi {
			get { return _fluxTi; }
			set {
				if (_fluxTi != value) {
					_fluxTi = value;
					RaisePropertyChanged(() => FluxTi);
				}
			}
		}
		public double? FluxKi {
			get { return _fluxKi; }
			set {
				if (_fluxKi != value) {
					_fluxKi = value;
					RaisePropertyChanged(() => FluxKi);
				}
			}
		}


		public double? SpeedKp {
			get { return _speedKp; }
			set {
				if (_speedKp != value) {
					_speedKp = value;
					RaisePropertyChanged(() => SpeedKp);
				}
			}
		}
		public double? SpeedTi {
			get { return _speedTi; }
			set {
				if (_speedTi != value) {
					_speedTi = value;
					RaisePropertyChanged(() => SpeedTi);
				}
			}
		}
		public double? SpeedKi {
			get { return _speedKi; }
			set {
				if (_speedKi != value) {
					_speedKi = value;
					RaisePropertyChanged(() => SpeedKi);
				}
			}
		}
	}
}