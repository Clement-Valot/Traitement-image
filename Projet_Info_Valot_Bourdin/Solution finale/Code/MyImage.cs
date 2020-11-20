using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Code
{
    class MyImage
    {
        //Champs
        Pixel[,] image;
        private byte[] TailleFichier;
        private byte[] Largeur;
        private byte[] Hauteur;
        private byte[] NbrBits;
        private byte[] Offset;
        private string TypeImage;

        //Constructeurs
        public MyImage(string myfile)
        {
            //Création d'un tableau qui contient toute l'image
            byte[] Myfile = File.ReadAllBytes(myfile);
            //Récupération du type d'image et conversion de l'ascii en string
            TypeImage = "";
            for (int i = 0; i < 2; i++)
            {
                char temp = (char)myfile[i];
                TypeImage += temp;
            }
            //Pour taillefichier,hauteur,largeur et NbrBits : création d'un tableau de byte de la bonne taille et récupération des valeurs dans le grand tableau.
            //A convertir avec Convertir_Endian_To_Int pour obtenir les nombres
            TailleFichier = new byte[4];
            for (int i = 2; i < 6; i++)
            {
                TailleFichier[i - 2] = Myfile[i];
            }
            Offset= new byte[4];
            for (int i = 10; i < 14; i++)
            {
                Offset[i - 10] = Myfile[i];
            }
            Largeur = new byte[4];
            for (int i = 18; i < 22; i++)
            {
                Largeur[i - 18] = Myfile[i];
            }
            Hauteur = new byte[4];
            for (int i = 22; i < 26; i++)
            {
                Hauteur[i - 22] = Myfile[i];
            }
            NbrBits = new byte[2];
            for (int i = 28; i < 30; i++)
            {
                NbrBits[i - 28] = Myfile[i];
            }
            int LargeurInt = Convertir_Endian_To_Int(Largeur);
            int HauteurInt = Convertir_Endian_To_Int(Hauteur);
            image = new Pixel[HauteurInt, LargeurInt];
 
            Pixel[] tabIntermédiaire = new Pixel[myfile.Length - 54];

            int j = 0;
            for (int i = 54; i < myfile.Length; i += 3)
            {
                int Red = myfile[i];
                int Green = myfile[i + 1];
                int Blue = myfile[i + 2];
                tabIntermédiaire[j] = new Pixel(Red, Green, Blue);
                j++;
            }
            j = 0;
            int ligne = 0;
            while (ligne < HauteurInt)
            {
                for (int colonne = 0; colonne < LargeurInt; colonne++)
                {
                    image[ligne, colonne] = tabIntermédiaire[j];
                    j++;
                }
                ligne++;
            }
        }

        public string typeImage
        {
            get { return TypeImage; }
        }
        public int HauteurImage
        {
            get
            {
                int hauteur=Convertir_Endian_To_Int(Hauteur);
                return hauteur;
            }
        }
        public int LargeurImage
        {
            get
            {
                int largeur = Convertir_Endian_To_Int(Largeur);
                return largeur;
            }
        }
        public Pixel [,] Image
        {
            get
            {
                return image;
            }
        }


        //Méthodes
        /// <summary>
        /// prend une instance de MyImage et la transforme en fichier binaire respectant la structure du fichier.bmp ou.csv
        /// </summary>
        /// <param name="file"></param>
        public void From_Image_To_File(string myfile)
        {

        }

        /// <summary>
        /// convertit une séquence d’octet au format little endian en entier.
        /// On parcourt chaque cellule du tableau de byte (séquence d'octets) de la fin vers le début (car on est en little endian et que le bit de poids
        /// le plus faible est au début - il faut donc inverser l'ordre de la séquence pour écrire l'entier car les bits se lisent dans l'ordre inverse des 
        /// entiers) et on rajoute ces bits convertis en string dans un autre string.
        /// A la fin de la boucle for, on convertit ce string en un entier que l'on retourne.
        /// </summary>
        /// <param name="tab"></param>
        /// <returns>L'entier converti à partir de la séquence d'octets.</returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int entier = 0;
            for(int i=0; i<tab.Length;i++)
            {
                entier+=tab[i]*(256^i);
            }
            return entier;
        }

        /// <summary>
        /// convertit un entier en séquence d’octets au format little endian.
        /// On sait que le format little endian n'est autre que la somme des multiplications des puissances de 256 par des entiers.
        /// Il suffit donc de prendre l'entier à convertir en paramètres et de trouver la puissance de 256 à partir de laquelle on dépasse cette entier.
        /// Ensuite, on itère une boucle for qui part de cette puissance -1 et qui va jusqu'à 0 et on divise l'entier par 256 à cette puissance.
        /// On retient le quotient de cette division que l'on insère dans la première case de notre tableau de séquences d'octets (car on est en little-endian)
        /// On prend le reste de cette division et on rééfectue les mêmes calcul que précédemment avec ce dernier en décrémentant la puissance.
        /// </summary>
        /// <param name="val">entier que l'on veut convertir en séquence d'octets au format little endian</param>
        /// <returns>La séquence d'octets en format little endian correspondant à l'entier entré en paramètres</returns>
        public byte[] Convertir_Int_To_Endian(int val)
        {
            bool found = false;
            int n = 0;
            while(found==false)
            {
                int Division = val / (256 ^ n);
                if(Division<=1)
                {
                    found = true;
                }
                else
                {
                    n++;
                }
            }
            int index = 0;
            byte[] tableauEndian = new byte[n];
            int reste = val;
            for(int puissance=n-1;puissance>=0;puissance--)
            {                
                int quotient = reste / (256 ^ puissance);
                tableauEndian[index] = Convert.ToByte(quotient);
                reste = quotient % (256 ^ puissance);
            }
            return tableauEndian;
        }

        /// <summary>
        /// Pour passer une image en couleur en une image grisée, on fait la moyenne des intensités des trois couleurs rouge, vert et bleu.
        /// On parcourt donc la matrice de pixel de notre image de départ et on va modifier la valeur de chacune des trois couleurs dans l'instance 
        /// matrice d'arrivée à l'aide des propriétés public int Red/Green/Blue. On fait la moyenne d'intensité des trois couleurs et on affecte cette même valeur
        /// à chaque paramètre de la classe Pixel (Rouge, Vert, Bleu)
        /// </summary>
        /// <param name="ImageDépart"></param>
        public void NiveauDeGris(MyImage ImageDépart)
        {
            Pixel[,] ImageArrivée = new Pixel[ImageDépart.HauteurImage, ImageDépart.LargeurImage];
            Pixel[,] ImageTransition = ImageDépart.Image;
            for (int ligne = 0; ligne < ImageArrivée.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < ImageArrivée.GetLength(1); colonne++)
                {
                    int CouleurGris= (ImageTransition[ligne, colonne].Red + ImageTransition[ligne, colonne].Green + ImageTransition[ligne, colonne].Blue)/ 3;
                    ImageArrivée[ligne, colonne].Red = CouleurGris;
                    ImageArrivée[ligne, colonne].Green = CouleurGris;
                    ImageArrivée[ligne, colonne].Blue = CouleurGris;
                }
            }
        }

        /// <summary>
        /// Méthode qui prend en paramètre l'image que l'on veut traiter et qui renvoie la même image tourner à 90° dans le sens horaire.
        /// On crée une deuxième matrice de pixel qui fera office d'image d'arrivée. On initialise sa largeur comme la hauteur de l'image de départ 
        /// et sa hauteur comme la largeur de l'image de départ.
        /// </summary>
        /// <param name="ImageDépart"></param>
        public void Rotation90(MyImage ImageDépart)
        {
            int NewHauteur = ImageDépart.LargeurImage;
            int NewLargeur = ImageDépart.HauteurImage;
            Pixel[,] ImageArrivée = new Pixel[NewHauteur, NewLargeur];
            for(int ligne=0;ligne<NewHauteur;ligne++)
            {
                for(int colonne=0;colonne<NewHauteur;colonne++)
                {
                    ImageArrivée[ligne, colonne] = ImageDépart.Image[colonne, NewLargeur - 1 - ligne];
                }
            }

        }

        public void MatriceCovolution(MyImage imageDépart)
        {
            int[,] MatriceFiltre = new int[3, 3];
            int largeur = imageDépart.LargeurImage;
            int hauteur = imageDépart.HauteurImage;
            for (int ligne=0; ligne<hauteur;ligne++)
            {
                for(int colonne = 0; colonne < largeur;colonne++)
                {
                    //On cherche les coins de la matrice de pixels
                    if((ligne==0%(hauteur-1)) && (colonne==0%(largeur-1)))
                    {
                        if(ligne==0 && colonne==0)
                        {

                        }
                        if(ligne==0 && colonne==largeur-1)
                        {

                        }
                    }
                }
            }

        }
        public int MultiplicationMatrice(int[,] matrice1, int Ligne, int Colonne, int décalageLigne, int décalageColonne, int [,] matrice2)
        {
            int somme = 0;
            for(int i=décalageLigne;i<matrice2.GetLength(0);i++)
            {
                for(int j=décalageColonne;j<matrice2.GetLength(1);j++)
                {
                    somme += matrice1[Ligne + i, Colonne + j] * matrice2[i, j];
                }
            }
            return somme;
        }
    }
}
