namespace DrillingRid.Commands.Contracts.RtuModbus {
	public struct TwoBytes
	{
		private readonly byte _low;
		private readonly byte _high;
		public TwoBytes(byte low, byte high)
			: this()
		{
			_low = low;
			_high = high;
		}

		public byte Low
		{
			get { return _low; }
		}

		public byte High
		{
			get { return _high; }
		}
	}
}

