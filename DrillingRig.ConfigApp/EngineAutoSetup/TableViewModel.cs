using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.AinSettings;

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

		public TableViewModel(string header) {
			Header = header;
		}

		/// <summary>
		/// Not thread safe!
		/// </summary>
		public void Update(IAinSettings settings) {
			// TODO: get params from incoming settings!!1
			Rs = settings?.Rs;
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
					RaisePropertyChanged(() => _lslAndLrl);
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
	}
}
