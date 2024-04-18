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
	/// Logique d'interaction pour Fractale.xaml
	/// </summary>
	public partial class Fractale : Page
	{
		public Fractale()
		{
			InitializeComponent();
			MyImage test = new MyImage("cascade.bmp");
			test.Fractalle(500, 500);
			test.FromImageToFile("TESTVAY.bmp");

			FrontImage.Source = new BitmapImage(new Uri(uriString: $"{AppDomain.CurrentDomain.BaseDirectory}/TESTWAY.bmp", UriKind.RelativeOrAbsolute));
		}
	}
}
