DROP DATABASE IF EXISTS asso;
CREATE DATABASE IF NOT EXISTS asso;
USE asso;

CREATE TABLE Client_entreprise(
   Id_Client_entreprise INT AUTO_INCREMENT,
   nom VARCHAR(50),
   adresse_entreprise VARCHAR(50),
   réferent VARCHAR(50),
   PRIMARY KEY(Id_Client_entreprise)
);

DROP TABLE IF EXISTS Cuisinier;
CREATE TABLE Cuisinier(
   Id_Cuisinier INT,
   nom_ VARCHAR(50),
   prénom VARCHAR(50),
   rue VARCHAR(50),
   numPorte INT ,
   codePosal INT,
   ville VARCHAR(50),
   telephone INT,
   email VARCHAR(50),
   metro VARCHAR(50),
   PRIMARY KEY(Id_Cuisinier),
   UNIQUE KEY idx_cuisinier_telephone (Id_Cuisinier, telephone)
);

CREATE TABLE historique(
   Id_Commande INT AUTO_INCREMENT,
   Id_Cuisinier INT ,
   Id_Client INT,
   Id_Client_entreprise INT ,
   enregistrement_dans_la_base VARCHAR(50),
   PRIMARY KEY(Id_Commande, Id_Cuisinier, Id_Client, Id_Client_entreprise)
);

DROP TABLE IF EXISTS Client_particulier;
CREATE TABLE Client_particulier(
   Id_Client INT,
   nom VARCHAR(50),
   prenom VARCHAR(50),
   rue VARCHAR(50),
   numPorte INT ,
   codePosal INT,
   ville VARCHAR(50),
   telephone INT ,
   email VARCHAR(50),
   metro VARCHAR(50),
   PRIMARY KEY(Id_Client),
   UNIQUE KEY uk_client_particulier (Id_Client, telephone)
);

DROP TABLE IF EXISTS Commande;
CREATE TABLE Commande(
   Id_Commande INT,
   Id_Cuisinier INT NOT NULL,
   Id_Client INT NOT NULL,
   composition VARCHAR(50),
   prix VARCHAR(50),
   quantite VARCHAR(50),
   plat VARCHAR(50),
   dateFab VARCHAR(50),
   datePer VARCHAR(50),
   regime VARCHAR(50),
   nature VARCHAR(50),
   ing1 VARCHAR(50),
   vol1 VARCHAR(50),
   ing2 VARCHAR(50),
   vol2 VARCHAR(50),
   ing3 VARCHAR(50),
   vol3 VARCHAR(50),
   ing4 VARCHAR(50),
   vol4 VARCHAR(50),
   
   PRIMARY KEY(Id_Commande)
);

CREATE TABLE plat(
   nom_plat INT AUTO_INCREMENT,
   photo VARCHAR(50),
   type_dessertplat_principalentrée ENUM('entrée', 'plat principal', 'dessert'),
   ingrédients VARCHAR(50),
   nationalité VARCHAR(50),
   date_de_fabrication VARCHAR(50),
   date_de_peremption VARCHAR(50),
   Id_Cuisinier INT NOT NULL,
   telephone INT NOT NULL,
   PRIMARY KEY(nom_plat, photo),
   FOREIGN KEY(Id_Cuisinier, telephone) REFERENCES Cuisinier(Id_Cuisinier, telephone)
);

CREATE TABLE commander(
   Id_Client INT,
   telephone INT,
   Id_Client_entreprise INT,
   Id_Commande INT,
   PRIMARY KEY(Id_Client, telephone, Id_Client_entreprise, Id_Commande),
   FOREIGN KEY(Id_Client, telephone) REFERENCES Client_particulier(Id_Client, telephone),
   FOREIGN KEY(Id_Client_entreprise) REFERENCES Client_entreprise(Id_Client_entreprise),
   FOREIGN KEY(Id_Commande) REFERENCES Commande(Id_Commande)
);

CREATE TABLE appartient(
   Id_Commande INT,
   nom_plat INT,
   photo VARCHAR(50),
   PRIMARY KEY(Id_Commande, nom_plat, photo),
   FOREIGN KEY(Id_Commande) REFERENCES Commande(Id_Commande),
   FOREIGN KEY(nom_plat, photo) REFERENCES plat(nom_plat, photo)
);

CREATE TABLE transaction(
   Id_Client INT,
   telephone INT,
   Id_Cuisinier INT,
   telephone_1 INT,
   PRIMARY KEY(Id_Client, telephone, Id_Cuisinier, telephone_1),
   FOREIGN KEY(Id_Client, telephone) REFERENCES Client_particulier(Id_Client, telephone),
   FOREIGN KEY(Id_Cuisinier, telephone_1) REFERENCES Cuisinier(Id_Cuisinier, telephone)
);

SET SQL_SAFE_UPDATES =0;

DESC Commande;
SELECT count(*) AS nbCommande FROM Commande;
SELECT * FROM Commande;

DESC Cuisinier;
SELECT count(*) AS nbCuisiner FROM Cuisinier;
SELECT * FROM Cuisinier;

DESC Client_particulier;
SELECT count(*) AS nbClients FROM Client_particulier;
SELECT * FROM Client_particulier;