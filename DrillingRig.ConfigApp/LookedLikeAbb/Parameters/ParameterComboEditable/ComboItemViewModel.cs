using AlienJust.Support.ModelViewViewModel;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Parameters.ParameterComboIntEditable
{
	class ComboItemViewModel<TModel> : ViewModelBase
	{
		public string ComboText { get; set; }
		public TModel ComboValue { get; set; }
	}
}