using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Controls;
using System.Windows;
using System;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Collections.Generic;

namespace MiniProjet_TraitementImage
{
	public partial class Histogramme
	{
		public Histogramme(int[,] matPixelR, int[,] matPixelG, int[,] matPixelB)
		{
			{
				SeriesCollection = new SeriesCollection
		{
			new LineSeries
			{
				Title = "Ligne Rouge",
				Values = PrepareMat(matPixelR),
				Foreground = Brushes.Red,
				ScalesYAt = 0
			},
			new LineSeries
			{
				Title = "Ligne Verte",
				Values = PrepareMat(matPixelG),
				Foreground = Brushes.DodgerBlue,
				ScalesYAt = 1
			},
			new LineSeries
			{
				Title = "Ligne Bleu",
				Values = PrepareMat(matPixelB),
				Foreground = Brushes.Green,
				ScalesYAt = 2
			}
			};

				AxisYCollection = new AxesCollection
			{
			new Axis { Title = "Rouge", Foreground = Brushes.Red },
			new Axis { Title = "Vert", Foreground = Brushes.DodgerBlue },
			new Axis { Title = "Bleu", Foreground = Brushes.Green }
			};

				InitializeComponent();

				DataContext = this;
			}
		}
		public AxesCollection AxisYCollection { get; set; }

		public SeriesCollection SeriesCollection { get; set; }

		private ChartValues<double> PrepareMat(int[,] colorMat)
		{
			int[] intensite = new int[256];

			for (int i = 0; i < colorMat.GetLength(0); i++)
				for (int j = 0; j < colorMat.GetLength(1); j++)
					intensite[colorMat[i, j]]++;

			ChartValues<double> chartValues = new ChartValues<double>(new double[intensite.Length]);

			for (int i = 0; i < intensite.Length; i++)
				chartValues[i] = intensite[i];

			return chartValues;
		}
	}
}