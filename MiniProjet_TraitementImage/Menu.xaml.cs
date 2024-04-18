using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniProjet_TraitementImage
{
    /// <summary>
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
		}

        private void MenuClick(object sender, RoutedEventArgs e)
        {
            Button inter = (Button)sender;
			switch ((string)inter.Content)
			{
				case "Image":       NavigationService.Navigate(new ChoixImage()); break;
				case "Fractale":    NavigationService.Navigate(new Fractale()); break;
				case "QR Code":     break;
                case "Codage":      NavigationService.Navigate(new Codage()); break;
			}
		}
	}
}
