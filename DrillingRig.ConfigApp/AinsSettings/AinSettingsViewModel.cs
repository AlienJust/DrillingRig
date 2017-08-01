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

namespace DrillingRig.ConfigApp.AinsSettings {
	class AinSettingsViewModel : ViewModelBase {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;

		private readonly RelayCommand _readSettingsCommand;
		private readonly RelayCommand _writeSettingsCommand;

		private ushort? _reserved00;
		private decimal? _kpW;
		private decimal? _kiW;

		private decimal? _fiNom;
		private short? _imax;
		private short? _udcMax;
		private short? _udcMin;
		private decimal? _fnom;
		private decimal? _fmax;

		private decimal? _dflLim; // DflLim
		private decimal? _flMinMin; // FlMinMin

		private short? _ioutMax;
		private decimal? _fiMin;
		private ushort? _dacCh;
		private ushort? _imcw;

		private short? _ia0;
		private short? _ib0;
		private short? _ic0;
		private short? _udc0;

		private decimal? _tauR;
		private decimal? _lm;
		private decimal? _lsl;
		private decimal? _lrl;

		private ushort? _reserved24;
		private decimal? _kpFi;
		private decimal? _kiFi;

		private ushort? _reserved28;
		private decimal? _kpId;
		private decimal? _kiId;

		private ushort? _reserved32;
		private decimal? _kpIq;
		private decimal? _kiIq;

		private decimal? _accDfDt;
		private decimal? _decDfDt;
		private decimal? _unom;
		private int? _unomd;

		private decimal? _tauFlLim; // tauflim

		private decimal? _rs;
		private decimal? _fmin;

		private decimal? _tauM;
		private decimal? _tauF;
		private decimal? _tauFSet;
		private decimal? _tauFi;

		private short? _idSetMin;
		private short? _idSetMax;

		private short? _uchMin;
		private short? _uchMax;

		private ushort? _reserved50;
		private ushort? _reserved51;

		private short? _np;
		private int? _nimpFloorCode;
		private int? _fanMode;
		private bool? _directCurrentMagnetization;

		private decimal? _umodThr; // UmodThr

		private decimal? _emdecDfdt;

		private short? _textMax;
		private short? _toHl;

		private readonly byte _zeroBasedAinNumber;

