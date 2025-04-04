using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    public class Noeud<T>
    {
        //Liste des attributs de la classe qui ont les mêmes nom que les colonnes des fichiers csv.
        public int IDStation { get; set; }
        public T LibelleStation { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

       
    }



}
