namespace BibliotekaKlasa.KlasePodataka.Modeli
{
    public class TopEpizodaModel
    {
        public int EpizodaId { get; set; }
        public string Naslov { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public double ProsecnaOcena { get; set; }
        public int BrojOcena { get; set; }
        public DateTime DatumPremijere { get; set; }
    }
}
