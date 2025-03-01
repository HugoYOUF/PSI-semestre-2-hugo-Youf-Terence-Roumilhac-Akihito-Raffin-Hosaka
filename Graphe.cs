using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SkiaSharp;


namespace PSI_hugo_Youf_Terence_Roumilhac_Akihito_Raffin
{
    internal class Graphe
    {
        private Dictionary<int, List<int>> listeAdjacence;
        private int[,] matriceAdjacence;
        private List<Noeud> noeuds;
        private List<Lien> liens;
        private int taille;

        public Graphe(int taille)
        {
            this.taille = taille;
            listeAdjacence = new Dictionary<int, List<int>>();
            matriceAdjacence = new int[taille + 1, taille + 1];
            noeuds = new List<Noeud>();
            liens = new List<Lien>();
        }

        public void AjouterNoeud(int id)
        {
            if (!listeAdjacence.ContainsKey(id))
            {
                noeuds.Add(new Noeud(id));
                listeAdjacence[id] = new List<int>();
            }
        }

        public void AjouterLien(int source, int destination)
        {
            if (!listeAdjacence[source].Contains(destination))
            {
                liens.Add(new Lien(new Noeud(source), new Noeud(destination)));
                listeAdjacence[source].Add(destination);
                listeAdjacence[destination].Add(source);
                matriceAdjacence[source, destination] = 1;
                matriceAdjacence[destination, source] = 1;
            }
        }

        public void InstancierGrapheDepuisFichier(string cheminFichier)
        {
            if (!File.Exists(cheminFichier))
            {
                Console.WriteLine("Fichier introuvable : " + cheminFichier);
                return;
            }

            foreach (string ligne in File.ReadLines(cheminFichier))
            {
                string[] elements = ligne.Split(' ');
                if (elements.Length == 2)
                {
                    int source = int.Parse(elements[0]);
                    int destination = int.Parse(elements[1]);
                    AjouterNoeud(source);
                    AjouterNoeud(destination);
                    AjouterLien(source, destination);
                }
            }
        }

        public void AfficherMatriceAdjacence()
        {
            for (int i = 1; i <= taille; i++)
            {
                for (int j = 1; j <= taille; j++)
                {
                    Console.Write(matriceAdjacence[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        public void ParcoursBFS(int depart)
        {
            Queue<int> queue = new Queue<int>();
            HashSet<int> visite = new HashSet<int>();
            queue.Enqueue(depart);
            visite.Add(depart);
            while (queue.Count > 0)
            {
                int noeud = queue.Dequeue();
                Console.Write(noeud + " ");
                foreach (var voisin in listeAdjacence[noeud])
                {
                    if (!visite.Contains(voisin))
                    {
                        visite.Add(voisin);
                        queue.Enqueue(voisin);
                    }
                }
            }
            Console.WriteLine();
        }

        public void ParcoursDFS(int depart, HashSet<int> visite = null)
        {
            if (visite == null) visite = new HashSet<int>();
            if (visite.Contains(depart)) return;
            Console.Write(depart + " ");
            visite.Add(depart);
            foreach (var voisin in listeAdjacence[depart])
            {
                ParcoursDFS(voisin, visite);
            }
        }

        public bool EstConnexe()
        {
            HashSet<int> visite = new HashSet<int>();
            ParcoursDFS(noeuds[0].Id, visite);
            return visite.Count == noeuds.Count;
        }

        public void DessinerGraphe(string cheminImage)
        {
            int largeur = 500, hauteur = 500;
            using var bitmap = new SKBitmap(largeur, hauteur);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);
            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 2,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke
            };
            using var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 16,
                IsAntialias = true
            };

            Random rnd = new Random();
            Dictionary<int, SKPoint> positions = new Dictionary<int, SKPoint>();

            // Positionner les nœuds aléatoirement
            foreach (var noeud in noeuds)
            {
                positions[noeud.Id] = new SKPoint(rnd.Next(50, largeur - 50), rnd.Next(50, hauteur - 50));
            }

            // Dessiner les liens
            foreach (var lien in liens)
            {
                SKPoint p1 = positions[lien.Source.Id];
                SKPoint p2 = positions[lien.Destination.Id];
                canvas.DrawLine(p1, p2, paint);
            }

            // Dessiner les nœuds
            using var nodePaint = new SKPaint { Color = SKColors.Blue, IsAntialias = true };
            foreach (var noeud in noeuds)
            {
                SKPoint p = positions[noeud.Id];
                canvas.DrawCircle(p, 10, nodePaint);
                canvas.DrawText(noeud.Id.ToString(), p.X - 5, p.Y - 5, textPaint);
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            File.WriteAllBytes(cheminImage, data.ToArray());
            Process.Start(new ProcessStartInfo(cheminImage) { UseShellExecute = true });
        }

    }
}   
