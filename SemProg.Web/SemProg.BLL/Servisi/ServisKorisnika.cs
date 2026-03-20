using Microsoft.EntityFrameworkCore;
using SemProg.BLL.Interfejsi;
using SemProg.DAL;

namespace SemProg.BLL.Servisi
{
    public class ServisKorisnika : IServisKorisnika
    {
        private readonly BazaPodatakaKontekst _baza;
        public ServisKorisnika(BazaPodatakaKontekst baza) => _baza = baza;

        public bool Provjeri(string korisnickoIme, string lozinka)
            => _baza.Korisnici.Any(k => k.KorisnickoIme == korisnickoIme && k.Lozinka == lozinka);

        public bool JeAdmin(string korisnickoIme)
            => _baza.Korisnici.Any(k => k.KorisnickoIme == korisnickoIme && k.Uloga == "admin");

        public int UzmiId(string korisnickoIme)
            => _baza.Korisnici.First(k => k.KorisnickoIme == korisnickoIme).Id;
    }
}
