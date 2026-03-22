namespace SemProg.DAL.Modeli
{
    public class Epizoda
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public DateTime DatumPremijere { get; set; }
        public int KreiraoId { get; set; }
        public Korisnik Kreator { get; set; }
    }
}
