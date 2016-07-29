using DrillingRig.ConfigApp.AvaDock;

namespace DrillingRig.ConfigApp.NewLook.Archive {
	class ArchivesViewModel : DockWindowViewModel, IArchivesViewModel {
		public ArchivesViewModel(IArchiveViewModel archive1Vm, IArchiveViewModel archive2Vm) {
			Archive1Vm = archive1Vm;
			Archive2Vm = archive2Vm;
		}

		public IArchiveViewModel Archive1Vm { get; }
		public IArchiveViewModel Archive2Vm { get; }
	}

	internal interface IArchivesViewModel {
		IArchiveViewModel Archive1Vm { get; }
		IArchiveViewModel Archive2Vm { get; }
	}
}
