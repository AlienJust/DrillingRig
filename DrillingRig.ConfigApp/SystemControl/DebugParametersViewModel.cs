using System.Collections.Generic;
using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.SystemControl {
	class DebugParametersViewModel : ViewModelBase {
		private IList<byte> _debugBytes;

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
		}

		

		public string Param01 => ConvertHelp.GetParamText(_debugBytes, 0, 1);

		public string Param02 => ConvertHelp.GetParamText(_debugBytes, 0, 2);

		public string Param11 => ConvertHelp.GetParamText(_debugBytes, 1, 1);

		public string Param12 => ConvertHelp.GetParamText(_debugBytes, 1, 2);

		public string Param21 => ConvertHelp.GetParamText(_debugBytes, 2, 1);

		public string Param22 => ConvertHelp.GetParamText(_debugBytes, 2, 2);

		public string Param31 => ConvertHelp.GetParamText(_debugBytes, 3, 1);

		public string Param32 => ConvertHelp.GetParamText(_debugBytes, 3, 2);

		public string Param41 => ConvertHelp.GetParamText(_debugBytes, 4, 1);

		public string Param42 => ConvertHelp.GetParamText(_debugBytes, 4, 2);

		public string Param51 => ConvertHelp.GetParamText(_debugBytes, 5, 1);

		public string Param52 => ConvertHelp.GetParamText(_debugBytes, 5, 2);

		public string Param61 => ConvertHelp.GetParamText(_debugBytes, 6, 1);

		public string Param62 => ConvertHelp.GetParamText(_debugBytes, 6, 2);

		public string Param71 => ConvertHelp.GetParamText(_debugBytes, 7, 1);

		public string Param72 => ConvertHelp.GetParamText(_debugBytes, 7, 2);
	}
}
