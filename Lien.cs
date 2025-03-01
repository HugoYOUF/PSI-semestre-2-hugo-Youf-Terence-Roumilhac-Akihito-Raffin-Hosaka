using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;

namespace PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin
{
    internal class Lien
    {
        public Noeud Source { get; }
        public Noeud Destination { get; }
        public Lien(Noeud source, Noeud destination)
        {
            Source = source;       /// Assigne le noeud source
            Destination = destination;  /// Assigne le noeud destination
        }   
    }
}
