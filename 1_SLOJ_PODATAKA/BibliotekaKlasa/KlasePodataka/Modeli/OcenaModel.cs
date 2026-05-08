namespace BibliotekaKlasa.KlasePodataka.Modeli
{
    public class OcenaModel
    {
        public int Id { get; set; }
        public int EpizodaId { get; set; }
        public int KorisnikId { get; set; }
        public int Vrednost { get; set; }
        public string Komentar { get; set; } = string.Empty;
        public DateTime OcenjeneNa { get; set; } = DateTime.Now;
    }
}
