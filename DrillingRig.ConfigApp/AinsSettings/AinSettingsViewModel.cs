using System;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.AinSettings;

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
		private readonly ICommand _importSettingCommand;
		private readonly ICommand _exportSettingsCommand;

		private int? _kpW;
		private int? _kiW;
		private short? _fiNom;
		private short? _imax;
		private short? _udcMax;
		private short? _udcMin;
		private short? _fnom;
		private short? _fmax;
		private short? _ioutMax;
		private short? _fiMin;
		private short? _dacCh;
		private short? _imcw;
		private short? _ia0;
		private short? _ib0;
		private short? _ic0;
		private short? _udc0;
		private short? _tauR;
		private short? _lm;
		private short? _lsl;
		private short? _lrl;
		private int? _kpFi;
		private int? _kiFi;
		private int? _kpId;
		private int? _kiId;
		private int? _kpIq;
		private int? _kiIq;
		private short? _accDfDt;
		private short? _decDfDt;
		private short? _unom;
		private short? _rs;
		private short? _fmin;
		private short? _tauM;
		private short? _tauF;
		private short? _tauFSet;
		private short? _tauFi;
		private short? _idSetMin;
		private short? _idSetMax;
		private int? _kpFe;
		private int? _kiFe;
		private short? _np;
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

			_importSettingCommand = new RelayCommand(ImportSettings);
			_exportSettingsCommand = new RelayCommand(ExportSettings);


			KpW = null;
			KiW = null;
			FiNom = null;
			Imax = null;
			UdcMax = null;
			UdcMin = null;
			Fnom = null;
			Fmax = null;
			IoutMax = null;
			FiMin = null;
			DacCh = null;
			Imcw = null;
			Ia0 = null;
			Ib0 = null;
			Ic0 = null;
			Udc0 = null;
			TauR = null;
			Lm = null;
			Lsl = null;
			Lrl = null;
			KpFi = null;
			KiFi = null;
			KpId = null;
			KiId = null;
			KpIq = null;
			KiIq = null;
			AccDfDt = null;
			DecDfDt = null;
			Unom = null;
			Rs = null;
			Fmin = null;
			TauM = null;
			TauF = null;
			TauFSet = null;
			TauFi = null;
			IdSetMin = null;
			IdSetMax = null;
			KpFe = null;
			KiFe = null;
			Np = null;
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
				throw new NotImplementedException("Not implemented yet");
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

				_logger.Log("Команда чтения настроек БС-Ethernet поставлена в очередь");
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
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
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
									Rs = result.Rs;
									Fmin = result.Fmin;
									TauM = result.TauM;
									TauF = result.TauF;
									TauFSet = result.TauFSet;
									TauFi = result.TauFi;
									IdSetMin = result.IdSetMin;
									IdSetMax = result.IdSetMax;
									KpFe = result.KpFe;
									KiFe = result.KiFe;
									Np = result.Np;
									EmdecDfdt = result.EmdecDfdt;
									TextMax = result.TextMax;
									ToHl = result.ToHl;
								});
								_logger.Log("Настройки БС-Ethernet успешно прочитаны");
							}
							catch (Exception exx)
							{
								// TODO: log exception about error on answer parsing
								throw new Exception("Ошибка при разборе ответа: " + exx.Message, exx);
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


		public ICommand ReadSettingsCommand {
			get { return _readSettingsCommand; }
		}

		public ICommand WriteSettingsCommand
		{
			get { return _writeSettingsCommand; }
		}

		public ICommand ImportSettingsCommand
		{
			get { return _importSettingCommand; }
		}

		public ICommand ExportSettingsCommand
		{
			get { return _exportSettingsCommand; }
		}

		public int? KpW {
			get { return _kpW; }
			set { if (_kpW != value) { _kpW = value; RaisePropertyChanged(() => KpW); } }
		}

		public int? KiW {
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

		public short? Fnom {
			get { return _fnom; }
			set { if (_fnom != value) { _fnom = value; RaisePropertyChanged(() => Fnom); } }
		}

		public short? Fmax {
			get { return _fmax; }
			set { if (_fmax != value) { _fmax = value; RaisePropertyChanged(() => Fmax); } }
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

		public short? TauR {
			get { return _tauR; }
			set { if (_tauR != value) { _tauR = value; RaisePropertyChanged(() => TauR); } }
		}

		public short? Lm {
			get { return _lm; }
			set { if (_lm != value) { _lm = value; RaisePropertyChanged(() => Lm); } }
		}

		public short? Lsl {
			get { return _lsl; }
			set { if (_lsl != value) { _lsl = value; RaisePropertyChanged(() => Lsl); } }
		}

		public short? Lrl {
			get { return _lrl; }
			set { if (_lrl != value) { _lrl = value; RaisePropertyChanged(() => Lrl); } }
		}

		public int? KpFi {
			get { return _kpFi; }
			set { if (_kpFi != value) { _kpFi = value; RaisePropertyChanged(() => KpFi); } }
		}

		public int? KiFi {
			get { return _kiFi; }
			set { if (_kiFi != value) { _kiFi = value; RaisePropertyChanged(() => KiFi); } }
		}

		public int? KpId {
			get { return _kpId; }
			set { if (_kpId != value) { _kpId = value; RaisePropertyChanged(() => KpId); } }
		}

		public int? KiId {
			get { return _kiId; }
			set { if (_kiId != value) { _kiId = value; RaisePropertyChanged(() => KiId); } }
		}

		public int? KpIq {
			get { return _kpIq; }
			set { if (_kpIq != value) { _kpIq = value; RaisePropertyChanged(() => KpIq); } }
		}

		public int? KiIq {
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

		public short? Unom {
			get { return _unom; }
			set { if (_unom != value) { _unom = value; RaisePropertyChanged(() => Unom); } }
		}

		public short? Rs {
			get { return _rs; }
			set { if (_rs != value) { _rs = value; RaisePropertyChanged(() => Rs); } }
		}

		public short? Fmin {
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

		public int? KpFe {
			get { return _kpFe; }
			set { if (_kpFe != value) { _kpFe = value; RaisePropertyChanged(() => KpFe); } }
		}

		public int? KiFe {
			get { return _kiFe; }
			set { if (_kiFe != value) { _kiFe = value; RaisePropertyChanged(() => KiFe); } }
		}

		public short? Np {
			get { return _np; }
			set { if (_np != value) { _np = value; RaisePropertyChanged(() => Np); } }
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
