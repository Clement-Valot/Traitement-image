using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Code
{
    class Program
    {
        static void Main(string[] args)
        {
            string NameImage = "";
            Console.WriteLine("Avec quelle photo souhaitez-vous faire des modifications ?");
            Console.WriteLine("1- coco (tapez 1)");
            Console.WriteLine("2- lac en montagne (tapez 2)");
            Console.WriteLine("3- lena (tapez 3)");
            Console.WriteLine("4- Image test (tapez 4)");
            Console.WriteLine("5- Fractale (tapez 5)");
            char touche = Convert.ToChar(Console.ReadLine());
            while (touche != '1' && touche != '2' && touche != '3' && touche != '4')
            {
                Console.Write("Votre saisie est erronée. Veuillez réessayer : ");
                touche = Convert.ToChar(Console.ReadLine());
            }
            if (touche == '1')
            {
                NameImage = "coco.bmp";
            }
            if (touche == '2')
            {
                NameImage = "lac_en_montagne.bmp";
            }
            if (touche == '3')
            {
                NameImage = "lena.bmp";
            }
            if (touche == '4')
            {
                NameImage = "Test.bmp";
            }
            if (touche == '5')
            {
                byte Hauteur = Convert.ToByte(400);
                byte Largeur = Convert.ToByte(800);
                Fractale fractale = new Fractale(Hauteur, Largeur);
                byte[] CodeImage = fractale.CreationCodeImage();
                Pixel[,] Image = fractale.CreationImage(CodeImage);
                fractale.ConstruireFractale(Image);
                Console.ReadLine();

            }
            MyImage NewImage = new MyImage(NameImage);
            NewImage.From_Image_To_File(NameImage);
            Console.WriteLine();

            Console.WriteLine("Que voulez-vous faire avec cette image ?");
            Console.WriteLine("1- La mettre en noir et blanc ");
            Console.WriteLine("2- La mettre en niveaux de gris ");
            Console.WriteLine("3- La faire tourner (90°, 180°, 270°) ");
            Console.WriteLine("4- La tourner en mode miroir ");
            Console.WriteLine("5- L'agrandir ");
            Console.WriteLine("6- La rétrécir ");
            Console.WriteLine("7- La rendre floue, Renforcer ses bords, Détecter ses contours, Fonction repoussage");
            char selection = Convert.ToChar(Console.ReadLine());
            while (selection != '1' && selection != '2' && selection != '3' && selection != '4' && selection != '5' && selection != '6' && selection != '7')
            {
                Console.Write("Votre saisie est erronée. Veuillez réessayer : ");
                selection = Convert.ToChar(Console.ReadLine());
            }
            if (selection == '1')
            {
                NewImage.NoirEtBlanc();
            }
            if (selection == '2')
            {
                NewImage.PassageGris();
            }
            if (selection == '3')
            {
                Console.WriteLine("De combien de degré voulez vous faire tourner l'image ?");
                Console.WriteLine("1- 90° ");
                Console.WriteLine("2- 180° ");
                Console.WriteLine("3- 270° ");
                char rotation = Convert.ToChar(Console.ReadLine());
                while (selection != '1' && selection != '2' && selection != '3')
                {
                    Console.Write("Votre saisie est erronée. Veuillez réessayer : ");
                    touche = Convert.ToChar(Console.ReadLine());
                }
                if (selection == '1')
                {
                    NewImage.Rotation90();
                }
                if (selection == '2')
                {
                    NewImage.Rotation90();
                    NewImage.Rotation90();
                }
                if (selection == '3')
                {
                    NewImage.Rotation90();
                    NewImage.Rotation90();
                    NewImage.Rotation90();
                }
            }
            if (selection == '4')
            {
                NewImage.Miroir();
            }
            if (selection == '5')
            {
                Console.Write("Par quel facteur voulez vous agrandir votre image? (Veuillez saisir un entier compris entre 1 et 100) ");
                int facteurA = Convert.ToInt32(Console.ReadLine());
                NewImage.Agrandir(facteurA);
            }
            if (selection == '6')
            {
                Console.Write("Par quel facteur voulez vous agrandir votre image? (Veuillez saisir un entier compris entre 1 et 100) ");
                int facteurR = Convert.ToInt32(Console.ReadLine());
                NewImage.Retrecir(facteurR);
            }
            if (selection == '7')
            {
                NewImage.MatriceFiltre();
            }
            Process.Start(NameImage);
        }
    }
}
