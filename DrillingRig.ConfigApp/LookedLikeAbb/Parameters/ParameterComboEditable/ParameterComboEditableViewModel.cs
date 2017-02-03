using System.Collections.Generic;
using AlienJust.Support.ModelViewViewModel;
using DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboIntEditable;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboEditable {
	class ParameterComboEditableViewModel<TComboModel> : ViewModelBase, ICheckableParameter {
		public string Name { get; }
		public IEnumerable<ComboItemViewModel<TComboModel>> ComboItems { get; }
		private ComboItemViewModel<TComboModel> _selectedComboItem;

		private bool _isChecked;

		public ParameterComboEditableViewModel(string name, IEnumerable<ComboItemViewModel<TComboModel>> comboItems)
		{
			Name = name;
			ComboItems = comboItems;
		}


		public ComboItemViewModel<TComboModel> SelectedComboItem
		{
			get { return _selectedComboItem; }
			set
			{
				if (_selectedComboItem != value) {
					_selectedComboItem = value;
					RaisePropertyChanged(() => SelectedComboItem);
				}
			}
		}

		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
				if (value != _isChecked) {
					_isChecked = value;
					RaisePropertyChanged(()=>IsChecked);
				}
			}
		}
	}
}