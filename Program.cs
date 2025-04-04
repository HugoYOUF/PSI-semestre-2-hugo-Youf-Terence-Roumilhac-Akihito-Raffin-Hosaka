using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ConsoleApp10;
using CsvHelper;
using CsvHelper.Configuration;

class Program
{
    static void Main()
    {
        // Configuration pour lire les fichiers CSV avec encodage UTF-8
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
            MissingFieldFound = null,
            Encoding = System.Text.Encoding.UTF8
        };

        // Lire les nœuds à partir du fichier CSV
        var noeuds = new Dictionary<int, Noeud<string>>();
        using (var reader = new StreamReader("Noeuds.csv", System.Text.Encoding.UTF8))
        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords(typeof(Noeud<string>));
            foreach (var record in records)
            {
                var noeud = (Noeud<string>)record;
                noeuds[noeud.IDStation] = noeud;
            }
        }

        // Lire les arêtes à partir du fichier CSV
        var liens = new List<Lien>();
        using (var reader = new StreamReader("Liens.csv", System.Text.Encoding.UTF8))
        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords<Lien>();
            foreach (var record in records)
            {
                liens.Add(record);
            }
        }

        // Créer une instance de Graph avec des nœuds de type string
        Graph<string> graphe = new Graph<string>();

        // Ajouter les nœuds et les liens au graphe
        foreach (var noeud in noeuds.Values)
        {
            graphe.AjouterNoeud(noeud);
        }

        foreach (var lien in liens)
        {
            graphe.AjouterLien(lien);
        }


        //Visualiser le graphe entier
        graphe.Visualiser("plan_metro.png");
            

        // Exemple de nœuds de départ et de destination
        string stationchef = "Porte Maillot";
        string stationclient = "Nation";

        int startNodeId = AvoirNoeudAvecLibelle(noeuds, stationchef);
        int endNodeId = AvoirNoeudAvecLibelle(noeuds, stationclient);

        var (distancesDijkstra, predecessorsDijkstra) = graphe.DijsktraChemin(startNodeId);

        // Reconstruire et afficher le chemin pour Dijkstra
        var cheminDijkstra = new List<int>();
        for (var step = endNodeId; step != startNodeId; step = predecessorsDijkstra[step])
        {
            cheminDijkstra.Add(step);
        }
        cheminDijkstra.Add(startNodeId);
        cheminDijkstra.Reverse();

        Console.WriteLine("Chemin via l'algorithme de Dijsktra : " + string.Join(" -> ", cheminDijkstra.Select(id => noeuds[id].LibelleStation)));

        // Exécuter Bellman-Ford
        var (distancesBellmanFord, predecessorsBellmanFord) = graphe.CheminBellman(startNodeId);
        var cheminBellmandford = new List<int>();
        for (var step = endNodeId; step != startNodeId; step = predecessorsBellmanFord[step])
        {
            cheminBellmandford.Add(step);
        }
        cheminBellmandford.Add(startNodeId);
        cheminBellmandford.Reverse();

        Console.WriteLine("Chemin via l'algorithme de BellmanFord " + string.Join(" -> ", cheminBellmandford.Select(id => noeuds[id].LibelleStation)));

        // Exécuter Floyd-Warshall
        /*
        var distancesFloydWarshall = graphe.FloydWarshall();
        var cheminFloydWarhsall = graphe.CheminFloyd(startNodeId, endNodeId, distancesFloydWarshall);
        foreach(var chemin in distancesFloydWarshall)
        {
            Console.WriteLine(chemin.ToString());
        }

        Console.WriteLine("Chemin via l'algorithme de Floyd-Warshall: ");

        Console.WriteLine("Chemin via l'algorithme de Floyd-Warshall: " + string.Join(" -> ", cheminFloydWarhsall.Select(id => noeuds[id].LibelleStation)));

        */
        //Nous n'avons pas réussi à obtenir une version qui marche de cet algorithme, comme le graphe qui n'affiche que le chemin le plus court.





    }

    public static int AvoirNoeudAvecLibelle(Dictionary<int, Noeud<string>> noeuds, string libelle)
    {
        foreach (var noeud in noeuds)
        {
            if (noeud.Value.LibelleStation.Equals(libelle, StringComparison.OrdinalIgnoreCase))
            {
                return noeud.Key;
            }
        }
        throw new ArgumentException($"Erreur, aucune station trouvé portant ce nom {libelle}");
    }

}
