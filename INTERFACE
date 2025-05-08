using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Spectre.Console;
using SC = Spectre.Console;

namespace LivInParisConsoleApp
{
    // Classes entité
    public class ClientParticulier
    {
        public int Id_Client { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Rue { get; set; }
        public int NumPorte { get; set; }
        public int CodePostal { get; set; }
        public string Ville { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Metro { get; set; }
    }

    public class ClientEntreprise
    {
        public int Id_Client_entreprise { get; set; }
        public string Nom { get; set; }
        public string AdresseEntreprise { get; set; }
        public string Referent { get; set; }
    }

    public class Cuisinier
    {
        public int Id_Cuisinier { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Rue { get; set; }
        public int NumPorte { get; set; }
        public int CodePostal { get; set; }
        public string Ville { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Metro { get; set; }
    }

    public class CommandeEntity
    {
        public int Id_Commande { get; set; }
        public int Id_Cuisinier { get; set; }
        public int Id_Client { get; set; }
        public string Composition { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
        public string Plat { get; set; }
        public DateTime DateFab { get; set; }
        public DateTime DatePer { get; set; }
        public string Regime { get; set; }
        public string Nature { get; set; }
        public string Ing1 { get; set; }
        public string Vol1 { get; set; }
        public string Ing2 { get; set; }
        public string Vol2 { get; set; }
        public string Ing3 { get; set; }
        public string Vol3 { get; set; }
        public string Ing4 { get; set; }
        public string Vol4 { get; set; }
    }

    // Couche d'accès aux données
    public static class DataAccess
    {
        private const string ConnString = "SERVER=localhost;PORT=3306;DATABASE=asso;UID=root;PASSWORD=TDRoumi01112005?";

        public static IEnumerable<dynamic> Query(string sql, object parameters = null)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            if (parameters != null)
                foreach (var prop in parameters.GetType().GetProperties())
                    cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters));

            using var reader = cmd.ExecuteReader();
            var results = new List<dynamic>();
            while (reader.Read())
            {
                var expando = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < reader.FieldCount; i++)
                    expando.Add(reader.GetName(i), reader.GetValue(i));
                results.Add(expando);
            }
            return results;
        }

        public static int Execute(string sql, object parameters = null)
        {
            using var conn = new MySqlConnection(ConnString);
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            if (parameters != null)
                foreach (var prop in parameters.GetType().GetProperties())
                    cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters));
            return cmd.ExecuteNonQuery();
        }
    }

    // Helper export JSON/XML
    public static class Exporter
    {
        public static void ToJson<T>(IEnumerable<T> list, string path)
        {
            var json = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, json);
            SC.AnsiConsole.MarkupLine($"[green]Exporté en JSON :[/] {path}");
        }

        public static void ToXml<T>(IEnumerable<T> list, string path)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            using var stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, new List<T>(list));
            SC.AnsiConsole.MarkupLine($"[green]Exporté en XML :[/] {path}");
        }
    }

    class Program
    {
        static void Main()
        {
            SC.AnsiConsole.Write(
                new SC.FigletText("Liv'In Paris").Centered().Color(SC.Color.CadetBlue));

            while (true)
            {
                var choix = SC.AnsiConsole.Prompt(
                    new SC.SelectionPrompt<string>()
                        .Title("[yellow]Menu Principal[/]")
                        .AddChoices(
                            "Module Clients",
                            "Module Cuisiniers",
                            "Module Commandes",
                            "Module Statistiques",
                            "Exporter JSON/XML",
                            "Quitter"
                        ));

                switch (choix)
                {
                    case "Module Clients": ClientsMenu(); break;
                    case "Module Cuisiniers": CuisiniersMenu(); break;
                    case "Module Commandes": CommandesMenu(); break;
                    case "Module Statistiques": StatsMenu(); break;
                    case "Exporter JSON/XML": ExportMenu(); break;
                    case "Quitter": return;
                }
            }
        }

        static void ClientsMenu()
        {
            var option = SC.AnsiConsole.Prompt(
                new SC.SelectionPrompt<string>()
                    .Title("[yellow]Module Clients[/]")
                    .AddChoices(
                        "Lister alphabétique",
                        "Lister par rue",
                        "Total achats par client",
                        "Ajouter client",
                        "Retour"
                    ));
            if (option == "Retour") return;

            switch (option)
            {
                case "Lister alphabétique":
                    RenderTable(DataAccess.Query("SELECT * FROM Client_particulier ORDER BY nom, prenom"));
                    break;
                case "Lister par rue":
                    RenderTable(DataAccess.Query("SELECT * FROM Client_particulier ORDER BY rue, numPorte"));
                    break;
                case "Total achats par client":
                    RenderTable(DataAccess.Query(
                        "SELECT cp.nom, cp.prenom, SUM(c.prix*c.quantite) AS TotalAchats FROM Client_particulier cp JOIN Commande c ON cp.Id_Client=c.Id_Client GROUP BY cp.Id_Client"));
                    break;
                case "Ajouter client":
                    var type = SC.AnsiConsole.Prompt(
                        new SC.SelectionPrompt<string>()
                            .Title("[yellow]Type de client[/]")
                            .AddChoices("Particulier", "Entreprise"));
                    if (type == "Particulier") AddClientParticulier(); else AddClientEntreprise();
                    break;
            }
        }

        static void AddClientParticulier()
        {
            var nom = SC.AnsiConsole.Ask<string>("Nom :");
            var prenom = SC.AnsiConsole.Ask<string>("Prénom :");
            var rue = SC.AnsiConsole.Ask<string>("Rue :");
            var num = SC.AnsiConsole.Ask<int>("N° de porte :");
            var cp = SC.AnsiConsole.Ask<int>("Code postal :");
            var ville = SC.AnsiConsole.Ask<string>("Ville :");
            var tel = SC.AnsiConsole.Ask<string>("Téléphone :");
            var email = SC.AnsiConsole.Ask<string>("E-mail :");
            var metro = SC.AnsiConsole.Ask<string>("Station métro :");

            DataAccess.Execute(
                "INSERT INTO Client_particulier (nom, prenom, rue, numPorte, codePostal, ville, telephone, email, metro) VALUES (@nom,@prenom,@rue,@num,@cp,@ville,@tel,@email,@metro)",
                new { nom, prenom, rue, num, cp, ville, tel, email, metro });
            SC.AnsiConsole.MarkupLine("[green]Client particulier ajouté[/]");
        }

        static void AddClientEntreprise()
        {
            var nomE = SC.AnsiConsole.Ask<string>("Nom entreprise :");
            var addr = SC.AnsiConsole.Ask<string>("Adresse :");
            var refent = SC.AnsiConsole.Ask<string>("Référent :");

            DataAccess.Execute(
                "INSERT INTO Client_entreprise (nom, adresse_entreprise, referent) VALUES (@nomE,@addr,@refent)",
                new { nomE, addr, refent });
            SC.AnsiConsole.MarkupLine("[green]Client entreprise ajouté[/]");
        }

        static void CuisiniersMenu()
        {
            var option = SC.AnsiConsole.Prompt(
                new SC.SelectionPrompt<string>()
                    .Title("[yellow]Module Cuisiniers[/]")
                    .AddChoices(
                        "Clients desservis",
                        "Plats par fréquence",
                        "Plat du jour",
                        "Ajouter cuisinier",
                        "Retour"
                    ));
            if (option == "Retour") return;
            switch (option)
            {
                case "Clients desservis":
                    var id = SC.AnsiConsole.Ask<int>("ID cuisinier :");
                    RenderTable(DataAccess.Query(
                        "SELECT DISTINCT cp.* FROM Client_particulier cp JOIN Commande c ON cp.Id_Client=c.Id_Client WHERE c.Id_Cuisinier=@id", new { id }));
                    break;
                case "Plats par fréquence":
                    RenderTable(DataAccess.Query(
                        "SELECT plat, COUNT(*) AS Frequence FROM Commande GROUP BY plat ORDER BY Frequence DESC"));
                    break;
                case "Plat du jour":
                    var special = DataAccess.Query("SELECT plat FROM Commande WHERE dateFab=CURDATE() LIMIT 1");
                    SC.AnsiConsole.MarkupLine($"[yellow]Plat du jour :[/] {special.First().plat}");
                    break;
                case "Ajouter cuisinier":
                    var nom = SC.AnsiConsole.Ask<string>("Nom :");
                    var prenom = SC.AnsiConsole.Ask<string>("Prénom :");
                    var rue = SC.AnsiConsole.Ask<string>("Rue :");
                    var num = SC.AnsiConsole.Ask<int>("N° de porte :");
                    var cp2 = SC.AnsiConsole.Ask<int>("Code postal :");
                    var ville = SC.AnsiConsole.Ask<string>("Ville :");
                    var tel = SC.AnsiConsole.Ask<string>("Téléphone :");
                    var email = SC.AnsiConsole.Ask<string>("E-mail :");
                    var metro = SC.AnsiConsole.Ask<string>("Station métro :");

                    DataAccess.Execute(
                        "INSERT INTO Cuisinier (nom_, prenom, rue, numPorte, codePostal, ville, telephone, email, metro) VALUES (@nom,@prenom,@rue,@num,@cp,@ville,@tel,@email,@metro)",
                        new { nom, prenom, rue, num, cp = cp2, ville, tel, email, metro });
                    SC.AnsiConsole.MarkupLine("[green]Cuisinier ajouté[/]");
                    break;
            }
        }

        static void CommandesMenu()
        {
            var option = SC.AnsiConsole.Prompt(
                new SC.SelectionPrompt<string>()
                    .Title("[yellow]Module Commandes[/]")
                    .AddChoices("Créer commande", "Modifier commande", "Calculer prix", "Retour"));
            if (option == "Retour") return;
            switch (option)
            {
                case "Créer commande": AddCommande(); break;
                case "Modifier commande": UpdateCommande(); break;
                case "Calculer prix":
                    var idCalc = SC.AnsiConsole.Ask<int>("ID commande :");
                    var result = DataAccess.Query("SELECT prix*quantite AS Total FROM Commande WHERE Id_Commande=@id", new { id = idCalc });
                    SC.AnsiConsole.MarkupLine($"[green]Total :[/] {result.First().Total}");
                    break;
            }
        }
        static void AddCommande()
        {
            var cui = SC.AnsiConsole.Ask<int>("ID cuisinier :");
            var cli = SC.AnsiConsole.Ask<int>("ID client :");
            var comp = SC.AnsiConsole.Ask<string>("Composition :");
            var prix = SC.AnsiConsole.Ask<decimal>("Prix unitaire :");
            var qty = SC.AnsiConsole.Ask<int>("Quantité :");
            var plat = SC.AnsiConsole.Ask<string>("Plat :");
            var df = SC.AnsiConsole.Ask<DateTime>("Date de fabrication (aaaa-MM-jj) :");
            var dp = SC.AnsiConsole.Ask<DateTime>("Date de péremption (aaaa-MM-jj) :");
            var reg = SC.AnsiConsole.Ask<string>("Régime :");
            var nat = SC.AnsiConsole.Ask<string>("Nature :");
            var i1 = SC.AnsiConsole.Ask<string>("Ingrédient 1 :"); var v1 = SC.AnsiConsole.Ask<string>("Volume 1 :");
            var i2 = SC.AnsiConsole.Ask<string>("Ingrédient 2 :"); var v2 = SC.AnsiConsole.Ask<string>("Volume 2 :");
            var i3 = SC.AnsiConsole.Ask<string>("Ingrédient 3 :"); var v3 = SC.AnsiConsole.Ask<string>("Volume 3 :");
            var i4 = SC.AnsiConsole.Ask<string>("Ingrédient 4 :"); var v4 = SC.AnsiConsole.Ask<string>("Volume 4 :");

            DataAccess.Execute(
                "INSERT INTO Commande (Id_Cuisinier, Id_Client, composition, prix, quantite, plat, dateFab, datePer, regime, nature, ing1, vol1, ing2, vol2, ing3, vol3, ing4, vol4) VALUES (@cui,@cli,@comp,@prix,@qty,@plat,@df,@dp,@reg,@nat,@i1,@v1,@i2,@v2,@i3,@v3,@i4,@v4)",
                new { cui, cli, comp, prix, qty, plat, df, dp, reg, nat, i1, v1, i2, v2, i3, v3, i4, v4 });
            SC.AnsiConsole.MarkupLine("[green]Commande créée[/]");
        }

        static void UpdateCommande()
        {
            var id = SC.AnsiConsole.Ask<int>("ID commande à modifier :");
            var champ = SC.AnsiConsole.Prompt(
                new SC.SelectionPrompt<string>()
                    .Title("Champ à modifier :")
                    .AddChoices("composition", "prix", "quantite", "plat", "dateFab", "datePer", "regime", "nature", "Retour"));
            if (champ == "Retour") return;
            var nv = SC.AnsiConsole.Ask<string>($"Nouvelle valeur pour {champ} :");
            object val = champ switch
            {
                "prix" => Convert.ToDecimal(nv),
                "quantite" => Convert.ToInt32(nv),
                _ when champ == "dateFab" || champ == "datePer" => DateTime.Parse(nv),
                _ => nv
            };
            DataAccess.Execute($"UPDATE Commande SET {champ}=@val WHERE Id_Commande=@id", new { val, id });
            SC.AnsiConsole.MarkupLine("[green]Commande modifiée[/]");
        }

        static void StatsMenu()
        {
            var option = SC.AnsiConsole.Prompt(
                new SC.SelectionPrompt<string>()
                    .Title("[yellow]Module Statistiques[/]")
                    .AddChoices(
                        "Livraisons par cuisinier",
                        "Commandes par période",
                        "Prix moyen des commandes",
                        "Commandes moyennes par client",
                        "Commandes clients par nationalité & période",
                        "Retour"
                    ));
            if (option == "Retour") return;
            switch (option)
            {
                case "Livraisons par cuisinier":
                    RenderTable(DataAccess.Query("SELECT Id_Cuisinier, COUNT(*) AS Livraisons FROM Commande GROUP BY Id_Cuisinier"));
                    break;
                case "Commandes par période":
                    var sd = SC.AnsiConsole.Ask<DateTime>("Date de début (aaaa-MM-jj) :");
                    var ed = SC.AnsiConsole.Ask<DateTime>("Date de fin (aaaa-MM-jj) :");
                    RenderTable(DataAccess.Query("SELECT * FROM Commande WHERE dateFab BETWEEN @sd AND @ed", new { sd, ed }));
                    break;
                case "Prix moyen des commandes":
                    var avg = DataAccess.Query("SELECT AVG(prix*quantite) AS Moyenne FROM Commande");
                    SC.AnsiConsole.MarkupLine($"[green]Prix moyen des commandes :[/] {avg.First().Moyenne:F2}");
                    break;
                case "Commandes moyennes par client":
                    var avgc = DataAccess.Query("SELECT AVG(cnt) AS MoyenneClients FROM(SELECT COUNT(*) AS cnt FROM Commande GROUP BY Id_Client) AS sub");
                    SC.AnsiConsole.MarkupLine($"[green]Commandes moyennes par client :[/] {avgc.First().MoyenneClients:F2}");
                    break;
                case "Commandes clients par nationalité & période":
                    var nat = SC.AnsiConsole.Ask<string>("Nationalité :");
                    var s = SC.AnsiConsole.Ask<DateTime>("Date de début (aaaa-MM-jj) :");
                    var e = SC.AnsiConsole.Ask<DateTime>("Date de fin (aaaa-MM-jj) :");
                    RenderTable(DataAccess.Query(
                        "SELECT c.Id_Client AS IdClient, COUNT(*) AS NombreCommandes FROM Commande c JOIN plat p ON c.plat=p.nom_plat WHERE p.nationalite=@nat AND c.dateFab BETWEEN @s AND @e GROUP BY c.Id_Client", new { nat, s, e }));
                    break;
            }
        }

        static void ExportMenu()
        {
            var option = SC.AnsiConsole.Prompt(
                new SC.SelectionPrompt<string>()
                    .Title("[yellow]Exporter JSON/XML[/]")
                    .AddChoices("Clients", "Cuisiniers", "Retour"));
            if (option == "Retour") return;
            switch (option)
            {
                case "Clients":
                    var clients = DataAccess.Query("SELECT * FROM Client_particulier");
                    Exporter.ToJson(clients, "clients.json");
                    Exporter.ToXml(clients, "clients.xml");
                    break;
                case "Cuisiniers":
                    var cuis = DataAccess.Query("SELECT * FROM Cuisinier");
                    Exporter.ToJson(cuis, "cuisiniers.json");
                    Exporter.ToXml(cuis, "cuisiniers.xml");
                    break;
            }
        }

        // Affichage tableau générique
        static void RenderTable(IEnumerable<dynamic> data)
        {
            var table = new SC.Table().Border(SC.TableBorder.Rounded);
            var firstRow = data.FirstOrDefault() as IDictionary<string, object>;
            if (firstRow == null)
            {
                SC.AnsiConsole.MarkupLine("[red]Aucune donnée.[/]");
                return;
            }
            foreach (var col in firstRow.Keys) table.AddColumn(col);
            foreach (IDictionary<string, object> row in data)
                table.AddRow(row.Values.Select(v => v?.ToString() ?? string.Empty).ToArray());
            SC.AnsiConsole.Render(table);
        }
    }
}
