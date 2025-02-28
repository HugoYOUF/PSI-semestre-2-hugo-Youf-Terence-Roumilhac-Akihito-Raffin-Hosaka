using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin
{
    internal class Graphe
    {
        public List<Noeud> noeuds; ///Liste des noeuds dans le graphe
        public List<Lien> liens;   ///Liste des liens entre les noeuds

        public Graphe() ///Constructeur naturel qui initialise un graphe vide
        {
            this.noeuds = new List<Noeud>(); ///Initialisation de la liste des noeuds
            this.liens = new List<Lien>();   ///Initialisation de la liste des liens
        }

        public List<Noeud> Noeuds  ///Accès consultation (get) et modification (set) de Noeuds
        {
            get
            {
                if (this.noeuds == null) ///Vérifie si la liste des noeuds est null et la réinitialise si nécessaire
                {
                    this.noeuds = new List<Noeud>();
                }
                return this.noeuds;
            }
            set { this.noeuds = value; }
        }

        public List<Lien> Liens  ///Accès consultation (get) et modification (set) de Liens
        {
            get
            {
                if (this.liens == null) /// Vérifie si la liste des liens est null, et la réinitialise si nécessaire
                {
                    this.liens = new List<Lien>();
                }
                return this.liens;
            }
            set { this.liens = value; }
        }

        public Noeud AjouterNoeud(int id) ///Méthode pour ajouter un noeuds au graphe
        {
            Noeud noeud = new Noeud(id);  /// Crée un nouvel objet Noeud avec l'ID donné
            Noeuds.Add(noeud);  /// Ajoute le noeud à la liste des noeuds du graphe
            return noeud;  /// Retourne le noeud ajouté
        }

        public void AjouterLien(Noeud source, Noeud destination)
        {
            Lien lien = new Lien(source, destination);
            Liens.Add(lien);
            /// Mise à jour des listes de liens pour chaque noeud
            source.Liens.Add(lien);
            destination.Liens.Add(lien);
        }
        public void AfficherGraphe() ///Méthode pour afficher la structure du graphe 
        {
            Console.WriteLine("Liste des nœuds et leurs connexions :");
            foreach (var noeud in Noeuds)  ///Parcours de tous les noeuds du graphe
            {
                Console.Write(noeud + " connecte a : ");
                foreach (var lien in noeud.Liens) /// Parcours des liens associés à ce noeud
                {
                    /// Identifier le nœud voisin dans le lien
                    Noeud voisin = (lien.Source == noeud) ? lien.Destination : lien.Source;
                    Console.Write(voisin.Id + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
