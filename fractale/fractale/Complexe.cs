using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code
{
    class Complexe
    {
        public double PartieRe;
        public double PartieIm;

        public Complexe(double PartieRe, double PartieIm)
        {
            this.PartieRe = PartieRe;
            this.PartieIm = PartieIm;
        }

        /// <summary>
        /// Cette méthode met un nombre complexe au carré et ressort la nouvelle partie imaginaire de ce nombre ainsi que sa
        /// nouvelle partie réelle : (a+ib)²=a²-b²+2aib --> PartieIm = 2ab et PartieRe = a²-b²
        /// </summary>
        public void AuCarré()
        {
            double temp = (PartieRe * PartieRe) - (PartieIm * PartieIm);
            PartieIm = 2 * PartieRe * PartieIm;
            PartieRe = temp;
        }

        public double Norme()
        {
            double norme = Math.Sqrt((PartieRe * PartieRe) + (PartieIm * PartieIm));
            return norme;
        }

        public void Ajouter(Complexe C)
        {
            PartieRe += C.PartieRe;
            PartieIm += C.PartieIm;
        }
    }
}
