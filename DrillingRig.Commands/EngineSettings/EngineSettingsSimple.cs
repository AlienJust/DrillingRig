namespace DrillingRig.Commands.EngineSettings {
	public class EngineSettingsSimple : IEngineSettings {
		public uint I2Tmax { get; set; }
		public uint Pnom { get; set; }
		public ushort Icontinious { get; set; }
		public ushort Mnom { get; set; }
		public ushort ZeroF { get; set; }
	}
}