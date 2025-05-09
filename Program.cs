using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using ConsoleApp10;
using CsvHelper;
using CsvHelper.Configuration;
using SkiaSharp;

class Program
{
    static void Main()
    {
        /// Configuration pour lire les fichiers CSV
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
            MissingFieldFound = null,
            Encoding = System.Text.Encoding.UTF8
        };

        /// Charger les nœuds depuis le fichier CSV
        var noeuds = new Dictionary<int, Noeud<string>>();
        using (var reader = new StreamReader("Noeuds.csv", System.Text.Encoding.UTF8))
        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords(typeof(Noeud<string>));
            foreach (var record in records)
            {
                var noeud = (Noeud<string>)record;
                if (noeud.LibelleStation != null)
                {
                    noeuds[noeud.IDStation] = noeud;
                }
            }
        }

        /// Charger les liens depuis le fichier CSV
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

        /// Initialiser le graphe et ajouter les nœuds et les liens
        Graph<string> graphe = new Graph<string>();

        foreach (var noeud in noeuds.Values)
        {
            graphe.AjouterNoeud(noeud);
        }

        foreach (var lien in liens)
        {
            graphe.AjouterLien(lien);
            /// Ajouter le lien dans l'autre sens pour simuler le fait que les lignes de métro peuvent être parcourues dans les deux sens
            if (lien.Suivant.HasValue)
            {
                graphe.AjouterLien(new Lien
                {
                    StationId = lien.Suivant.Value,
                    Suivant = lien.StationId,
                    TempsAvecStationSuivante = lien.TempsAvecStationSuivante,
                    Ligne = lien.Ligne,
                    TempsdeChangement = lien.TempsdeChangement,
                    Precedent = lien.Precedent,
                    TempsAvecStationPrecedente = lien.TempsAvecStationPrecedente
                });
            }
        }

        /// Visualiser le graphe
        graphe.Visualiser("plan_metro.png");

        /// Afficher les stations à un lien de distance de chaque station
        graphe.AfficherStationsAUnLienDeDistance();

        /// Calculer le chemin le plus court avec Dijkstra
        string stationChef = "Porte Maillot";
        string stationClient = "Gare de Lyon";

        int startNodeId = AvoirNoeudAvecLibelle(noeuds, stationChef);
        int endNodeId = AvoirNoeudAvecLibelle(noeuds, stationClient);

        var cheminPlusCourt = graphe.DijkstraCheminPlusCourt(startNodeId, endNodeId);

        Console.WriteLine("Plus court chemin : " + string.Join(" -> ", cheminPlusCourt.Select(id => noeuds[id].LibelleStation)));

        /// Calculer le temps de trajet total
        double tempsTotal = 0;
        for (int i = 0; i < cheminPlusCourt.Count - 1; i++)
        {
            int currentId = cheminPlusCourt[i];
            int nextId = cheminPlusCourt[i + 1];

            var lien = liens.FirstOrDefault(l => l.StationId == currentId && l.Suivant == nextId);
            if (lien != null)
            {
                tempsTotal += lien.TempsAvecStationSuivante;
            }
        }

        Console.WriteLine($"Temps de trajet total : {tempsTotal} minutes");

        /// Visualiser le chemin le plus court
        graphe.VisualiserChemin("chemin_plus_court.png", cheminPlusCourt);

        Console.WriteLine($"Le graphe est biparti : {graphe.EstBiparti()}");
        Console.WriteLine($"Le graphe est connexe : {graphe.EstConnexe()}");
    }

    /// Trouver un nœud par son libellé
    public static int AvoirNoeudAvecLibelle(Dictionary<int, Noeud<string>> noeuds, string libelle)
    {
        foreach (var noeud in noeuds)
        {
            if (noeud.Value.LibelleStation != null && noeud.Value.LibelleStation.Equals(libelle, StringComparison.OrdinalIgnoreCase))
            {
                return noeud.Key;
            }
        }
        throw new ArgumentException($"Erreur, aucune station trouvée portant ce nom {libelle}");
    }
}

