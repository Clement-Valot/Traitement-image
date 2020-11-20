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
        public Pixel(int Rouge,int Vert, int Bleu)
        {
            this.Rouge = Rouge;
            this.Vert = Vert;
            this.Bleu = Bleu;
            int[] pixel = { Rouge, Vert, Bleu };
        }

        public int Red
        {
            get
            {
                return this.Rouge;
            }
            set
            {
                this.Rouge=value;
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
    }
}
