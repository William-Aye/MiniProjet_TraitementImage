using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;

namespace MiniProjet_TraitementImage
{
	internal class MyImage
	{
		#region Attributs
		private Bitmap test;
		private string image;
		private int tailleFichier;
		private int offset;
		private int headerInfo;
		private int largeur;
		private int hauteur;
		private int nbBitParCouleur;
		private Uri uriSource;
		private Pixel[,] matPixel;
		#endregion

		#region Propriétés
		public string Image { get { return image; } set { image = value; } }
		public int TailleFichier { get { return tailleFichier; } set { tailleFichier = value; } }
		public int Offset { get { return offset; } set { offset = value; } }
		public int HeaderInfo { get { return headerInfo; } set { headerInfo = value; } }
		public int Largeur { get { return largeur; } set { largeur = value; } }
		public int Hauteur { get { return hauteur; } set { hauteur = value; } }
		public int NbBitParCouleur { get { return nbBitParCouleur; } set { nbBitParCouleur = value; } }
		public Uri UriSource { get { return uriSource; } set { uriSource = value; } }
		public Pixel[,] MatPixel { get { return matPixel; } set { matPixel = value; } }
		#endregion

		#region Constructeurs
		public MyImage(string myFile)
		{
			byte[] byteFile = File.ReadAllBytes(myFile);
			//AffichageImage(byteFile);
			//Console.WriteLine(byteFile.Length);
			image = "" + Convert.ToChar(byteFile[0]) + Convert.ToChar(byteFile[1]);
			tailleFichier = ConvertirEndianToInt(byteFile, 2);
			offset = ConvertirEndianToInt(byteFile, 10);
			headerInfo = ConvertirEndianToInt(byteFile, 14);
			largeur = ConvertirEndianToInt(byteFile, 18);
			hauteur = ConvertirEndianToInt(byteFile, 22);
			nbBitParCouleur = ConvertirEndianToInt2(byteFile, 28);
			matPixel = new Pixel[hauteur, largeur];
			int indic = offset;
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					matPixel[i, j] = new Pixel(byteFile[indic], byteFile[indic + 1], byteFile[indic + 2]);
					indic += 3;
				}
		}

