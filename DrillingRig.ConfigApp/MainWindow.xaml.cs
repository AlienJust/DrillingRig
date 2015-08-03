using System.Windows;
using AlienJust.Adaptation.WindowsPresentation;

namespace DrillingRig.ConfigApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel(new WpfUiNotifier(Dispatcher), new WpfWindowSystem());
		}
	}
}
