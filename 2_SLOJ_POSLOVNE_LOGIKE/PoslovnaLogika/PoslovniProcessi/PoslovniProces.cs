using BibliotekaKlasa.KlasePodataka.Modeli;
using BibliotekaKlasa.KlasePodataka.Repozitorijumi;
using BibliotekaKlasa.Servisi;
using BibliotekaKlasa.TehnoloskeKlase;

namespace PoslovnaLogika.PoslovniProcessi
{
    /// <summary>
    /// Klasa Poslovni Proces - centralna klasa BLL (Business Logic Layer).
    /// Bavi se poslovnim pravilima i procesima vezanim za epizode i ocene.
    /// 
    /// Glavno poslovno pravilo: Top 5 i Top 10 najbolje ocenjenih epizoda.
    /// Top liste se azuriraju sa svakom ocenom i cuvaju u XML fajlu.
    /// Vidljive su na pocetnoj strani bez login-a i nakon prijave.
    /// </summary>
    public class PoslovniProces
    {
        private readonly string _konekcioniString;
        private readonly XmlTopListeServis _xmlServis;

        public PoslovniProces(string konekcioniString, XmlTopListeServis xmlServis)
        {
            _konekcioniString = konekcioniString;
            _xmlServis = xmlServis;
        }

        // ============================================================
        // POSLOVNA PRAVILA - TOP LISTE
        // ============================================================

        /// <summary>
        /// Azurira Top5 i Top10 liste u XML fajlu.
        /// Poziva se automatski nakon svake ocene (poslovno pravilo).
        /// </summary>
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
        /// Ocenjivanje epizode - centralni poslovni proces.
        /// Nakon ocene automatski azurira Top liste.
        /// </summary>
        public bool OceniEpizodu(int epizodaId, int korisnikId, int vrednost, string komentar)
        {
            if (vrednost < 1 || vrednost > 5)
                throw new ArgumentException("Ocena mora biti izmedju 1 i 5.");

            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
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

            // POSLOVNO PRAVILO: azuriraj Top liste odmah nakon ocene
            AzurirajTopListe();

            return true;
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

        /// <summary>
        /// Brise epizodu zajedno sa svim njenim ocenama.
        /// Redosled je bitan: najpre ocene, pa epizoda (FK constraint).
        /// Nakon brisanja azurira Top liste i resetuje ID brojac u bazi.
        /// </summary>
        public void ObrisiEpizodu(int id)
        {
            var konekcijaObjekat = new KonekcijaKlasa(_konekcioniString);
            var ocenaRepo = new OcenaRepo(konekcijaObjekat);
            var epizodaRepo = new EpizodaRepo(konekcijaObjekat);

            // Korak 1: obrisi sve ocene za ovu epizodu (FK constraint)
            ocenaRepo.ObrisiZaEpizodu(id);

            // Korak 2: obrisi samu epizodu
            epizodaRepo.Obrisi(id);

            // Korak 3: resetuj ID brojac da sledeca epizoda dobije najmanji slobodan ID
            epizodaRepo.ResetuiIdBrojac();

            // Korak 4: azuriraj top liste jer su se ocene promenile
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