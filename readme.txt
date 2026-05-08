====================================================================
  SEBICAN Reviews - SEMP Slojevita Arhitektura
  Predmet: Razvoj Informacionih Sistema
  Student: Papic Petar IT 47/22
  Mentor: prof. Ljubica Kazi
  Fakultet: Tehnicki fakultet "Mihajlo Pupin", Zrenjanin
====================================================================

OPIS PROJEKTA
-------------
SEBICAN Reviews je veb aplikacija razvijena u ASP.NET Core 8 MVC
arhitekturi sa slojevitom (4-slojnom) strukturom projekta.
Korisnici mogu pregledati epizode i ocenjivati ih ocenom od 1 do 5.
Sistem automatski azurira Top 5 i Top 10 liste najbolje ocenjenih
epizoda i cuva ih u XML fajlu koji je dostupan svim posetiocima
bez potrebe za prijavom.

====================================================================
ARHITEKTURA - 4 SLOJA
====================================================================

1_SLOJ_PODATAKA / BibliotekaKlasa
  - TehnoloskeKlase: KonekcijaKlasa, BaznaKonekcijaNova, TabelaKlasa
  - KlasePodataka: ADO.NET modeli i repozitorijumi (SqlCommand, SqlDataReader)
  - KlasePodatakaEF: Entity Framework Core modeli, kontekst i repozitorijumi
  - Servisi: XmlTopListeServis (perzistencija Top listi u XML fajlu)

2_SLOJ_POSLOVNE_LOGIKE / PoslovnaLogika
  - PoslovniProcessi: PoslovniProces.cs (centralna BLL klasa)
  - Ogranicenja.cs (ucitava ogranicenja iz JSON fajla)

3_SLOJ_SERVISA / SEMP_REST_API
  - REST API kontroleri (GET top5, top10, sve-sortirane, CRUD epizoda)

4_PREZENTACIONI_SLOJ / SEMP_Aplikacija
  - Controllers: Pocetna, Nalog, Epizoda, Ocena
  - Views: Pocetna, Nalog, Epizoda, Ocena (Rang lista, Oceni)

====================================================================
POSLOVNO PRAVILO - TOP 5 I TOP 10 EPIZODA
====================================================================

Glavno poslovno pravilo sistema je automatsko odrzavanje rang listi
Top 5 i Top 10 najbolje ocenjenih epizoda.

Tok:
  Korisnik oceni epizodu
    -> PoslovniProces.OceniEpizodu()
       -> OcenaRepo cuva ocenu u SQL bazu
       -> PoslovniProces.AzurirajTopListe() se automatski poziva
          -> OcenaRepo.DajTopEpizodeDataSet() cita iz baze (DataSet)
          -> XmlTopListeServis cuva rezultat u XML fajl

XML fajl lokacija: wwwroot/podaci/top_epizode.xml

Vidljivost:
  - Pocetna strana  -> Top 5 (vidljivo BEZ prijave)
  - Rang lista      -> Top 10 + sve epizode sortirane (vidljivo BEZ prijave)
  - Oceni epizode   -> zahteva prijavu

====================================================================
DAL - 3 PRISTUPA PRISTUPU PODACIMA
====================================================================

  Pristup 1: ADO.NET + SqlCommand + SqlDataReader
    -> KorisnikRepo, EpizodaRepo

  Pristup 2: ADO.NET + TabelaKlasa (DataSet/DataAdapter)
    -> OcenaRepo (koristi TabelaKlasa za DajTopEpizodeDataSet)

  Pristup 3: Entity Framework Core + LINQ
    -> EpizodaEFRepo, OcenaEFRepo

====================================================================
POKRETANJE PROJEKTA
====================================================================

KORAK 1 - Baza podataka:
  Otvoriti SQL Server Management Studio (SSMS)
  Pokrenuti skriptu: BazaPodataka/SEMP_Baza.sql
  Skripta kreira bazu SEMP2025SLOZEN sa svim tabelama i podacima.

KORAK 2 - Konekcioni string:
  Otvoriti: 4_PREZENTACIONI_SLOJ/SEMP_Aplikacija/appsettings.json
  Podesiti Server= prema vasoj SQL Server instanci.
  Primer: "Server=.;Database=SEMP2025SLOZEN;Trusted_Connection=True;
           TrustServerCertificate=True;"

KORAK 3 - Pokretanje:
  Otvoriti SEMP_Slozen.sln u Visual Studio 2022
  Desni klik na SEMP_Aplikacija -> Set as Startup Project
  Pokrenuti aplikaciju (zelena strelica)

====================================================================
TEST NALOZI
====================================================================

  Korisnicko ime  | Lozinka    | Uloga
  ----------------|------------|----------
  admin           | admin123   | admin
  marko           | marko123   | korisnik
  ana             | ana123     | korisnik
  petar           | petar123   | korisnik
  jovana          | jovana123  | korisnik

====================================================================
TEHNOLOGIJE
====================================================================

  - .NET 8 / ASP.NET Core MVC
  - SQL Server (ADO.NET + Entity Framework Core)
  - Bootstrap 5 (CDN, dark tema)
  - XML (System.Xml.Linq) za Top liste
  - Session autentifikacija
  - JSON za ogranicenja (ogranicenjeUpisa.json)

====================================================================
STRUKTURA FAJLOVA
====================================================================

SEMP_Slozen/
  SEMP_Slozen.sln
  BazaPodataka/
    SEMP_Baza.sql
  1_SLOJ_PODATAKA/
    BibliotekaKlasa/
  2_SLOJ_POSLOVNE_LOGIKE/
    PoslovnaLogika/
  3_SLOJ_SERVISA/
    SEMP_REST_API/
  4_PREZENTACIONI_SLOJ/
    SEMP_Aplikacija/

====================================================================