		public AinSettingsViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl, byte zeroBasedAinNumber) {
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

			Reserved00 = null; // 0
			KpW = null; // 1
			KiW = null; // 2 3
			FiNom = null; // 4
			Imax = null; // 5
			UdcMax = null; // 6
			UdcMin = null; // 7
			Fnom = null;// 8
			Fmax = null; // 9

			DflLim = null; // 10
			FlMinMin = null; // 11

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

			Reserved24 = null; // 24
			KpFi = null; // 25
			KiFi = null; // 26-27

			Reserved28 = null; // 28
			KpId = null; // 29
			KiId = null; // 30-31

			Reserved32 = null; // 32
			KpIq = null; // 33
			KiIq = null; // 34-35

			AccDfDt = null; // 36
			DecDfDt = null; // 37
			Unom = null; // 38
			UnomD = null;

			TauFlLim = null; // 39

			Rs = null; // 40
			Fmin = null; // 41

			TauM = null; // 42
			TauF = null; // 43
			TauFSet = null; // 44
			TauFi = null; // 45

			IdSetMin = null; // 46
			IdSetMax = null; // 47

			UchMin = null; // 48
			UchMax = null; // 49

			Reserved50 = null; // 50
			Reserved51 = null; // 51

			Np = null; // 52.0-4
			NimpFloorCode = null; // 52.5-7
			FanMode = null; //52.8-15

			UmodThr = null; // 53

			EmdecDfdt = null; // 54
			TextMax = null; // 55
			ToHl = null; // 56


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
						BytesPair.FromUnsignedShortLowFirst(Reserved00.Value),
						KpW.Value,
						KiW.Value,
						FiNom.Value,
						Imax.Value,
						UdcMax.Value,
						UdcMin.Value,
						Fnom.Value,
						Fmax.Value,
						DflLim.Value,
						FlMinMin.Value,
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
						BytesPair.FromUnsignedShortLowFirst(Reserved24.Value),
						KpFi.Value,
						KiFi.Value,
						BytesPair.FromUnsignedShortLowFirst(Reserved28.Value),
						KpId.Value,
						KiId.Value,
						BytesPair.FromUnsignedShortLowFirst(Reserved32.Value),
						KpIq.Value,
						KiIq.Value,
						AccDfDt.Value,
						DecDfDt.Value,
						Unom.Value,
						TauFlLim.Value,
						Rs.Value,
						Fmin.Value,
						TauM.Value,
						TauF.Value,
						TauFSet.Value,
						TauFi.Value,
						IdSetMin.Value,
						IdSetMax.Value,
						BytesPair.FromSignedShortLowFirst((short)(UchMin.Value / 65536.0)),
						BytesPair.FromSignedShortLowFirst((short)(UchMax.Value / 65536.0)),
						BytesPair.FromUnsignedShortLowFirst(Reserved50.Value),
						BytesPair.FromUnsignedShortLowFirst(Reserved51.Value),

						Np.Value,
						NimpFloorCode.Value,
						AinTelemetryFanWorkmodeExtensions.FromIoBits(FanMode.Value),
						DirectCurrentMagnetization.Value,

						UmodThr.Value,
						EmdecDfdt.Value,
						TextMax.Value,
						ToHl.Value, false, false, false);
				}
				catch (Exception ex) {
					throw new Exception("убедитесь, что все значения настроек заполнены", ex);
				}
				var cmd = new WriteAinSettingsCommand(_zeroBasedAinNumber, ainSettings);

				_logger.Log("Команда записи настроек АИН" + (_zeroBasedAinNumber + 1) + " поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(0.3), 2
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes);
								if (result) {
									_logger.Log("Настройки АИН" + (_zeroBasedAinNumber + 1) + " успешно записаны");
								}
								else {
									throw new Exception("странно, флаг записи результата = False");
								}
							}
							catch (Exception exx) {
								// TODO: log exception about error on answer parsing
								throw new Exception("ошибка при разборе ответа: " + exx.Message, exx);
							}
						}
						catch (Exception ex) {
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду записи настроек БС-Ethernet в очередь: " + ex.Message);
			}
		}

		private void ReadSettings() {
			try {
				_logger.Log("Подготовка к чтению настроек АИН");

				var cmd = new ReadAinSettingsCommand(_zeroBasedAinNumber);

				_logger.Log("Команда чтения настроек АИН" + (_zeroBasedAinNumber + 1) + " поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(0.3), 2
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("ошибка при передаче данных: " + exception.Message, exception);
							}

							try {
								var result = cmd.GetResult(bytes);
								_userInterfaceRoot.Notifier.Notify(() => {
									Reserved00 = result.Reserved00.LowFirstUnsignedValue;
									KpW = result.KpW;
									KiW = result.KiW;
									FiNom = result.FiNom;
									Imax = result.Imax;
									UdcMax = result.UdcMax;
									UdcMin = result.UdcMin;
									Fnom = result.Fnom;
									Fmax = result.Fmax;
									DflLim = result.DflLim;
									FlMinMin = result.FlMinMin;
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
									Reserved24 = result.Reserved24.LowFirstUnsignedValue;
									KpFi = result.KpFi;
									KiFi = result.KiFi;
									Reserved28 = result.Reserved28.LowFirstUnsignedValue;
									KpId = result.KpId;
									KiId = result.KiId;
									Reserved32 = result.Reserved32.LowFirstUnsignedValue;
									KpIq = result.KpIq;
									KiIq = result.KiIq;
									AccDfDt = result.AccDfDt;
									DecDfDt = result.DecDfDt;
									Unom = result.Unom;
									TauFlLim = result.TauFlLim;
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

									Reserved50 = result.Reserved50.LowFirstUnsignedValue;
									Reserved51 = result.Reserved51.LowFirstUnsignedValue;

									Np = (short)result.Np;
									NimpFloorCode = result.NimpFloorCode;
									FanMode = result.FanMode.ToIoBits();
									DirectCurrentMagnetization = result.DirectCurrentMagnetization;

									UmodThr = result.UmodThr;
									EmdecDfdt = result.EmdecDfdt;
									TextMax = result.TextMax;
									ToHl = result.ToHl;
								});
								_logger.Log("Настройки АИН" + (_zeroBasedAinNumber + 1) + " успешно прочитаны");
							}
							catch (Exception exx) {
								// TODO: log exception about error on answer parsing
								throw new Exception("ошибка при разборе ответа: " + exx.Message, exx);
							}
						}
						catch (Exception ex) {
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду чтения настроек БС-Ethernet в очередь: " + ex.Message);
			}
		}


		public ICommand ReadSettingsCommand => _readSettingsCommand;

		public ICommand WriteSettingsCommand => _writeSettingsCommand;

		public ICommand ImportSettingsCommand { get; }

		public ICommand ExportSettingsCommand { get; }



		public ushort? Reserved00 {
			get => _reserved00;
			set { if (_reserved00 != value) { _reserved00 = value; RaisePropertyChanged(() => Reserved00); } }
		}

		public decimal? KpW {
			get => _kpW;
			set { if (_kpW != value) { _kpW = value; RaisePropertyChanged(() => KpW); } }
		}


		public decimal? KiW {
			get => _kiW;
			set { if (_kiW != value) { _kiW = value; RaisePropertyChanged(() => KiW); } }
		}

		public decimal? FiNom {
			get => _fiNom;
			set { if (_fiNom != value) { _fiNom = value; RaisePropertyChanged(() => FiNom); } }
		}

		public short? Imax {
			get => _imax;
			set { if (_imax != value) { _imax = value; RaisePropertyChanged(() => Imax); } }
		}

		public short? UdcMax {
			get => _udcMax;
			set { if (_udcMax != value) { _udcMax = value; RaisePropertyChanged(() => UdcMax); } }
		}

		public short? UdcMin {
			get => _udcMin;
			set { if (_udcMin != value) { _udcMin = value; RaisePropertyChanged(() => UdcMin); } }
		}

		public decimal? Fnom {
			get => _fnom;
			set { if (_fnom != value) { _fnom = value; RaisePropertyChanged(() => Fnom); } }
		}

		public decimal? Fmax {
			get => _fmax;
			set { if (_fmax != value) { _fmax = value; RaisePropertyChanged(() => Fmax); } }
		}

		public decimal? DflLim {
			get => _dflLim;
			set { if (_dflLim != value) { _dflLim = value; RaisePropertyChanged(() => DflLim); } }
		}

		public decimal? FlMinMin {
			get => _flMinMin;
			set { if (_flMinMin != value) { _flMinMin = value; RaisePropertyChanged(() => FlMinMin); } }
		}

		public short? IoutMax {
			get => _ioutMax;
			set { if (_ioutMax != value) { _ioutMax = value; RaisePropertyChanged(() => IoutMax); } }
		}

		public decimal? FiMin {
			get => _fiMin;
			set { if (_fiMin != value) { _fiMin = value; RaisePropertyChanged(() => FiMin); } }
		}

		public ushort? DacCh {
			get => _dacCh;
			set { if (_dacCh != value) { _dacCh = value; RaisePropertyChanged(() => DacCh); } }
		}

		public ushort? Imcw {
			get => _imcw;
			set { if (_imcw != value) { _imcw = value; RaisePropertyChanged(() => Imcw); } }
		}

		public short? Ia0 {
			get => _ia0;
			set { if (_ia0 != value) { _ia0 = value; RaisePropertyChanged(() => Ia0); } }
		}

		public short? Ib0 {
			get => _ib0;
			set { if (_ib0 != value) { _ib0 = value; RaisePropertyChanged(() => Ib0); } }
		}

		public short? Ic0 {
			get => _ic0;
			set { if (_ic0 != value) { _ic0 = value; RaisePropertyChanged(() => Ic0); } }
		}

		public short? Udc0 {
			get => _udc0;
			set { if (_udc0 != value) { _udc0 = value; RaisePropertyChanged(() => Udc0); } }
		}

		public decimal? TauR {
			get => _tauR;
			set { if (_tauR != value) { _tauR = value; RaisePropertyChanged(() => TauR); } }
		}
		public decimal? Lm {
			get => _lm;
			set { if (_lm != value) { _lm = value; RaisePropertyChanged(() => Lm); } }
		}
		public decimal? Lsl {
			get => _lsl;
			set { if (_lsl != value) { _lsl = value; RaisePropertyChanged(() => Lsl); } }
		}
		public decimal? Lrl {
			get => _lrl;
			set { if (_lrl != value) { _lrl = value; RaisePropertyChanged(() => Lrl); } }
		}


		public ushort? Reserved24 {
			get => _reserved24;
			set { if (_reserved24 != value) { _reserved24 = value; RaisePropertyChanged(() => Reserved24); } }
		}

		public decimal? KpFi {
			get => _kpFi;
			set { if (_kpFi != value) { _kpFi = value; RaisePropertyChanged(() => KpFi); } }
		}

		public decimal? KiFi {
			get => _kiFi;
			set { if (_kiFi != value) { _kiFi = value; RaisePropertyChanged(() => KiFi); } }
		}

		public ushort? Reserved28 {
			get => _reserved28;
			set { if (_reserved28 != value) { _reserved28 = value; RaisePropertyChanged(() => Reserved28); } }
		}

		public decimal? KpId {
			get => _kpId;
			set { if (_kpId != value) { _kpId = value; RaisePropertyChanged(() => KpId); } }
		}

		public decimal? KiId {
			get => _kiId;
			set { if (_kiId != value) { _kiId = value; RaisePropertyChanged(() => KiId); } }
		}

		public ushort? Reserved32 {
			get => _reserved32;
			set { if (_reserved32 != value) { _reserved32 = value; RaisePropertyChanged(() => Reserved32); } }
		}

		public decimal? KpIq {
			get => _kpIq;
			set { if (_kpIq != value) { _kpIq = value; RaisePropertyChanged(() => KpIq); } }
		}

		public decimal? KiIq {
			get => _kiIq;
			set { if (_kiIq != value) { _kiIq = value; RaisePropertyChanged(() => KiIq); } }
		}

		public decimal? AccDfDt {
			get => _accDfDt;
			set { if (_accDfDt != value) { _accDfDt = value; RaisePropertyChanged(() => AccDfDt); } }
		}

		public decimal? DecDfDt {
			get => _decDfDt;
			set { if (_decDfDt != value) { _decDfDt = value; RaisePropertyChanged(() => DecDfDt); } }
		}

		public decimal? Unom {
			get => _unom;
			set {
				if (_unom != value) {
					if (value == null) {
						_unom = null;
						_unomd = null;
					}
					else {
						_unom = value;
						_unomd = (int)Math.Round(_unom.Value * (decimal)Math.Sqrt(2.0));
					}
					RaisePropertyChanged(() => Unom);
					RaisePropertyChanged(() => UnomD);

				}
			}
		}

		public int? UnomD {
			get => _unomd;
			set {
				if (_unomd != value) {
					if (value == null) {
						_unom = null;
						_unomd = null;
					}
					else {
						_unomd = value;
						_unom = value / (decimal)Math.Sqrt(2.0);
					}
					RaisePropertyChanged(() => Unom);
					RaisePropertyChanged(() => Unom);
				}
			}
		}

		public decimal? TauFlLim {
			get => _tauFlLim;
			set { if (_tauFlLim != value) { _tauFlLim = value; RaisePropertyChanged(() => TauFlLim); } }
		}

		public decimal? Rs {
			get => _rs;
			set { if (_rs != value) { _rs = value; RaisePropertyChanged(() => Rs); } }
		}

		public decimal? Fmin {
			get => _fmin;
			set { if (_fmin != value) { _fmin = value; RaisePropertyChanged(() => Fmin); } }
		}

		public decimal? TauM {
			get => _tauM;
			set { if (_tauM != value) { _tauM = value; RaisePropertyChanged(() => TauM); } }
		}

		public decimal? TauF {
			get => _tauF;
			set { if (_tauF != value) { _tauF = value; RaisePropertyChanged(() => TauF); } }
		}

		public decimal? TauFSet {
			get => _tauFSet;
			set { if (_tauFSet != value) { _tauFSet = value; RaisePropertyChanged(() => TauFSet); } }
		}

		public decimal? TauFi {
			get => _tauFi;
			set { if (_tauFi != value) { _tauFi = value; RaisePropertyChanged(() => TauFi); } }
		}

		public short? IdSetMin {
			get => _idSetMin;
			set { if (_idSetMin != value) { _idSetMin = value; RaisePropertyChanged(() => IdSetMin); } }
		}

		public short? IdSetMax {
			get => _idSetMax;
			set { if (_idSetMax != value) { _idSetMax = value; RaisePropertyChanged(() => IdSetMax); } }
		}

		public short? UchMin {
			get => _uchMin;
			set { if (_uchMin != value) { _uchMin = value; RaisePropertyChanged(() => UchMin); } }
		}

		public short? UchMax {
			get => _uchMax;
			set { if (_uchMax != value) { _uchMax = value; RaisePropertyChanged(() => UchMax); } }
		}

		public ushort? Reserved50 {
			get => _reserved50;
			set { if (_reserved50 != value) { _reserved50 = value; RaisePropertyChanged(() => Reserved50); } }
		}
		public ushort? Reserved51 {
			get => _reserved51;
			set { if (_reserved51 != value) { _reserved51 = value; RaisePropertyChanged(() => Reserved51); } }
		}

		public short? Np {
			get => _np;
			set { if (_np != value) { _np = value; RaisePropertyChanged(() => Np); } }
		}

		public int? NimpFloorCode {
			get => _nimpFloorCode;
			set { if (_nimpFloorCode != value) { _nimpFloorCode = value; RaisePropertyChanged(() => NimpFloorCode); } }
		}

		public int? FanMode {
			get => _fanMode;
			set { if (_fanMode != value) { _fanMode = value; RaisePropertyChanged(() => FanMode); } }
		}

		public bool? DirectCurrentMagnetization {
			get => _directCurrentMagnetization;
			set {
				if (_directCurrentMagnetization != value) {
					_directCurrentMagnetization = value;
					RaisePropertyChanged(() => DirectCurrentMagnetization);
				}
			}
		}

		public decimal? UmodThr {
			get => _umodThr;
			set { if (_umodThr != value) { _umodThr = value; RaisePropertyChanged(() => UmodThr); } }
		}

		public decimal? EmdecDfdt {
			get => _emdecDfdt;
			set { if (_emdecDfdt != value) { _emdecDfdt = value; RaisePropertyChanged(() => EmdecDfdt); } }
		}

		public short? TextMax {
			get => _textMax;
			set { if (_textMax != value) { _textMax = value; RaisePropertyChanged(() => TextMax); } }
		}

		public short? ToHl {
			get => _toHl;
			set { if (_toHl != value) { _toHl = value; RaisePropertyChanged(() => ToHl); } }
		}


	}
}
