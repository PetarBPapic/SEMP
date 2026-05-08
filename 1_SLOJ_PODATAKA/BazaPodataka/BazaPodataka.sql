CREATE DATABASE SEMP2025SLOZEN;
GO

USE SEMP2025SLOZEN;
GO

-- Korisnici
CREATE TABLE Korisnici (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    KorisnickoIme NVARCHAR(64) NOT NULL UNIQUE,
    Lozinka NVARCHAR(256) NOT NULL,
    Uloga NVARCHAR(32) NOT NULL DEFAULT 'korisnik'
);
GO

-- Epizode
CREATE TABLE Epizode (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Naslov NVARCHAR(128) NOT NULL,
    Opis NVARCHAR(512) NOT NULL,
    DatumPremijere DATETIME NOT NULL,
    KreiraoId INT NOT NULL,
    CONSTRAINT FK_Epizode_Korisnici FOREIGN KEY (KreiraoId) 
        REFERENCES Korisnici(Id) ON DELETE RESTRICT
);
GO

-- Ocene
CREATE TABLE Ocene (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EpizodaId INT NOT NULL,
    KorisnikId INT NOT NULL,
    Vrednost INT NOT NULL CHECK (Vrednost BETWEEN 1 AND 5),
    Komentar NVARCHAR(512) NULL,
    OcenjeneNa DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Ocene_Epizode FOREIGN KEY (EpizodaId)
        REFERENCES Epizode(Id) ON DELETE RESTRICT,
    CONSTRAINT FK_Ocene_Korisnici FOREIGN KEY (KorisnikId)
        REFERENCES Korisnici(Id) ON DELETE CASCADE
);
GO

-- Top liste epizoda (XML persistencija poslovnog pravila)
-- XML fajl se koristi za cuvanje Top5 i Top10 lista
-- Fajl: wwwroot/podaci/top_epizode.xml

-- Seed podaci
INSERT INTO Korisnici (KorisnickoIme, Lozinka, Uloga) VALUES
('admin', 'admin123', 'admin'),
('marko', 'marko123', 'korisnik'),
('ana', 'ana123', 'korisnik'),
('petar', 'petar123', 'korisnik');
GO

INSERT INTO Epizode (Naslov, Opis, DatumPremijere, KreiraoId) VALUES
('Pilot - Početak Svega', 'Uvod u svet serije. Upoznajemo glavne likove i početnu situaciju.', '2024-01-15', 1),
('Tajna Prošlost', 'Otkriva se misterija iz prošlosti glavnog lika.', '2024-01-22', 1),
('Izdaja', 'Neko iz ekipe prelazi na drugu stranu. Ko će to biti?', '2024-01-29', 1),
('Povratak', 'Lik kojeg smo izgubili se vraća. Ali promenjen.', '2024-02-05', 1),
('Finalna Bitka', 'Sve se zbiva u ovu jednu noć. Ko će preživeti?', '2024-02-12', 1),
('Epilog', 'Kako su se stvari završile. I novi početak.', '2024-02-19', 1),
('Novi Počeci', 'Druga sezona. Novi likovi, novi problemi.', '2024-09-01', 1),
('Mračni Grad', 'Istraga u gradu prepunom tajni.', '2024-09-08', 1),
('Izgubljena Uspomena', 'Neko nema sećanja. Ali neko ih ima previše.', '2024-09-15', 1),
('Zora', 'Vrhunac druge sezone. Ništa neće biti isto.', '2024-09-22', 1);
GO
