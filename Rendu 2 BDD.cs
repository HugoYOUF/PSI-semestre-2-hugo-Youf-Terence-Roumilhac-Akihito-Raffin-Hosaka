using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LivInParisConsole
{
    class Program
    {
        static void Main()
        {
          
            string connectionString = "Data Source=.;Initial Catalog=LivInParis;Integrated Security=True;";

            DatabaseManager dbManager = new DatabaseManager(connectionString);
            bool quitter = false;
            while (!quitter)
            {
                Console.WriteLine("=== Menu Principal ===");
                Console.WriteLine("1. Gestion Clients");
                Console.WriteLine("2. Gestion Cuisiniers");
                Console.WriteLine("3. Gestion Commandes");
                Console.WriteLine("4. Statistiques");
                Console.WriteLine("5. Quitter");
                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();
                Console.WriteLine();

                switch (choix)
                {
                    case "1":
                        ClientModule(dbManager);
                        break;
                    case "2":
                        CuisinierModule(dbManager);
                        break;
                    case "3":
                        CommandeModule(dbManager);
                        break;
                    case "4":
                        StatistiquesModule(dbManager);
                        break;
                    case "5":
                        quitter = true;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void ClientModule(DatabaseManager db)
        {
            bool retour = false;
            while (!retour)
            {
                Console.WriteLine("=== Gestion Clients ===");
                Console.WriteLine("1. Ajouter Client");
                Console.WriteLine("2. Modifier Client");
                Console.WriteLine("3. Supprimer Client");
                Console.WriteLine("4. Afficher Clients");
                Console.WriteLine("5. Retour");
                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();
                Console.WriteLine();

                switch (choix)
                {
                    case "1":
                        Console.Write("Type de client (Particulier/Entreprise) : ");
                        string type = Console.ReadLine();
                        Console.Write("Nom : ");
                        string nom = Console.ReadLine();
                        Console.Write("Prénom (laisser vide si Entreprise) : ");
                        string prenom = Console.ReadLine();
                        Console.Write("Adresse : ");
                        string adresse = Console.ReadLine();
                        Console.Write("Téléphone : ");
                        string tel = Console.ReadLine();
                        Console.Write("Email : ");
                        string email = Console.ReadLine();
                        string nomReferent = null;
                        if (type.ToLower() == "entreprise")
                        {
                            Console.Write("Nom du référent : ");
                            nomReferent = Console.ReadLine();
                        }
                        db.AjouterClient(type, nom, prenom, adresse, tel, email, nomReferent);
                        Console.WriteLine("Client ajouté avec succès.");
                        break;
                    case "2":
                        Console.Write("ID du client à modifier : ");
                        int idModif = int.Parse(Console.ReadLine());
                        Console.Write("Nouveau nom : ");
                        string newNom = Console.ReadLine();
                        Console.Write("Nouvelle adresse : ");
                        string newAdresse = Console.ReadLine();
                        db.ModifierClient(idModif, newNom, newAdresse);
                        Console.WriteLine("Client modifié avec succès.");
                        break;
                    case "3":
                        Console.Write("ID du client à supprimer : ");
                        int idSupp = int.Parse(Console.ReadLine());
                        db.SupprimerClient(idSupp);
                        Console.WriteLine("Client supprimé avec succès.");
                        break;
                    case "4":
                        var clients = db.ObtenirClients();
                        Console.WriteLine("Liste des clients :");
                        foreach (var c in clients)
                        {
                            Console.WriteLine($"ID : {c.Id}, Type : {c.TypeClient}, Nom : {c.Nom}, Prénom : {c.Prenom}, Adresse : {c.Adresse}");
                        }
                        break;
                    case "5":
                        retour = true;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void CuisinierModule(DatabaseManager db)
        {
            bool retour = false;
            while (!retour)
            {
                Console.WriteLine("=== Gestion Cuisiniers ===");
                Console.WriteLine("1. Ajouter Cuisinier");
                Console.WriteLine("2. Modifier Cuisinier");
                Console.WriteLine("3. Supprimer Cuisinier");
                Console.WriteLine("4. Afficher Cuisiniers");
                Console.WriteLine("5. Retour");
                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();
                Console.WriteLine();

                switch (choix)
                {
                    case "1":
                        Console.Write("Nom : ");
                        string nom = Console.ReadLine();
                        Console.Write("Prénom : ");
                        string prenom = Console.ReadLine();
                        Console.Write("Adresse : ");
                        string adresse = Console.ReadLine();
                        Console.Write("Téléphone : ");
                        string tel = Console.ReadLine();
                        Console.Write("Email : ");
                        string email = Console.ReadLine();
                        db.AjouterCuisinier(nom, prenom, adresse, tel, email);
                        Console.WriteLine("Cuisinier ajouté avec succès.");
                        break;
                    case "2":
                        Console.Write("ID du cuisinier à modifier : ");
                        int idModif = int.Parse(Console.ReadLine());
                        Console.Write("Nouveau nom : ");
                        string newNom = Console.ReadLine();
                        Console.Write("Nouvelle adresse : ");
                        string newAdresse = Console.ReadLine();
                        db.ModifierCuisinier(idModif, newNom, newAdresse);
                        Console.WriteLine("Cuisinier modifié avec succès.");
                        break;
                    case "3":
                        Console.Write("ID du cuisinier à supprimer : ");
                        int idSupp = int.Parse(Console.ReadLine());
                        db.SupprimerCuisinier(idSupp);
                        Console.WriteLine("Cuisinier supprimé avec succès.");
                        break;
                    case "4":
                        var cuisiniers = db.ObtenirCuisiniers();
                        Console.WriteLine("Liste des cuisiniers :");
                        foreach (var c in cuisiniers)
                        {
                            Console.WriteLine($"ID : {c.Id}, Nom : {c.Nom}, Prénom : {c.Prenom}, Adresse : {c.Adresse}");
                        }
                        break;
                    case "5":
                        retour = true;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void CommandeModule(DatabaseManager db)
        {
            bool retour = false;
            while (!retour)
            {
                Console.WriteLine("=== Gestion Commandes ===");
                Console.WriteLine("1. Créer Commande");
                Console.WriteLine("2. Modifier Commande");
                Console.WriteLine("3. Afficher Commande et Calculer Prix");
                Console.WriteLine("4. Retour");
                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();
                Console.WriteLine();

                switch (choix)
                {
                    case "1":
                        Console.Write("ID Client : ");
                        int clientId = int.Parse(Console.ReadLine());
                        Console.Write("ID Cuisinier : ");
                        int cuisinierId = int.Parse(Console.ReadLine());
                        Console.Write("Montant : ");
                        decimal montant = decimal.Parse(Console.ReadLine());
                        Console.Write("Statut : ");
                        string statut = Console.ReadLine();
                        db.CreerCommande(clientId, cuisinierId, montant, statut);
                        Console.WriteLine("Commande créée avec succès.");
                        break;
                    case "2":
                        Console.Write("ID de la commande à modifier : ");
                        int commandeId = int.Parse(Console.ReadLine());
                        Console.Write("Nouveau montant : ");
                        decimal nouveauMontant = decimal.Parse(Console.ReadLine());
                        db.ModifierCommande(commandeId, nouveauMontant);
                        Console.WriteLine("Commande modifiée avec succès.");
                        break;
                    case "3":
                        Console.Write("ID de la commande : ");
                        int idCmd = int.Parse(Console.ReadLine());
                        Commande cmd = db.ObtenirCommande(idCmd);
                        if (cmd != null)
                        {
                            Console.WriteLine($"Commande ID : {cmd.Id}, Client : {cmd.ClientId}, Cuisinier : {cmd.CuisinierId}, Montant : {cmd.Montant}, Statut : {cmd.Statut}");
                            decimal prixCalculé = db.CalculerPrixCommande(cmd.Id);
                            Console.WriteLine("Prix calculé : " + prixCalculé);
                        }
                        else
                        {
                            Console.WriteLine("Commande non trouvée.");
                        }
                        break;
                    case "4":
                        retour = true;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void StatistiquesModule(DatabaseManager db)
        {
            bool retour = false;
            while (!retour)
            {
                Console.WriteLine("=== Statistiques ===");
                Console.WriteLine("1. Nombre de livraisons par cuisinier");
                Console.WriteLine("2. Liste des commandes d'un client sur une période");
                Console.WriteLine("3. Retour");
                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();
                Console.WriteLine();

                switch (choix)
                {
                    case "1":
                        var stats = db.StatistiquesLivraisonsParCuisinier();
                        Console.WriteLine("Livraisons par cuisinier :");
                        foreach (var stat in stats)
                        {
                            Console.WriteLine($"Cuisinier ID : {stat.Key}, Nombre de livraisons : {stat.Value}");
                        }
                        break;
                    case "2":
                        Console.Write("ID du client : ");
                        int clientId = int.Parse(Console.ReadLine());
                        Console.Write("Date début (yyyy-MM-dd) : ");
                        DateTime dateDebut = DateTime.Parse(Console.ReadLine());
                        Console.Write("Date fin (yyyy-MM-dd) : ");
                        DateTime dateFin = DateTime.Parse(Console.ReadLine());
                        var commandes = db.ObtenirCommandesClient(clientId, dateDebut, dateFin);
                        Console.WriteLine("Commandes trouvées :");
                        foreach (var c in commandes)
                        {
                            Console.WriteLine($"Commande ID : {c.Id}, Date : {c.DateCommande}, Montant : {c.Montant}");
                        }
                        break;
                    case "3":
                        retour = true;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
                Console.WriteLine();
            }
        }
    }
    public class Client
    {
        public int Id { get; set; }
        public string TypeClient { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string NomReferent { get; set; }
        public DateTime DateCreation { get; set; }
    }

    public class Cuisinier
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public DateTime DateInscription { get; set; }
    }

    public class Commande
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CuisinierId { get; set; }
        public DateTime DateCommande { get; set; }
        public decimal Montant { get; set; }
        public string Statut { get; set; }
    }

    public class DatabaseManager
    {
        private readonly string connectionString;
        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AjouterClient(string type, string nom, string prenom, string adresse, string telephone, string email, string nomReferent)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Clients 
                                 (TypeClient, Nom, Prenom, Adresse, Telephone, Email, NomReferent, DateCreation)
                                 VALUES (@TypeClient, @Nom, @Prenom, @Adresse, @Telephone, @Email, @NomReferent, GETDATE())";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TypeClient", type);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", string.IsNullOrEmpty(prenom) ? (object)DBNull.Value : prenom);
                    cmd.Parameters.AddWithValue("@Adresse", adresse);
                    cmd.Parameters.AddWithValue("@Telephone", telephone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@NomReferent", string.IsNullOrEmpty(nomReferent) ? (object)DBNull.Value : nomReferent);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModifierClient(int clientId, string nouveauNom, string nouvelleAdresse)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Clients SET Nom = @Nom, Adresse = @Adresse WHERE ClientID = @ClientID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nom", nouveauNom);
                    cmd.Parameters.AddWithValue("@Adresse", nouvelleAdresse);
                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SupprimerClient(int clientId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Clients WHERE ClientID = @ClientID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Client> ObtenirClients()
        {
            List<Client> clients = new List<Client>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ClientID, TypeClient, Nom, Prenom, Adresse, Telephone, Email, NomReferent, DateCreation FROM Clients";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                Id = (int)reader["ClientID"],
                                TypeClient = reader["TypeClient"].ToString(),
                                Nom = reader["Nom"].ToString(),
                                Prenom = reader["Prenom"] == DBNull.Value ? "" : reader["Prenom"].ToString(),
                                Adresse = reader["Adresse"].ToString(),
                                Telephone = reader["Telephone"].ToString(),
                                Email = reader["Email"].ToString(),
                                NomReferent = reader["NomReferent"] == DBNull.Value ? "" : reader["NomReferent"].ToString(),
                                DateCreation = (DateTime)reader["DateCreation"]
                            });
                        }
                    }
                }
            }
            return clients;
        }

        public void AjouterCuisinier(string nom, string prenom, string adresse, string telephone, string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Cuisiniers 
                                 (Nom, Prenom, Adresse, Telephone, Email, DateInscription)
                                 VALUES (@Nom, @Prenom, @Adresse, @Telephone, @Email, GETDATE())";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@Adresse", adresse);
                    cmd.Parameters.AddWithValue("@Telephone", telephone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModifierCuisinier(int cuisinierId, string nouveauNom, string nouvelleAdresse)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Cuisiniers SET Nom = @Nom, Adresse = @Adresse WHERE CuisinierID = @CuisinierID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nom", nouveauNom);
                    cmd.Parameters.AddWithValue("@Adresse", nouvelleAdresse);
                    cmd.Parameters.AddWithValue("@CuisinierID", cuisinierId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SupprimerCuisinier(int cuisinierId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Cuisiniers WHERE CuisinierID = @CuisinierID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CuisinierID", cuisinierId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Cuisinier> ObtenirCuisiniers()
        {
            List<Cuisinier> cuisiniers = new List<Cuisinier>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CuisinierID, Nom, Prenom, Adresse, Telephone, Email, DateInscription FROM Cuisiniers";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cuisiniers.Add(new Cuisinier
                            {
                                Id = (int)reader["CuisinierID"],
                                Nom = reader["Nom"].ToString(),
                                Prenom = reader["Prenom"].ToString(),
                                Adresse = reader["Adresse"].ToString(),
                                Telephone = reader["Telephone"].ToString(),
                                Email = reader["Email"].ToString(),
                                DateInscription = (DateTime)reader["DateInscription"]
                            });
                        }
                    }
                }
            }
            return cuisiniers;
        }

        public void CreerCommande(int clientId, int cuisinierId, decimal montant, string statut)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Commandes 
                                 (ClientID, CuisinierID, Montant, Statut, DateCommande)
                                 VALUES (@ClientID, @CuisinierID, @Montant, @Statut, GETDATE())";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    cmd.Parameters.AddWithValue("@CuisinierID", cuisinierId);
                    cmd.Parameters.AddWithValue("@Montant", montant);
                    cmd.Parameters.AddWithValue("@Statut", statut);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModifierCommande(int commandeId, decimal nouveauMontant)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Commandes SET Montant = @Montant WHERE CommandeID = @CommandeID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Montant", nouveauMontant);
                    cmd.Parameters.AddWithValue("@CommandeID", commandeId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Commande ObtenirCommande(int commandeId)
        {
            Commande commande = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CommandeID, ClientID, CuisinierID, DateCommande, Montant, Statut FROM Commandes WHERE CommandeID = @CommandeID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CommandeID", commandeId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            commande = new Commande
                            {
                                Id = (int)reader["CommandeID"],
                                ClientId = (int)reader["ClientID"],
                                CuisinierId = (int)reader["CuisinierID"],
                                DateCommande = (DateTime)reader["DateCommande"],
                                Montant = (decimal)reader["Montant"],
                                Statut = reader["Statut"].ToString()
                            };
                        }
                    }
                }
            }
            return commande;
        }
        public decimal CalculerPrixCommande(int commandeId)
        {
            decimal montant = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Montant FROM Commandes WHERE CommandeID = @CommandeID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CommandeID", commandeId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        montant = (decimal)result;
                    }
                }
            }
            return montant;
        }

        public Dictionary<int, int> StatistiquesLivraisonsParCuisinier()
        {
            Dictionary<int, int> stats = new Dictionary<int, int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CuisinierID, COUNT(*) AS NombreLivraisons FROM Commandes GROUP BY CuisinierID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int cuisinierId = (int)reader["CuisinierID"];
                            int nbLivraisons = (int)reader["NombreLivraisons"];
                            stats[cuisinierId] = nbLivraisons;
                        }
                    }
                }
            }
            return stats;
        }

        public List<Commande> ObtenirCommandesClient(int clientId, DateTime dateDebut, DateTime dateFin)
        {
            List<Commande> commandes = new List<Commande>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT CommandeID, ClientID, CuisinierID, DateCommande, Montant, Statut 
                                 FROM Commandes 
                                 WHERE ClientID = @ClientID AND DateCommande BETWEEN @DateDebut AND @DateFin";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    cmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                    cmd.Parameters.AddWithValue("@DateFin", dateFin);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            commandes.Add(new Commande
                            {
                                Id = (int)reader["CommandeID"],
                                ClientId = (int)reader["ClientID"],
                                CuisinierId = (int)reader["CuisinierID"],
                                DateCommande = (DateTime)reader["DateCommande"],
                                Montant = (decimal)reader["Montant"],
                                Statut = reader["Statut"].ToString()
                            });
                        }
                    }
                }
            }
            return commandes;
        }
    }
}
