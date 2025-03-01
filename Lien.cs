using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;

namespace PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin
{
    /// Classe qui représente un lien entre deux noeuds
    internal class Lien
    {
        /// Propriété qui contient le noeud source
        public Noeud Source { get; }
        
        /// Propriété qui contient le noeud destination
        public Noeud Destination { get; }

        /// Constructeur qui initialise un lien entre deux noeuds
        public Lien(Noeud source, Noeud destination)
        {
            Source = source;       /// Assigne le noeud source
            Destination = destination;  /// Assigne le noeud destination
        }   
    }
}
