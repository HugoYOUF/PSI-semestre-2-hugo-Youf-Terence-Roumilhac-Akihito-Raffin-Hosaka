using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    public class Lien
    {
        //Liste des attributs de la classe qui ont le même nom que les colonnes des fichiers csv
        public int StationId { get; set; }
        public int? Suivant { get; set; }
        public double TempsAvecStationSuivante { get; set; }
        public string Ligne { get; set; }
        public double TempsdeChangement { get; set; }

        public Lien()
        {
            TempsAvecStationSuivante = Convert.ToDouble(TempsAvecStationSuivante);
            TempsdeChangement = Convert.ToDouble(TempsdeChangement);
        }
    }

}
