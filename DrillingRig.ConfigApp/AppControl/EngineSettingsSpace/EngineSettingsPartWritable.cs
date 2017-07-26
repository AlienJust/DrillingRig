namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace {
	public class EngineSettingsPartWritable : IEngineSettingsPart {
		public ushort? Inom { get; set; }
		public ushort? Nnom { get; set; }
		public ushort? Nmax { get; set; }
		public decimal? Pnom { get; set; }
		public decimal? CosFi { get; set; }
		public decimal? Eff { get; set; }
		public ushort? Mass { get; set; }
		public ushort? MmM { get; set; }
		public ushort? Height { get; set; }

		public uint? I2Tmax { get; set; }
		public ushort? Icontinious { get; set; }
		public ushort? ZeroF { get; set; }
	}
}