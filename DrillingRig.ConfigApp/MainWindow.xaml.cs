using System.Windows;
using AlienJust.Adaptation.WindowsPresentation;
using MahApps.Metro.Controls;

namespace DrillingRig.ConfigApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel(new WpfUiNotifier(Dispatcher), new WpfWindowSystem());
		}
	}
}
