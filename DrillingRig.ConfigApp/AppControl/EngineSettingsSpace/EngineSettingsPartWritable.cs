namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace {
	public class EngineSettingsPartWritable : IEngineSettingsPart {
		public ushort? Inom { get; set; }
		public ushort? Nnom { get; set; }
		public ushort? Nmax { get; set; }
		public double? Pnom { get; set; }
		public double? CosFi { get; set; }
		public double? Eff { get; set; }
		public ushort? Mass { get; set; }
		public ushort? MmM { get; set; }
		public ushort? Height { get; set; }

		public uint? I2Tmax { get; set; }
		public ushort? Icontinious { get; set; }
		public ushort? ZeroF { get; set; }
	}
}