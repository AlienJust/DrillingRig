using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DrillingRig.ConfigApp.AvaDock {
	public class DockManagerViewModel {
		/// <summary>Gets a collection of all visible documents</summary>
		public ObservableCollection<DockWindowViewModel> Documents { get; }

		public ObservableCollection<DockWindowViewModel> Anchorables { get; }

		public DockManagerViewModel(IEnumerable<DockWindowViewModel> dockWindowViewModels, IEnumerable<DockWindowViewModel> ancorWindowViewModels) {
			Documents = new ObservableCollection<DockWindowViewModel>();
			Anchorables = new ObservableCollection<DockWindowViewModel>();

			foreach (var document in dockWindowViewModels) {
				document.PropertyChanged += DockWindowViewModelOnPropertyChanged;
				if (!document.IsClosed)
					Documents.Add(document);
			}

			foreach (var document in ancorWindowViewModels) {
				document.PropertyChanged += DockWindowViewModelOnPropertyChanged;
				if (!document.IsClosed)
					Anchorables.Add(document);
			}
		}

		private void DockWindowViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e) {
			DockWindowViewModel document = sender as DockWindowViewModel;
			if (document == null) throw new NullReferenceException("Cannot get sender as DockWindowViewModel");
			if (e.PropertyName == nameof(DockWindowViewModel.IsClosed)) {
				if (!document.IsClosed)
					Documents.Add(document);
				else
					Documents.Remove(document);
			}
		}
	}
}