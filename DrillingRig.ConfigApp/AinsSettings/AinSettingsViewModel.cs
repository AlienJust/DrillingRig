using System;
using System.Windows.Input;
using AlienJust.Support.Collections;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.AinSettings;
using DrillingRig.ConfigApp.AppControl.CommandSenderHost;
using DrillingRig.ConfigApp.AppControl.NotifySendingEnabled;
using DrillingRig.ConfigApp.AppControl.TargetAddressHost;

namespace DrillingRig.ConfigApp.AinsSettings
{
	class AinSettingsViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;

		private readonly RelayCommand _readSettingsCommand;
		private readonly RelayCommand _writeSettingsCommand;

		private double? _kpW;
		private double? _kiW;

		private short? _fiNom;
		private short? _imax;
		private short? _udcMax;
		private short? _udcMin;
		private double? _fnom;
		private double? _fmax;

		private short? _empty10;
		private short? _empty11;

		private short? _ioutMax;
		private short? _fiMin;
		private short? _dacCh;
		private short? _imcw;

		private short? _ia0;
		private short? _ib0;
		private short? _ic0;
		private short? _udc0;

		private double? _tauR;
		private double? _lm;
		private double? _lsl;
		private double? _lrl;

		private double? _kpFi;
		private double? _kiFi;
		private double? _kpId;
		private double? _kiId;
		private double? _kpIq;
		private double? _kiIq;

		private short? _accDfDt;
		private short? _decDfDt;
		private double? _unom;

		private short? _empty39;

		private double? _rs;
		private double? _fmin;

		private short? _tauM;
		private short? _tauF;
		private short? _tauFSet;
		private short? _tauFi;
		private short? _idSetMin;
		private short? _idSetMax;

		private short? _uchMin;
		private short? _uchMax;
		private short? _np;

		private short? _empty53;

		private short? _emdecDfdt;
		private short? _textMax;
		private short? _toHl;

		private readonly byte _zeroBasedAinNumber;

