using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using AlienJust.Support.Loggers.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using DrillingRig.Commands.SystemControl;
using DrillingRig.ConfigApp.AinTelemetry;
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

		private readonly RelayCommand _commandPanLeftFast;
		private readonly RelayCommand _commandPanLeft;
		private readonly RelayCommand _commandPanRight;
		private readonly RelayCommand _commandPanRightFast;

		private readonly RelayCommand _commandPanUpFast;
		private readonly RelayCommand _commandPanUp;
		private readonly RelayCommand _commandPanDown;
		private readonly RelayCommand _commandPanDownFast;

		private readonly RelayCommand _commandZoomOut;
		private readonly RelayCommand _commandZoomIn;
		private readonly RelayCommand _commandZoomAll;

		private IList<byte> _debugBytes;
		private readonly LineSeries _points1;
		private readonly LineSeries _points2;
		private readonly LineSeries _points3;
		private readonly LineSeries _points4;
		
		private readonly TrendControlViewModel _trendControlVm1;
		private readonly TrendControlViewModel _trendControlVm2;
		private readonly TrendControlViewModel _trendControlVm3;
		private readonly TrendControlViewModel _trendControlVm4;
		private bool _addPoints1AsSigned;
		private bool _addPoints2AsSigned;
		private bool _addPoints3AsSigned;
		private bool _addPoints4AsSigned;

		private readonly PlotModel _plotVm;
		private readonly PlotController _plotCr;
		private readonly TelemetryCommonViewModel _commonTelemetryVm;

		public SystemControlViewModel(ICommandSenderHost commandSenderHost, ITargetAddressHost targerAddressHost, IUserInterfaceRoot userInterfaceRoot, ILogger logger, IWindowSystem windowSystem, INotifySendingEnabled sendingEnabledControl, ILinkContol linkControl) {
			_commandSenderHost = commandSenderHost;
			_targerAddressHost = targerAddressHost;
			_userInterfaceRoot = userInterfaceRoot;
			_logger = logger;
			_windowSystem = windowSystem;
			_sendingEnabledControl = sendingEnabledControl;
			_linkControl = linkControl;

			_commonTelemetryVm = new TelemetryCommonViewModel(_logger);

			_cmdSetBootloader = new RelayCommand(SetBootloader, () => _sendingEnabledControl.IsSendingEnabled);
			_cmdRestart = new RelayCommand(Restart, () => _sendingEnabledControl.IsSendingEnabled);
			_cmdFlash = new RelayCommand(Flash, () => _sendingEnabledControl.IsSendingEnabled);

			_sendingEnabledControl.SendingEnabledChanged += SendingEnabledControlOnSendingEnabledChanged;

			_plotVm = new PlotModel {Title = "Графики"};
			_plotVm.Axes.Add(new DateTimeAxis());
			_plotVm.Axes.Add(new LinearAxis());
			//_plotVm.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

			_plotCr = new PlotController();
			_plotCr.UnbindAll();
			_plotCr.BindMouseDown(OxyMouseButton.Left, PlotCommands.Track);
			_plotCr.Bind(new OxyMouseDownGesture(OxyMouseButton.Right), PlotCommands.PanAt);
			_plotCr.Bind(new OxyMouseDownGesture(OxyMouseButton.Left), PlotCommands.ZoomRectangle);
			_plotCr.Bind(new OxyMouseEnterGesture(OxyModifierKeys.None), PlotCommands.HoverPointsOnlyTrack);
			_plotCr.BindMouseWheel(PlotCommands.ZoomWheel);
			_plotCr.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.None, 2, PlotCommands.ResetAt);

			_points1 = new LineSeries { Color = OxyColor.FromRgb(255, 0, 0) };
			_points2 = new LineSeries { Color = OxyColor.FromRgb(0, 128, 0) };
			_points3 = new LineSeries { Color = OxyColor.FromRgb(0, 0, 128) };
			_points4 = new LineSeries { Color = OxyColor.FromRgb(128, 0, 128) };

			_plotVm.Series.Add(_points1);
			_plotVm.Series.Add(_points2);
			_plotVm.Series.Add(_points3);
			_plotVm.Series.Add(_points4);

			_trendControlVm1= new TrendControlViewModel("Параметр 1", this);
			_trendControlVm2 = new TrendControlViewModel("Параметр 2", this);
			_trendControlVm3 = new TrendControlViewModel("Параметр 3", this);
			_trendControlVm4 = new TrendControlViewModel("Параметр 4", this);

			_commandPanLeftFast = new RelayCommand(() =>
			{
				_plotVm.PanAllAxes(_plotVm.PlotArea.Width / 4.0, 0);
				_plotVm.InvalidatePlot(false);
			});

			_commandPanLeft = new RelayCommand(() => {
				_plotVm.PanAllAxes(_plotVm.PlotArea.Width / 20.0, 0);
				_plotVm.InvalidatePlot(false);
			});

			_commandPanRight = new RelayCommand(() =>
			{
				_plotVm.PanAllAxes(_plotVm.PlotArea.Width / -20.0, 0);
				_plotVm.InvalidatePlot(false);
			});

			_commandPanRightFast = new RelayCommand(() =>
			{
				_plotVm.PanAllAxes(_plotVm.PlotArea.Width / -4.0, 0);
				_plotVm.InvalidatePlot(false);
			});

			_commandZoomOut = new RelayCommand(() => {
				_plotVm.ZoomAllAxes(0.8);
				_plotVm.InvalidatePlot(false);
			});

			_commandZoomIn = new RelayCommand(() =>
			{
				_plotVm.ZoomAllAxes(1.25);
				_plotVm.InvalidatePlot(false);
			});

			_commandZoomAll = new RelayCommand(() =>
			{
				_plotVm.ResetAllAxes();
				_plotVm.InvalidatePlot(false);
			});



			_commandPanUpFast = new RelayCommand(() =>
			{
				_plotVm.PanAllAxes(0, _plotVm.PlotArea.Height/ 4.0);
				_plotVm.InvalidatePlot(false);
			});

			_commandPanUp = new RelayCommand(() =>
			{
				_plotVm.PanAllAxes(0, _plotVm.PlotArea.Height / 20.0);
				_plotVm.InvalidatePlot(false);
			});

			_commandPanDown = new RelayCommand(() =>
			{
				_plotVm.PanAllAxes(0, _plotVm.PlotArea.Height / -20.0);
				_plotVm.InvalidatePlot(false);
			});

			_commandPanDownFast = new RelayCommand(() =>
			{
				_plotVm.PanAllAxes(0, _plotVm.PlotArea.Height / -4.0);
				_plotVm.InvalidatePlot(false);
			});
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

			double value1 = _addPoints1AsSigned ? ToInt16(_debugBytes[0], _debugBytes[1])*1.0 : ToUInt16(_debugBytes[0], _debugBytes[1])*1.0;
			double value2 = _addPoints2AsSigned ? ToInt16(_debugBytes[2], _debugBytes[3])*1.0 : ToUInt16(_debugBytes[2], _debugBytes[3])*1.0;
			double value3 = _addPoints3AsSigned ? ToInt16(_debugBytes[4], _debugBytes[5])*1.0 : ToUInt16(_debugBytes[4], _debugBytes[5])*1.0;
			double value4 = _addPoints4AsSigned ? ToInt16(_debugBytes[6], _debugBytes[7])*1.0 : ToUInt16(_debugBytes[6], _debugBytes[7])*1.0;

			_points1.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value1));
			_points2.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value2));
			_points3.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value3));
			_points4.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, value4));

			_plotVm.InvalidatePlot(true);
		}

		private short ToInt16(byte lowByte, byte highByte) {
			if (BitConverter.IsLittleEndian) {
				return BitConverter.ToInt16(new[] {highByte, lowByte}, 0);
			}
			return BitConverter.ToInt16(new[] {lowByte, highByte}, 0);
		}

		private ushort ToUInt16(byte lowByte, byte highByte) {
			if (BitConverter.IsLittleEndian) {
				return BitConverter.ToUInt16(new[] {highByte, lowByte}, 0);
			}
			return BitConverter.ToUInt16(new[] {lowByte, highByte}, 0);
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
				const int bytesCountPerValue = 2;
				const int valuesCountInRow = 2;
				const int bytesInRow = bytesCountPerValue*valuesCountInRow;
				int firstCurrentRowByteIndex = zeroBasedRow*bytesInRow;
				
				int lowByteIndex = firstCurrentRowByteIndex + (oneBasedCol - 1) * bytesCountPerValue;
				int highByteIndex = firstCurrentRowByteIndex + ((oneBasedCol - 1) * bytesCountPerValue + 1);

				var b = ToUInt16(_debugBytes[lowByteIndex], _debugBytes[highByteIndex]);
				
				return b.ToString("X4") + " (" + b + ")";
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

		
		public PlotModel PlotVm {
			get { return _plotVm; }
		}

		public PlotController PlotCr {
			get { return _plotCr; }
		}

		public void ClearTrendData(string name) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name) {
				case "Параметр 1":
					_points1.Points.Clear();
					_plotVm.InvalidatePlot(true);
					break;
				case "Параметр 2":
					_points2.Points.Clear();
					_plotVm.InvalidatePlot(true);
					break;
				case "Параметр 3":
					_points3.Points.Clear();
					_plotVm.InvalidatePlot(true);
					break;
				case "Параметр 4":
					_points4.Points.Clear();
					_plotVm.InvalidatePlot(true);
					break;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public bool GetTrendVisibility(string name) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name) {
				case "Параметр 1":
					return _points1.IsVisible;
				case "Параметр 2":
					return _points2.IsVisible;
				case "Параметр 3":
					return _points3.IsVisible;
				case "Параметр 4":
					return _points4.IsVisible;
				default:
					throw new Exception("Неизвестное название параметра: " + name);
			}
		}

		public void SetTrendVisibility(string name, bool value) {
			if (name == null) throw new ArgumentNullException("name");
			switch (name) {
				case "Параметр 1":
					_points1.IsVisible = value;
					break;
				case "Параметр 2":
					_points2.IsVisible = value;
					break;
				case "Параметр 3":
					_points3.IsVisible = value;
					break;
				case "Параметр 4":
					_points4.IsVisible = value;
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

		public RelayCommand CommandPanLeft {
			get { return _commandPanLeft; }
		}

		public RelayCommand CommandPanRight {
			get { return _commandPanRight; }
		}

		public RelayCommand CommandZoomOut {
			get { return _commandZoomOut; }
		}

		public RelayCommand CommandZoomIn {
			get { return _commandZoomIn; }
		}

		public RelayCommand CommandPanLeftFast {
			get { return _commandPanLeftFast; }
		}

		public RelayCommand CommandPanRightFast {
			get { return _commandPanRightFast; }
		}

		public RelayCommand CommandZoomAll {
			get { return _commandZoomAll; }
		}

		public RelayCommand CommandPanUpFast {
			get { return _commandPanUpFast; }
		}

		public RelayCommand CommandPanUp {
			get { return _commandPanUp; }
		}

		public RelayCommand CommandPanDown {
			get { return _commandPanDown; }
		}

		public RelayCommand CommandPanDownFast {
			get { return _commandPanDownFast; }
		}

		public TelemetryCommonViewModel CommonTelemetryVm {
			get { return _commonTelemetryVm; }
		}
	}
}
