namespace DrillingRig.ConfigApp.NewLook.Archive {
	interface IArchiveRecordViewModel {
		string Time { get; }
		string FaultState { get; }
		string Mcw { get; }
		string Msw { get; }
	}
}