/// Classe représentant un lien entre deux stations
public class Lien
{
    public int StationId { get; set; }
    public int? Suivant { get; set; }
    public double TempsAvecStationSuivante { get; set; }
    public string Ligne { get; set; }
    public double TempsdeChangement { get; set; }
    public int? Precedent { get; set; }
    public double TempsAvecStationPrecedente { get; set; }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Lien other = (Lien)obj;
        return StationId == other.StationId && Suivant == other.Suivant;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StationId, Suivant);
    }
}

/// Classe représentant un nœud dans le graphe
public class Noeud<T>
{
    public int IDStation { get; set; }
    public T LibelleStation { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public List<int> Voisins { get; set; } = new List<int>();
}

/// Classe représentant un graphe
public class Graph<T>
{
    public Dictionary<int, Noeud<T>> noeuds;
    public List<Lien> liens;
    private Dictionary<string, SKColor> couleursLignes = new Dictionary<string, SKColor>();
    private SKColor[] colors = new SKColor[]
    {
        SKColors.Red, SKColors.Blue, SKColors.Green, SKColors.Orange, SKColors.Purple,
        SKColors.Yellow, SKColors.Cyan, SKColors.Magenta, SKColors.Lime, SKColors.Teal
    };

    /// Constructeur du graphe
    public Graph()
    {
        noeuds = new Dictionary<int, Noeud<T>>();
        liens = new List<Lien>();
    }

    /// Ajouter un nœud au graphe
    public void AjouterNoeud(Noeud<T> noeud)
    {
        if (!noeuds.ContainsKey(noeud.IDStation))
        {
            noeuds[noeud.IDStation] = noeud;
        }
    }

    /// Ajouter un lien au graphe
    public void AjouterLien(Lien lien)
    {
        if (!liens.Any(l => l.Equals(lien)))
        {
            liens.Add(lien);
            if (noeuds.ContainsKey(lien.StationId) && lien.Suivant.HasValue && noeuds.ContainsKey(lien.Suivant.Value))
            {
                noeuds[lien.StationId].Voisins.Add(lien.Suivant.Value);
                noeuds[lien.Suivant.Value].Voisins.Add(lien.StationId);
            }
        }
    }

    /// Vérifier si le graphe est biparti
    public bool EstBiparti()
    {
        var couleur = new Dictionary<int, int>();
        var file = new Queue<int>();

        foreach (var noeud in noeuds.Keys)
        {
            couleur[noeud] = -1; // -1 signifie non coloré
        }

        foreach (var noeud in noeuds.Keys)
        {
            if (couleur[noeud] == -1)
            {
                file.Enqueue(noeud);
                couleur[noeud] = 0; // Commencez par la couleur 0

                while (file.Count > 0)
                {
                    var current = file.Dequeue();

                    foreach (var voisin in noeuds[current].Voisins)
                    {
                        if (couleur[voisin] == -1)
                        {
                            couleur[voisin] = couleur[current] ^ 1; // Alternez la couleur
                            file.Enqueue(voisin);
                        }
                        else if (couleur[voisin] == couleur[current])
                        {
                            return false; // Le graphe n'est pas biparti
                        }
                    }
                }
            }
        }

        return true; // Le graphe est biparti
    }

    /// Vérifier si le graphe est connexe
    public bool EstConnexe()
    {
        var visited = new HashSet<int>();
        var queue = new Queue<int>();

        if (noeuds.Count == 0)
            return true; // Un graphe vide est considéré comme connexe

        var startNode = noeuds.Keys.First();
        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var voisin in noeuds[current].Voisins)
            {
                if (!visited.Contains(voisin))
                {
                    visited.Add(voisin);
                    queue.Enqueue(voisin);
                }
            }
        }

