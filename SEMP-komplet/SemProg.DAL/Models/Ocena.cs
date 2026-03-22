namespace SemProg.DAL.Modeli
{
    public class Ocena
    {
        public int Id { get; set; }
        public int EpizodaId { get; set; }
        public Epizoda Epizoda { get; set; }
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        public int Vrednost { get; set; }
        public string Komentar { get; set; }
        public DateTime OcenjeneNa { get; set; } = DateTime.Now;
    }
}
