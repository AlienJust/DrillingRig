namespace DrillingRig.ConfigApp.AppControl.TargetAddressHost {
	class TargetAddressHostThreadSafe : ITargetAddressHostSettable {
		private readonly object _addressSync;
		private byte _targetAddress;

		public TargetAddressHostThreadSafe(byte targetAddress) {
			_addressSync = new object();
			_targetAddress = targetAddress;
		}

		public void SetTargetAddress(byte targetAddress) {
			lock (_addressSync) {
				_targetAddress = targetAddress;
			}
		}

		public byte TargetAddress
		{
			get { lock (_addressSync) return _targetAddress; }
		}
	}
}