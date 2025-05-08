DROP DATABASE IF EXISTS asso;
CREATE DATABASE IF NOT EXISTS asso;
USE asso;

-- Suppression des tables dans l’ordre des dépendances
DROP TABLE IF EXISTS historique;
DROP TABLE IF EXISTS appartient;
DROP TABLE IF EXISTS commander;
DROP TABLE IF EXISTS plat;
DROP TABLE IF EXISTS Commande;
DROP TABLE IF EXISTS Client_particulier;
DROP TABLE IF EXISTS Cuisinier;
DROP TABLE IF EXISTS Client_entreprise;

-- Table client entreprise
CREATE TABLE Client_entreprise (
  Id_Client_entreprise INT AUTO_INCREMENT PRIMARY KEY,
  nom                  VARCHAR(50) NOT NULL,
  adresse_entreprise   VARCHAR(100),
  referent             VARCHAR(50)
);

-- Table client particulier
CREATE TABLE Client_particulier (
  Id_Client   INT AUTO_INCREMENT PRIMARY KEY,
  nom         VARCHAR(50) NOT NULL,
  prenom      VARCHAR(50),
  rue         VARCHAR(100),
  numPorte    INT,
  codePostal  INT,
  ville       VARCHAR(50),
  telephone   VARCHAR(20),
  email       VARCHAR(100),
  metro       VARCHAR(50)
);

-- Table cuisinier
CREATE TABLE Cuisinier (
  Id_Cuisinier INT AUTO_INCREMENT PRIMARY KEY,
  nom_         VARCHAR(50) NOT NULL,
  prenom       VARCHAR(50),
  rue          VARCHAR(100),
  numPorte     INT,
  codePostal   INT,
  ville        VARCHAR(50),
  telephone    VARCHAR(20) UNIQUE,
  email        VARCHAR(100),
  metro        VARCHAR(50)
);

-- Table commande
CREATE TABLE Commande (
  Id_Commande   INT AUTO_INCREMENT PRIMARY KEY,
  Id_Cuisinier  INT NOT NULL,
  Id_Client     INT NOT NULL,
  composition   VARCHAR(255),
  prix          DECIMAL(10,2),
  quantite      INT,
  plat          VARCHAR(100),
  dateFab       DATE,
  datePer       DATE,
  regime        VARCHAR(50),
  nature        VARCHAR(50),
  ing1          VARCHAR(50), vol1 VARCHAR(50),
  ing2          VARCHAR(50), vol2 VARCHAR(50),
  ing3          VARCHAR(50), vol3 VARCHAR(50),
  ing4          VARCHAR(50), vol4 VARCHAR(50),
  FOREIGN KEY (Id_Cuisinier) REFERENCES Cuisinier(Id_Cuisinier),
  FOREIGN KEY (Id_Client)    REFERENCES Client_particulier(Id_Client)
);

-- Table plat
CREATE TABLE plat (
  nom_plat                INT AUTO_INCREMENT PRIMARY KEY,
  photo                   VARCHAR(255),
  type_dessertplat_entree ENUM('entrée','plat principal','dessert'),
  ingredients             VARCHAR(255),
  nationalite             VARCHAR(50),
  date_de_fabrication     DATE,
  date_de_peremption      DATE,
  Id_Cuisinier            INT NOT NULL,
  FOREIGN KEY (Id_Cuisinier) REFERENCES Cuisinier(Id_Cuisinier)
);

-- Table commander (liaison client–commande)
CREATE TABLE commander (
  Id_Client           INT NOT NULL,
  Id_Client_entreprise INT,
  Id_Commande         INT NOT NULL,
  PRIMARY KEY (Id_Client, Id_Client_entreprise, Id_Commande),
  FOREIGN KEY (Id_Client)            REFERENCES Client_particulier(Id_Client),
  FOREIGN KEY (Id_Client_entreprise) REFERENCES Client_entreprise(Id_Client_entreprise),
  FOREIGN KEY (Id_Commande)          REFERENCES Commande(Id_Commande)
);

-- Table appartient (liaison commande–plat)
CREATE TABLE appartient (
  Id_Commande INT NOT NULL,
  nom_plat    INT NOT NULL,
  photo       VARCHAR(255),
  PRIMARY KEY (Id_Commande, nom_plat),
  FOREIGN KEY (Id_Commande) REFERENCES Commande(Id_Commande),
  FOREIGN KEY (nom_plat)    REFERENCES plat(nom_plat)
);

-- Table historique
CREATE TABLE historique (
  Id_Historique          INT AUTO_INCREMENT PRIMARY KEY,
  Id_Commande            INT NOT NULL,
  Id_Cuisinier           INT NOT NULL,
  Id_Client              INT,
  Id_Client_entreprise   INT,
  enregistrement_dans_la_base VARCHAR(255),
  FOREIGN KEY (Id_Commande)          REFERENCES Commande(Id_Commande),
  FOREIGN KEY (Id_Cuisinier)         REFERENCES Cuisinier(Id_Cuisinier),
  FOREIGN KEY (Id_Client)            REFERENCES Client_particulier(Id_Client),
  FOREIGN KEY (Id_Client_entreprise) REFERENCES Client_entreprise(Id_Client_entreprise)
);

-- Table transaction
CREATE TABLE `transaction` (
  Id_Client     INT NOT NULL,
  Id_Cuisinier  INT NOT NULL,
  PRIMARY KEY (Id_Client, Id_Cuisinier),
  FOREIGN KEY (Id_Client)    REFERENCES Client_particulier(Id_Client),
  FOREIGN KEY (Id_Cuisinier) REFERENCES Cuisinier(Id_Cuisinier)
);

SET SQL_SAFE_UPDATES = 0;

-- Quelques contrôles
DESC Commande;
SELECT COUNT(*) AS nbCommandes FROM Commande;
SELECT * FROM Commande;

DESC Cuisinier;
SELECT COUNT(*) AS nbCuisiniers FROM Cuisinier;
SELECT * FROM Cuisinier;

DESC Client_particulier;
SELECT COUNT(*) AS nbClients FROM Client_particulier;
SELECT * FROM Client_particulier;
