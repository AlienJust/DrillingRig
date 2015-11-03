using AlienJust.Support.ModelViewViewModel;
using DrillingRig.Commands.BsEthernetSettings;

namespace DrillingRig.ConfigApp.BsEthernetSettings
{
	class FtRoleViewModel : ViewModelBase {
		private readonly FriquencyTransformerRole _role;
		public FtRoleViewModel(FriquencyTransformerRole role) {
			_role = role;
		}

		public string Text { get { return _role.ToText(); } }

		public FriquencyTransformerRole Role { get { return _role; } }
	}
}
