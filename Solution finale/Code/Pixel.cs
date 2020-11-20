using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code
{
    class Pixel
    {
        //Champs
        private int Rouge;
        private int Vert;
        private int Bleu;
        private int[] pixel;

        //Constructeurs
        public Pixel(int Rouge, int Vert, int Bleu)
        {
            this.Rouge = Rouge;
            this.Vert = Vert;
            this.Bleu = Bleu;
        }

        //Propriétés
        public int Red
        {
            get
            {
                return this.Rouge;
            }
            set
            {
                this.Rouge = value;
            }
        }
        public int Green
        {
            get
            {
                return this.Vert;
            }
            set
            {
                this.Vert = value;
            }
        }
        public int Blue
        {
            get
            {
                return this.Bleu;
            }
            set
            {
                this.Bleu = value;
            }
        }

        //Méthodes
        public void MultiplierValeurs(int multiplieur)
        {
            Rouge = Rouge * multiplieur;
            Vert = Vert * multiplieur;
            Bleu = Bleu * multiplieur;
        }
        public void DiviserValeurs(int diviseur)
        {
            if (diviseur == 0)
            {
                if (Rouge < 0)
                {
                    Rouge = 0;
                }
                if (Vert < 0)
                {
                    Vert = 0;
                }
                if (Bleu < 0)
                {
                    Bleu = 0;
                }
                if (Rouge > 255)
                {
                    Rouge = 255;
                }
                if (Vert > 255)
                {
                    Vert = 255;
                }
                if (Bleu > 255)
                {
                    Bleu = 255;
                }
            }
            else
            {
                Rouge = Rouge / diviseur;
                Vert = Vert / diviseur;
                Bleu = Bleu / diviseur;
            }
        }
        public void AdditionnerPixels(Pixel additionnant)
        {
            Rouge += additionnant.Red;
            Vert += additionnant.Green;
            Bleu += additionnant.Blue;
        }

        public void NiveauDeGris()
        {
            int CouleurGris = (Rouge + Vert + Bleu) / 3;
            Rouge = CouleurGris;
            Vert = CouleurGris;
            Bleu = CouleurGris;
        }
    }
}
