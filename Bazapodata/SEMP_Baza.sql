-- ============================================================
-- SEMP_Slozen - Kreiranje baze podataka
-- Server: DESKTOP-32N2DQM
-- ============================================================

USE master;
GO

-- Obrisi bazu ako vec postoji (za cist pocetak)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SEMP2025SLOZEN')
BEGIN
    ALTER DATABASE SEMP2025SLOZEN SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SEMP2025SLOZEN;
END
GO

-- Kreiraj bazu
CREATE DATABASE SEMP2025SLOZEN;
GO

USE SEMP2025SLOZEN;
GO

-- ============================================================
-- TABELE
-- ============================================================

CREATE TABLE Korisnici (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    KorisnickoIme NVARCHAR(64) NOT NULL,
    Lozinka NVARCHAR(256) NOT NULL,
    Uloga NVARCHAR(32) NOT NULL DEFAULT 'korisnik',
    CONSTRAINT UQ_Korisnici_KorisnickoIme UNIQUE (KorisnickoIme)
);
GO

CREATE TABLE Epizode (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Naslov NVARCHAR(128) NOT NULL,
    Opis NVARCHAR(512) NOT NULL,
    DatumPremijere DATETIME NOT NULL,
    KreiraoId INT NOT NULL,
    CONSTRAINT FK_Epizode_Korisnici FOREIGN KEY (KreiraoId)
        REFERENCES Korisnici(Id)
);
GO

CREATE TABLE Ocene (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EpizodaId INT NOT NULL,
    KorisnikId INT NOT NULL,
    Vrednost INT NOT NULL,
    Komentar NVARCHAR(512) NULL,
    OcenjeneNa DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Ocene_Epizode FOREIGN KEY (EpizodaId)
        REFERENCES Epizode(Id),
    CONSTRAINT FK_Ocene_Korisnici FOREIGN KEY (KorisnikId)
        REFERENCES Korisnici(Id),
    CONSTRAINT CHK_Ocene_Vrednost CHECK (Vrednost >= 1 AND Vrednost <= 5)
);
GO

-- ============================================================
-- SEED PODACI - Korisnici
-- ============================================================

INSERT INTO Korisnici (KorisnickoIme, Lozinka, Uloga) VALUES
('admin',  'admin123',  'admin'),
('marko',  'marko123',  'korisnik'),
('ana',    'ana123',    'korisnik'),
('petar',  'petar123',  'korisnik'),
('jovana', 'jovana123', 'korisnik');
GO

-- ============================================================
-- SEED PODACI - Epizode
-- ============================================================

INSERT INTO Epizode (Naslov, Opis, DatumPremijere, KreiraoId) VALUES
('Pilot - Pocetak Svega',
 'Uvod u svet serije. Upoznajemo glavne likove i pocetnu situaciju koja ce promeniti sve.',
 '2024-01-15', 1),

('Tajna Proslost',
 'Otkriva se misterija iz proslosti glavnog lika. Nista nije onako kako se cinilo.',
 '2024-01-22', 1),

('Izdaja',
 'Neko iz ekipe prelazi na drugu stranu. Prijateljstvo je stavljeno na tesku probu.',
 '2024-01-29', 1),

('Povratak',
 'Lik kojeg smo izgubili se vraca - ali promenjen. Da li je to ista osoba?',
 '2024-02-05', 1),

('Finalna Bitka',
 'Sve se zbiva u ovu jednu noc. Sudbine svih likova se odlucuju. Ko ce preziveti?',
 '2024-02-12', 1),

('Epilog',
 'Kako su se stvari zavrsile. Zatvaranje prvih prica i nagovestaj novog pocetka.',
 '2024-02-19', 1),

('Novi Poceci',
 'Druga sezona pocinje. Novi likovi ulaze u igru, novi problemi se pojavljuju.',
 '2024-09-01', 1),

('Mracni Grad',
 'Istraga u gradu prepunom tajni. Svako ima nesto da krije.',
 '2024-09-08', 1),

('Izgubljena Uspomena',
 'Neko nema secanja. Ali neko ih ima previse. Sta se zapravo desilo one noci?',
 '2024-09-15', 1),

('Zora',
 'Vrhunac druge sezone. Sve dolazi na naplatu. Nista nece biti isto posle ovoga.',
 '2024-09-22', 1);
GO

-- ============================================================
-- SEED PODACI - Ocene (da se odmah vide top liste)
-- ============================================================

-- Epizoda 5 - Finalna Bitka (treba da bude #1)
INSERT INTO Ocene (EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa) VALUES
(5, 2, 5, 'Neverovatna epizoda, najbolja u seriji!', '2024-02-13'),
(5, 3, 5, 'Savrseno napisana i odglumljena.', '2024-02-13'),
(5, 4, 4, 'Odlicna, malo mi je nedostajao vise razvoj lika.', '2024-02-14'),
(5, 5, 5, 'Ovo je razlog zasto pratim ovu seriju!', '2024-02-14');

-- Epizoda 1 - Pilot
INSERT INTO Ocene (EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa) VALUES
(1, 2, 4, 'Dobar uvod, zainteresovalo me.', '2024-01-16'),
(1, 3, 5, 'Odlican pilot, odmah me uvuklo!', '2024-01-16'),
(1, 4, 4, 'Solidno, jedva cekam nastavak.', '2024-01-17');

-- Epizoda 10 - Zora
INSERT INTO Ocene (EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa) VALUES
(10, 2, 5, 'Bolje od finala prve sezone!', '2024-09-23'),
(10, 3, 4, 'Jako dobro, par stvari moglo biti bolje.', '2024-09-23'),
(10, 5, 5, 'Maestralno!', '2024-09-24');

-- Epizoda 3 - Izdaja
INSERT INTO Ocene (EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa) VALUES
(3, 2, 4, 'Nisam ocekivao taj obrt!', '2024-01-30'),
(3, 4, 5, 'Najbolja epizoda do sada.', '2024-01-30'),
(3, 5, 4, 'Dobar razvoj price.', '2024-01-31');

-- Epizoda 4 - Povratak
INSERT INTO Ocene (EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa) VALUES
(4, 3, 5, 'Emotivno i mocno.', '2024-02-06'),
(4, 5, 4, 'Lepo uradjeno.', '2024-02-06');

-- Epizoda 2 - Tajna Proslost
INSERT INTO Ocene (EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa) VALUES
(2, 2, 3, 'Malo spora ali zanimljiva.', '2024-01-23'),
(2, 4, 4, 'Dobra ekspozicija.', '2024-01-23');

-- Epizoda 7 - Novi Poceci
INSERT INTO Ocene (EpizodaId, KorisnikId, Vrednost, Komentar, OcenjeneNa) VALUES
(7, 3, 3, 'Solidan start druge sezone.', '2024-09-02'),
(7, 5, 4, 'Nove dinamike su zanimljive.', '2024-09-02');
GO

-- ============================================================
-- PROVERA
-- ============================================================

SELECT 'Korisnici' AS Tabela, COUNT(*) AS BrojSlogova FROM Korisnici
UNION ALL
SELECT 'Epizode', COUNT(*) FROM Epizode
UNION ALL
SELECT 'Ocene', COUNT(*) FROM Ocene;
GO

PRINT '✓ Baza SEMP2025SLOZEN uspesno kreirana!';
PRINT '✓ Korisnici, Epizode i Ocene su popunjeni.';
PRINT '✓ Mozete pokrenuti aplikaciju.';
GO