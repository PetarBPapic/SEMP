using BibliotekaKlasa.TehnoloskeKlase;

namespace BibliotekaKlasa.KlasePodataka.Repozitorijumi
{
    /// <summary>
    /// Apstraktna bazna klasa za sve repozitorijume.
    /// Sadrzi zajednicki KonekcijaKlasa objekat koji nasledjuju
    /// EpizodaRepo, KorisnikRepo i OcenaRepo.
    /// </summary>
    public abstract class BaseRepo
    {
        protected KonekcijaKlasa _konekcijaObjekat;

        protected BaseRepo(KonekcijaKlasa konekcijaObjekat)
        {
            _konekcijaObjekat = konekcijaObjekat;
        }
    }
}