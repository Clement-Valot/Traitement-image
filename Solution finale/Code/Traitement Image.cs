using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Code
{
    class Traitement_Image
    {
        private MyImage ImageDépart;
        private Pixel[,] ImageArrivée;

        public Traitement_Image(MyImage ImageDépart)
        {
            this.ImageDépart = ImageDépart;
        }

        public void NiveauDeGris(MyImage ImageDépart)
        {
            Pixel[,] ImageArrivée = new Pixel[ImageDépart.HauteurImage, ImageDépart.LargeurImage];
            for(int ligne = 0; ligne<ImageArrivée.GetLength(0);ligne++)
            {
                for(int colonne=0; colonne<ImageArrivée.GetLength(1);colonne++)
                {
                    int CouleurGris=ImageDépart[0,0].NiveauGris
                    ImageArrivée[ligne,colonne]= 
                }
            }

        }
    }
}