		public MyImage(byte[] byteFile)
		{
			image = "" + Convert.ToChar(byteFile[0]) + Convert.ToChar(byteFile[1]);
			tailleFichier = ConvertirEndianToInt(byteFile, 2);
			offset = ConvertirEndianToInt(byteFile, 10);
			headerInfo = ConvertirEndianToInt(byteFile, 14);
			largeur = ConvertirEndianToInt(byteFile, 18);
			hauteur = ConvertirEndianToInt(byteFile, 22);
			nbBitParCouleur = ConvertirEndianToInt2(byteFile, 28);
			matPixel = new Pixel[hauteur, largeur];
			int indic = offset;
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					matPixel[i, j] = new Pixel(byteFile[indic], byteFile[indic + 1], byteFile[indic + 2]);
					indic += 3;
				}
		}
		#endregion

		#region Méthodes
		public void FromImageToFile(string file)
		{
			if (matPixel != null && matPixel.Length != 0)
			{
				byte[] byteFile = new byte[offset + matPixel.Length * 3];
			byteFile[0] = (byte)image[0];
			byteFile[1] = (byte)image[1];

			byte[] interByte = ConvertirIntToEndian(tailleFichier, 4);
			for (int i = 2; i < 6; i++)
				byteFile[i] = interByte[i - 2];

			interByte = ConvertirIntToEndian(offset, 4);
			for (int i = 10; i < 14; i++)
				byteFile[i] = interByte[i - 10];

			interByte = ConvertirIntToEndian(headerInfo, 4);
			for (int i = 14; i < 18; i++)
				byteFile[i] = interByte[i - 14];

			interByte = ConvertirIntToEndian(largeur, 4);
			for (int i = 18; i < 22; i++)
				byteFile[i] = interByte[i - 18];

			interByte = ConvertirIntToEndian(hauteur, 4);
			for (int i = 22; i < 26; i++)
				byteFile[i] = interByte[i - 22];

			interByte = ConvertirIntToEndian(nbBitParCouleur, 2);
			byteFile[28] = interByte[0];
			byteFile[29] = interByte[1];

			int indic = offset;
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					byteFile[indic] = matPixel[i, j].B;
					byteFile[indic + 1] = matPixel[i, j].G;
					byteFile[indic + 2] = matPixel[i, j].R;
					indic += 3;
				}
				try
				{
					File.WriteAllBytes(file, byteFile);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					Console.WriteLine("Erreur : veuiller donc recommencer");
				}
			}
			else
			{
				Console.WriteLine("Il y a un problème avec la matrice de pixel");
			}
		}

		public void AffichageImage(byte[] byteFile)
		{
			for (int i = 0; i < offset; i++)
				Console.Write($"{byteFile[i]}. ");
			Console.WriteLine();

			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
					Console.Write($"{matPixel[i, j].B},{matPixel[i, j].G},{matPixel[i, j].R}; ");
			Console.WriteLine();
		}

		#region Conversions
		public int ConvertirEndianToInt(byte[] data, int startIndex)
		{
			int num = 0;
			for (int i = startIndex; i < startIndex + 4; i++)
				num += (int)Math.Pow(256, (i - startIndex)) * data[i];

			return num;
		}

		public int ConvertirEndianToInt2(byte[] data, int startIndex)
		{
			int num = 0;
			for (int i = startIndex; i < startIndex + 2; i++)
				num += (int)Math.Pow(256, (i - startIndex)) * data[i];

			return num;
		}

		public byte[] ConvertirIntToEndian(int val, int taille)
		{
			byte[] retour = new byte[taille];
			for (int i = 0; i < taille; i++, val /= 256)
				retour[i] = (byte)(val % 256);

			return retour;
		}
		public byte VerifByte(double a)
		{
			if (a < 0) return Convert.ToByte(0);
			else if (a > 255) return Convert.ToByte(255);
			else return Convert.ToByte(a);
		}
		#endregion

		#region ModifFormats
		public void AgrandirImage(int facteur)
		{
			hauteur *= facteur;
			largeur *= facteur;
			Pixel[,] matArriv = new Pixel[hauteur, largeur];
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
					matArriv[i, j] = matPixel[i / facteur, j / facteur];

			matPixel = matArriv;
		}

		public void ReduireImage(int facteur)
		{
			hauteur /= facteur;
			largeur /= facteur;
			Pixel[,] matArriv = new Pixel[hauteur, largeur];
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
					matArriv[i, j] = matPixel[i * facteur, j * facteur];

			matPixel = matArriv;
		}
		#endregion

		public void NuanceDeGris()
		{
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					byte valGris = (byte)((matPixel[i, j].R * 0.3) + (matPixel[i, j].G * 0.59) + (matPixel[i, j].B * 0.11));
					matPixel[i, j].B = valGris;
					matPixel[i, j].G = valGris;
					matPixel[i, j].R = valGris;
				}
		}

		public void NoirEtBlanc()
		{
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					byte valGris = (byte)((matPixel[i, j].R * 0.3) + (matPixel[i, j].G * 0.59) + (matPixel[i, j].B * 0.11));
					if (valGris > 125)
						valGris = 255;
					else
						valGris = 0;
					matPixel[i, j].B = valGris;
					matPixel[i, j].G = valGris;
					matPixel[i, j].R = valGris;
				}

		}

		public void Superposition(string file)
		{
			MyImage image = new MyImage(file);
			int hauteurMax, largeurMax;
			if (image.hauteur > hauteur)
				hauteurMax = image.hauteur;
			else
				hauteurMax = hauteur;
			if (image.largeur > largeur)
				largeurMax = image.largeur;
			else
				largeurMax = image.largeur;
			Pixel[,] matInter = new Pixel[hauteurMax, largeurMax];
			for (int i = 0; i < hauteurMax; i++)
				for (int j = 0; j < largeurMax; j++)
				{
					if (i < image.hauteur
					 && j < image.largeur
					 && i < hauteur
					 && j < largeur)
						matInter[i, j] = new Pixel((byte)((image.MatPixel[i, j].B + matPixel[i, j].B) / 2),
												 (byte)((image.MatPixel[i, j].G + matPixel[i, j].G) / 2),
												 (byte)((image.MatPixel[i, j].R + matPixel[i, j].R) / 2));
					else if (i < image.hauteur
						  && j < image.largeur)
						matInter[i, j] = new Pixel(image.MatPixel[i, j].B, image.MatPixel[i, j].G, image.MatPixel[i, j].R);
					else
						matInter[i, j] = new Pixel(matPixel[i, j].B, matPixel[i, j].G, matPixel[i, j].R);
				}

			hauteur = hauteurMax;
			largeur = largeurMax;
			matPixel = matInter;
		}

		public void MiroirMarrantPourMathis()
		{
			Pixel[,] matInter = (Pixel[,])matPixel.Clone();
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					matPixel[i, j].B = matInter[i, largeur - j - 1].B;
					matPixel[i, j].G = matInter[i, largeur - j - 1].G;
					matPixel[i, j].R = matInter[i, largeur - j - 1].R;
				}

			matPixel = matInter;
		}

		public void Miroir()
		{
			Pixel[,] matInter = new Pixel[hauteur, largeur];
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
					matInter[i, j] = new Pixel(matPixel[i, j].B, matPixel[i, j].G, matPixel[i, j].R);

			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					matPixel[i, j].B = matInter[i, largeur - (j + 1)].B;
					matPixel[i, j].G = matInter[i, largeur - (j + 1)].G;
					matPixel[i, j].R = matInter[i, largeur - (j + 1)].R;
				}

			matPixel = matInter;
		}

		public void Rotation(double angle)
		{
			double rad = Math.PI * angle / 180;
			int l = (int)Math.Max(Math.Abs(hauteur * Math.Sin(rad) + largeur * Math.Cos(rad)), Math.Abs(-hauteur * Math.Sin(rad) + largeur * Math.Cos(rad)));
			int h = (int)Math.Max(Math.Abs(hauteur * Math.Cos(rad) - largeur * Math.Sin(rad)), Math.Abs(-hauteur * Math.Cos(rad) - largeur * Math.Sin(rad)));
			l++; h++;
			//Pour que l'image puisse pouvoir être afficher il faut que la formule de la taille du
			//fichier qui est : (l * nbBitParCouleur / 32) * 4 * h soit égale au nombre
			//de pixel dans la matrice de pixel donc la formule : h * l * 3.
			//Il y a besoin de vérifier ceci seulement si il y a un problème sur l'arondi par exemple l et h
			bool changement = true;
			while ((l * nbBitParCouleur / 32) * 4 * h != h * l * 3)
			{
				if (changement)
				{
					h++;
					changement = false;
				}
				else
				{
					l++;
					changement = true;
				}
			}
			double refX = h / 2, refY = l / 2;
			Pixel[,] matArriv = new Pixel[h, l];
			int hInter, lInter;
			double iCalc, jCalc;
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					iCalc = i - hauteur / 2.0;
					jCalc = j - largeur / 2.0;
					lInter = (int)(iCalc * Math.Sin(rad) + jCalc * Math.Cos(rad) + refY);
					hInter = (int)(iCalc * Math.Cos(rad) - jCalc * Math.Sin(rad) + refX);
					if (hInter >= 0 && hInter < h && lInter >= 0 && lInter < l)
					{
						matArriv[hInter, lInter] = matPixel[i, j];
					}
				}
			for (int i = 0; i < h; i++)
			{
				for (int j = 0; j < l; j++)
				{
					if (matArriv[i, j] == null)
					{
						matArriv[i, j] = new Pixel(0, 0, 0);
					}
				}
			}

			hauteur = h;
			largeur = l;
			matPixel = matArriv;
		}

		public void Convolution(double[,] noyauConv)
		{
			Pixel[,] matArriv = null;

			if (matPixel != null && matPixel.Length > 0
				&& noyauConv != null && noyauConv.Length > 0)
			{
				matArriv = new Pixel[matPixel.GetLength(0), matPixel.GetLength(1)];

				for (int i = 0; i < matPixel.GetLength(0); i++)
					for (int j = 0; j < matPixel.GetLength(1); j++)
					{
						double valR = 0, valG = 0, valB = 0;

						for (int v = -1; v < noyauConv.GetLength(0)-1; v++)
							for (int w = -1; w < noyauConv.GetLength(1)-1; w++)
							{
								if (i + v >= 0 && i + v < matPixel.GetLength(0) &&
									j + w >= 0 && j + w < matPixel.GetLength(1))
								{
									valR += noyauConv[v + 1, w + 1] * Convert.ToDouble(matPixel[i + v, j + w].R);
									valG += noyauConv[v + 1, w + 1] * Convert.ToDouble(matPixel[i + v, j + w].G);
									valB += noyauConv[v + 1, w + 1] * Convert.ToDouble(matPixel[i + v, j + w].B);
								}
							}
						matArriv[i, j] = new Pixel(VerifByte(valB), VerifByte(valG), VerifByte(valR));

					}
			}
			matPixel = matArriv;
		}

		public void ConvolutionCirculaire(double[,] noyauConv)
		{
			Pixel[,] matArriv = null;

			if (matPixel != null && matPixel.Length > 0
				&& noyauConv != null && noyauConv.Length > 0)
			{
				matArriv = new Pixel[hauteur, largeur];
				int valh = noyauConv.GetLength(0) / 2;
				int vall = noyauConv.GetLength(1) / 2;

				for (int i = 0; i < hauteur; i++)
					for (int j = 0; j < largeur; j++)
					{
						double valR = 0, valG = 0, valB = 0;

						for (int v = -valh; v <= valh; v++)
							for (int w = -vall; w <= vall; w++)
							{
								int tempHaut = i + v;
								int tempLarg = j + w;

								if (tempHaut < 0) tempHaut = hauteur + tempHaut;
								if (tempLarg < 0) tempLarg = largeur + tempLarg;
								if (tempHaut > hauteur - 1) tempHaut = tempHaut - hauteur;
								if (tempLarg > largeur - 1) tempLarg = tempLarg - largeur;

								valR += noyauConv[v + valh, w + vall] * Convert.ToDouble(matPixel[tempHaut, tempLarg].R);
								valG += noyauConv[v + valh, w + vall] * Convert.ToDouble(matPixel[tempHaut, tempLarg].G);
								valB += noyauConv[v + valh, w + vall] * Convert.ToDouble(matPixel[tempHaut, tempLarg].B);
							}
						matArriv[i, j] = new Pixel(VerifByte(valB), VerifByte(valG), VerifByte(valR));

					}
			}
			matPixel = matArriv;
		}

		public int[,] Hist(string ind)
		{
			int[,] res = new int[hauteur, largeur];
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
					switch (ind)
					{
						case "R": res[i, j] = matPixel[i, j].R; break;
						case "G": res[i, j] = matPixel[i, j].G; break;
						case "B": res[i, j] = matPixel[i, j].B; break;
					}

			return res;
		}

		#region CodageIm
		public byte[] CoderImage(string fichier)
		{

			byte[] byteFile = File.ReadAllBytes(fichier);

			byte[] nouvelleImage = new byte[offset + matPixel.Length * 3];
			nouvelleImage[0] = (byte)image[0];
			nouvelleImage[1] = (byte)image[1];

			byte[] interByte = ConvertirIntToEndian(tailleFichier, 4);
			for (int i = 2; i < 6; i++)
				nouvelleImage[i] = interByte[i - 2];

			interByte = ConvertirIntToEndian(offset, 4);
			for (int i = 10; i < 14; i++)
				nouvelleImage[i] = interByte[i - 10];

			interByte = ConvertirIntToEndian(headerInfo, 4);
			for (int i = 14; i < 18; i++)
				nouvelleImage[i] = interByte[i - 14];

			interByte = ConvertirIntToEndian(largeur, 4);
			for (int i = 18; i < 22; i++)
				nouvelleImage[i] = interByte[i - 18];

			interByte = ConvertirIntToEndian(hauteur, 4);
			for (int i = 22; i < 26; i++)
				nouvelleImage[i] = interByte[i - 22];

			interByte = ConvertirIntToEndian(nbBitParCouleur, 2);
			nouvelleImage[28] = interByte[0];
			nouvelleImage[29] = interByte[1];

			int indic = offset;
			byte inter;
			byte inter2;
			int compteurTotIAC = 0; //Compteur info image a coder
			byte[] donnerIAC = new byte[2];
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					if (i * largeur + j < ConvertirEndianToInt(byteFile, 10) /*Offset de l'image a coder*/)
					{
						donnerIAC[0] = (byte)(byteFile[compteurTotIAC] >> 4);
						donnerIAC[1] = (byte)(byteFile[compteurTotIAC] & 0b00001111);
						nouvelleImage[indic] = (byte)((matPixel[i, j].B & 0b11110000) | donnerIAC[0]);
						nouvelleImage[indic + 1] = (byte)((matPixel[i, j].G & 0b11110000) | donnerIAC[1]);
						nouvelleImage[indic + 2] = (byte)(matPixel[i, j].G & 0b11110000);
						compteurTotIAC++;
						indic += 3;
					}
					else if (compteurTotIAC < ConvertirEndianToInt(byteFile, 2) /*Taille du fichier*/)
					{
						inter = (byte)(matPixel[i, j].B & 0b11110000);
						inter2 = (byte)(byteFile[compteurTotIAC] >> 4);
						nouvelleImage[indic] = (byte)(inter | inter2);
						inter = (byte)(matPixel[i, j].G & 0b11110000);
						inter2 = (byte)(byteFile[compteurTotIAC + 1] >> 4);
						nouvelleImage[indic + 1] = (byte)(inter | inter2);
						inter = (byte)(matPixel[i, j].R & 0b11110000);
						inter2 = (byte)(byteFile[compteurTotIAC + 2] >> 4);
						nouvelleImage[indic + 2] = (byte)(inter | inter2);
						compteurTotIAC += 3;
						indic += 3;
					}
					else
					{
						nouvelleImage[indic] = matPixel[i, j].B;
						nouvelleImage[indic + 1] = matPixel[i, j].G;
						nouvelleImage[indic + 2] = matPixel[i, j].R;
						indic += 3;
					}
				}
			return (nouvelleImage);
		}

		public void DecoderImage(string imageCacher)
		{
			byte[] donnerIAC = new byte[2];
			byte inter;
			List<byte> imageCacherBL = new List<byte>();
			for (int i = 0; i < hauteur; i++)
				for (int j = 0; j < largeur; j++)
				{
					if (imageCacherBL.Count < 54)
					{
						donnerIAC[0] = (byte)(matPixel[i, j].B << 4);
						donnerIAC[1] = (byte)(matPixel[i, j].G & 0b00001111);
						imageCacherBL.Add((byte)(donnerIAC[0] | donnerIAC[1]));
					}
					else
					{
						imageCacherBL.Add((byte)(matPixel[i, j].B << 4));
						imageCacherBL.Add((byte)(matPixel[i, j].G << 4));
						imageCacherBL.Add((byte)(matPixel[i, j].R << 4));
					}
				}
			try
			{
				File.WriteAllBytes(imageCacher, imageCacherBL.ToArray());
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine("Erreur : veuiller donc recommencer");
			}
		}
		#endregion

		public void Fractalle(int largeur, int hauteur)
		{
			int lPossible = largeur, hPossible = hauteur;
			bool changement = true;
			while ((lPossible * nbBitParCouleur / 32) * 4 * hPossible != hPossible * lPossible * 3)
			{
				if (changement)
					hPossible++;
				else
					lPossible++;

				changement = !changement;
			}

			if (lPossible <= 0
				|| hPossible <= 0
				|| lPossible > 30000
				|| hPossible > 30000)
			{
				return;
			}

			Pixel[,] matRetour = new Pixel[hPossible, lPossible];
			Complex nbComplexe;
			int nbIteration;
			int couleur;
			for (int i = -hPossible; i < hPossible; i++)
				for (int j = -lPossible; j < lPossible; j++)
				{
					nbComplexe = new Complex((j + 0.0) / lPossible, (i + 0.0) / hPossible);
					nbIteration = Mandelbrot(nbComplexe);
					couleur = (255 - (nbIteration * 255 / 50));
					matRetour[(i + hPossible) / 2, (j + lPossible) / 2] = new Pixel((byte)(couleur * 0.46), (byte)(couleur * 0.38), (byte)(couleur * 0.09));
				}

			this.hauteur = hPossible;
			this.largeur = lPossible;
			matPixel = matRetour;
		}

		public int Mandelbrot(Complex nbComplexe)
		{
			Complex z = 0;
			int n = 0;
			while (Complex.Abs(z) < 2 && n < 50)
			{
				z = Complex.Pow(z, 2) + nbComplexe;
				n++;
			}
			return n;
		}
		#endregion

	}
}