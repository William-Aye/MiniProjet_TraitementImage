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
	/// Logique d'interaction pour Codage.xaml
	/// </summary>
	public partial class Codage : Page
	{
		public Codage()
		{
			InitializeComponent();

			MyImage test = new MyImage("cascade.bmp");
			MyImage test2 = new MyImage(test.CoderImage("coco.bmp"));
			test2.FromImageToFile("CodeTest.bmp");
			test2.DecoderImage("CodeTest.bmp");
			test2.FromImageToFile("DecodeTest.bmp");

			CodeImage.Source = new BitmapImage(new Uri(uriString: $"{AppDomain.CurrentDomain.BaseDirectory}/CodeTest.bmp", UriKind.RelativeOrAbsolute));
			DecodeImage.Source = new BitmapImage(new Uri(uriString: $"{AppDomain.CurrentDomain.BaseDirectory}/DecodeTest.bmp", UriKind.RelativeOrAbsolute));
		}
	}
}
