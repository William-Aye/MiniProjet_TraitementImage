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
	public partial class ChoixImage : Page
	{
		public string nomImage = "blanc";
		public string paraImage = "aucun";
		public int filtre = 0;

		public ChoixImage()
		{
			InitializeComponent();
			VisibilityBouton();
		}

		private void ImageClick(object sender, RoutedEventArgs e)
		{
			Button inter = (Button)sender;
			nomImage = (string)inter.Content;

			VisibilityBouton();
		}

		private void ParamClick(object sender, RoutedEventArgs e)
		{
			Button inter = (Button)sender;
			switch ((string)inter.Content)
			{
				case "Aucun": paraImage = "rien"; break;
				case "Agrandir": paraImage = "agr"; break;
				case "Rétrécir": paraImage = "ret"; break;
				case "Nuance": paraImage = "nua"; break;
				case "Superpositions": paraImage = "sup"; break;
				case "Miroir": paraImage = "mir"; break;
				case "Rotation": paraImage = "rot"; break;
				case "Histogramme": paraImage = "his"; break;
				case "Filtre": paraImage = "fil"; break;
			}
			testText.Text = paraImage;

			VisibilityBouton();
		}

		private void FiltreClick(object sender, RoutedEventArgs e)
		{
			Button inter = (Button)sender;
			switch ((string)inter.Content)
			{
				case "Contraste": filtre = 1; break;
				case "Flou": filtre = 2; break;
				case "Flou de Gauss": filtre = 3; break;
				case "Amélioration des bords": filtre = 4; break;
				case "Détection des bords": filtre = 5; break;
				case "Repoussage": filtre = 6; break;
				case "Filtre de Sobel": filtre = 7; break;
				case "Amélioration de la netteté": filtre = 8; break;
				case "test": filtre = 9; break;
			}

			VisibilityBouton();
		}
		private void RunClick(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new PresentationImage(nomImage, paraImage, filtre));
		}

		private void VisibilityBouton()
		{
			BoutonAucun.Visibility          = Visibility.Hidden;
			BoutonAgrandir.Visibility       = Visibility.Hidden;
			BoutonRetrecir.Visibility       = Visibility.Hidden;
			BoutonNuance.Visibility         = Visibility.Hidden;
			BoutonSuperpositions.Visibility = Visibility.Hidden;
			BoutonMiroir.Visibility         = Visibility.Hidden;
			BoutonRotation.Visibility       = Visibility.Hidden;
			BoutonHistogramme.Visibility    = Visibility.Hidden;
			BoutonFiltre.Visibility         = Visibility.Hidden;

			BoutonConstraste.Visibility  = Visibility.Hidden;
			BoutonFlou.Visibility        = Visibility.Hidden;
			BoutonFlouGauss.Visibility   = Visibility.Hidden;
			BoutonAmeBords.Visibility    = Visibility.Hidden;
			BoutonDetBords.Visibility    = Visibility.Hidden;
			BoutonRepoussage.Visibility  = Visibility.Hidden;
			BoutonSobel.Visibility       = Visibility.Hidden;
			BoutonNettete.Visibility     = Visibility.Hidden;
			BoutonEmboss.Visibility        = Visibility.Hidden;

			BoutonRun.Visibility = Visibility.Hidden;

			if (nomImage != "blanc")
			{
				BoutonAucun.Visibility          = Visibility.Visible;
				BoutonAgrandir.Visibility       = Visibility.Visible;
				BoutonRetrecir.Visibility       = Visibility.Visible;
				BoutonNuance.Visibility         = Visibility.Visible;
				BoutonSuperpositions.Visibility = Visibility.Visible;
				BoutonMiroir.Visibility         = Visibility.Visible;
				BoutonRotation.Visibility       = Visibility.Visible;
				BoutonHistogramme.Visibility    = Visibility.Visible;
				BoutonFiltre.Visibility         = Visibility.Visible;
			}

			if (paraImage == "fil")
			{
				BoutonConstraste.Visibility  = Visibility.Visible;
				BoutonFlou.Visibility        = Visibility.Visible;
				BoutonFlouGauss.Visibility   = Visibility.Visible;
				BoutonAmeBords.Visibility    = Visibility.Visible;
				BoutonDetBords.Visibility    = Visibility.Visible;
				BoutonRepoussage.Visibility  = Visibility.Visible;
				BoutonSobel.Visibility       = Visibility.Visible;
				BoutonNettete.Visibility     = Visibility.Visible;
				BoutonEmboss.Visibility        = Visibility.Visible;
			}

			if (paraImage == "fil" && filtre != 0)
				BoutonRun.Visibility = Visibility.Visible;
			else if (paraImage != "fil" && paraImage != "aucun")
				BoutonRun.Visibility = Visibility.Visible;
		}
	}
}
