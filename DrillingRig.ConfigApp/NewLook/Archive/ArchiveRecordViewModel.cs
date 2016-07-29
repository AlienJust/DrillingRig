using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.FaultArchive;

namespace DrillingRig.ConfigApp.NewLook.Archive {
	class ArchiveRecordViewModel : ViewModelBase, IArchiveRecordViewModel {
		private readonly IArchiveRecord _archiveRecord;

		public ArchiveRecordViewModel(IArchiveRecord archiveRecord) {
			_archiveRecord = archiveRecord;
		}

		public string Time => _archiveRecord.Year.ToString("D2") + "." + _archiveRecord.Month.ToString("D2") + "." + _archiveRecord.Day.ToString("D2") + " " + _archiveRecord.Hour.ToString("D2") + ":" + _archiveRecord.Minute.ToString("D2") + ":" + _archiveRecord.Second.ToString("D2");

		public string FaultState => _archiveRecord.FaultState.ToString("X4");

		public string Mcw => _archiveRecord.Mcw.ToString("X4");

		public string Msw => _archiveRecord.Msw.ToString("X4");
	}
}
