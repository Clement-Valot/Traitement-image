using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code
{
    class Fractale
    {
        private byte Largeur;
        private byte Hauteur;
        private Pixel[,] ImageScratch;

        public Fractale(byte Hauteur, byte Largeur)
        {
            this.Largeur = Largeur;
            this.Hauteur = Hauteur;
        }

        public byte LARGEUR
        {
            get
            {
                return this.Largeur;
            }
            set
            {
                this.Largeur = value;
            }
        }
        public byte HAUTEUR
        {
            get
            {
                return this.Hauteur;
            }
            set
            {
                this.Hauteur = value;
            }
        }

        public byte[] CreationCodeImage()
        {
            byte[] CodeImage = { 66, 77, 230, 4, 0, 0, 0, 0, 0, 0, 54, 0, 0, 0, 40, 0, 0, 0, Largeur, 0, 0, 0, Hauteur, 0, 0, 0, 1, 0, 24, 0, 0, 0, 0, 0, 176, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 54; i < Largeur * Hauteur * 3; i++)
            {
                CodeImage[i] = 0;
            }
            return CodeImage;
        }

        public Pixel[,] CreationImage(byte[] CodeImage)
        {
            int H = 0;
            int L = 0;
            for (int i = 54; i < Largeur * Hauteur * 3; i++)
            {
                if (L >= Largeur)
                {
                    H++;
                    L = 0;
                }
                ImageScratch[H, L].Red = CodeImage[i];
                ImageScratch[H, L].Green = CodeImage[i + 1];
                ImageScratch[H, L].Blue = CodeImage[i + 2];
                L++;
            }
            return ImageScratch;
        }

        public void ConstruireFractale(Pixel[,] ImageScratch)
        {
            for (int x = -Largeur / 2; x < Largeur / 2; x++)
            {
                for (int y = -Hauteur / 2; y < Hauteur / 2; y++)
                {
                    double a = (double)(x) / (double)(Largeur / 4);
                    double b = (double)(y) / (double)(Hauteur / 4);
                    Complexe C = new Complexe(a, b);
                    Complexe Z = new Complexe(0, 0);
                    int itération = 0;
                    while (itération < 100)
                    {
                        Z.AuCarré();
                        Z.Ajouter(C);
                        if (Z.Norme() <= 2.0)
                        {
                            ImageScratch[x, y].Red = 256;
                            ImageScratch[x, y].Green = 256;
                            ImageScratch[x, y].Blue = 256;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

    }
}
