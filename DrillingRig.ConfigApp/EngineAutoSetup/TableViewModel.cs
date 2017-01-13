using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;
using DrillingRig.Commands.EngineTests;

namespace DrillingRig.ConfigApp.EngineAutoSetup {
	class TableViewModel : ViewModelBase {
		public string Header { get; }

		private int? _rs;
		private int? _rr;
		private int? _lslAndLrl;
		private int? _lm;
		private int? _flNom;
		private int? _j;
		private int? _tr;
		private int? _roverL;

		private double? _idIqKp;
		private double? _idIqTi;

		public TableViewModel(string header) {
			Header = header;
		}

		/// <summary>
		/// Not thread safe!
		/// </summary>
		public void Update(IEngineTestResult testResult, IAinSettings settings) {
			// TODO: define what to do if some method param value is null
			Rs = settings?.Rs;
			Rr = settings?.Rs / 2;
			LslAndLrl = settings?.Lrl;
			Lm = settings?.Lm;
			FlNom = settings?.FiNom;
			Tr = settings?.TauR;
			IdIqKp = settings?.KpId;
			//IdIqTi = settings. // TODO: ASK ROMAN

			if (testResult != null) {
				Rr = testResult.Rr;
				J = testResult.J;
				RoverL = testResult.RoverL;

				// Берём дублирующиеся параметры из результатов только, если настройки не вычитаны
				if (settings == null) {
					Rs = testResult.Rs;
					LslAndLrl = testResult.Lsl;
					Lm = testResult.Lm;
					FlNom = testResult.FlNom;
					Tr = testResult.Tr;
				}
			}
		}

		public int? Rs {
			get { return _rs; }
			set {
				if (_rs != value) {
					_rs = value;
					RaisePropertyChanged(() => Rs);
				}
			}
		}

		public int? Rr {
			get { return _rr; }
			set {
				if (_rr != value) {
					_rr = value;
					RaisePropertyChanged(() => Rr);
				}
			}
		}

		public int? LslAndLrl {
			get { return _lslAndLrl; }
			set {
				if (_lslAndLrl != value) {
					_lslAndLrl = value;
					RaisePropertyChanged(() => LslAndLrl);
				}
			}
		}

		public int? Lm {
			get { return _lm; }
			set {
				if (_lm != value) {
				}
				_lm = value;
				RaisePropertyChanged(() => Lm);
			}
		}

		public int? FlNom {
			get { return _flNom; }
			set {
				if (_flNom != value) {
					_flNom = value;
					RaisePropertyChanged(() => FlNom);
				}
			}
		}

		public int? J {
			get { return _j; }
			set {
				if (_j != value) {
					_j = value;
					RaisePropertyChanged(() => J);
				}
			}
		}

		public int? Tr {
			get { return _tr; }
			set {
				if (_tr != value) {
					_tr = value;
					RaisePropertyChanged(() => Tr);
				}
			}
		}

		public int? RoverL {
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
	}
}