		public AinSettingsViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl, byte zeroBasedAinNumber)
		{
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;
			_zeroBasedAinNumber = zeroBasedAinNumber;

			_readSettingsCommand = new RelayCommand(ReadSettings, () => _sendingEnabledControl.IsSendingEnabled);
			_writeSettingsCommand = new RelayCommand(WriteSettings, () => _sendingEnabledControl.IsSendingEnabled);

			ImportSettingsCommand = new RelayCommand(ImportSettings);
			ExportSettingsCommand = new RelayCommand(ExportSettings);


			KpW = null; // 0 1
			KiW = null; // 2 3
			FiNom = null; // 4
			Imax = null; // 5
			UdcMax = null; // 6
			UdcMin = null; // 7
			Fnom = null;// 8
			Fmax = null; // 9

			Empty10 = null; // 10
			Empty11 = null; // 11

			IoutMax = null; //12
			FiMin = null; // 13
			DacCh = null; // 14
			Imcw = null; // 15

			Ia0 = null; // 16
			Ib0 = null; // 17
			Ic0 = null; // 18
			Udc0 = null; // 19
			TauR = null; // 20

			Lm = null; // 21
			Lsl = null; // 22
			Lrl = null; // 23

			KpFi = null; // 24
			KiFi = null; // 26
			KpId = null; // 28
			KiId = null; // 30
			KpIq = null; // 32
			KiIq = null; // 34

			AccDfDt = null; // 35
			DecDfDt = null; // 36
			Unom = null; // 37

			Empty39 = null; // 38

			Rs = null; // 
			Fmin = null; // 
			TauM = null; // 
			TauF = null; // 
			TauFSet = null; // 
			TauFi = null; // 
			IdSetMin = null; // 
			IdSetMax = null; // 

			UchMin = null; // 
			UchMax = null; // 
			Np = null; // 

			Empty53 = null;

			EmdecDfdt = null;
			TextMax = null;
			ToHl = null;

			
			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool issendingenabled) {
			_readSettingsCommand.RaiseCanExecuteChanged();
			_writeSettingsCommand.RaiseCanExecuteChanged();
		}

		private void ImportSettings() {
			_logger.Log("Начало импорта настроек");
			throw new NotImplementedException("Not implemented yet");
		}

		private void ExportSettings() {
			_logger.Log("Начало экспорта настроек");
			throw new NotImplementedException("Not implemented yet");
		}

		private void WriteSettings() {
			try {
				_logger.Log("Подготовка к записи настроек АИН");
				IAinSettings ainSettings;
				try {
				ainSettings = new AinSettingsSimple(
					BytesPair.FromSignedShortLowFirst(0), // TODO:
					KpW.Value,
					KiW.Value,
					FiNom.Value,
					Imax.Value,
					UdcMax.Value,
					UdcMin.Value,
					Fnom.Value,
					Fmax.Value,
					Empty10.Value,
					Empty11.Value,
					IoutMax.Value,
					FiMin.Value,
					DacCh.Value,
					Imcw.Value,
					Ia0.Value,
					Ib0.Value,
					Ic0.Value,
					Udc0.Value,
					TauR.Value,
					Lm.Value,
					Lsl.Value,
					Lrl.Value,
					BytesPair.FromSignedShortLowFirst(0), // TODO:
					KpFi.Value,
					KiFi.Value,
					BytesPair.FromSignedShortLowFirst(0), // TODO:
					KpId.Value,
					KiId.Value,
					BytesPair.FromSignedShortLowFirst(0), // TODO:
					KpIq.Value,
					KiIq.Value,
					AccDfDt.Value,
					DecDfDt.Value,
					Unom.Value,
					Empty39.Value,
					Rs.Value,
					Fmin.Value,
					TauM.Value,
					TauF.Value,
					TauFSet.Value,
					TauFi.Value,
					IdSetMin.Value,
					IdSetMax.Value,
					BytesPair.FromSignedShortLowFirst(0), // TODO:
					BytesPair.FromSignedShortLowFirst((short)(UchMin.Value / 65536.0)),
					BytesPair.FromSignedShortLowFirst(0), // TODO:
					BytesPair.FromSignedShortLowFirst((short)(UchMax.Value / 65536.0)),
					Np.Value,
					
					0, // TODO: NimpFloorCode
					AinTelemetryFanWorkmode.AllwaysOff, // TODO: FanMode
					
					Empty53.Value,
					EmdecDfdt.Value,
					TextMax.Value,
					ToHl.Value, false, false, false);
				}
				catch (Exception ex)
				{
					throw new Exception("убедитесь, что все значения настроек заполнены", ex);
				}
				var cmd = new WriteAinSettingsCommand(_zeroBasedAinNumber, ainSettings);

				_logger.Log("Команда записи настроек АИН" + (_zeroBasedAinNumber + 1) + " поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() =>
					{
						try
						{
							if (exception != null)
							{
								throw new Exception("ошибка при передаче данных: " + exception.Message, exception);
							}

							try
							{
								var result = cmd.GetResult(bytes);
								if (result) {
									_logger.Log("Настройки АИН" + (_zeroBasedAinNumber + 1) + " успешно записаны");
								}
								else {
									throw new Exception("странно, флаг записи результата = False");
								}
							}
							catch (Exception exx)
							{
								// TODO: log exception about error on answer parsing
								throw new Exception("ошибка при разборе ответа: " + exx.Message, exx);
							}
						}
						catch (Exception ex)
						{
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду записи настроек БС-Ethernet в очередь: " + ex.Message);
			}
		}

		private void ReadSettings() {
			try
			{
				_logger.Log("Подготовка к чтению настроек АИН");

				var cmd = new ReadAinSettingsCommand(_zeroBasedAinNumber);

				_logger.Log("Команда чтения настроек АИН" + (_zeroBasedAinNumber + 1) + " поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() =>
					{
						try
						{
							if (exception != null)
							{
								throw new Exception("ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes);
								_userInterfaceRoot.Notifier.Notify(() => {
									KpW = result.KpW;
									KiW = result.KiW;
									FiNom = result.FiNom;
									Imax = result.Imax;
									UdcMax = result.UdcMax;
									UdcMin = result.UdcMin;
									Fnom = result.Fnom;
									Fmax = result.Fmax;
									Empty10 = result.DflLim;
									Empty11 = result.FlMinMin;
									IoutMax = result.IoutMax;
									FiMin = result.FiMin;
									DacCh = result.DacCh;
									Imcw = result.Imcw;
									Ia0 = result.Ia0;
									Ib0 = result.Ib0;
									Ic0 = result.Ic0;
									Udc0 = result.Udc0;
									TauR = result.TauR;
									Lm = result.Lm;
									Lsl = result.Lsl;
									Lrl = result.Lrl;
									KpFi = result.KpFi;
									KiFi = result.KiFi;
									KpId = result.KpId;
									KiId = result.KiId;
									KpIq = result.KpIq;
									KiIq = result.KiIq;
									AccDfDt = result.AccDfDt;
									DecDfDt = result.DecDfDt;
									Unom = result.Unom;
									Empty39 = result.TauFlLim;
									Rs = result.Rs;
									Fmin = result.Fmin;
									TauM = result.TauM;
									TauF = result.TauF;
									TauFSet = result.TauFSet;
									TauFi = result.TauFi;
									IdSetMin = result.IdSetMin;
									IdSetMax = result.IdSetMax;
									UchMin = result.UchMin.LowFirstSignedValue;
									UchMax = result.UchMax.LowFirstSignedValue;
									Np = (short)result.Np;
									// TODO: nimp, fanmode

									Empty53 = result.UmodThr;
									EmdecDfdt = result.EmdecDfdt;
									TextMax = result.TextMax;
									ToHl = result.ToHl;
								});
								_logger.Log("Настройки АИН" + (_zeroBasedAinNumber + 1) + " успешно прочитаны");
							}
							catch (Exception exx)
							{
								// TODO: log exception about error on answer parsing
								throw new Exception("ошибка при разборе ответа: " + exx.Message, exx);
							}
						}
						catch (Exception ex)
						{
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex)
			{
				_logger.Log("Не удалось поставить команду чтения настроек БС-Ethernet в очередь: " + ex.Message);
			}
		}


		public ICommand ReadSettingsCommand => _readSettingsCommand;

		public ICommand WriteSettingsCommand => _writeSettingsCommand;

		public ICommand ImportSettingsCommand { get; }

		public ICommand ExportSettingsCommand { get; }

		public double? KpW {
			get { return _kpW; }
			set { if (_kpW != value) { _kpW = value; RaisePropertyChanged(() => KpW); } }
		}

		public double? KiW {
			get { return _kiW; }
			set { if (_kiW != value) { _kiW = value; RaisePropertyChanged(() => KiW); } }
		}

		public short? FiNom {
			get { return _fiNom; }
			set { if (_fiNom != value) { _fiNom = value; RaisePropertyChanged(() => FiNom); } }
		}

		public short? Imax {
			get { return _imax; }
			set { if (_imax != value) { _imax = value; RaisePropertyChanged(() => Imax); } }
		}

		public short? UdcMax {
			get { return _udcMax; }
			set { if (_udcMax != value) { _udcMax = value; RaisePropertyChanged(() => UdcMax); } }
		}

		public short? UdcMin {
			get { return _udcMin; }
			set { if (_udcMin != value) { _udcMin = value; RaisePropertyChanged(() => UdcMin); } }
		}

		public double? Fnom {
			get { return _fnom; }
			set { if (_fnom != value) { _fnom = value; RaisePropertyChanged(() => Fnom); } }
		}

		public double? Fmax {
			get { return _fmax; }
			set { if (_fmax != value) { _fmax = value; RaisePropertyChanged(() => Fmax); } }
		}

		public short? Empty10
		{
			get { return _empty10; }
			set { if (_empty10 != value) { _empty10 = value; RaisePropertyChanged(() => Empty10); } }
		}

		public short? Empty11
		{
			get { return _empty11; }
			set { if (_empty11 != value) { _empty11 = value; RaisePropertyChanged(() => Empty11); } }
		}

		public short? IoutMax {
			get { return _ioutMax; }
			set { if (_ioutMax != value) { _ioutMax = value; RaisePropertyChanged(() => IoutMax); } }
		}

		public short? FiMin {
			get { return _fiMin; }
			set { if (_fiMin != value) { _fiMin = value; RaisePropertyChanged(() => FiMin); } }
		}

		public short? DacCh {
			get { return _dacCh; }
			set { if (_dacCh != value) { _dacCh = value; RaisePropertyChanged(() => DacCh); } }
		}

		public short? Imcw {
			get { return _imcw; }
			set { if (_imcw != value) { _imcw = value; RaisePropertyChanged(() => Imcw); } }
		}

		public short? Ia0 {
			get { return _ia0; }
			set { if (_ia0 != value) { _ia0 = value; RaisePropertyChanged(() => Ia0); } }
		}

		public short? Ib0 {
			get { return _ib0; }
			set { if (_ib0 != value) { _ib0 = value; RaisePropertyChanged(() => Ib0); } }
		}

		public short? Ic0 {
			get { return _ic0; }
			set { if (_ic0 != value) { _ic0 = value; RaisePropertyChanged(() => Ic0); } }
		}

		public short? Udc0 {
			get { return _udc0; }
			set { if (_udc0 != value) { _udc0 = value; RaisePropertyChanged(() => Udc0); } }
		}

		public double? TauR {
			get { return _tauR; }
			set { if (_tauR != value) { _tauR = value; RaisePropertyChanged(() => TauR); } }
		}
		public double? Lm {
			get { return _lm; }
			set { if (_lm != value) { _lm = value; RaisePropertyChanged(() => Lm); } }
		}
		public double? Lsl {
			get { return _lsl; }
			set { if (_lsl != value) { _lsl = value; RaisePropertyChanged(() => Lsl); } }
		}
		public double? Lrl {
			get { return _lrl; }
			set { if (_lrl != value) { _lrl = value; RaisePropertyChanged(() => Lrl); } }
		}

		public double? KpFi {
			get { return _kpFi; }
			set { if (_kpFi != value) { _kpFi = value; RaisePropertyChanged(() => KpFi); } }
		}

		public double? KiFi {
			get { return _kiFi; }
			set { if (_kiFi != value) { _kiFi = value; RaisePropertyChanged(() => KiFi); } }
		}

		public double? KpId {
			get { return _kpId; }
			set { if (_kpId != value) { _kpId = value; RaisePropertyChanged(() => KpId); } }
		}

		public double? KiId {
			get { return _kiId; }
			set { if (_kiId != value) { _kiId = value; RaisePropertyChanged(() => KiId); } }
		}

		public double? KpIq {
			get { return _kpIq; }
			set { if (_kpIq != value) { _kpIq = value; RaisePropertyChanged(() => KpIq); } }
		}

		public double? KiIq {
			get { return _kiIq; }
			set { if (_kiIq != value) { _kiIq = value; RaisePropertyChanged(() => KiIq); } }
		}

		public short? AccDfDt {
			get { return _accDfDt; }
			set { if (_accDfDt != value) { _accDfDt = value; RaisePropertyChanged(() => AccDfDt); } }
		}

		public short? DecDfDt {
			get { return _decDfDt; }
			set { if (_decDfDt != value) { _decDfDt = value; RaisePropertyChanged(() => DecDfDt); } }
		}

		public double? Unom {
			get { return _unom; }
			set { if (_unom != value) { _unom = value; RaisePropertyChanged(() => Unom); } }
		}

		public short? Empty39
		{
			get { return _empty39; }
			set { if (_empty39 != value) { _empty39 = value; RaisePropertyChanged(() => Empty39); } }
		}

		public double? Rs {
			get { return _rs; }
			set { if (_rs != value) { _rs = value; RaisePropertyChanged(() => Rs); } }
		}

		public double? Fmin {
			get { return _fmin; }
			set { if (_fmin != value) { _fmin = value; RaisePropertyChanged(() => Fmin); } }
		}

		public short? TauM {
			get { return _tauM; }
			set { if (_tauM != value) { _tauM = value; RaisePropertyChanged(() => TauM); } }
		}

		public short? TauF {
			get { return _tauF; }
			set { if (_tauF != value) { _tauF = value; RaisePropertyChanged(() => TauF); } }
		}

		public short? TauFSet {
			get { return _tauFSet; }
			set { if (_tauFSet != value) { _tauFSet = value; RaisePropertyChanged(() => TauFSet); } }
		}

		public short? TauFi {
			get { return _tauFi; }
			set { if (_tauFi != value) { _tauFi = value; RaisePropertyChanged(() => TauFi); } }
		}

		public short? IdSetMin {
			get { return _idSetMin; }
			set { if (_idSetMin != value) { _idSetMin = value; RaisePropertyChanged(() => IdSetMin); } }
		}

		public short? IdSetMax {
			get { return _idSetMax; }
			set { if (_idSetMax != value) { _idSetMax = value; RaisePropertyChanged(() => IdSetMax); } }
		}

		public short? UchMin {
			get { return _uchMin; }
			set { if (_uchMin != value) { _uchMin = value; RaisePropertyChanged(() => UchMin); } }
		}

		public short? UchMax {
			get { return _uchMax; }
			set { if (_uchMax != value) { _uchMax = value; RaisePropertyChanged(() => UchMax); } }
		}

		public short? Np {
			get { return _np; }
			set { if (_np != value) { _np = value; RaisePropertyChanged(() => Np); } }
		}

		public short? Empty53
		{
			get { return _empty53; }
			set { if (_empty53 != value) { _empty53 = value; RaisePropertyChanged(() => Empty53); } }
		}

		public short? EmdecDfdt {
			get { return _emdecDfdt; }
			set { if (_emdecDfdt != value) { _emdecDfdt = value; RaisePropertyChanged(() => EmdecDfdt); } }
		}

		public short? TextMax {
			get { return _textMax; }
			set { if (_textMax != value) { _textMax = value; RaisePropertyChanged(() => TextMax); } }
		}

		public short? ToHl {
			get { return _toHl; }
			set { if (_toHl != value) { _toHl = value; RaisePropertyChanged(() => ToHl); } }
		}
	}
}
