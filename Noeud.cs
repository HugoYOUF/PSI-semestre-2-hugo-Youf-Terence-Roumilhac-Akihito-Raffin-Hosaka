using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;

namespace PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin
{
    internal class Noeud
    {
        /// Id unique pour chaque nœud
        public int Id { get; set; }

        /// Constructeur qui initialise l'ID du nœud
        public Noeud(int id) => Id = id;
    }
}
