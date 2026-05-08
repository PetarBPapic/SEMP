namespace BibliotekaKlasa.KlasePodataka.Modeli
{
    public class KorisnikModel
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
        public string Uloga { get; set; } = "korisnik";
    }
}
