using System.Windows.Navigation;

namespace MiniProjet_TraitementImage
{
	public partial class MainWindow : NavigationWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			this.NavigationService.Navigate(new Menu());
		}
	}
}
