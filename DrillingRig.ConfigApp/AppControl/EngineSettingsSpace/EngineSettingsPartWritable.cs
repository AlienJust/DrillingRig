namespace DrillingRig.ConfigApp.AppControl.EngineSettingsSpace {
	public class EngineSettingsPartWritable : IEngineSettingsPart {
		public ushort? Inom { get; set; }
		public ushort? Nnom { get; set; }
		public ushort? Nmax { get; set; }
		public uint? Pnom { get; set; }
		public ushort? CosFi { get; set; }
		public ushort? Eff { get; set; }
		public ushort? Mass { get; set; }
		public ushort? MmM { get; set; }
		public ushort? Height { get; set; }

		public uint? I2Tmax { get; set; }
		public ushort? Icontinious { get; set; }
		public ushort? ZeroF { get; set; }
	}
}