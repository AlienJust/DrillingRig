namespace DrillingRig.Commands.AinSettings {
	public class AinSettingsSimple : IAinSettings {
		private readonly int _kpW;
		private readonly int _kiW;
		private readonly short _fiNom;
		private readonly short _imax;
		private readonly short _udcMax;
		private readonly short _udcMin;
		private readonly short _fnom;
		private readonly short _fmax;

		private readonly short _empty10;
		private readonly short _empty11;
		
		private readonly short _ioutMax;
		private readonly short _fiMin;
		private readonly short _dacCh;
		private readonly short _imcw;
		private readonly short _ia0;
		private readonly short _ib0;
		private readonly short _ic0;
		private readonly short _udc0;
		private readonly short _tauR;
		private readonly short _lm;
		private readonly short _lsl;
		private readonly short _lrl;
		private readonly int _kpFi;
		private readonly int _kiFi;
		private readonly int _kpId;
		private readonly int _kiId;
		private readonly int _kpIq;
		private readonly int _kiIq;
		private readonly short _accDfDt;
		private readonly short _decDfDt;
		private readonly short _unom;

		private readonly short _empty39;
		
		private readonly short _rs;
		private readonly short _fmin;
		private readonly short _tauM;
		private readonly short _tauF;
		private readonly short _tauFSet;
		private readonly short _tauFi;
		private readonly short _idSetMin;
		private readonly short _idSetMax;
		private readonly int _kpFe;
		private readonly int _kiFe;
		private readonly short _np;

		private readonly short _empty53;

		private readonly short _emdecDfdt;
		private readonly short _textMax;
		private readonly short _toHl;
		public AinSettingsSimple(
			int kpW, 
			int kiW, 
			short fiNom, 
			short imax, 
			short udcMax, 
			short udcMin, 
			short fnom, 
			short fmax,

			short empty10,
			short empty11,

			short ioutMax, 
			short fiMin, 
			short dacCh, 
			short imcw, 
			short ia0, 
			short ib0, 
			short ic0, 
			short udc0, 
			short tauR, 
			short lm, 
			short lsl, 
			short lrl, 
			int kpFi, 
			int kiFi, 
			int kpId, 
			int kiId, 
			int kpIq, 
			int kiIq, 
			short accDfDt, 
			short decDfDt, 
			short unom,

			short empty39,

			short rs, 
			short fmin, 
			short tauM, 
			short tauF, 
			short tauFSet, 
			short tauFi, 
			short idSetMin, 
			short idSetMax, 
			int kpFe, 
			int kiFe, 
			short np,

			short empty53,

			short emdecDfdt, 
			short textMax, 
			short toHl) {
			_kpW = kpW;
			_kiW = kiW;
			_fiNom = fiNom;
			_imax = imax;
			_udcMax = udcMax;
			_udcMin = udcMin;
			_fnom = fnom;
			_fmax = fmax;
			
			_empty10 = empty10;
			_empty11 = empty11;

			_ioutMax = ioutMax;
			_fiMin = fiMin;
			_dacCh = dacCh;
			_imcw = imcw;
			_ia0 = ia0;
			_ib0 = ib0;
			_ic0 = ic0;
			_udc0 = udc0;
			_tauR = tauR;
			_lm = lm;
			_lsl = lsl;
			_lrl = lrl;
			_kpFi = kpFi;
			_kiFi = kiFi;
			_kpId = kpId;
			_kiId = kiId;
			_kpIq = kpIq;
			_kiIq = kiIq;
			_accDfDt = accDfDt;
			_decDfDt = decDfDt;
			_unom = unom;

			_empty39 = empty39;

			_rs = rs;
			_fmin = fmin;
			_tauM = tauM;
			_tauF = tauF;
			_tauFSet = tauFSet;
			_tauFi = tauFi;
			_idSetMin = idSetMin;
			_idSetMax = idSetMax;
			_kpFe = kpFe;
			_kiFe = kiFe;
			
			_np = np;

			_empty53 = empty53;

			_emdecDfdt = emdecDfdt;
			_textMax = textMax;
			_toHl = toHl;
		}

		public int KpW {
			get { return _kpW; }
		}

		public int KiW {
			get { return _kiW; }
		}

		public short FiNom {
			get { return _fiNom; }
		}

		public short Imax {
			get { return _imax; }
		}

		public short UdcMax {
			get { return _udcMax; }
		}

		public short UdcMin {
			get { return _udcMin; }
		}

		public short Fnom {
			get { return _fnom; }
		}

		public short Fmax {
			get { return _fmax; }
		}

		public short Empty10 {
			get { return _empty10; }
		}

		public short Empty11
		{
			get { return _empty11; }
		}

		public short IoutMax {
			get { return _ioutMax; }
		}

		public short FiMin {
			get { return _fiMin; }
		}

		public short DacCh {
			get { return _dacCh; }
		}

		public short Imcw {
			get { return _imcw; }
		}

		public short Ia0 {
			get { return _ia0; }
		}

		public short Ib0 {
			get { return _ib0; }
		}

		public short Ic0 {
			get { return _ic0; }
		}

		public short Udc0 {
			get { return _udc0; }
		}

		public short TauR {
			get { return _tauR; }
		}

		public short Lm {
			get { return _lm; }
		}

		public short Lsl {
			get { return _lsl; }
		}

		public short Lrl {
			get { return _lrl; }
		}

		public int KpFi {
			get { return _kpFi; }
		}

		public int KiFi {
			get { return _kiFi; }
		}

		public int KpId {
			get { return _kpId; }
		}

		public int KiId {
			get { return _kiId; }
		}

		public int KpIq {
			get { return _kpIq; }
		}

		public int KiIq {
			get { return _kiIq; }
		}

		public short AccDfDt {
			get { return _accDfDt; }
		}

		public short DecDfDt {
			get { return _decDfDt; }
		}

		public short Unom {
			get { return _unom; }
		}

		public short Empty39
		{
			get { return _empty39; }
		}

		public short Rs {
			get { return _rs; }
		}

		public short Fmin {
			get { return _fmin; }
		}

		public short TauM {
			get { return _tauM; }
		}

		public short TauF {
			get { return _tauF; }
		}

		public short TauFSet {
			get { return _tauFSet; }
		}

		public short TauFi {
			get { return _tauFi; }
		}

		public short IdSetMin {
			get { return _idSetMin; }
		}

		public short IdSetMax {
			get { return _idSetMax; }
		}

		public int KpFe {
			get { return _kpFe; }
		}

		public int KiFe {
			get { return _kiFe; }
		}

		public short Np {
			get { return _np; }
		}

		public short Empty53
		{
			get { return _empty53; }
		}

		public short EmdecDfdt {
			get { return _emdecDfdt; }
		}

		public short TextMax {
			get { return _textMax; }
		}

		public short ToHl {
			get { return _toHl; }
		}
	}
}