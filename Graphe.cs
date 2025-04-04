using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace ConsoleApp10
{
    public class Graph<T>
    {
        //Liste des attributs de la classe
        public Dictionary<int, Noeud<T>> noeuds;
        public List<Lien> liens;
        public Dictionary<int, int> nodeIndexMap;
        public Dictionary<int, int> indexNodeMap;

        // Liste de couleurs prédéfinies pour la visualisation des lignes de métro
        private SKColor[] colors = new SKColor[]
        {
            SKColors.Red, SKColors.Blue, SKColors.Green, SKColors.Orange, SKColors.Purple,
            SKColors.Yellow, SKColors.Cyan, SKColors.Magenta, SKColors.Lime, SKColors.Teal
        };

        public Graph()
        {
            noeuds = new Dictionary<int, Noeud<T>>();
            liens = new List<Lien>();
            nodeIndexMap = new Dictionary<int, int>();
            indexNodeMap = new Dictionary<int, int>();
        }

        public void AjouterNoeud(Noeud<T> noeud)
        {
            noeuds[noeud.IDStation] = noeud;
        }

        public void AjouterLien(Lien lien)
        {
            liens.Add(lien);
        }



        public void Visualiser(string chemin, int marge = 200)
        {
            // Déterminer les limites des coordonnées pour ajuster l'affichage
            double minLongitude = double.MaxValue;
            double maxLongitude = double.MinValue;
            double minLatitude = double.MaxValue;
            double maxLatitude = double.MinValue;

            foreach (var noeud in noeuds.Values)
            {
                if (noeud.Longitude < minLongitude) minLongitude = noeud.Longitude;
                if (noeud.Longitude > maxLongitude) maxLongitude = noeud.Longitude;
                if (noeud.Latitude < minLatitude) minLatitude = noeud.Latitude;
                if (noeud.Latitude > maxLatitude) maxLatitude = noeud.Latitude;
            }

            using (var surface = SKSurface.Create(new SKImageInfo(3000, 2000)))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                // Attribution d'une couleur à chaque ligne de métro
                var couleursLignes = new Dictionary<string, SKColor>();

                // Dessiner les nœuds
                foreach (var noeud in noeuds.Values)
                {
                    var x = (float)(marge + (3000 - 2 * marge) * (noeud.Longitude - minLongitude) / (maxLongitude - minLongitude));
                    var y = (float)(marge + (2000 - 2 * marge) * (1 - (noeud.Latitude - minLatitude) / (maxLatitude - minLatitude)));

                    // Dessiner un cercle pour le nœud
                    canvas.DrawCircle(x, y, 5, new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Fill });

                    // Afficher le nom de la station
                    canvas.DrawText(noeud.LibelleStation.ToString(), x + 10, y + 10, new SKPaint { Color = SKColors.Black, TextSize = 12 });
                }

                // Dessiner les liens entre les nœuds
                foreach (var lien in liens)
                {
                    if (!couleursLignes.ContainsKey(lien.Ligne))
                    {
                        couleursLignes[lien.Ligne] = colors[couleursLignes.Count % colors.Length];
                    }

                    if (lien.Suivant.HasValue && noeuds.ContainsKey(lien.StationId) && noeuds.ContainsKey(lien.Suivant.Value))
                    {
                        var startX = (float)(marge + (3000 - 2 * marge) * (noeuds[lien.StationId].Longitude - minLongitude) / (maxLongitude - minLongitude));
                        var startY = (float)(marge + (2000 - 2 * marge) * (1 - (noeuds[lien.StationId].Latitude - minLatitude) / (maxLatitude - minLatitude)));
                        var endX = (float)(marge + (3000 - 2 * marge) * (noeuds[lien.Suivant.Value].Longitude - minLongitude) / (maxLongitude - minLongitude));
                        var endY = (float)(marge + (2000 - 2 * marge) * (1 - (noeuds[lien.Suivant.Value].Latitude - minLatitude) / (maxLatitude - minLatitude)));

                        var couleur = couleursLignes[lien.Ligne];
                        canvas.DrawLine(startX, startY, endX, endY, new SKPaint { Color = couleur, StrokeWidth = 2 });

                        // Afficher le temps entre les stations
                        var midX = (startX + endX) / 2;
                        var midY = (startY + endY) / 2;
                        canvas.DrawText($"{lien.TempsAvecStationSuivante} min", midX, midY, new SKPaint { Color = SKColors.Black, TextSize = 10 });

                        // Afficher le temps de changement à la station de départ
                        canvas.DrawText($"Changement: {lien.TempsdeChangement} min", startX, startY + 20, new SKPaint { Color = SKColors.Black, TextSize = 8 });
                    }
                }

                using (var image = surface.Snapshot())
                using (var d = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var s = File.OpenWrite(chemin))
                {
                    d.SaveTo(s);
                }
            }

            // Ouvrir l'image généré
            Process.Start(new ProcessStartInfo(chemin) { UseShellExecute = true });
        }

        public (Dictionary<int, double>, Dictionary<int, int>) DijsktraChemin(int startNodeId)
        {
            var distances = new Dictionary<int, double>();
            var predecesseurs = new Dictionary<int, int>();
            var priorite = new SortedSet<(int NodeId, double Distance)>();

            foreach (var noeud in noeuds.Keys)
            {
                distances[noeud] = double.MaxValue;
            }
            distances[startNodeId] = 0;
            priorite.Add((startNodeId, 0));

            while (priorite.Count > 0)
            {
                var current = priorite.First();
                priorite.Remove(current);
                var currentNodeId = current.NodeId;

                foreach (var lien in liens.Where(e => e.StationId == currentNodeId))
                {
                    if (lien.Suivant.HasValue && noeuds.ContainsKey(lien.Suivant.Value))
                    {
                        var neighborId = lien.Suivant.Value;
                        var newDistance = distances[currentNodeId] + lien.TempsAvecStationSuivante;

                        if (newDistance < distances[neighborId])
                        {
                            priorite.Remove((neighborId, distances[neighborId]));
                            distances[neighborId] = newDistance;
                            predecesseurs[neighborId] = currentNodeId;
                            priorite.Add((neighborId, newDistance));
                        }
                    }
                }
            }

            // Vérifier les distances finales
            /*
            foreach (var kvp in distances)
            {
                Console.WriteLine($"Temps de transport jusqu'à {kvp.Key}: {(kvp.Value == double.MaxValue ? "infini" : kvp.Value.ToString())}");
            }
            */

            return (distances, predecesseurs);
        }


        public Dictionary<int, double> BellmanFord(int noeuddepart)
        {
            var distances = new Dictionary<int, double>();

            foreach (var noeud in noeuds.Keys)
            {
                distances[noeud] = double.MaxValue;
            }
            distances[noeuddepart] = 0;

            for (int i = 0; i < noeuds.Count - 1; i++)
            {
                foreach (var lien in liens)
                {
                    if (lien.Suivant.HasValue && distances[lien.StationId] != double.MaxValue)
                    {
                        var voisin = lien.Suivant.Value;
                        var nouvdistance = distances[lien.StationId] + lien.TempsAvecStationSuivante;

                        if (nouvdistance < distances[voisin])
                        {
                            distances[voisin] = nouvdistance;
                        }
                    }
                }
            }

            // Vérification des cycles de poids négatif
            foreach (var lien in liens)
            {
                if (lien.Suivant.HasValue && distances[lien.StationId] != double.MaxValue)
                {
                    var voisin = lien.Suivant.Value;
                    var nouvdistance = distances[lien.StationId] + lien.TempsAvecStationSuivante;

                    if (nouvdistance < distances[voisin])
                    {
                        throw new InvalidOperationException("Le graphe contient des cycles néfatifs ");
                    }
                }
            }

            return distances;
        }

        public (Dictionary<int, double>, Dictionary<int, int>) CheminBellman(int noeuddepart)
        {
            var distances = new Dictionary<int, double>();
            var predecesseurs = new Dictionary<int, int>();

            foreach (var noeud in noeuds.Keys)
            {
                distances[noeud] = double.MaxValue;
            }
            distances[noeuddepart] = 0;

            for (int i = 0; i < noeuds.Count - 1; i++)
            {
                foreach (var lien in liens)
                {
                    if (lien.Suivant.HasValue && distances[lien.StationId] != double.MaxValue)
                    {
                        var neighborId = lien.Suivant.Value;
                        var newDistance = distances[lien.StationId] + lien.TempsAvecStationSuivante;

                        if (newDistance < distances[neighborId])
                        {
                            distances[neighborId] = newDistance;
                            predecesseurs[neighborId] = lien.StationId;
                        }
                    }
                }
            }

            // Vérification des cycles de poids négatif
            foreach (var lien in liens)
            {
                if (lien.Suivant.HasValue && distances[lien.StationId] != double.MaxValue)
                {
                    var voisin = lien.Suivant.Value;
                    var nouvdistance = distances[lien.StationId] + lien.TempsAvecStationSuivante;

                    if (nouvdistance < distances[voisin])
                    {
                        throw new InvalidOperationException("Le graphe contient un cycle négatif");
                    }
                }
            }

            return (distances, predecesseurs);
        }


        public double[,] FloydWarshall()
        {
            // Initialiser les mappings des identifiants des nœuds à des indices continus
            int index = 0;
            nodeIndexMap.Clear();
            indexNodeMap.Clear();

            foreach (var noeud in noeuds.Keys)
            {
                nodeIndexMap[noeud] = index;
                indexNodeMap[index] = noeud;
                index++;
            }

            var distances = new double[noeuds.Count, noeuds.Count];

            // Initialisation des distances
            for (int i = 0; i < noeuds.Count; i++)
            {
                for (int j = 0; j < noeuds.Count; j++)
                {
                    distances[i, j] = (i == j) ? 0 : double.MaxValue;
                }
            }

            // Initialisation avec les liens directs
            foreach (var lien in liens)
            {
                if (lien.Suivant.HasValue && nodeIndexMap.ContainsKey(lien.StationId) && nodeIndexMap.ContainsKey(lien.Suivant.Value))
                {
                    distances[nodeIndexMap[lien.StationId], nodeIndexMap[lien.Suivant.Value]] = lien.TempsAvecStationSuivante;
                }
            }

            // Algorithme de Floyd-Warshall
            for (int k = 0; k < noeuds.Count; k++)
            {
                for (int i = 0; i < noeuds.Count; i++)
                {
                    for (int j = 0; j < noeuds.Count; j++)
                    {
                        if (distances[i, k] != double.MaxValue && distances[k, j] != double.MaxValue)
                        {
                            distances[i, j] = Math.Min(distances[i, j], distances[i, k] + distances[k, j]);
                        }
                    }
                }
            }

            // Vérification des cycles de poids négatif
            for (int i = 0; i < noeuds.Count; i++)
            {
                if (distances[i, i] < 0)
                {
                    throw new InvalidOperationException("Le graphe contient un cycle de poids négatif.");
                }
            }

            return distances;
        }


        public List<int> CheminFloyd(int startNodeId, int endNodeId, double[,] distances)
        {
            var path = new List<int> { startNodeId };
            var currentNodeId = startNodeId;

            while (currentNodeId != endNodeId)
            {
                var nextNodeId = -1;
                var minDistance = double.MaxValue;

                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    if (distances[nodeIndexMap[currentNodeId], i] != double.MaxValue &&
                        distances[nodeIndexMap[currentNodeId], i] + distances[i, nodeIndexMap[endNodeId]] < minDistance)
                    {
                        minDistance = distances[nodeIndexMap[currentNodeId], i] + distances[i, nodeIndexMap[endNodeId]];
                        nextNodeId = indexNodeMap[i];
                    }
                }

                if (nextNodeId == -1) break;

                path.Add(nextNodeId);
                currentNodeId = nextNodeId;
            }

            return path;
        }



        


    }
}
