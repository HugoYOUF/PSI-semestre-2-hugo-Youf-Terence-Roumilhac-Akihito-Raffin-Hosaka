using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin
{
    internal class Lien
    {
        private Noeud source;       /// Noeud source de la relation
        private Noeud destination;  /// Noeud destination de la relation


        public Lien(Noeud source, Noeud destination)  /// Constructeur naturel, crée un lien entre deux nœuds
        {
            this.source = source;
            this.destination = destination;
        }

        public Noeud Source ///Accès consultation (get) et modification (set) de Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        public Noeud Destination   ///Accès consultation (get) et modification (set) de Destination
        {
            get { return this.destination; }
            set { this.destination = value; }
        }

        public override string ToString()
        {
            return "Lien entre" + Source.Id + " et " + Destination.Id;
        }
    }
}

