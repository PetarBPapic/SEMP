namespace SemProg.BLL.Interfejsi
{
    public interface IServisKorisnika
    {
        bool Provjeri(string korisnickoIme, string lozinka);
        bool JeAdmin(string korisnickoIme);
        int UzmiId(string korisnickoIme);
    }
}
