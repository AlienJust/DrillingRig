using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.SystemControl;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DrillingRig.ConfigApp.SystemControl {
	internal class SystemControlViewModel : ViewModelBase, IDebugInformationShower, INamedTrendsControl {
		private readonly ICommandSenderHost _commandSenderHost;
		private readonly ITargetAddressHost _targerAddressHost;
		private readonly IUserInterfaceRoot _userInterfaceRoot;
		private readonly ILogger _logger;
		private readonly IWindowSystem _windowSystem;
		private readonly INotifySendingEnabled _sendingEnabledControl;
		private readonly ILinkContol _linkControl;

		private readonly RelayCommand _cmdSetBootloader;
		private readonly RelayCommand _cmdRestart;
		private readonly RelayCommand _cmdFlash;

		private IList<byte> _debugBytes;
		private readonly ObservableCollection<DataPoint> _points1;
		private readonly ObservableCollection<DataPoint> _points2;
		private readonly ObservableCollection<DataPoint> _points3;
		private readonly ObservableCollection<DataPoint> _points4;
		private double _gridPlotHeight;
		private readonly PlotModel _plotModel;
		private bool _arePoints1Visible;
		private bool _arePoints2Visible;
		private bool _arePoints3Visible;
		private bool _arePoints4Visible;

		private readonly TrendControlViewModel _trendControlVm1;
		private readonly TrendControlViewModel _trendControlVm2;
		private readonly TrendControlViewModel _trendControlVm3;
		private readonly TrendControlViewModel _trendControlVm4;
		private bool _addPoints1AsSigned;
		private bool _addPoints2AsSigned;
		private bool _addPoints3AsSigned;
		private bool _addPoints4AsSigned;


		public SystemControlViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl, ILinkContol linkControl) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;
			_linkControl = linkControl;

			_cmdSetBootloader = new RelayCommand(SetBootloader, () => _sendingEnabledControl.IsSendingEnabled);
			_cmdRestart = new RelayCommand(Restart, () => _sendingEnabledControl.IsSendingEnabled);
			_cmdFlash = new RelayCommand(Flash, () => _sendingEnabledControl.IsSendingEnabled);

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;

			_points1 = new ObservableCollection<DataPoint>();
			_points2 = new ObservableCollection<DataPoint>();
			_points3 = new ObservableCollection<DataPoint>();
			_points4 = new ObservableCollection<DataPoint>();
			_plotModel = new PlotModel {Title = "Hello plot"};
			_plotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

			_arePoints1Visible = true;
			_arePoints2Visible = true;
			_arePoints3Visible = true;
			_arePoints4Visible = true;

			_trendControlVm1= new TrendControlViewModel("Параметр 1", this);
			_trendControlVm2 = new TrendControlViewModel("Параметр 2", this);
			_trendControlVm3 = new TrendControlViewModel("Параметр 3", this);
			_trendControlVm4 = new TrendControlViewModel("Параметр 4", this);
		}

		private void SendingEnabledControlOnSendingEnabledChanged(bool issendingenabled) {
			_cmdSetBootloader.RaiseCanExecuteChanged();
			_cmdRestart.RaiseCanExecuteChanged();
			_cmdFlash.RaiseCanExecuteChanged();
		}

		private void SetBootloader() {
			try {
				_logger.Log("Переход в режим bootloader");

				var cmd = new SetBootloaderCommand();

				_logger.Log("Команда перехода в режим bootloader поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
							}
							_logger.Log("Команда перехода в режим bootloader была отправлена");
						}
						catch (Exception ex) {
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду перехода в режим bootloader в очередь: " + ex.Message);
			}
		}


		private void Flash() {
			try {
				_logger.Log("Переход в режим bootloader");

				var cmd = new SetBootloaderCommand();

				_logger.Log("Команда перехода в режим bootloader поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								//throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
								_logger.Log("Произошла ошибка при передаче данных, но это нормально");
							}
							_logger.Log("Команда перехода в режим bootloader была отправлена, отключаемся от COM-порта");
							_linkControl.CloseComPort();
							var psi = new ProcessStartInfo("flash.bat");
							var process = new Process {StartInfo = psi};
							process.Start();
						}
						catch (Exception ex) {
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду перехода в режим bootloader в очередь: " + ex.Message);
			}
		}

		private void Restart() {
			var cmd = new RestartCommand();
			try {
				_logger.Log(cmd.Name);
				_logger.Log("Команда <" + cmd.Name + "> поставлена в очередь");
				_commandSenderHost.Sender.SendCommandAsync(
					_targerAddressHost.TargetAddress
					, cmd
					, TimeSpan.FromSeconds(1)
					, (exception, bytes) => _userInterfaceRoot.Notifier.Notify(() => {
						try {
							if (exception != null) {
								throw new Exception("Ошибка при передаче данных: " + exception.Message, exception);
							}
							_logger.Log("Команда <" + cmd.Name + "> была отправлена");
						}
						catch (Exception ex) {
							_logger.Log(ex.Message);
						}
					}));
			}
			catch (Exception ex) {
				_logger.Log("Не удалось поставить команду <" + cmd.Name + "> в очередь: " + ex.Message);
			}
		}


		public ICommand CmdSetBootloader {
			get { return _cmdSetBootloader; }
		}

		public RelayCommand CmdRestart {
			get { return _cmdRestart; }
		}

		public void ShowBytes(IList<byte> bytes) {
			_debugBytes = bytes;
			RaisePropertyChanged(() => Param01);
			RaisePropertyChanged(() => Param02);

			RaisePropertyChanged(() => Param11);
			RaisePropertyChanged(() => Param12);

			RaisePropertyChanged(() => Param21);
			RaisePropertyChanged(() => Param22);

			RaisePropertyChanged(() => Param31);
			RaisePropertyChanged(() => Param32);

			RaisePropertyChanged(() => Param41);
			RaisePropertyChanged(() => Param42);

			RaisePropertyChanged(() => Param51);
			RaisePropertyChanged(() => Param52);

			RaisePropertyChanged(() => Param61);
			RaisePropertyChanged(() => Param62);

			RaisePropertyChanged(() => Param71);
			RaisePropertyChanged(() => Param72);

			// TODO: rework for big endian and little endian archs

			double value1 = _addPoints1AsSigned ? BitConverter.ToInt16(new[] {_debugBytes[1], _debugBytes[0]}, 0)*1.0 : BitConverter.ToUInt16(new[] {_debugBytes[1], _debugBytes[0]}, 0)*1.0;
			double value2 = _addPoints2AsSigned ? BitConverter.ToInt16(new[] {_debugBytes[3], _debugBytes[2]}, 0)*1.0 : BitConverter.ToUInt16(new[] {_debugBytes[3], _debugBytes[2]}, 0)*1.0;
			double value3 = _addPoints3AsSigned ? BitConverter.ToInt16(new[] {_debugBytes[5], _debugBytes[4]}, 0)*1.0 : BitConverter.ToUInt16(new[] {_debugBytes[5], _debugBytes[4]}, 0)*1.0;
			double value4 = _addPoints4AsSigned ? BitConverter.ToInt16(new[] {_debugBytes[7], _debugBytes[6]}, 0)*1.0 : BitConverter.ToUInt16(new[] {_debugBytes[7], _debugBytes[6]}, 0)*1.0;

			_points1.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value1));
			_points2.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value2));
			_points3.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value3));
			_points4.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value4));
		}

		// todo: move text methods to extentions or some static class
		private string GetByteText(int zeroBasedRow, int oneBasedCol) {
			try {
				var b = _debugBytes[zeroBasedRow*4 + oneBasedCol - 1];
				return b.ToString("X2") + " (" + b + ")";
			}
			catch {
				return "----";
			}
		}

		private string GetUShortText(int zeroBasedRow, int oneBasedCol) {
			try {
				// TODO: rework for big endian and little endian archs
				var b = BitConverter.ToUInt16(new[] {
					_debugBytes[zeroBasedRow*2 + ((oneBasedCol - 1)*2 + 1)],
					_debugBytes[zeroBasedRow*2 + (oneBasedCol - 1)*2]
				}, 0);
				//var b = _debugBytes[zeroBasedRow*2 + (oneBasedCol - 1)*2] + _debugBytes[zeroBasedRow*2 + ((oneBasedCol - 1)*2 + 1)] ;
				return b.ToString("X4") + " (" + b + ")" +
					_debugBytes[zeroBasedRow * 2 + (oneBasedCol - 1) * 2].ToString("X2") +
					_debugBytes[zeroBasedRow * 2 + ((oneBasedCol - 1) * 2 + 1)].ToString("X2");
			}
			catch {
				return "--------";
			}
		}

		public string Param01 {
			get { return GetUShortText(0, 1); }
		}

		public string Param02 {
			get { return GetUShortText(0, 2); }
		}

		public string Param11 {
			get { return GetUShortText(1, 1); }
		}

		public string Param12 {
			get { return GetUShortText(1, 2); }
		}

		public string Param21 {
			get { return GetUShortText(2, 1); }
		}

		public string Param22 {
			get { return GetUShortText(2, 2); }
		}

		public string Param31 {
			get { return GetUShortText(3, 1); }
		}

		public string Param32 {
			get { return GetUShortText(3, 2); }
		}

		public string Param41 {
			get { return GetUShortText(4, 1); }
		}

		public string Param42 {
			get { return GetUShortText(4, 2); }
		}

		public string Param51 {
			get { return GetUShortText(5, 1); }
		}

		public string Param52 {
			get { return GetUShortText(5, 2); }
		}

		public string Param61 {
			get { return GetUShortText(6, 1); }
		}

		public string Param62 {
			get { return GetUShortText(6, 2); }
		}

		public string Param71 {
			get { return GetUShortText(7, 1); }
		}

		public string Param72 {
			get { return GetUShortText(7, 2); }
		}

		public RelayCommand CmdFlash {
			get { return _cmdFlash; }
		}

		public ObservableCollection<DataPoint> Points1 {
			get { return _points1; }
		}

		public ObservableCollection<DataPoint> Points2 {
			get { return _points2; }
		}

		public ObservableCollection<DataPoint> Points3 {
			get { return _points3; }
		}

		public ObservableCollection<DataPoint> Points4 {
			get { return _points4; }
		}

		public double GridPlotHeight {
			get { return _gridPlotHeight; }
			set {
				if (Math.Abs(_gridPlotHeight - value) > 0.1) {
					_gridPlotHeight = value;
					RaisePropertyChanged(() => GridPlotHeight);
				}
			}
		}

		public PlotModel PlModel {
			get { return _plotModel; }
		}

		public void ClearTrendData(string name) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name) {
				case "Параметр 1":
					_points1.Clear();
					break;
				case "Параметр 2":
					_points2.Clear();
					break;
				case "Параметр 3":
					_points3.Clear();
					break;
				case "Параметр 4":
					_points4.Clear();
					break;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public bool GetTrendVisibility(string name) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name) {
				case "Параметр 1":
					return _arePoints1Visible;
				case "Параметр 2":
					return _arePoints2Visible;
				case "Параметр 3":
					return _arePoints3Visible;
				case "Параметр 4":
					return _arePoints4Visible;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public void SetTrendVisibility(string name, bool value) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name) {
				case "Параметр 1":
					ArePoints1Visible = value;
					break;
				case "Параметр 2":
					ArePoints2Visible = value;
					break;
				case "Параметр 3":
					ArePoints3Visible = value;
					break;
				case "Параметр 4":
					ArePoints4Visible = value;
					break;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public bool GetSignedFlag(string name) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name)
			{
				case "Параметр 1":
					return _addPoints1AsSigned;
				case "Параметр 2":
					return _addPoints2AsSigned;
				case "Параметр 3":
					return _addPoints3AsSigned;
				case "Параметр 4":
					return _addPoints4AsSigned;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public void SetSignedFlag(string name, bool isSigned) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name)
			{
				case "Параметр 1":
					_addPoints1AsSigned = isSigned;
					break;
				case "Параметр 2":
					_addPoints2AsSigned = isSigned;
					break;
				case "Параметр 3":
					_addPoints3AsSigned = isSigned;
					break;
				case "Параметр 4":
					_addPoints4AsSigned = isSigned;
					break;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public bool ArePoints1Visible {
			get { return _arePoints1Visible; }
			set {
				if (_arePoints1Visible != value) {
					_arePoints1Visible = value;
					RaisePropertyChanged(() => ArePoints1Visible);
				}
			}
		}

		public bool ArePoints2Visible {
			get { return _arePoints2Visible; }
			set {
				if (_arePoints2Visible != value) {
					_arePoints2Visible = value;
					RaisePropertyChanged(() => ArePoints2Visible);
				}
			}
		}

		public bool ArePoints3Visible {
			get { return _arePoints3Visible; }
			set {
				if (_arePoints3Visible != value) {
					_arePoints3Visible = value;
					RaisePropertyChanged(() => ArePoints3Visible);
				}
			}
		}

		public bool ArePoints4Visible {
			get { return _arePoints4Visible; }
			set {
				if (_arePoints4Visible != value) {
					_arePoints4Visible = value;
					RaisePropertyChanged(() => ArePoints4Visible);
				}
			}
		}

		public TrendControlViewModel TrendControlVm1 {
			get { return _trendControlVm1; }
		}

		public TrendControlViewModel TrendControlVm2 {
			get { return _trendControlVm2; }
		}

		public TrendControlViewModel TrendControlVm3 {
			get { return _trendControlVm3; }
		}

		public TrendControlViewModel TrendControlVm4 {
			get { return _trendControlVm4; }
		}
	}
}
