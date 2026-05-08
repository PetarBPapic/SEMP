using Microsoft.EntityFrameworkCore;
using BibliotekaKlasa.KlasePodatakaEF.ModeliEF;
using BibliotekaKlasa.KlasePodatakaEF.KontekstEF;

namespace BibliotekaKlasa.KlasePodatakaEF.RepozitorijumiEF
{
    /// <summary>
    /// EF repozitorijum za Epizode - treci pristup pristupu podacima u DAL-u (Entity Framework Core).
    /// </summary>
    public class EpizodaEFRepo
    {
        private readonly AppDbContext _kontekst;

        public EpizodaEFRepo(AppDbContext kontekst)
        {
            _kontekst = kontekst;
        }

        public void Dodaj(EpizodaEntityModel epizodaEntityModelObjekat)
        {
            if (epizodaEntityModelObjekat == null) return;
            _kontekst.EpizodeEntityModelObjektiDBSet.Add(epizodaEntityModelObjekat);
            _kontekst.SaveChanges();
        }

        public void Izmeni(EpizodaEntityModel epizodaEntityModelObjekat)
        {
            if (epizodaEntityModelObjekat == null) return;
            _kontekst.EpizodeEntityModelObjektiDBSet.Update(epizodaEntityModelObjekat);
            _kontekst.SaveChanges();
        }

        public void Obrisi(int id)
        {
            var epizodaEntityModelObjekat = _kontekst.EpizodeEntityModelObjektiDBSet.Find(id);
            if (epizodaEntityModelObjekat == null) return;
            _kontekst.EpizodeEntityModelObjektiDBSet.Remove(epizodaEntityModelObjekat);
            _kontekst.SaveChanges();
        }

        public List<EpizodaEntityModel> DajSve()
        {
            return _kontekst.EpizodeEntityModelObjektiDBSet
                .OrderByDescending(e => e.DatumPremijere)
                .ToList();
        }

        public EpizodaEntityModel? DajPoId(int id)
        {
            return _kontekst.EpizodeEntityModelObjektiDBSet.Find(id);
        }
    }

    /// <summary>
    /// EF repozitorijum za Ocene - koristi LINQ upite za top liste.
    /// </summary>
    public class OcenaEFRepo
    {
        private readonly AppDbContext _kontekst;

        public OcenaEFRepo(AppDbContext kontekst)
        {
            _kontekst = kontekst;
        }

        public void Dodaj(OcenaEntityModel ocenaEntityModelObjekat)
        {
            if (ocenaEntityModelObjekat == null) return;
            _kontekst.OceneEntityModelObjektiDBSet.Add(ocenaEntityModelObjekat);
            _kontekst.SaveChanges();
        }

        public void Izmeni(OcenaEntityModel ocenaEntityModelObjekat)
        {
            if (ocenaEntityModelObjekat == null) return;
            _kontekst.OceneEntityModelObjektiDBSet.Update(ocenaEntityModelObjekat);
            _kontekst.SaveChanges();
        }

        public List<OcenaEntityModel> DajZaEpizodu(int epizodaId)
        {
            return _kontekst.OceneEntityModelObjektiDBSet
                .Where(o => o.EpizodaId == epizodaId)
                .Include(o => o.Korisnik)
                .ToList();
        }

        public OcenaEntityModel? DajPoEpizodaIdIKorisnikId(int epizodaId, int korisnikId)
        {
            return _kontekst.OceneEntityModelObjektiDBSet
                .FirstOrDefault(o => o.EpizodaId == epizodaId && o.KorisnikId == korisnikId);
        }
    }
}
