using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.AvaDock {
	public abstract class DockWindowViewModel : ViewModelBase {
		private ICommand _closeCommand;
		public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(Close));

		private bool _isClosed;
		public bool IsClosed {
			get { return _isClosed; }
			set {
				if (_isClosed != value) {
					_isClosed = value;
					RaisePropertyChanged(nameof(IsClosed));
				}
			}
		}
	
		private bool _canClose;
		public bool CanClose {
			get { return _canClose; }
			set {
				if (_canClose != value) {
					_canClose = value;
					RaisePropertyChanged(nameof(CanClose));
				}
			}
		}
	
		private string _title;
		public string Title {
			get { return _title; }
			set {
				if (_title != value) {
					_title = value;
					RaisePropertyChanged(nameof(Title));
				}
			}
		}

		protected DockWindowViewModel() {
			CanClose = true;
			IsClosed = false;
		}

		public void Close() {
			IsClosed = true;
		}
	}
}