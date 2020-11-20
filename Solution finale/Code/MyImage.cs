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
        private Pixel[,] image;
        private int TailleFichier;
        private byte[] TabTailleFichier = new byte[4];
        private int largeur;
        private byte[] TabLargeur = new byte[4];
        private int hauteur;
        private byte[] TabHauteur = new byte[4];
        private int nbrbits;
        private byte[] NbrBits = new byte[2];
        private int offset;
        private byte[] Offset = new byte[4];
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
                char temp = (char)Myfile[i];
                TypeImage += temp;
            }
            //Pour taillefichier,hauteur,largeur et NbrBits : création d'un tableau de byte de la bonne taille et récupération des valeurs dans le grand tableau.
            //A convertir avec Convertir_Endian_To_Int pour obtenir les nombres
            for (int i = 2; i < 6; i++)
            {
                TabTailleFichier[i - 2] = Myfile[i];
            }
            TailleFichier = Convertir_Endian_To_Int(TabTailleFichier);

            for (int i = 10; i < 14; i++)
            {
                Offset[i - 10] = Myfile[i];
            }
            offset = Convertir_Endian_To_Int(Offset);

            for (int i = 18; i < 22; i++)
            {
                TabLargeur[i - 18] = Myfile[i];
            }
            largeur = Convertir_Endian_To_Int(TabLargeur);

            for (int i = 22; i < 26; i++)
            {
                TabHauteur[i - 22] = Myfile[i];
            }
            hauteur = Convertir_Endian_To_Int(TabHauteur);

            for (int i = 28; i < 30; i++)
            {
                NbrBits[i - 28] = Myfile[i];
            }
            nbrbits = Convertir_Endian_To_Int(NbrBits);

            //Après avoir ordonné notre header et définit les caractéristiques de notre image en endian et en int, on va créer notre image
            //à proprement parler en l'agencant en une matrice de pixel de dimensions trouvées précédemment dans le header.
            //Les nombres suivants le header correspondent respectivement à l'intensité de la couleur rouge suivie de l'intensité de la couleur 
            //verte suivie de l'intensité de la couleur bleue et ce schéma se répète autant de fois que la taille de l'image (hauteur*largeur)
            //C'est pour cela que l'on prend Myfile[k], Myfile[k + 1], Myfile[k + 2] et que l'on incrémente k de 3 et non de 1 pour ne pas reprendre les mêmes valeurs.
            image = new Pixel[hauteur, largeur];
            int k = 54; //On part de k=54 car de 0 à 53, les valeurs du tableau MyFile correspondent aux valeurs du header
            //On parcourt chaque pixel de notre matrice de pixel avec une double boucle for en prenant soin de bien partir en bas à gauche
            for (int i = hauteur - 1; i >= 0; i--)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(Myfile[k], Myfile[k + 1], Myfile[k + 2]);
                    k += 3;// On passe au trois données du pixel suivant
                }
            }
        }

        //Propriétés
        public string typeImage
        {
            get { return TypeImage; }
        }
        public int HauteurImage
        {
            get
            {
                return hauteur;
            }
            set
            {
                this.hauteur = value;
            }
        }
        public int LargeurImage
        {
            get
            {
                return largeur;
            }
            set
            {
                this.largeur = value;
            }
        }
        public Pixel[,] Image
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
        public void From_Image_To_File(string NewFile)
        {
            //byte[] MyFile = File.ReadAllBytes(myFile);
            byte[] tab = new byte[TailleFichier];
            //les valeurs suivantes sont toujours les mêmes pour les images que l'on traite
            tab[0] = 66;
            tab[1] = 77;
            for (int i = 2; i < 6; i++)
            {
                tab[i] = TabTailleFichier[i - 2];
            }
            for (int i = 6; i < 10; i++)
            {
                tab[i] = 0;
            }
            for (int i = 14; i < 18; i++)
            {
                tab[i] = Offset[i - 14];
            }
            for (int i = 18; i < 22; i++)
            {
                tab[i] = TabLargeur[i - 18];
            }
            for (int i = 22; i < 26; i++)
            {
                tab[i] = TabHauteur[i - 22];
            }
            //les valeurs suivantes sont toujours les mêmes pour les images que l'on traite
            tab[26] = 1;
            tab[27] = 0;
            for (int i = 28; i < 30; i++)
            {
                tab[i] = NbrBits[i - 28];
            }
            for (int i = 30; i < 34; i++)
            {
                tab[i] = 0;
            }
            tab[34] = 176;
            tab[35] = 4;
            for (int i = 36; i < 54; i++)
            {
                tab[i] = 0;
            }
            int index = 54;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)

                {
                    tab[index] = Convert.ToByte(image[i, j].Red);
                    tab[index + 1] = Convert.ToByte(image[i, j].Green);
                    tab[index + 2] = Convert.ToByte(image[i, j].Blue);
                    index += 3;
                }
            }
            File.WriteAllBytes(NewFile, tab);
        }

        /// <summary>
        /// convertit une séquence d’octet au format little endian en entier.
        /// 
        /// </summary>
        /// <param name="tab"></param>
        /// <returns>L'entier converti à partir de la séquence d'octets.</returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int entier = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                entier += tab[i] * ((int)Math.Pow(256, i));
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
            while (found == false)
            {
                int Division = val / (int)Math.Pow(256, n);
                if (Division <= 1)
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
            for (int puissance = n - 1; puissance >= 0; puissance--)
            {
                int quotient = Convert.ToInt32(reste / (Math.Pow(256, puissance)));
                tableauEndian[index] = Convert.ToByte(quotient);
                reste = Convert.ToInt32(quotient % (Math.Pow(256, puissance)));
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
        public void PassageGris()
        {
            for (int ligne = image.GetLength(0) - 1; ligne >= 0; ligne--)
            {
                for (int colonne = 0; colonne < image.GetLength(1); colonne++)
                {
                    image[ligne, colonne].NiveauDeGris();
                }
            }
        }

        /// <summary>
        /// Pour passer d'une image en couleur à une image composée uniquement de pixels noirs [0,0,0] et blancs [255,255,255]
        /// </summary>
        public void NoirEtBlanc()
        {
            //Le seuil correspond à la valeur moyenne de la somme des trois intensités max (255) d'un pixel
            //Si la somme des 3 intensités de couleur est au dessus de ce seuil alors on a un pixel blanc et noir sinon
            int seuil = 255 * 3 / 2;
            for (int ligne = image.GetLength(0) - 1; ligne >= 0; ligne--)
            {
                for (int colonne = 0; colonne < image.GetLength(1); colonne++)
                {
                    int somme = image[ligne, colonne].Red + image[ligne, colonne].Green + image[ligne, colonne].Blue;
                    if (somme <= seuil) //noir
                    {
                        image[ligne, colonne].Red = 0;
                        image[ligne, colonne].Green = 0;
                        image[ligne, colonne].Blue = 0;
                    }
                    else //blanc
                    {
                        image[ligne, colonne].Red = 255;
                        image[ligne, colonne].Green = 255;
                        image[ligne, colonne].Blue = 255;
                    }
                }
            }
        }

        /// <summary>
        /// Méthode qui prend en paramètre l'image que l'on veut traiter et qui renvoie la même image tourner à 90° dans le sens horaire.
        /// On crée une deuxième matrice de pixel qui fera office d'image d'arrivée. On initialise sa largeur comme la hauteur de l'image de départ 
        /// et sa hauteur comme la largeur de l'image de départ.
        /// </summary>
        /// <param name="ImageDépart"></param>
        public void Rotation90()
        {
            int NewHauteur = largeur;
            int NewLargeur = hauteur;
            Pixel[,] ImageArrivée = new Pixel[NewHauteur, NewLargeur];
            for (int ligne = hauteur - 1; ligne >= 0; ligne--)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    ImageArrivée[ligne, colonne] = Image[hauteur - 1 - colonne, ligne];
                }
            }
        }

        /// <summary>
        /// Méthode qui permet d'agrandir une image (mais n'effectue pas un zoom!).
        /// on doit donc agrandir les dimensions et la taille de l'image en fonction du facteur d'agrandissement rentré en paramètres.
        /// Le principe est de dupliquer le pixel à la position [ligne,colonne] autant de fois que le facteur d'agrandissement.
        /// Donc si on double la taille d'une image, le pixel à la position [0,0] sera aussi à la position [0,1], [1,0] et [1,1] de l'image d'arrivée.
        /// 
        /// Pour ce faire, on initialise une nouvelle image à ces dimensions puis on parcourt chaque pixel de l'image de départ.
        /// Dans la deuxième boucle for, on remet une autre double boucle for qui va cette fois-ci parcourir chaque pixel de l'iùmage d'arrivée.
        /// On Associe à chacune de ces cellules le pixel de l'image de départ sur lequel on est dans la première boucle for. 
        /// 
        /// </summary>
        /// <param name="facteurAgrandissement"></param>
        public void Agrandir(int facteurAgrandissement)
        {
            int Newhauteur = hauteur * facteurAgrandissement;
            int Newlargeur = largeur * facteurAgrandissement;
            Pixel[,] ImageArrivée = new Pixel[Newlargeur, Newhauteur];
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    //On réinitialise les valeurs des lignes et colonnes de départ de l'itération à chaque fois
                    //que l'on change de pixel de l'image de départ
                    int newligne = ligne * facteurAgrandissement;
                    int newcolonne = colonne * facteurAgrandissement;
                    for (int i = newligne; i < newligne + facteurAgrandissement; i++)
                    {
                        for (int j = newcolonne; j < colonne + facteurAgrandissement; j++)
                        {
                            ImageArrivée[i, j] = Image[ligne, colonne];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Pour rétrécir une image, on va faire la méthode "inverse" de l'agrandissement.
        /// C'est à dire que pour un pixel de l'image d'arrivée, on va prendre un nombre de pixels de l'image de départ
        /// correspondant à une matrice carrée de pixels de taille le facteur d'agrandissement et partant du pixel en haut à gauche.
        /// Ensuite, on fait la moyenne de toutes les intensités de couleurs (RGB) des pixels et on rentre ces valeurs dans 
        /// le pixel de l'image d'arrivée.
        /// </summary>
        /// <param name="facteurRétrécissement">valeur entière correpondant au taux par lequelle on va rétrécir l'image</param>
        public void Retrecir(int facteurRétrécissement)
        {
            //Pour obtenir la hauteur et la largeur de notre matrice d'arrivée, on effectue une division de la hauteur d'origine
            //par le facteur de rétrécissement. Cependant lorsque l'on fait ce genre de division, on renvoie un entier alors que
            //les divisions ne se font pas toujours entre nombres multiples entre eux. On aura donc par la suite un problème 
            //d'indexation. Il faut donc aussi prendre en compte le reste de ce quotient car il devient non négligeable dans le 
            //cas où le facteur de rétrécissement est très grand. Dans le cas où la hauteur/largeur n'est pas divisible par 
            //le facteur de rétrécissement, il faut donc rajouter une ligne/colonne dans l'image d'arrivée. C'est pour cela
            //que dans les formules si dessous on ajoute à la hauteur le facteur- le reste du quotient de la hauteur pour ensuite
            //le diviser par le facteur car cela nous donne un nombre entier correspondant à une unité de plus que le quotient 
            //traditionnel de la hauteur par le facteur.
            int Newhauteur = (hauteur + (facteurRétrécissement - hauteur % facteurRétrécissement)) / facteurRétrécissement;
            int Newlargeur = (largeur + (facteurRétrécissement - largeur % facteurRétrécissement)) / facteurRétrécissement;
            Pixel[,] ImageArrivée = new Pixel[Newlargeur, Newhauteur];
            for (int ligne = 0; ligne < Newhauteur; ligne++)
            {
                for (int colonne = 0; colonne < Newlargeur; colonne++)
                {
                    //Comme à chaque fois que l'on change de pixel dans la matrice d'arrivée, on change de matrice de pixel
                    //dans la matrice de départ. Donc lorsque l'on a parcouru une matrice [3,3] de pixels, on passe à la suivante
                    //et on part dès lors de l'index [ligne,colonne*facteurRétrécissement] car on doit se décaler vu que les cases
                    //colonne jusqu'à colonne+facteurRétrécissement-1 auront déjà été utilisées.
                    Pixel pixeltransition = new Pixel(0, 0, 0);
                    //Ici, on doit vérifier que l'on est pas dans "l'extra colonne/ligne" de la nouvelle matrice pour éviter
                    //des erreurs d'indexation
                    int newligne = ligne * facteurRétrécissement;
                    int newcolonne = colonne * facteurRétrécissement;
                    int endligne = 0;
                    int endcolonne = 0;
                    if (ligne == Newhauteur - 1)
                    {
                        endligne = hauteur % facteurRétrécissement;
                    }
                    if (colonne == Newlargeur - 1)
                    {
                        endcolonne = largeur % facteurRétrécissement;
                    }

                    for (int i = newligne; i < newligne + facteurRétrécissement - endligne; i++)
                    {
                        for (int j = newcolonne; j < newcolonne + facteurRétrécissement - endcolonne; j++)
                        {
                            pixeltransition.AdditionnerPixels(image[newligne, newcolonne]);
                        }
                    }
                    //Une fois que l'on a additionner les valeurs des couleurs RGB des pixels, il faut diviser ces
                    //3 valeurs par le nombre de pixels étudier afin d'obtenir la moyenne des intensités de couleur
                    int diviseur = facteurRétrécissement * facteurRétrécissement;
                    pixeltransition.DiviserValeurs(diviseur);
                    ImageArrivée[ligne, colonne] = pixeltransition;
                }
            }
        }

        /// <summary>
        /// Méthode qui transforme l'image en son reflet dans un miroir placé à sa droite.
        /// Les pixels à gauche vont à droite symétriquement par rapport à l'axe du milieu de l'image.
        /// </summary>
        public void Miroir()
        {
            Pixel[,] ImageArrivée = new Pixel[hauteur, largeur];
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur - 1; colonne++)
                {
                    ImageArrivée[ligne, colonne] = Image[ligne, largeur - colonne - 1];
                }
            }
        }

        /// <summary>
        /// Méthode qui va appliquer le filtre sur l'image grâce à la matrice de convolution choisie à l'aide la méthode ChoisirFiltre.
        /// Pour appliquer le filtre, il faut multiplier chaque case et son contour par la matrice de convolution. 
        /// La case et son contour doivent former une matrice carrée de dimensions égales à celles de la matrice de convolution.
        /// Mais il ne s'agit pas d'une multiplication de matrices classiques: on doit multiplier le 'pixel' en haut à gauche de la matrice de l'image
        /// par le nombre en haut à gauche de la matrice de convolution et ainsi de suite jusqu'à la fin.
        /// La somme de tous ses termes donne le nouveau pixel de l'image filtrée. On répète cette opération pour tous les pixels de l'image de départ.
        /// 
        /// Cependant plusieurs problèmes se posent:
        /// - le premier est un problème d'index. En effet, si on prend le pixel en haut à gauche de l'image, ce dernier n'a pas 
        /// de contour parfait, il lui manque cinq pixels si la matrice de convolution est de dimension 3*3. Il faut donc prendre
        /// chaque case cas par cas pour sélectionner la bonne matrice de contour de pixel.
        /// - le deuxième est que l'on traite des pixels et non des nombres ce qui fait que la multiplication est plus complexe:
        /// il faudrait multiplier chaque composante RGB du pixel par le nombre de la matrice de convolution puis additionner
        /// ces composantes pour donner le pixel final.
        /// </summary>
        public void MatriceFiltre()
        {
            int[,] MatriceConvolution = new int[3, 3];
            int coefficient = 0;//ce coefficient correspond à la somme des termes de la matrice de convolution
            Console.WriteLine("Quel filtre voulez-vous appliquer à votre image?");
            Console.WriteLine("- Flou (Tapez F)");
            Console.WriteLine("- Détection de contour (Tapez D)");
            Console.WriteLine("- Repoussage (Tapez R)");
            Console.WriteLine("- Renforcements des bords (Tapez B)");
            char touche = Convert.ToChar(Console.ReadLine());
            while (touche != 'F' && touche != 'D' && touche != 'R' && touche != 'B')
            {
                Console.WriteLine("Votre saisie est erronée. Veuillez réessayer (Faites attention à bien écrire les lettres en majuscules): ");
                touche = Convert.ToChar(Console.ReadLine());
            }
            if (touche == 'F')
            {
                int[,] matrice = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                MatriceConvolution = matrice;
                coefficient = 9;
            }
            if (touche == 'D')
            {
                int[,] matrice = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                MatriceConvolution = matrice;
                coefficient = 0;
            }
            if (touche == 'R')
            {
                int[,] matrice = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
                MatriceConvolution = matrice;
                coefficient = 1;
            }
            if (touche == 'B')
            {
                int[,] matrice = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
                MatriceConvolution = matrice;
                coefficient = 0;
            }
            Pixel[,] ImageArrivée = new Pixel[hauteur, largeur];
            Pixel[,] ImageTransition = image;
            int NbrCases = (hauteur / 2) + 1;//nombre de lignes/colonnes après le point central de la matrice de convolution           
            //On parcourt chaque pixels de l'image avec la double boucle for
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    Pixel pixeltransition = new Pixel(0, 0, 0);
                    int décalageLigneFin = 0;
                    int décalageLigneDébut = 0;
                    int décalageColonneFin = 0;
                    int décalageColonneDébut = 0;
                    //On veut savoir si les cases de la matrice de convolution sont bien dans la matrice que l'on veut filtrer
                    if (ligne - NbrCases < 0)
                    //si le numéro de la ligne moins NbrCases est inférieur à 0, on est hors-index au dessus de la matrice.
                    //Il faut donc redéfinir la ligne d'où l'on commence la double itération pour faire la multiplication.
                    {
                        décalageLigneDébut = ligne + NbrCases;
                    }
                    if (ligne + NbrCases > hauteur - 1)//De même si la ligne + NbrCases est supérieure à la hauteur -1 de la matrice
                    {
                        décalageLigneFin = ligne - (hauteur - 1) + NbrCases;
                    }
                    if (colonne - NbrCases < 0)
                    {
                        décalageColonneDébut = colonne + NbrCases;
                    }
                    if (colonne + NbrCases > largeur - 1)
                    {
                        décalageColonneFin = colonne - (largeur - 1) + NbrCases;
                    }
                    //on doit déterminer les coordonnées (lignes et colonnes) du début de l'itération pour la multiplication
                    int newligne = ligne - NbrCases;
                    int newcolonne = colonne - NbrCases;
                    for (int i = newligne + décalageLigneDébut; i < newligne + MatriceConvolution.GetLength(0) - décalageLigneFin; i++)
                    {
                        for (int j = newcolonne + décalageColonneDébut; j < newcolonne + MatriceConvolution.GetLength(1) - décalageColonneFin; j++)
                        {
                            // d'abord on fait la multiplication de du pixel de la matrice de convolution avec le pixel de l'image
                            image[i, j].MultiplierValeurs(MatriceConvolution[i - newligne, j - newcolonne]);
                            // Ensuite on additionne la valeur obtenue avec le/les pixels précédents
                            pixeltransition.AdditionnerPixels(image[i, j]);
                            // Pour ne pas modifier les pixels de l'image de départ, on égalise image[i,j] avec l'image de transition [i,j]
                            image[i, j] = ImageTransition[i, j];
                        }
                    }
                    //On divise le pixel de transition final (après avoir fait la somme de tous les produits) pour ne pas dépasser 255 ou 0
                    pixeltransition.DiviserValeurs(coefficient);
                    //On donne la valeur du pixel de transition final au pixel de l'image d'arrivée
                    ImageArrivée[ligne, colonne] = pixeltransition;
                }
            }
        }
    }
}
