namespace BibliotekaKlasa.KlasePodataka.Modeli
{
    public class EpizodaModel
    {
        public int Id { get; set; }
        public string Naslov { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public DateTime DatumPremijere { get; set; }
        public int KreiraoId { get; set; }

        public int ZarnIdz { get; set; }
    }
}
