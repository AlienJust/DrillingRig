using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.SystemControl
{
	class TrendControlViewModel : ViewModelBase {
		private readonly string _name;
		private readonly INamedTrendsControl _trendsControl;
		private ICommand _clearTrendDataCommand;
		public TrendControlViewModel(string name, INamedTrendsControl trendsControl) {
			_name = name;
			_trendsControl = trendsControl;
			_clearTrendDataCommand = new RelayCommand(ClearTrendData);
		}

		private void ClearTrendData() {
			_trendsControl.ClearTrendData(_name);
		}

		public bool IsTrendVisible {
			get { return _trendsControl.GetTrendVisibility(_name); }
			set {
				if (value != IsTrendVisible) {
					_trendsControl.SetTrendVisibility(_name, value);
					RaisePropertyChanged(()=>IsTrendVisible);
				}
			}
		}

		public bool IsSigned
		{
			get { return _trendsControl.GetSignedFlag(_name); }
			set
			{
				if (value != IsSigned)
				{
					_trendsControl.SetSignedFlag(_name, value);
					RaisePropertyChanged(() => IsSigned);
				}
			}
		}

		public ICommand ClearTrendCommand
		{
			get { return _clearTrendDataCommand; }
			set { _clearTrendDataCommand = value; }
		}

		public string Name { get { return _name; } }
	}
}
