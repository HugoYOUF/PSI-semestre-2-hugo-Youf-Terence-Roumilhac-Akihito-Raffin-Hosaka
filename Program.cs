using System;
using System.Collections.Generic;
using PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin;

    class Program
    {
        public static void Main(string[] args)
        {
            Graphe graphe = new Graphe();

            /// Création de quelques nœuds (membres)
            Noeud n1 = graphe.AjouterNoeud(1);
            Noeud n2 = graphe.AjouterNoeud(2);
            Noeud n3 = graphe.AjouterNoeud(3);
            Noeud n4 = graphe.AjouterNoeud(4);

            /// Création des liens (relations) entre les membres
            graphe.AjouterLien(n1, n2);
            graphe.AjouterLien(n2, n3);
            graphe.AjouterLien(n3, n4);
            graphe.AjouterLien(n4, n1);
            graphe.AjouterLien(n1, n3);

            /// Affichage du graphe
            graphe.AfficherGraphe();

            Console.ReadLine();
        }
    }
