using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.AppControl.LoggerHost;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterBooleanReadonly;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterStringReadonly;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Group106Settings.ImvcParameter {
	class ImcwParameterViewModel : ViewModelBase {
		private ushort? _fullValue;
		private bool? _bit00;
		private bool? _bit01;
		private bool? _bit02;
		private bool? _bit03;
		private bool? _bit04;
		private bool? _bit05;
		private bool? _bit06;
		private bool? _bit07;
		public string[] Bit0809Vms { get; }
		private string _selectedBit0809Vm;
		public string[] Bit1011Vms { get; }
		private string _selectedBit1011Vm;
		private bool? _bit12;
		private bool? _bit13;
		private bool? _bit14;
		private bool? _bit15;

		public ImcwParameterViewModel() {
			_fullValue = null;
			_bit00 = null;
			_bit01 = null;
			_bit02 = null;
			_bit03 = null;
			_bit04 = null;
			_bit05 = null;
			_bit06 = null;
			_bit07 = null;
			Bit0809Vms = new[] { "0 - ведущий", "1 - ведомый 1", "2 - ведомый 2", "3 - резерв" };
			_selectedBit0809Vm = null;
			Bit1011Vms = new[] { "0 - одиночная работа", "1 - 2 блока, есть только ведомый 1", "2 - 3 блока", "3 - резерв" };
			_selectedBit1011Vm = null;
			_bit12 = null;
			_bit13 = null;
			_bit14 = null;
			_bit15 = null;
		}




		public void UpdateTelemetry(ushort? imcw) {
			FullValue = imcw;

			if (imcw.HasValue) {
				//ParameterLiteralVm.CurrentValue = imcw.Value.ToString("X4");
				_bit00 = (imcw.Value & 0x0001) == 0x0001; 
				_bit01 = (imcw.Value & 0x0002) == 0x0002; 
				_bit02 = (imcw.Value & 0x0004) == 0x0004; 
				_bit03 = (imcw.Value & 0x0008) == 0x0008; 
				_bit04 = (imcw.Value & 0x0010) == 0x0010; 
				_bit05 = (imcw.Value & 0x0020) == 0x0020; 
				_bit06 = (imcw.Value & 0x0040) == 0x0040; 
				_bit07 = (imcw.Value & 0x0080) == 0x0080; 

				var bits0809Index = (imcw.Value & 0x0300) >> 8;
				_selectedBit0809Vm = Bit0809Vms[bits0809Index]; 

				var bits1011Index = (imcw.Value & 0x0C00) >> 10;
				_selectedBit1011Vm = Bit1011Vms[bits1011Index]; 

				_bit12 = (imcw.Value & 0x1000) == 0x1000; 
				_bit13 = (imcw.Value & 0x2000) == 0x2000; 
				_bit14 = (imcw.Value & 0x4000) == 0x4000; 
				_bit15 = (imcw.Value & 0x8000) == 0x8000;
				RaiseBitsChanges();
			}
			else {
				_bit00 = null;
				_bit01 = null;
				_bit02 = null;
				_bit03 = null;
				_bit04 = null;
				_bit05 = null;
				_bit06 = null;
				_bit07 = null;
				_selectedBit0809Vm = null;
				_selectedBit1011Vm = null;
				_bit12 = null;
				_bit13 = null;
				_bit14 = null;
				_bit15 = null;
				RaiseBitsChanges();
			}
		}

		void RaiseBitsChanges()
		{
			RaisePropertyChanged(() => Bit00);
			RaisePropertyChanged(() => Bit01);
			RaisePropertyChanged(() => Bit02);
			RaisePropertyChanged(() => Bit03);
			RaisePropertyChanged(() => Bit04);
			RaisePropertyChanged(() => Bit05);
			RaisePropertyChanged(() => Bit06);
			RaisePropertyChanged(() => Bit07);

			RaisePropertyChanged(() => SelectedBit0809Vm);
			RaisePropertyChanged(() => SelectedBit1011Vm);

			RaisePropertyChanged(() => Bit12);
			RaisePropertyChanged(() => Bit13);
			RaisePropertyChanged(() => Bit14);
			RaisePropertyChanged(() => Bit15);
		}

		public ushort? FullValue {
			get { return _fullValue; }
			set {
				if (_fullValue != value) {
					_fullValue = value;
					RaisePropertyChanged(() => FullValue);
					if (!_fullValue.HasValue)
					{
						_bit00 = null;
						_bit01 = null;
						_bit02 = null;
						_bit03 = null;
						_bit04 = null;
						_bit05 = null;
						_bit06 = null;
						_bit07 = null;
						_selectedBit0809Vm = null;
						_selectedBit1011Vm = null;
						_bit12 = null;
						_bit13 = null;
						_bit14 = null;
						_bit15 = null;
					}
					else
					{
						_bit00 = (_fullValue.Value & 0x0001) != 0;
						_bit01 = (_fullValue.Value & 0x0002) != 0;
						_bit02 = (_fullValue.Value & 0x0004) != 0;
						_bit03 = (_fullValue.Value & 0x0008) != 0;
						_bit04 = (_fullValue.Value & 0x0010) != 0;
						_bit05 = (_fullValue.Value & 0x0020) != 0;
						_bit06 = (_fullValue.Value & 0x0040) != 0;
						_bit07 = (_fullValue.Value & 0x0080) != 0;
						_selectedBit0809Vm = Bit0809Vms[(_fullValue.Value & 0x0300) >> 8];
						_selectedBit1011Vm = Bit1011Vms[(_fullValue.Value & 0x0C00) >> 10];
						_bit12 = (_fullValue.Value & 0x1000) != 0;
						_bit13 = (_fullValue.Value & 0x2000) != 0;
						_bit14 = (_fullValue.Value & 0x4000) != 0;
						_bit15 = (_fullValue.Value & 0x8000) != 0;
					}
					RaiseBitsChanges();
				}
			}
		}

		public bool? Bit00 {
			get { return _bit00; }
			set {
				if (_bit00 != value) {
					_bit00 = value;
					RaisePropertyChanged(() => Bit00);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit01 {
			get { return _bit01; }
			set {
				if (_bit01 != value) {
					_bit01 = value;
					RaisePropertyChanged(() => Bit01);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit02 {
			get { return _bit02; }
			set {
				if (_bit02 != value) {
					_bit02 = value;
					RaisePropertyChanged(() => Bit02);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit03 {
			get { return _bit03; }
			set {
				if (_bit03 != value) {
					_bit03 = value;
					RaisePropertyChanged(() => Bit03);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit04 {
			get { return _bit04; }
			set {
				if (_bit04 != value) {
					_bit04 = value;
					RaisePropertyChanged(() => Bit04);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit05 {
			get { return _bit05; }
			set {
				if (_bit05 != value) {
					_bit05 = value;
					RaisePropertyChanged(() => Bit05);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit06 {
			get { return _bit06; }
			set {
				if (_bit06 != value) {
					_bit06 = value;
					RaisePropertyChanged(() => Bit06);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit07 {
			get { return _bit07; }
			set {
				if (_bit07 != value) {
					_bit07 = value;
					RaisePropertyChanged(() => Bit07);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit12 {
			get { return _bit12; }
			set {
				if (_bit12 != value) {
					_bit12 = value;
					RaisePropertyChanged(() => Bit12);
					TryBuildFullValue();
				}
			}
		}
		public bool? Bit13 {
			get { return _bit13; }
			set {
				if (_bit13 != value) {
					_bit13 = value;
					RaisePropertyChanged(() => Bit13);
					TryBuildFullValue();
				}
			}
		}

		public bool? Bit14 {
			get { return _bit14; }
			set {
				if (_bit14 != value) {
					_bit14 = value;
					RaisePropertyChanged(() => Bit14);
					TryBuildFullValue();
				}
			}
		}

		public bool? Bit15 {
			get { return _bit15; }
			set {
				if (_bit15 != value) {
					_bit15 = value;
					RaisePropertyChanged(() => Bit15);
					TryBuildFullValue();
				}
			}
		}

		public string SelectedBit0809Vm {
			get { return _selectedBit0809Vm; }
			set {
				if (_selectedBit0809Vm != value) {
					_selectedBit0809Vm = value;
					RaisePropertyChanged(() => SelectedBit0809Vm);
					TryBuildFullValue();
				}
			}
		}

		public string SelectedBit1011Vm {
			get { return _selectedBit1011Vm; }
			set {
				if (_selectedBit1011Vm != value) {
					_selectedBit1011Vm = value;
					RaisePropertyChanged(() => SelectedBit1011Vm);
					TryBuildFullValue();
				}
			}
		}

		private void TryBuildFullValue() {
			if (_bit00.HasValue
					&& _bit01.HasValue
					&& _bit02.HasValue
					&& _bit03.HasValue
					&& _bit04.HasValue
					&& _bit05.HasValue
					&& _bit06.HasValue
					&& _bit07.HasValue
					&& _selectedBit0809Vm != null
					&& _selectedBit1011Vm != null
					&& _bit12.HasValue
					&& _bit13.HasValue
					&& _bit14.HasValue
					&& _bit15.HasValue
			)
			{
				ushort fullValue = 0;
				if (_bit00.Value) fullValue += 0x0001;
				if (_bit01.Value) fullValue += 0x0002;
				if (_bit02.Value) fullValue += 0x0004;
				if (_bit03.Value) fullValue += 0x0008;
				if (_bit04.Value) fullValue += 0x0010;
				if (_bit05.Value) fullValue += 0x0020;
				if (_bit06.Value) fullValue += 0x0040;
				if (_bit07.Value) fullValue += 0x0080;

				for (int i = 0; i < Bit0809Vms.Length; ++i) {
					if (_selectedBit0809Vm == Bit0809Vms[i]) {
						fullValue = (ushort)(fullValue | (i << 8));
						break;
					}
				}

				for (int i = 0; i < Bit1011Vms.Length; ++i) {
					if (_selectedBit1011Vm == Bit1011Vms[i]) {
						fullValue = (ushort)(fullValue | (i << 10));
						break;
					}
				}

				if (_bit12.Value) fullValue += 0x1000;
				if (_bit13.Value) fullValue += 0x2000;
				if (_bit14.Value) fullValue += 0x4000;
				if (_bit15.Value) fullValue += 0x8000;

				_fullValue = fullValue;
				RaisePropertyChanged(()=>FullValue);
			}
		}
	}
}
