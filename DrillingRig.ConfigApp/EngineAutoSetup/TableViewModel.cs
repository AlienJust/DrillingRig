using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.Commands.EngineTests;

namespace DrillingRig.ConfigApp.EngineAutoSetup {
	class TableViewModel : ViewModelBase {
		public string Header { get; }

		private decimal? _rs;
		private double? _rr; // rs/2
		private decimal? _lslAndLrl;
		private decimal? _lm;
		private decimal? _flNom;
		private double? _j;
		private decimal? _tr;
		private double? _roverL;

		private decimal? _idIqKp;
		private decimal? _idIqTi;
		private decimal? _idIqKi;

		private decimal? _fluxKp;
		private decimal? _fluxTi;
		private decimal? _fluxKi;

		private decimal? _speedKp;
		private decimal? _speedTi;
		private decimal? _speedKi;

		public TableViewModel(string header) {
			Header = header;
		}

		/// <summary>
		/// Not thread safe!
		/// </summary>
		public void Update(IEngineTestResult testResult, IAinSettings settings, decimal f0) {
			// TODO: define what to do if some method param value is null
			Rs = settings?.Rs;

			LslAndLrl = settings?.Lrl;
			Lm = settings?.Lm;
			FlNom = settings?.FiNom;
			Tr = settings?.TauR;

			IdIqKp = settings?.KpId; // для асинхронника Kp ID = Kp IQ
			if (settings != null) {
				IdIqTi = 1.0m / (settings.KiId * f0);
			}
			IdIqKi = settings?.KiId;

			FluxKp = settings?.KpFi;
			if (settings != null) {
				FluxTi = 1.0m / (settings.KiFi * f0);
			}
			FluxKi = settings?.KiFi;

			SpeedKp = settings?.KpW;
			if (settings != null) {
				SpeedTi = 1.0m / (settings.KiW * f0);
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
				J = settings == null ? 1 : (short?)null;
				RoverL = settings == null ? 0 : (short?)null;
			}
		}

		public decimal? Rs {
			get => _rs;
			set {
				if (_rs != value) {
					_rs = value;
					RaisePropertyChanged(() => Rs);
				}
			}
		}

		public double? Rr {
			get => _rr;
			set {
				if (_rr != value) {
					_rr = value;
					RaisePropertyChanged(() => Rr);
				}
			}
		}

		public decimal? LslAndLrl {
			get => _lslAndLrl;
			set {
				if (_lslAndLrl != value) {
					_lslAndLrl = value;
					RaisePropertyChanged(() => LslAndLrl);
				}
			}
		}

		public decimal? Lm {
			get => _lm;
			set {
				if (_lm != value) {
				}
				_lm = value;
				RaisePropertyChanged(() => Lm);
			}
		}

		public decimal? FlNom {
			get => _flNom;
			set {
				if (_flNom != value) {
					_flNom = value;
					RaisePropertyChanged(() => FlNom);
				}
			}
		}

		public double? J {
			get => _j;
			set {
				if (_j != value) {
					_j = value;
					RaisePropertyChanged(() => J);
				}
			}
		}

		public decimal? Tr {
			get => _tr;
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


		public decimal? IdIqKp {
			get => _idIqKp;
			set {
				if (_idIqKp != value) {
					_idIqKp = value;
					RaisePropertyChanged(() => IdIqKp);
				}
			}
		}
		public decimal? IdIqTi {
			get => _idIqTi;
			set {
				if (_idIqTi != value) {
					_idIqTi = value;
					RaisePropertyChanged(() => IdIqTi);
				}
			}
		}
		public decimal? IdIqKi {
			get => _idIqKi;
			set {
				if (_idIqKi != value) {
					_idIqKi = value;
					RaisePropertyChanged(() => IdIqKi);
				}
			}
		}


		public decimal? FluxKp {
			get => _fluxKp;
			set {
				if (_fluxKp != value) {
					_fluxKp = value;
					RaisePropertyChanged(() => FluxKp);
				}
			}
		}
		public decimal? FluxTi {
			get => _fluxTi;
			set {
				if (_fluxTi != value) {
					_fluxTi = value;
					RaisePropertyChanged(() => FluxTi);
				}
			}
		}
		public decimal? FluxKi {
			get => _fluxKi;
			set {
				if (_fluxKi != value) {
					_fluxKi = value;
					RaisePropertyChanged(() => FluxKi);
				}
			}
		}


		public decimal? SpeedKp {
			get => _speedKp;
			set {
				if (_speedKp != value) {
					_speedKp = value;
					RaisePropertyChanged(() => SpeedKp);
				}
			}
		}
		public decimal? SpeedTi {
			get => _speedTi;
			set {
				if (_speedTi != value) {
					_speedTi = value;
					RaisePropertyChanged(() => SpeedTi);
				}
			}
		}
		public decimal? SpeedKi {
			get => _speedKi;
			set {
				if (_speedKi != value) {
					_speedKi = value;
					RaisePropertyChanged(() => SpeedKi);
				}
			}
		}
	}
}