        return visited.Count == noeuds.Count; // Le graphe est connexe si tous les nœuds sont visités
    }

    /// Algorithme de Dijkstra pour trouver le chemin le plus court
    public List<int> DijkstraCheminPlusCourt(int startNodeId, int endNodeId)
    {
        var distances = new Dictionary<int, double>();
        var predecessors = new Dictionary<int, int>();
        var priorityQueue = new SortedSet<(int NodeId, double Distance)>();

        foreach (var noeud in noeuds.Keys)
        {
            distances[noeud] = double.MaxValue;
        }
        distances[startNodeId] = 0;
        priorityQueue.Add((startNodeId, 0));

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.First();
            priorityQueue.Remove(current);
            var currentNodeId = current.NodeId;

            foreach (var voisinId in noeuds[currentNodeId].Voisins)
            {
                var lien = liens.FirstOrDefault(l => l.StationId == currentNodeId && l.Suivant == voisinId);
                if (lien != null)
                {
                    var newDistance = distances[currentNodeId] + lien.TempsAvecStationSuivante;

                    if (newDistance < distances[voisinId])
                    {
                        priorityQueue.Remove((voisinId, distances[voisinId]));
                        distances[voisinId] = newDistance;
                        predecessors[voisinId] = currentNodeId;
                        priorityQueue.Add((voisinId, newDistance));
                    }
                }
            }
        }

        var chemin = new List<int>();
        for (int curr = endNodeId; curr != startNodeId; curr = predecessors[curr])
        {
            chemin.Add(curr);
        }
        chemin.Add(startNodeId);
        chemin.Reverse();

        return chemin;
    }

    /// Afficher les stations à un lien de distance de chaque station
    public void AfficherStationsAUnLienDeDistance()
    {
        var stationsTraitees = new HashSet<string>();
        foreach (var noeud in noeuds.Values)
        {
            if (noeud.LibelleStation != null && !stationsTraitees.Contains(noeud.LibelleStation.ToString()))
            {
                var voisins = Voisins(noeud.LibelleStation.ToString());
                Console.WriteLine($"Station {noeud.LibelleStation} a des liens avec : {string.Join(", ", voisins.Select(id => noeuds[id].LibelleStation))}");
                stationsTraitees.Add(noeud.LibelleStation.ToString());
            }
        }
    }

    /// Trouver les voisins d'un nœud par son nom
    public IEnumerable<int> Voisins(string libelleStation)
    {
        var voisins = new HashSet<int>();
        foreach (var noeud in noeuds.Values.Where(n => n.LibelleStation != null && string.Equals(n.LibelleStation.ToString(), libelleStation, StringComparison.OrdinalIgnoreCase)))
        {
            foreach (var voisinId in noeud.Voisins)
            {
                voisins.Add(voisinId);
            }
        }
        return voisins;
    }

    /// Visualiser le graphe
    public void Visualiser(string chemin, int marge = 200)
    {
        double minLongitude = noeuds.Values.Min(noeud => noeud.Longitude);
        double maxLongitude = noeuds.Values.Max(noeud => noeud.Longitude);
        double minLatitude = noeuds.Values.Min(noeud => noeud.Latitude);
        double maxLatitude = noeuds.Values.Max(noeud => noeud.Latitude);

        using (var surface = SKSurface.Create(new SKImageInfo(3000, 2000)))
        {
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            foreach (var noeud in noeuds.Values)
            {
                var x = (float)(marge + (3000 - 2 * marge) * (noeud.Longitude - minLongitude) / (maxLongitude - minLongitude));
                var y = (float)(marge + (2000 - 2 * marge) * (1 - (noeud.Latitude - minLatitude) / (maxLatitude - minLatitude)));

                canvas.DrawCircle(x, y, 5, new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Fill });
                canvas.DrawText(noeud.LibelleStation.ToString(), x + 10, y + 10, new SKPaint { Color = SKColors.Black, TextSize = 12 });
            }

            foreach (var lien in liens)
            {
                if (lien.Suivant.HasValue && noeuds.ContainsKey(lien.StationId) && noeuds.ContainsKey(lien.Suivant.Value))
                {
                    if (!couleursLignes.ContainsKey(lien.Ligne))
                    {
                        couleursLignes[lien.Ligne] = colors[couleursLignes.Count % colors.Length];
                    }

                    var startX = (float)(marge + (3000 - 2 * marge) * (noeuds[lien.StationId].Longitude - minLongitude) / (maxLongitude - minLongitude));
                    var startY = (float)(marge + (2000 - 2 * marge) * (1 - (noeuds[lien.StationId].Latitude - minLatitude) / (maxLatitude - minLatitude)));
                    var endX = (float)(marge + (3000 - 2 * marge) * (noeuds[lien.Suivant.Value].Longitude - minLongitude) / (maxLongitude - minLongitude));
                    var endY = (float)(marge + (2000 - 2 * marge) * (1 - (noeuds[lien.Suivant.Value].Latitude - minLatitude) / (maxLatitude - minLatitude)));

                    var couleur = couleursLignes[lien.Ligne];
                    canvas.DrawLine(startX, startY, endX, endY, new SKPaint { Color = couleur, StrokeWidth = 2 });

                    var midX = (startX + endX) / 2;
                    var midY = (startY + endY) / 2;
                    canvas.DrawText($"{lien.TempsAvecStationSuivante} min", midX, midY, new SKPaint { Color = SKColors.Black, TextSize = 10 });
                }
            }

            using (var image = surface.Snapshot())
            using (var d = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(chemin))
            {
                d.SaveTo(stream);
            }
        }

        Process.Start(new ProcessStartInfo(chemin) { UseShellExecute = true });
    }

    /// Visualiser le chemin le plus court
    public void VisualiserChemin(string cheminFichier, List<int> chemin, int marge = 200)
    {
        double minLon = noeuds.Values.Min(n => n.Longitude);
        double maxLon = noeuds.Values.Max(n => n.Longitude);
        double minLat = noeuds.Values.Min(n => n.Latitude);
        double maxLat = noeuds.Values.Max(n => n.Latitude);

        int width = 3000, height = 2000;
        using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
        {
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            var paintEdge = new SKPaint { Color = SKColors.Red, StrokeWidth = 4 };
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                if (!noeuds.ContainsKey(chemin[i]) || !noeuds.ContainsKey(chemin[i + 1]))
                {
                    continue;
                }

                var a = noeuds[chemin[i]];
                var b = noeuds[chemin[i + 1]];
                float x1 = (float)(marge + (width - 2 * marge) * (a.Longitude - minLon) / (maxLon - minLon));
                float y1 = (float)(marge + (height - 2 * marge) * (1 - (a.Latitude - minLat) / (maxLat - minLat)));
                float x2 = (float)(marge + (width - 2 * marge) * (b.Longitude - minLon) / (maxLon - minLon));
                float y2 = (float)(marge + (height - 2 * marge) * (1 - (b.Latitude - minLat) / (maxLat - minLat)));
                canvas.DrawLine(x1, y1, x2, y2, paintEdge);
            }

            var paintNode = new SKPaint { Color = SKColors.Blue, Style = SKPaintStyle.Fill };
            var paintText = new SKPaint { Color = SKColors.Black, TextSize = 14 };
            foreach (var id in chemin)
            {
                if (!noeuds.ContainsKey(id))
                {
                    continue;
                }

                var n = noeuds[id];
                float x = (float)(marge + (width - 2 * marge) * (n.Longitude - minLon) / (maxLon - minLon));
                float y = (float)(marge + (height - 2 * marge) * (1 - (n.Latitude - minLat) / (maxLat - minLat)));
                canvas.DrawCircle(x, y, 10, paintNode);
                canvas.DrawText(n.LibelleStation.ToString(), x + 12, y + 12, paintText);
            }

            using (var img = surface.Snapshot())
            using (var data = img.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(cheminFichier))
            {
                data.SaveTo(stream);
            }
        }

        Process.Start(new ProcessStartInfo(cheminFichier) { UseShellExecute = true });
    }
}
