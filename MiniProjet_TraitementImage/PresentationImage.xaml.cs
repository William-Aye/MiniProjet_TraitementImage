using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

using static System.Net.WebRequestMethods;
using Point = System.Drawing.Point;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace MiniProjet_TraitementImage
{
    /// <summary>
    /// Logique d'interaction pour PresentationImage.xaml
    /// </summary>
    public partial class PresentationImage : Page
    {
        public string nomImage;
        public string paraImage;
        public int filtre;
        public string nomImageDl = "TESTVAY";
        public string dataVerif;

        public PresentationImage(string nameImage, string paraIm, int filtre = 0)
        {
            InitializeComponent();
            if (nameImage != null)
                nomImage = nameImage;
            paraImage = paraIm;
            this.filtre = filtre;

            tbStatus.Text = nomImage;
            testText.Text = paraImage;

            if (paraImage == "agr" || paraImage == "ret" || paraImage == "rot" || paraImage == "sup")
                BoutonRun.Visibility = Visibility.Hidden;
			else
			{
				BarText.Visibility = Visibility.Hidden;
				BoutonSave.Visibility = Visibility.Hidden;
			}
		}

        private void RunClick(object sender, RoutedEventArgs e)
        {
			BoutonRun.Visibility = Visibility.Hidden;
			MyImage test = new MyImage($"{nomImage}.bmp");
            switch (paraImage)
            {
                case "agr": test.AgrandirImage(Convert.ToInt32(dataVerif)); break;
                case "ret": test.ReduireImage(Convert.ToInt32(dataVerif)); break;
                case "nua": test.NuanceDeGris(); break;
                case "sup": test.Superposition(dataVerif); break;
                case "mir": test.Miroir(); break;
                case "rot": test.Rotation(Convert.ToInt32(dataVerif)); break;
                case "his": NavigationService.Navigate(new Histogramme(test.Hist("R"), test.Hist("G"), test.Hist("B")));  break; 
                case "fil": test.ConvolutionCirculaire(BaseDeDonnéesMatConv(filtre)); break;
            }
            test.FromImageToFile($"{nomImageDl}.bmp");

            FrontImage.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/{nomImageDl}.bmp", UriKind.RelativeOrAbsolute));
        }

        private void NavClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            int number = 0;
            bool verif = false;
            if (paraImage == "agr" || paraImage == "ret" || paraImage == "rot")
                verif = int.TryParse(BarText.Text, out number);
            else if (paraImage == "sup")
                dataVerif = BarText.Text;

            if (verif)
            {
                dataVerif = Convert.ToString(number);
                BoutonRun.Visibility = Visibility.Visible;
            }
        }

        static double[,] BaseDeDonnéesMatConv(int choix)
        {
            double[,] effet = null;

            switch (choix)
            {
                //Augmenter les contrastes
                case 1: effet = new double[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } }; break;
                //Flou
                case 2: effet = new double[3, 3] { { 0.0625, 0.125, 0.0625 }, { 0.125, 0.25, 0.125 }, { 0.0625, 0.125, 0.0625 } }; break;
                //Gros Flou (de Gauss)
                case 3: effet = new double[5, 5] { { 0.00390625, 0.015625, 0.0234375, 0.015625, 0.00390625 }, { 0.015625, 0.0625, 0.09375, 0.0625, 0.015625 }, { 0.0234375, 0.09375, 0.140625, 0.09375, 0.0234375 }, { 0.015625, 0.0625, 0.09375, 0.0625, 0.015625 }, { 0.00390625, 0.015625, 0.0234375, 0.015625, 0.00390625 } }; break;
                //Amélioration des bords
                case 4: effet = new double[3, 3] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } }; break; ;
                //Détection des bords
                case 5: effet = new double[3, 3] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } }; break; ;
                //Repoussage
                case 6: effet = new double[3, 3] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } }; break;
                //Filtre de Sobel
                case 7: effet = new double[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 0 } }; break;
                //Amélioration de la netteté
                case 8: effet = new double[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } }; break;
                //filtre emboss
                case 9: effet = new double[3, 3] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } }; break;
            }
            return effet;
        }
    }
}
