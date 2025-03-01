using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;
using PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        string cheminFichier = "soc-karate.txt";
        Graphe graphe = new Graphe(34);

        /// Instancier le graphe, parcours et vérification du graphe
        graphe.InstancierGrapheDepuisFichier(cheminFichier);
        Console.WriteLine("Matrice d'adjacence du graphe :");
        graphe.AfficherMatriceAdjacence();
        Console.WriteLine("Parcours en largeur :");
        graphe.ParcoursBFS(1);
        Console.WriteLine("Parcours en profondeur :");
        graphe.ParcoursDFS(1);
        Console.WriteLine("Le graphe est connexe ? " + graphe.EstConnexe());

        /// Génération et affichage de l'image
        Console.WriteLine("Génération et affichage du graphe");
        graphe.DessinerGraphe("graphe.png");
    }
}
