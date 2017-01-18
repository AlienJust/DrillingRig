using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.WindowControl {
	class WindowControlViewModel : ViewModelBase {
		private bool _isBsEthernetLogWindowShown;
		private bool _isOscilloscopeWindowShown;
		private bool _isChartWindowShown;

		public bool IsBsEthernetLogWindowShown {
			get { return _isBsEthernetLogWindowShown; }
			set {
				if (_isBsEthernetLogWindowShown != value) {
					_isBsEthernetLogWindowShown = value;
					RaisePropertyChanged(()=> IsBsEthernetLogWindowShown);
				} }
		}

		public bool IsOscilloscopeWindowShown {
			get { return _isOscilloscopeWindowShown; }
			set {
				if (_isOscilloscopeWindowShown != value) {
					_isOscilloscopeWindowShown = value;
					RaisePropertyChanged(() => IsOscilloscopeWindowShown);
				}
			}
		}

		public bool IsChartWindowShown {
			get { return _isChartWindowShown; }
			set {
				if (_isChartWindowShown != value) {
					_isChartWindowShown = value;
					RaisePropertyChanged(() => IsChartWindowShown);
				}
			}
		}
	}
}
