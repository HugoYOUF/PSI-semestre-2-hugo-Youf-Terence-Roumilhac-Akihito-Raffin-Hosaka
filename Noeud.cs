using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin
{
    internal class Noeud
    {
        public int id; /// Identifiant unique du noeud
        public List<Lien> liens;  /// Liste des liens (relations) connectés à ce noeud

        public Noeud(int id)  /// Constructeur naturel, initialise le noeud avec son identifiant
        {
            this.id = id;
            this.liens = new List<Lien>();
        }

        public int Id ///Accès consultation (get) et modification (set) de Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public List<Lien> Liens  ///Accès consultation (get) et modification (set) de Liens
        {
            get
            {
                if (this.liens == null)
                {
                    this.liens = new List<Lien>();
                }
                return this.liens;
            }
            set { this.liens = value; }

        }
        public override string ToString()
        {
            return "Noeud " + Id;
        }
    }

}
