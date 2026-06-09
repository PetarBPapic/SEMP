using BibliotekaKlasa.KlasePodataka.Modeli;
using BibliotekaKlasa.KlasePodataka.Repozitorijumi;
using BibliotekaKlasa.Servisi;
using BibliotekaKlasa.TehnoloskeKlase;
using System.Text.Json;

namespace PoslovnaLogika.PoslovniProcessi
{
    public class PoslovniProces
    {
        private readonly string _konekcioniString;
        private readonly XmlTopListeServis _xmlServis;
        private readonly string _restApiUrl;

        public PoslovniProces(string konekcioniString, XmlTopListeServis xmlServis,
                              string restApiUrl = "https://localhost:7001")
        {
            _konekcioniString = konekcioniString;
            _xmlServis = xmlServis;
            _restApiUrl = restApiUrl;
        }

        // ============================================================
        // POSLOVNA PRAVILA - TOP LISTE
        // ============================================================

        public void AzurirajTopListe()
        {
            var sve = UzmiSveEpizodeSaProsekom();
            _xmlServis.AzurirajTopListe(sve);
        }

        public List<TopEpizodaModel> DajTop5IzXml()
        {
            return _xmlServis.UcitajTop5();
        }

        public List<TopEpizodaModel> DajTop10IzXml()
        {
            return _xmlServis.UcitajTop10();
        }

        public List<TopEpizodaModel> DajSveEpizodeSortirane()
        {
            var sve = UzmiSveEpizodeSaProsekom();
            return sve.OrderByDescending(e => e.ProsecnaOcena)
                      .ThenByDescending(e => e.BrojOcena)
                      .ToList();
        }

        public string DajDatumAzuriranja()
        {
            return _xmlServis.DajDatumAzuriranja();
        }

        // ============================================================
        // OCENJIVANJE
        // ============================================================

        /// <summary>
        /// POSLOVNO PRAVILO (AKO-ONDA):
        ///   AKO je korisnik vec dao MaksOcenaPoKorisniku ocena danas
        ///   (parametar se cita pozivom REST servisa iz JSON fajla,
        ///   a broj danasanjih ocena se proverava stored procedurom)
        ///   ONDA se nova ocena odbija.
        /// </summary>
        public bool OceniEpizodu(int epizodaId, int korisnikId, int vrednost, string komentar)
        {
            if (vrednost < 1 || vrednost > 5)
                throw new ArgumentException("Ocena mora biti između 1 i 5.");

            // Korak 1: citanje parametra putem REST servisa (iz JSON fajla)
            int maksOcena = UzmiMaksOcenaPoKorisniku();

            // Korak 2: provera uslova putem stored procedure
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var epizodaSpRepo = new EpizodaSpRepo(konekcijaObjekat);
            int danasOcena = epizodaSpRepo.DajBrojOcenaKorisnikaDanas(korisnikId);

            // Korak 3: primena ogranicenja
            if (danasOcena >= maksOcena)
                throw new InvalidOperationException(
                    $"Dostigli ste dnevni limit od {maksOcena} ocena. Pokušajte ponovo sutra.");

            // Korak 4: snimanje ocene
            var ocenaRepo = new OcenaRepo(konekcijaObjekat);
            var ocenaModelObjekat = new OcenaModel
            {
                EpizodaId = epizodaId,
                KorisnikId = korisnikId,
                Vrednost = vrednost,
                Komentar = komentar ?? "",
                OcenjeneNa = DateTime.Now
            };
            ocenaRepo.IzmeniIliDodaj(ocenaModelObjekat);

            // Korak 5: automatska akcija - azuriraj Top liste
            AzurirajTopListe();

            return true;
        }

        /// <summary>
        /// Cita parametar MaksOcenaPoKorisniku pozivom REST servisa.
        /// Ako servis nije dostupan, koristi podrazumevanu vrednost 50.
        /// </summary>
        private int UzmiMaksOcenaPoKorisniku()
        {
            try
            {
                using var klijent = new HttpClient();
                klijent.Timeout = TimeSpan.FromSeconds(3);
                var odgovor = klijent.GetStringAsync($"{_restApiUrl}/api/epizode/ogranicenje")
                                     .GetAwaiter().GetResult();
                var podaci = JsonSerializer.Deserialize<Dictionary<string, int>>(odgovor,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (podaci != null && podaci.ContainsKey("MaksOcenaPoKorisniku"))
                    return podaci["MaksOcenaPoKorisniku"];
            }
            catch
            {
                // Servis nije dostupan - koristimo podrazumevanu vrednost
            }
            return 50;
        }

        // ============================================================
        // Zanri
        // ============================================================

        public List<ZarnModel> DajSveZarne()
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var zarnRepo = new ZarnRepo(konekcijaObjekat);
            return zarnRepo.DajSve();
        }


        // ============================================================
        // EPIZODE
        // ============================================================

        public List<EpizodaModel> DajSveEpizode()
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var epizodaRepo = new EpizodaRepo(konekcijaObjekat);
            return epizodaRepo.DajSve();
        }

        public EpizodaModel? DajEpizodu(int id)
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var epizodaRepo = new EpizodaRepo(konekcijaObjekat);
            return epizodaRepo.DajPoId(id);
        }

        public void DodajEpizodu(EpizodaModel epizodaModelObjekat)
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var epizodaRepo = new EpizodaRepo(konekcijaObjekat);
            epizodaRepo.Dodaj(epizodaModelObjekat);
        }

        public void IzmeniEpizodu(EpizodaModel epizodaModelObjekat)
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var epizodaRepo = new EpizodaRepo(konekcijaObjekat);
            epizodaRepo.Izmeni(epizodaModelObjekat);
        }

        public void ObrisiEpizodu(int id)
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var ocenaRepo = new OcenaRepo(konekcijaObjekat);
            var epizodaRepo = new EpizodaRepo(konekcijaObjekat);

            ocenaRepo.ObrisiZaEpizodu(id);
            epizodaRepo.Obrisi(id);
            epizodaRepo.ResetuiIdBrojac();
            AzurirajTopListe();
        }

        // ============================================================
        // OCENE I STATISTIKA
        // ============================================================

        public List<OcenaModel> DajOceneZaEpizodu(int epizodaId)
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var ocenaRepo = new OcenaRepo(konekcijaObjekat);
            return ocenaRepo.DajZaEpizodu(epizodaId);
        }

        public OcenaModel? DajOcenuKorisnika(int epizodaId, int korisnikId)
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var ocenaRepo = new OcenaRepo(konekcijaObjekat);
            return ocenaRepo.DajPoEpizodaIdIKorisnikId(epizodaId, korisnikId);
        }

        // ============================================================
        // KORISNICI
        // ============================================================

        public KorisnikModel? ProveraKorisnika(string korisnickoIme, string lozinka)
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var korisnikRepo = new KorisnikRepo(konekcijaObjekat);
            var korisnik = korisnikRepo.DajPoKorisnickomImenu(korisnickoIme);
            if (korisnik == null || korisnik.Lozinka != lozinka)
                return null;
            return korisnik;
        }

        // ============================================================
        // POMOCNE METODE
        // ============================================================

        private List<TopEpizodaModel> UzmiSveEpizodeSaProsekom()
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var ocenaRepo = new OcenaRepo(konekcijaObjekat);
            return ocenaRepo.DajTopEpizodeDataSet(1000);
        }
    }
}