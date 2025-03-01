using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;
using PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin;
using System.Diagnostics;

// Classe représentant un nœud (membre de l'association)


class Program
{
    static void Main()
    {
        string cheminFichier = "soc-karate.txt"; // Spécifiez le chemin du fichier contenant la matrice d'adjacence
        Graphe graphe = new Graphe(34);
        graphe.InstancierGrapheDepuisFichier(cheminFichier);

        Console.WriteLine("Matrice d'adjacence du graphe :");
        graphe.AfficherMatriceAdjacence();

        Console.WriteLine("Parcours en largeur :");
        graphe.ParcoursBFS(1);

        Console.WriteLine("Parcours en profondeur :");
        graphe.ParcoursDFS(1);

        Console.WriteLine("Le graphe est connexe ? " + graphe.EstConnexe());

        // Génération et affichage du graphe
        Console.WriteLine("Génération et affichage du graphe");
        graphe.DessinerGraphe("graphe.png");
        

    }
}
